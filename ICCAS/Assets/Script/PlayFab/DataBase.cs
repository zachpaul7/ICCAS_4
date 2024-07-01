using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
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

    #region ����� ������ ���� ���� �� �ʱ�ȭ
    public PlayerData playerData = new PlayerData();
    public CharacterData characterData = new CharacterData();
    #endregion

    #region ���� ������ ����
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
