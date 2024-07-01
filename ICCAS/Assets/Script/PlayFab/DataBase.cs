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
        // �̱��� �ν��Ͻ� �ʱ�ȭ
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ���� �ٲ� ������Ʈ�� �ı����� �ʵ��� ��
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� ���ο� ������Ʈ�� �ı�
        }
    }

    private void DisplayPlayfabError(PlayFabError error) => Debug.LogError("error : " + error.GenerateErrorReport());

    #region ���� ������ ����
    public PlayerData playerData = new PlayerData();
    public CharacterData characterData = new CharacterData();

    // ������ ����
    public void SaveJsonToPlayfab()
    {
        Dictionary<string, string> dataDic = new Dictionary<string, string>();

        // ������ �����ϱ� �߰�
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

    // ������ �ҷ�����
    public void GetUserData()
    {
        var request = new GetUserDataRequest() { PlayFabId = PlayFabLogin.instance.myID };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            foreach (var eachData in result.Data)
            {
                string key = eachData.Key;

                // ������ �ҷ����� �߰�
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

    #region ���� ������ ����
    // ������ ����
    public List<UpgradeData>upgradeData = new List<UpgradeData>();

    [ContextMenu("To Json Data")]
    void SavePlayerDataToJson()
    {
        string jsonData = JsonMapper.ToJson(upgradeData);
        string path = Path.Combine(Application.dataPath, "Data/UpgradeData.json");
        File.WriteAllText(path, jsonData);

        Debug.Log("���� �Ϸ�");
    }

    [ContextMenu("From Json Data")]
    void LoadPlayerDataFromJson()
    {
        var data = Resources.Load<TextAsset>("Data/UpgradeData");
        string jsonData = data.ToString();
        upgradeData = JsonMapper.ToObject<List<UpgradeData>>(jsonData);

        Debug.Log("�ε� �Ϸ�");
    }
    #endregion

    #region ��ȭ �߰� ����
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
    [Header("�÷��̾� ���൵")]
    public int level;
    public int exp;
    public int gold;
    public int topStage;
    public int cSelect;  // ĳ���� Select

    [Header("Daily")]
    public bool isSurvey;
}

[Serializable]
public struct CharacterData
{
    [Header("ĳ���� ����")]
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