using LitJson;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DataBase : MonoBehaviour
{
    public static DataBase instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void SaveData()
    {
        // 서버 저장
        SaveJsonToPlayfab();

        // 로컬 저장
        string jsonData = JsonMapper.ToJson(settingInfo);
        string path = Application.persistentDataPath + "/SettingInfo.json";
        File.WriteAllText(path, jsonData);
    }

    public void Initialized()
    {
        AddGold(0);
        AddPlayerLv(0);

        try
        {
            // 게임 세팅 정보 불러오기
            string path = Path.Combine(Application.persistentDataPath, "SettingInfo.json");
            string jsonData = File.ReadAllText(path);
            if (jsonData.Length > 0)
            {
                settingInfo = JsonMapper.ToObject<SettingInfo>(jsonData);
            }
        }
        catch
        {
            Debug.Log("로컬 파일 없음");
            // 게임 세팅 정보 저장
            string jsonData = JsonMapper.ToJson(settingInfo);
            string path = Application.persistentDataPath + "/SettingInfo.json";
            File.WriteAllText(path, jsonData);
        }
    }

    private void DisplayPlayfabError(PlayFabError error) => Debug.LogError("error : " + error.GenerateErrorReport());

    #region 서버 데이터 관리
    public PlayerData playerData = new PlayerData();
    public CharacterData characterData = new CharacterData();
    public SelfCheckScore selfCheckScores = new SelfCheckScore();
    private const int MaxScores = 30;

    public void SaveJsonToPlayfab()
    {
        Dictionary<string, string> dataDic = new Dictionary<string, string>
        {
            { "PlayerData", JsonUtility.ToJson(playerData) },
            { "CharacterData", JsonUtility.ToJson(characterData) },
            { "SelfCheckScores", JsonUtility.ToJson(selfCheckScores) }
        };

        SetUserData(dataDic);
    }

    public void SetUserData(Dictionary<string, string> data)
    {
        var request = new UpdateUserDataRequest() { Data = data, Permission = UserDataPermission.Public };
        try
        {
            PlayFabClientAPI.UpdateUserData(request, (result) =>
            {
                Debug.Log("Update Player Data!");
            }, DisplayPlayfabError);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void GetUserData(Action onComplete = null)
    {
        var request = new GetUserDataRequest() { PlayFabId = PlayFabLogin.instance.myID };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            foreach (var eachData in result.Data)
            {
                string key = eachData.Key;

                if (key == "PlayerData")
                {
                    playerData = JsonUtility.FromJson<PlayerData>(eachData.Value.Value);
                }
                if (key == "CharacterData")
                {
                    characterData = JsonUtility.FromJson<CharacterData>(eachData.Value.Value);
                }
                if (key == "SelfCheckScores")
                {
                    selfCheckScores = JsonUtility.FromJson<SelfCheckScore>(eachData.Value.Value);
                }
            }

            if (PlayFabLogin.instance.username == null)
            {
                PlayFabLogin.instance.username = playerData.nickName;
            }

            if (PlayFabLogin.instance.isLogin == false)
            {
                PlayFabLogin.instance.isLogin = true;
            }

            onComplete?.Invoke();
        }, DisplayPlayfabError);
    }
    #endregion

    #region 로컬 데이터 관리
    public List<UpgradeData> upgradeData = new List<UpgradeData>();
    public List<ItemData> itemData = new List<ItemData>();
    public SettingInfo settingInfo;
    public List<CharacterInfo> characterInfos = new List<CharacterInfo>();
    public List<EnemyInfo> enemyInfos = new List<EnemyInfo>();
    public PlayerInfo playerInfo;

    [ContextMenu("To Json Data")]
    void SavePlayerDataToJson()
    {
        string jsonData = JsonMapper.ToJson(upgradeData);
        string path = Path.Combine(Application.dataPath, "Data/UpgradeData.json");
        File.WriteAllText(path, jsonData);

        jsonData = JsonMapper.ToJson(itemData);
        path = Path.Combine(Application.dataPath, "Data/ItemData.json");
        File.WriteAllText(path, jsonData);

        jsonData = JsonMapper.ToJson(characterInfos);
        path = Path.Combine(Application.dataPath, "Data/CharacterInfo.json");
        File.WriteAllText(path, jsonData);

        jsonData = JsonMapper.ToJson(enemyInfos);
        path = Path.Combine(Application.dataPath, "Data/EnemyInfo.json");
        File.WriteAllText(path, jsonData);

        jsonData = JsonMapper.ToJson(playerInfo);
        path = Path.Combine(Application.dataPath, "Data/PlayerInfo.json");
        File.WriteAllText(path, jsonData);

        Debug.Log("저장 완료");
    }

    [ContextMenu("From Json Data")]
    void LoadPlayerDataFromJson()
    {
        var data = Resources.Load<TextAsset>("Data/UpgradeData");
        string jsonData = data.ToString();
        upgradeData = JsonMapper.ToObject<List<UpgradeData>>(jsonData);

        data = Resources.Load<TextAsset>("Data/ItemData");
        jsonData = data.ToString();
        itemData = JsonMapper.ToObject<List<ItemData>>(jsonData);

        data = Resources.Load<TextAsset>("Data/CharacterInfo");
        jsonData = data.ToString();
        characterInfos = JsonMapper.ToObject<List<CharacterInfo>>(jsonData);

        data = Resources.Load<TextAsset>("Data/EnemyInfo");
        jsonData = data.ToString();
        enemyInfos = JsonMapper.ToObject<List<EnemyInfo>>(jsonData);

        data = Resources.Load<TextAsset>("Data/PlayerInfo");
        jsonData = data.ToString();
        playerInfo = JsonMapper.ToObject<PlayerInfo>(jsonData);

        Debug.Log("로드 완료");
    }
    #endregion

    #region 재화 추가 로직
    public void AddGold(int amount)
    {
        playerData.gold += amount;
        for (int i = 0; i < UIManager.instance.goldText.Length; i++)
            UIManager.instance.goldText[i].text = playerData.gold.ToString();
    }

    public void AddPlayerLv(int amount)
    {
        int prevPLv = playerData.level;

        playerData.exp += amount;

        while (playerInfo.maxExp[playerData.level] <= playerData.exp)
        {
            playerData.exp -= playerInfo.maxExp[playerData.level];
            playerData.level++;
        }

        UIManager.instance.playerLv[0].text = "Lv. " + playerData.level;
        UIManager.instance.playerLv[1].text = playerData.level.ToString();

        UIManager.instance.playerLvBar[0].GetComponent<Image>().fillAmount = (float)playerData.exp / playerInfo.maxExp[playerData.level];
        UIManager.instance.playerLvBar[1].GetComponent<TextMeshProUGUI>().text = playerData.exp + " / " + playerInfo.maxExp[playerData.level];

        if(prevPLv < playerData.level)
        {
            UIManager.instance.playerLvPanel.SetActive(true);
            UIManager.instance.playerLvPanelText.text = playerData.level.ToString();
        }
    }

    public void AddCharacterLv(int index, int amount)
    {
        characterData.level[index] += amount;

        if (characterData.level[index] >= 9)
        {
            UIManager.instance.chUI.yMaxLv0[index].SetActive(true);
            UIManager.instance.chUI.nMaxLv0[index].SetActive(false);
            UIManager.instance.chUI.yMaxLv1[index].SetActive(true);
            UIManager.instance.chUI.nMaxLv1[index].SetActive(false);
        }
        else
        {
            UIManager.instance.chUI.yMaxLv0[index].SetActive(false);
            UIManager.instance.chUI.nMaxLv0[index].SetActive(true);
            UIManager.instance.chUI.yMaxLv1[index].SetActive(false);
            UIManager.instance.chUI.nMaxLv1[index].SetActive(true);
        }

        UIManager.instance.clLevel0[index].text = (characterData.level[index] + 1).ToString();
        UIManager.instance.clLevel1[index].text = (characterData.level[index] + 1).ToString();
        UIManager.instance.clLevel2[index].text = "Lv. " + (characterData.level[index] + 1);
    }

    
    #endregion

    #region SelfCheckScore 추가 로직
    public void AddSelfCheckScore(int score)
    {
        selfCheckScores.checkScore.Add(score);

        if (selfCheckScores.checkScore.Count > MaxScores)
        {
            selfCheckScores.checkScore.RemoveAt(0); // 가장 오래된 데이터를 삭제합니다.
        }

        playerData.daily += 1;

        SaveData(); // 업데이트된 데이터를 PlayFab에 저장합니다.
    }

    public void AddPoseMeasureScore(int score)
    {
        selfCheckScores.poseScore.Add(score);

        if (selfCheckScores.poseScore.Count > MaxScores)
        {
            selfCheckScores.poseScore.RemoveAt(0); // 가장 오래된 데이터를 삭제합니다.
        }

        SaveData(); // 업데이트된 데이터를 PlayFab에 저장합니다.
    }

    public void AddExerciseScore(int exCur, int exMax)
    {
        selfCheckScores.exCur.Add(exCur);
        selfCheckScores.exMax.Add(exMax);

        if (selfCheckScores.exCur.Count > MaxScores)
        {
            selfCheckScores.exCur.RemoveAt(0); // 가장 오래된 데이터를 삭제합니다.
            selfCheckScores.exMax.RemoveAt(0); // 가장 오래된 데이터를 삭제합니다.
        }

        SaveData(); // 업데이트된 데이터를 PlayFab에 저장합니다.
    }
    #endregion
}

