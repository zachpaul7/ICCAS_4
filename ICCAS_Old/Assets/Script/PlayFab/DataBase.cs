using LitJson;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.PfEditor.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public static DataBase instance;

    private void Awake()
    {
        // 싱글톤 인스턴스 초기화
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 오브젝트가 파괴되지 않도록 함
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 새로운 오브젝트를 파괴
        }
    }

    private void DisplayPlayfabError(PlayFabError error) => Debug.LogError("error : " + error.GenerateErrorReport());

    #region 서버 데이터 관리
    public PlayerData playerData = new PlayerData();
    public CharacterData characterData = new CharacterData();

    // 데이터 저장
    public void SaveJsonToPlayfab()
    {
        Dictionary<string, string> dataDic = new Dictionary<string, string>();

        // 데이터 저장하기 추가
        dataDic.Add("PlayerData", JsonUtility.ToJson(playerData));
        dataDic.Add("CharacterData", JsonUtility.ToJson(characterData));

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

    // 데이터 불러오기
    public void GetUserData()
    {
        var request = new GetUserDataRequest() { PlayFabId = PlayFabLogin.instance.myID };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            foreach (var eachData in result.Data)
            {
                string key = eachData.Key;

                // 데이터 불러오기 추가
                if (key == "PlayerData")
                {
                    playerData = JsonUtility.FromJson<PlayerData>(eachData.Value.Value);

                }
                if (key == "CharacterData")
                {
                    characterData = JsonUtility.FromJson<CharacterData>(eachData.Value.Value);

                }
            }

        }, DisplayPlayfabError);
    }
    #endregion

    #region 로컬 데이터 관리
    // 데이터 변수
    public List<UpgradeData>upgradeData = new List<UpgradeData>();

    [ContextMenu("To Json Data")]
    void SavePlayerDataToJson()
    {
        string jsonData = JsonMapper.ToJson(upgradeData);
        string path = Path.Combine(Application.dataPath, "Data/UpgradeData.json");
        File.WriteAllText(path, jsonData);

        Debug.Log("저장 완료");
    }

    [ContextMenu("From Json Data")]
    void LoadPlayerDataFromJson()
    {
        var data = Resources.Load<TextAsset>("Data/UpgradeData");
        string jsonData = data.ToString();
        upgradeData = JsonMapper.ToObject<List<UpgradeData>>(jsonData);

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
}

[Serializable]
public struct PlayerData
{
    [Header("플레이어 진행도")]
    public int level;
    public int exp;
    public int gold;
    public int topStage;
    public int cSelect;  // 캐릭터 Select

    [Header("Daily")]
    public bool isSurvey;
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