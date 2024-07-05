using PlayFab.MultiplayerModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    #region 패널들
    public GameObject profile;
    public GameObject profilePanel;
    #endregion

    #region 자가진단
    public GameObject selfCheckPanel;
    public Toggle[] selfToggle;
    private int selfCheckScore;
    #endregion

    #region 옵션
    public GameObject optionPanel;
    #endregion

    #region Lobby 초기 세팅
    public void InitLobbyUI()
    {
        profile.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = DataBase.instance.playerData.nickName;
        profile.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Lv. " + DataBase.instance.playerData.level;
    }
    #endregion

    #region 프로필 패널
    public void OpenProfilePanel()
    {
        profilePanel.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = DataBase.instance.playerData.nickName;
        profilePanel.transform.GetChild(1).GetChild(2).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = DataBase.instance.playerData.level.ToString();

        profilePanel.SetActive(true);
    }
    #endregion

    #region 설문조사
    // 사용자가 SelfCheck를 완료했는지 확인하고 완료되지 않았으면 SelfCheck 열기
    public void OpenSelfCheckPanel()
    {
        // 서버 시간을 가져오는 비동기 요청을 수행합니다.
        PlayFabManager.instance.GetServerTime((serverTime) =>
        {
            // lastSurveyDate를 string에서 DateTime으로 변환합니다.
            DateTime lastSurveyDate = DateTime.MinValue;
            if (!string.IsNullOrEmpty(DataBase.instance.playerData.lastSurveyDate))
            {
                lastSurveyDate = DateTime.Parse(DataBase.instance.playerData.lastSurveyDate);
            }

            // 마지막 설문조사 날짜가 오늘인지 확인합니다.
            if (serverTime.Date == lastSurveyDate.Date)
            {
                // 만약 오늘 이미 설문조사를 완료했다면, 경고 메시지를 출력하고 메서드를 종료합니다.
                Debug.Log("오늘은 이미 설문조사를 완료했습니다.");
                return;
            }

            // 마지막 설문조사 날짜를 현재 서버 시간으로 업데이트합니다.
            DataBase.instance.playerData.lastSurveyDate = serverTime.ToString("yyyy-MM-dd");

            selfCheckPanel.SetActive(true);
        });
    }

    public void SelfCheckScore()
    {
        selfCheckScore = 0;

        for (int i = 0; i < selfToggle.Length; i++)
        {
            if (selfToggle[i].isOn)
            {
                selfCheckScore++;
            }
        }

        DataBase.instance.AddSelfCheckScore(selfCheckScore);

        // 업데이트된 데이터를 PlayFab에 저장합니다.
        DataBase.instance.SaveData();

        selfCheckPanel.SetActive(false);
    }
    #endregion

    #region 세팅 패널
    public void OnChangedSetting(int index)
    {
        if (DataBase.instance.settingInfo.optionToggle[index] == optionPanel.transform.GetChild(1).GetChild(2).GetChild(0).GetChild(index).GetChild(2).GetComponent<Toggle>().isOn)
        {
            return;
        }
        else
        {
            DataBase.instance.settingInfo.optionToggle[index] = optionPanel.transform.GetChild(1).GetChild(2).GetChild(0).GetChild(index).GetChild(2).GetComponent<Toggle>().isOn;
        }
    }

    public void OpenSettingPanel()
    {
        optionPanel.transform.GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetComponent<Toggle>().isOn = DataBase.instance.settingInfo.optionToggle[0];
        optionPanel.transform.GetChild(1).GetChild(2).GetChild(0).GetChild(1).GetChild(2).GetComponent<Toggle>().isOn = DataBase.instance.settingInfo.optionToggle[1];

        optionPanel.SetActive(true);
    }
    #endregion
}
