using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void DisplayPlayfabError(PlayFabError error) => Debug.LogError("error : " + error.GenerateErrorReport());

    public void GetServerTime(Action<DateTime> onComplete)
    {
        PlayFabClientAPI.GetTime(new GetTimeRequest(),
            result =>
            {
                onComplete(result.Time);
            }, DisplayPlayfabError);
    }
}