[Serializable]
public struct PlayerData
{
    [Header("시스템")]
    public string nickName;

    [Header("플레이어 진행도")]
    public int level;
    public int exp;
    public int gold;
    public int curStage;
    public int topStage;
    public int cSelect;  // 캐릭터 Select

    [Header("Daily")]
    public int daily;
    public string lastSurveyDate;

    [Header("인벤토리")]
    public bool[] characterSkin;
    public int[] cSkinEquip;
    public int[] itemAmount;
}

[Serializable]
public struct CharacterData
{
    [Header("캐릭터 정보")]
    public bool[] characterOpen;
    public int[] level;
    public bool[] skinOpen;
}

[Serializable]
public struct SelfCheckScore
{
    public string lastUpdateDate;
    public List<int> checkScore;
    public List<int> poseScore;
    public List<int> exCur;
    public List<int> exMax;
}

[Serializable]
public struct CharacterInfo
{
    public int[] maxHp;
    public int[] damage;

    public CharacterInfo(JsonData data)
    {
        maxHp = new int[data["maxHp"].Count];
        damage = new int[data["damage"].Count];

        for(int i = 0; i < maxHp.Length; i++)
        {
            maxHp[i] = int.Parse(data["maxHp"][i].ToString());
            damage[i] = int.Parse(data["damage"][i].ToString());
        }
    }
}

