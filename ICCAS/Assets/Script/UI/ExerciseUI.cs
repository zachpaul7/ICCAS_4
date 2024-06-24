using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseUI : MonoBehaviour
{
    #region 챕터
    public GameObject[] chapterPanel;  // 챕터 패널
    public GameObject[] startLockImg;
    #endregion

    #region 스테이지 
    public GameObject[] stageSelectPanels;
    public GameObject[] stagePanels;
   
    public GameObject[][] sBtns;
    public GameObject[][] saBtns;
    public GameObject[][] scBtns;

    private TextMeshProUGUI[] stageText;
    private TextMeshProUGUI[] expText;
    private TextMeshProUGUI[] goldText;
    
    public Button[] ecBtns;
    #endregion

    #region Exercise
    public GameObject exercisePanel;
    public Toggle[] exerciseToggle;
    public bool[] exerciseSelect;
    #endregion


    #region 챕터 설정
    private void Awake()
    {
        SetChapter();
        SetListener();
    }

    public void SetChapter()
    {
        stageText = new TextMeshProUGUI[stagePanels.Length];
        expText = new TextMeshProUGUI[stagePanels.Length];
        goldText = new TextMeshProUGUI[stagePanels.Length];
        sBtns = new GameObject[stagePanels.Length][];
        saBtns = new GameObject[stagePanels.Length][];
        scBtns = new GameObject[stagePanels.Length][];
        ecBtns = new Button[stagePanels.Length];

        for(int i = 0; i < stagePanels.Length; i++)
        {
            stageText[i] = stagePanels[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            expText[i] = stagePanels[i].transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            goldText[i] = stagePanels[i].transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            sBtns[i] = new GameObject[5];
            saBtns[i] = new GameObject[5];
            scBtns[i] = new GameObject[5];
            ecBtns[i] = stagePanels[i].transform.GetChild(3).GetComponent<Button>();

            for(int j = 0; j < sBtns[i].Length; j++)
            {
                sBtns[i][j] = stageSelectPanels[i].transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(j).GetChild(1).GetChild(0).gameObject;
                saBtns[i][j] = stageSelectPanels[i].transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(j).GetChild(1).GetChild(1).gameObject;
                scBtns[i][j] = stageSelectPanels[i].transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(j).GetChild(1).GetChild(2).gameObject;
            }
        }
    }
    public void SetListener()
    {
        for(int i = 0; i < stageSelectPanels.Length; i++)
        {
            for(int j = 0; j < sBtns[i].Length; j++)
            {
                int temp = j;
                sBtns[i][j].GetComponent<Button>().onClick.AddListener(() => SelectStage(temp));
            }
        }

        for(int i = 0; i < ecBtns.Length; i++)
        {
            ecBtns[i].onClick.AddListener(OpenExercise);
        }
    }
    #endregion

    #region 챕터 선택
    // 탭에서 Exercise 선택시 현재 최고 스트이지 챕터로 보여주기
    public void OpenExercisePanel(int index)
    {
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
    public void SelectChapter(int index)
    {
        UIManager.instance.chapterSelect = index;

        OpenSelecStage(index);
        SelectStage(0);
    }

    public void OpenSelecStage(int index)
    {
        // 현재 최고 스테이지 저장후 이전 스테이지들은 clear 로 표시 다음 스테이지는 잠금 표시 하게 만들어야함

        for(int i = 0; i < 5; i++)
        {
            if (i == index)
                stageSelectPanels[i].SetActive(true);
            else
                stageSelectPanels[i].SetActive(false);
        }
    }

    public void SelectStage(int index)
    {
        for (int i = 0; i < 5; i++)
        {
            if (i == index)
            {
                sBtns[UIManager.instance.chapterSelect][i].SetActive(false);
                saBtns[UIManager.instance.chapterSelect][i].SetActive(true);
            }
            else
            {
                sBtns[UIManager.instance.chapterSelect][i].SetActive(true);
                saBtns[UIManager.instance.chapterSelect][i].SetActive(false);
            }
        }

        SetStageText(UIManager.instance.chapterSelect, index);
    }

    private void SetStageText(int chapterNum, int stageNum)
    {
        if(stageNum == 4)
            stageText[chapterNum].text = "BOSS ";
        else
            stageText[chapterNum].text = "STAGE " + (stageNum + 1);
    }
    #endregion

    #region Exercise
    public void OpenExercise()
    {
        exercisePanel.SetActive(true);
    }

    public void OnRaid(int index)
    {
        for(int i = 0; i < exerciseToggle.Length; i++)
        {
            if(i == index)
            {
                if (exerciseToggle[i].isOn)
                {
                    exerciseSelect[i] = true;
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

    }
    #endregion
}
