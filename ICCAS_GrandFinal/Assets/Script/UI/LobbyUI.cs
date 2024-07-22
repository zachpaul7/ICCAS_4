using PlayFab.MultiplayerModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

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

    #region 중립자세 측정
    public GameObject pmPanel;
    public GameObject pmResultText;
    public GameObject pmAccText;
    public GameObject[] pmText;
    public GameObject pmBtn;
    private int tCount;
    private int pmScore;
    #endregion

    #region 운동 추천
    public GameObject exObj;
    public GameObject[] exRecObj;
    #endregion

    #region 게임 방법
    private int panelSelect;
    public GameObject gamePlayPanel;
    public GameObject[] explainPanel;
    public GameObject nextBtn;
    public GameObject prevBtn;
    #endregion

    #region 옵션
    public GameObject optionPanel;
    #endregion

    #region 결과값
    public GameObject resultPanel;
    public GameObject resultPrefab;
    public GameObject[] resultPrefabs;
    #endregion

    #region Lobby 초기 세팅
    public void InitLobbyUI()
    {
        profile.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = DataBase.instance.playerData.nickName;
        profilePanel.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = DataBase.instance.playerData.nickName;
        DataBase.instance.AddPlayerLv(0);
    }
    #endregion

    #region 프로필 패널
    public void OpenProfilePanel()
    {
        InitLobbyUI();

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

        OpenPoseMeasurePanel();
    }
    #endregion

    #region 중립자세 측정
    public void OpenPoseMeasurePanel()
    {
        pmResultText.SetActive(false);
        pmPanel.SetActive(true);
        StartCoroutine(OpenPoseMeasureCoroutine());
    }

    IEnumerator OpenPoseMeasureCoroutine()
    {
        yield return YieldCache.WaitForSeconds(5);

        // poseEstimator 생성
        UIManager.instance.exUI.poseEstimator = Instantiate(UIManager.instance.exUI.exPosePrefab);
        Debug.Log("포즈 시작");

        yield return YieldCache.WaitForSeconds(2);

        PoseEvaluation.instance.GetStretchingSelect(6);

        yield return new WaitUntil(() => PoseEvaluation.instance.exerciseFin);

        tCount = PoseEvaluation.instance.trueCount;

        UIManager.instance.exUI.announceText.text = string.Empty;
        UIManager.instance.exUI.countText.text = string.Empty;
        UIManager.instance.exUI.announcePanel.SetActive(false);

        Destroy(UIManager.instance.exUI.poseEstimator);

        Debug.Log("파괴");

        UIManager.instance.exUI.mainC.GetComponent<Camera>().orthographic = true;
        UIManager.instance.exUI.mainC.transform.position = new Vector3(0, 0, UIManager.instance.exUI.mainC.transform.position.z);
        UIManager.instance.exUI.mainC.GetComponent<Camera>().orthographicSize = 5;

        pmResultText.SetActive(true);

        pmAccText.GetComponent<TextMeshProUGUI>().text = "Accuracy : " + (((float)tCount / 6) * 100) + "%";
        pmResultText.SetActive(true);

        if (tCount <= 3)
            pmText[1].SetActive(true);
        else
            pmText[0].SetActive(true);

        pmBtn.SetActive(true);
    }
    
    public void OnClickPoseMeasureSubmit()
    {
        PoseMeasureScore(tCount);
    }

    public void PoseMeasureScore(int tcount)
    {
        DataBase.instance.AddPoseMeasureScore(tcount);

        // 업데이트된 데이터를 PlayFab에 저장합니다.
        DataBase.instance.SaveData();

        RecommandExercise();
    }

    public void RecommandExercise()
    {
        for (int i = 0; i < 5; i++)
        {
            exRecObj[i].SetActive(false);
        }

        for (int i = 0; i < 7; i++)
        {
            switch (i)
            {
                case 0:
                    if (!selfToggle[0])
                        return;

                    exRecObj[4].SetActive(true);
                    break;
                case 1:
                    if (!selfToggle[1])
                        return;

                    exRecObj[3].SetActive(true);
                    break;
                case 2:
                    if (!selfToggle[2])
                        return;

                    exRecObj[0].SetActive(true);
                    break;
                case 3:
                    if (!selfToggle[3])
                        return;

                    exRecObj[1].SetActive(true);
                    break;
                case 4:
                    if (!selfToggle[4])
                        return;

                    exRecObj[1].SetActive(true);
                    break;
                case 5:
                    if (!selfToggle[5])
                        return;

                    exRecObj[2].SetActive(true);
                    break;
                case 6:
                    if (!selfToggle[6])
                        return;

                    exRecObj[0].SetActive(true);
                    break;
            }
        }

        pmPanel.SetActive(false);
        exObj.SetActive(true);
    }

    public void SubmitBtn()
    {
        exObj.SetActive(false);
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

    #region 결과 보기
    public void SetResult()
    {
        resultPrefabs = new GameObject[DataBase.instance.playerData.daily];
        int startDay = 0;
        int forDay = 0;

        if (DataBase.instance.playerData.daily >= 30)
        {
            startDay = DataBase.instance.playerData.daily - 30;
            forDay = 30;
        }
        else
        {
            startDay = 0;
            forDay = DataBase.instance.selfCheckScores.checkScore.Count;
        }
            

        for (int i = 0; i < forDay; i++)
        {
            resultPrefabs[i] = Instantiate(resultPrefab, resultPanel.transform.GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(0).transform);

            resultPrefabs[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Day " + (startDay + i + 1);
            
            // 자가진단
            resultPrefabs[i].transform.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = (float)DataBase.instance.selfCheckScores.checkScore[i] / 7;
            resultPrefabs[i].transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = 
                DataBase.instance.selfCheckScores.checkScore[i] + "/7";

            // 중립 자세
            if (DataBase.instance.selfCheckScores.poseScore.Count == 0)
            {
                resultPrefabs[i].transform.GetChild(2).GetChild(1).GetComponent<Image>().fillAmount = 0;
                resultPrefabs[i].transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "0%";
            }
            else
            {
                resultPrefabs[i].transform.GetChild(2).GetChild(1).GetComponent<Image>().fillAmount = ((float)DataBase.instance.selfCheckScores.poseScore[i] / 6);
                resultPrefabs[i].transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = 
                    (((float)DataBase.instance.selfCheckScores.poseScore[i] / 6) * 100).ToString("F1") + "%";
            }

            // 운동 점수
            if(DataBase.instance.selfCheckScores.exCur.Count == 0 || DataBase.instance.selfCheckScores.exMax.Count == 0)
            {

                resultPrefabs[i].transform.GetChild(2).GetChild(2).GetComponent<Image>().fillAmount = 0;
                resultPrefabs[i].transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "0%";
            }
            else
            {

                resultPrefabs[i].transform.GetChild(2).GetChild(2).GetComponent<Image>().fillAmount =
                    ((float)DataBase.instance.selfCheckScores.exCur[i] / DataBase.instance.selfCheckScores.exMax[i]);
                resultPrefabs[i].transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    (((float)DataBase.instance.selfCheckScores.exCur[i] / DataBase.instance.selfCheckScores.exMax[i]) * 100).ToString("F1") + "%";
            }
        }
    }

    public void CloseResult(GameObject gameObject)
    {
        for (int i = 0; i < resultPrefabs.Length; i++)
        {
            Destroy(resultPrefabs[i]);
        }

        gameObject.SetActive(false);
    }

    public void OpenResult()
    {
        SetResult();

        resultPanel.SetActive(true);
    }
    #endregion

    #region 게임 방법 패널
    public void OpenHowToPlayPanel()
    {
        OpenChapterSelect();
        gamePlayPanel.SetActive(true);
    }

    // 챕터 선택 열기
    private void OpenChapterSelect()
    {
        ExplainBtn(0);
        
        for (int i = 0; i < explainPanel.Length; i++)
        {
            if (i == panelSelect)
            {
                explainPanel[i].SetActive(true);
            }
            else
            {
                explainPanel[i].SetActive(false);
            }
        }
    }

    public void ExplainBtn(int index)
    {
        panelSelect = index;

        if (index == 0)
        {
            prevBtn.SetActive(false);
            nextBtn.SetActive(true);
        }
        else if (index == 9)
        {
            prevBtn.SetActive(true);
            nextBtn.SetActive(false);
        }
        else
        {
            prevBtn.SetActive(true);
            nextBtn.SetActive(true);
        }

        explainPanel[index].SetActive(true);
    }

    // 챕터 선택 버튼
    public void OnClickChapterSelect(int index)
    {
        switch (index)
        {
            // PrevBtn
            case 0:
                if (panelSelect <= 0)
                {
                    return;
                }
                else
                {
                    explainPanel[panelSelect].SetActive(false);
                    explainPanel[panelSelect - 1].SetActive(true);

                    panelSelect = panelSelect - 1;
                }

                break;

            // NextBtn
            case 1:
                if (panelSelect >= 9)
                {
                    return;
                }
                else
                {
                    explainPanel[panelSelect].SetActive(false);
                    explainPanel[panelSelect + 1].SetActive(true);

                    panelSelect = panelSelect + 1;
                }
                break;
        }

        if (panelSelect == 0)
        {
            prevBtn.SetActive(false);
            nextBtn.SetActive(true);
        }
        else if (panelSelect == 9)
        {
            prevBtn.SetActive(true);
            nextBtn.SetActive(false);
        }
        else
        {
            prevBtn.SetActive(true);
            nextBtn.SetActive(true);
        }
    }
    #endregion
}
