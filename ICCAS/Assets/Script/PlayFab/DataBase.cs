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

    #region 사용할 데이터 변수 선언 및 초기화
    public PlayerData playerData = new PlayerData();
    public CharacterData characterData = new CharacterData();
    #endregion

    #region 서버 데이터 관리
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