[Serializable]
public struct PlayerInfo
{
    public int[] maxExp;
    public int[] rewardExp;

    public PlayerInfo(JsonData data)
    {
        maxExp = new int[data["maxExp"].Count];
        rewardExp = new int[data["rewardExp"].Count];

        for (int i = 0; i < maxExp.Length; i++)
        {
            maxExp[i] = int.Parse(data["maxExp"][i].ToString());
            rewardExp[i] = int.Parse(data["rewardExp"][i].ToString());
        }
    }
}

[Serializable]
public struct EnemyInfo
{
    public int maxHp;
    public int damage;
    public int gold;

    public EnemyInfo(JsonData data)
    {
        maxHp = int.Parse(data["maxHp"].ToString());
        damage = int.Parse(data["damage"].ToString());
        gold = int.Parse(data["gold"].ToString());
    }
}

[Serializable]
public struct UpgradeData
{
    public int maxLevel;
    public int unlockGold;
    public int upgradeGold;

    public UpgradeData(JsonData data)
    {
        maxLevel = int.Parse(data["maxLevel"].ToString());
        unlockGold = int.Parse(data["unlockGold"].ToString());
        upgradeGold = int.Parse(data["upgradeGold"].ToString());
    }
}

[Serializable]
public struct ItemData
{
    public int cost;
    public int amount;

    public ItemData(JsonData data)
    {
        cost = int.Parse(data["cost"].ToString());
        amount = int.Parse(data["amount"].ToString());
    }
}

[Serializable]
public struct SettingInfo
{
    public bool[] optionToggle;

    public SettingInfo(JsonData data)
    {
        optionToggle = new bool[data["optionToggle"].Count];
        for (int i = 0; i < optionToggle.Length; i++)
        {
            optionToggle[i] = bool.Parse(data["optionToggle"][i].ToString());
        }
    }
}