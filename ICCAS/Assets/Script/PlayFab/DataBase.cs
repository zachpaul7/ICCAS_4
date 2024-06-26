using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public static DataBase instance;

    private void Awake()
    {
        if(instance != null)
            Destroy(instance);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void DisplayPlayfabError(PlayFabError error) => Debug.LogError("error : " + error.GenerateErrorReport());

    #region 사용할 데이터 변수 선언 및 초기화
    public PlayerData playerData = new PlayerData();
    public EnemyData enemyData = new EnemyData();
    #endregion

    #region 데이터 관리
    // 데이터 저장
    public void SaveJsonToPlayfab()
    {
        Dictionary<string, string> dataDic = new Dictionary<string, string>();

        // 데이터 저장하기 추가
        dataDic.Add("DataContent", JsonUtility.ToJson(playerData));
        dataDic.Add("DataContent1", JsonUtility.ToJson(enemyData));

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
                if (key == "DataContent")
                {
                    playerData = JsonUtility.FromJson<PlayerData>(eachData.Value.Value);

                    Debug.Log(playerData);
                }
                if (key == "DataContent1")
                {
                    enemyData = JsonUtility.FromJson<EnemyData>(eachData.Value.Value);

                    Debug.Log(enemyData);
                }
            }

        }, DisplayPlayfabError);
    }
    #endregion
}

[Serializable]
public struct PlayerData
{
    public string name;
    public int value;
    public bool isCheck;
}

[Serializable]
public struct EnemyData
{
    public string name;
    public int value;
}
