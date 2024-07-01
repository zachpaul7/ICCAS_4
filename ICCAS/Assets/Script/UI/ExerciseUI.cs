using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseUI : MonoBehaviour
{
    public int chapterSelect;  // 챕터 선택
    public int stageSelect;  // 스테이지 선택

    #region 챕터
    public GameObject[] chapterPanel;  // 챕터 패널
    public GameObject[] startLockImg;  // 챕터 잠금
    #endregion

    #region 스테이지 
    public GameObject[] stageSelectPanels;
    public GameObject[] stagePanels;

    private GameObject[][] sBtns;  // Stage DeActive 상태, 미클리어 상태
    private GameObject[][] saBtns;  // Stage Active 상태
    private GameObject[][] scBtns;  // Stage DeActive 상태, 클리어 상태
    private GameObject[][] slBtns;  // Stage Lock 이미지

    private TextMeshProUGUI[] stageText;
    private TextMeshProUGUI[] expText;
    private TextMeshProUGUI[] goldText;

    private Button[] ecBtns;
    #endregion

    #region Exercise
    public Toggle[] exerciseToggle;
    public bool[] exerciseSelect;
    public GameObject exercisePanel;
    private int exerciseNum;
    #endregion

    #region 챕터 설정
    private void Awake()
    {
        SetChapter();
        SetListener();
    }

    // 챕터 설정
    public void SetChapter()
    {
        stageText = new TextMeshProUGUI[stagePanels.Length];
        expText = new TextMeshProUGUI[stagePanels.Length];
        goldText = new TextMeshProUGUI[stagePanels.Length];
        sBtns = new GameObject[stagePanels.Length][];
        saBtns = new GameObject[stagePanels.Length][];
        scBtns = new GameObject[stagePanels.Length][];
        slBtns = new GameObject[stagePanels.Length][];
        ecBtns = new Button[stagePanels.Length];

        for (int i = 0; i < stagePanels.Length; i++)
        {
            stageText[i] = stagePanels[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            expText[i] = stagePanels[i].transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            goldText[i] = stagePanels[i].transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            sBtns[i] = new GameObject[5];
            saBtns[i] = new GameObject[5];
            scBtns[i] = new GameObject[5];
            slBtns[i] = new GameObject[5];
            ecBtns[i] = stagePanels[i].transform.GetChild(3).GetComponent<Button>();

            for (int j = 0; j < sBtns[i].Length; j++)
            {
                sBtns[i][j] = stageSelectPanels[i].transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(j).GetChild(1).GetChild(0).gameObject;
                saBtns[i][j] = stageSelectPanels[i].transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(j).GetChild(1).GetChild(1).gameObject;
                scBtns[i][j] = stageSelectPanels[i].transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(j).GetChild(1).GetChild(2).gameObject;
                slBtns[i][j] = stageSelectPanels[i].transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(j).GetChild(1).GetChild(3).gameObject;
            }
        }
    }

    // 리스너 설정
    public void SetListener()
    {
        for (int i = 0; i < stageSelectPanels.Length; i++)
        {
            for (int j = 0; j < sBtns[i].Length; j++)
            {
                int tempi = i;
                int tempj = j;
                sBtns[i][j].GetComponent<Button>().onClick.AddListener(() => SelectStage(tempj));
                saBtns[i][j].GetComponent<Button>().onClick.AddListener(() => SelectStage(tempj));
                scBtns[i][j].GetComponent<Button>().onClick.AddListener(() => SelectStage(tempj));
            }
        }

        for (int i = 0; i < ecBtns.Length; i++)
        {
            ecBtns[i].onClick.AddListener(OpenExercise);
        }
    }
    #endregion

    #region 챕터 선택

    // Exercise 탭에서 현재 최고 스테이지 챕터로 보여주기
    public void OpenExercisePanel(int index)
    {
        UnlockChpater();

        for (int i = 0; i < chapterPanel.Length; i++)
        {
            if (i == index)
            {
                chapterPanel[i].SetActive(true);
            }
            else
            {
                chapterPanel[i].SetActive(false);
            }
        }
    }

    // 챕터 잠금 해제
    public void UnlockChpater()
    {
        int curChapter = DataBase.instance.playerData.topStage / 5;

        for (int i = 0; i < chapterPanel.Length; i++)
        {
            if (i <= curChapter)
            {
                chapterPanel[i].transform.GetChild(6).gameObject.SetActive(false);
            }
            else
            {
                chapterPanel[i].transform.GetChild(6).gameObject.SetActive(true);

            }
        }
    }

    // 이전 챕터로 가기 버튼에 할당
    public void PrevOnClickChapterSelect(int index)
    {
        chapterPanel[index].SetActive(false);
        chapterPanel[index - 1].SetActive(true);
    }

    // 다음 챕터로 가기 버튼에 할당
    public void NextOnClickChapterSelect(int index)
    {
        chapterPanel[index].SetActive(false);
        chapterPanel[index + 1].SetActive(true);
    }
    #endregion

    #region 스테이지 선택

    // 선택된 챕터로 스테이지 Select
    public void SelectChapter(int index)
    {
        // Chapter를 직접 받아서 Select 에 넘겨줌
        chapterSelect = index;

        // ChapterSelect와 DataBase에 있는 top스테이지의 챕터가 같은지 확인
        if (chapterSelect == (DataBase.instance.playerData.topStage / 5))
        {
            // 선택한 Chapter 열어주기
            OpenSelectChapter(index);

            SelectStage(DataBase.instance.playerData.topStage % 5);

            // 열려있는 스테이지까지 Lock 이미지 열어주기
            UnlockStage(index);
        }
        else
        {
            // 선택한 Chapter 열어주기
            OpenSelectChapter(index);

            SelectStage(0);

            // 열려있는 스테이지까지 Lock 이미지 열어주기
            UnlockStage(index);
        }
    }

    // 스테이지 패널 열기
    public void OpenSelectChapter(int index)
    {
        // 들어온 index 값에 맞춰서 Chapter열기
        for (int i = 0; i < 5; i++)
        {
            if (i == index)
                stageSelectPanels[i].SetActive(true);
            else
                stageSelectPanels[i].SetActive(false);
        }
    }

    // 스테이지 선택
    public void SelectStage(int stageNum)
    {
        if (chapterSelect < (DataBase.instance.playerData.topStage / 5))
        {
            for (int i = 0; i < 5; i++)
            {
                if(i == stageNum)
                {
                    SelectStageIcon(1, i);
                }
                else
                {
                    SelectStageIcon(2, i);
                }
                
            }
        }
        else if(chapterSelect == (DataBase.instance.playerData.topStage / 5))
        {
            for (int i = 0; i < 5; i++)
            {
                if (i == stageNum)
                {
                    SelectStageIcon(1, i);
                }
                else
                {
                    int stage = (DataBase.instance.playerData.topStage % 5);
                    
                    if (i < stage)
                    {
                        SelectStageIcon(2, i);
                    }
                    else
                    {
                        SelectStageIcon(0, i);
                    }
                }
            }
        }

        SetStageText(chapterSelect, stageNum);
    }

    public void SelectStageIcon(int index, int stageNum)
    {
        switch (index)
        {
            case 0:
                // Stage를 선택하지 않음 (DeActive), 미클리어 상태
                sBtns[chapterSelect][stageNum].SetActive(true);
                saBtns[chapterSelect][stageNum].SetActive(false);
                scBtns[chapterSelect][stageNum].SetActive(false);
                break;
            case 1:
                // Stage를 선택함 (Active) 
                sBtns[chapterSelect][stageNum].SetActive(false);
                saBtns[chapterSelect][stageNum].SetActive(true);
                scBtns[chapterSelect][stageNum].SetActive(false);
                break;
            case 2:
                // Stage를 선택하지 않음 (DeActive), 클리어 상태
                sBtns[chapterSelect][stageNum].SetActive(false);
                saBtns[chapterSelect][stageNum].SetActive(false);
                scBtns[chapterSelect][stageNum].SetActive(true);
                break;
        }
    }

    private void SetStageText(int chapterNum, int stageNum)
    {
        if (stageNum == 4)
            stageText[chapterNum].text = "BOSS ";
        else
            stageText[chapterNum].text = "STAGE " + (stageNum + 1);
    }

    private void UnlockStage(int index)
    {
        int chapterNum = DataBase.instance.playerData.topStage / 5;
        int stageNum = DataBase.instance.playerData.topStage % 5;

        if(index < chapterNum)
        {
            for (int i = 0; i < 5; i++)
            {
                slBtns[index][i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                if(i <= stageNum) 
                {
                    slBtns[chapterNum][i].SetActive(false);
                }
                else
                {
                    slBtns[chapterNum][i].SetActive(true);
                }
            }
        }

        
    }
    #endregion

    #region Exercise
    public void OpenExercise()
    {
        exercisePanel.SetActive(true);
    }

    public void OnRaid(int index)
    {
        for (int i = 0; i < exerciseToggle.Length; i++)
        {
            if (i == index)
            {
                if (exerciseToggle[i].isOn)
                {
                    exerciseSelect[i] = true;
                    exerciseNum = i;
                }
                else
                {
                    exerciseSelect[i] = false;
                }
            }
            else
            {
                exerciseSelect[i] = false;
            }
        }
    }

    public void SelectExercise()
    {
        switch (exerciseNum)
        {
            case 0:
                Debug.Log("1번 운동");

                break;

            case 1:
                Debug.Log("2번 운동");

                break;

            case 2:
                Debug.Log("3번 운동");

                break;
        }
    }
    #endregion
}