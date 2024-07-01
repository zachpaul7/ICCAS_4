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
    public int chapterSelect;  // é�� ����
    public int stageSelect;  // �������� ����

    #region é��
    public GameObject[] chapterPanel;  // é�� �г�
    public GameObject[] startLockImg;  // é�� ���
    #endregion

    #region �������� 
    public GameObject[] stageSelectPanels;
    public GameObject[] stagePanels;

    private GameObject[][] sBtns;  // Stage DeActive ����, ��Ŭ���� ����
    private GameObject[][] saBtns;  // Stage Active ����
    private GameObject[][] scBtns;  // Stage DeActive ����, Ŭ���� ����
    private GameObject[][] slBtns;  // Stage Lock �̹���

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

    #region é�� ����
    private void Awake()
    {
        SetChapter();
        SetListener();
    }

    // é�� ����
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

    // ������ ����
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

    #region é�� ����

    // Exercise �ǿ��� ���� �ְ� �������� é�ͷ� �����ֱ�
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

    // é�� ��� ����
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

    // ���� é�ͷ� ���� ��ư�� �Ҵ�
    public void PrevOnClickChapterSelect(int index)
    {
        chapterPanel[index].SetActive(false);
        chapterPanel[index - 1].SetActive(true);
    }

    // ���� é�ͷ� ���� ��ư�� �Ҵ�
    public void NextOnClickChapterSelect(int index)
    {
        chapterPanel[index].SetActive(false);
        chapterPanel[index + 1].SetActive(true);
    }
    #endregion

    #region �������� ����

    // ���õ� é�ͷ� �������� Select
    public void SelectChapter(int index)
    {
        // Chapter�� ���� �޾Ƽ� Select �� �Ѱ���
        chapterSelect = index;

        // ChapterSelect�� DataBase�� �ִ� top���������� é�Ͱ� ������ Ȯ��
        if (chapterSelect == (DataBase.instance.playerData.topStage / 5))
        {
            // ������ Chapter �����ֱ�
            OpenSelectChapter(index);

            SelectStage(DataBase.instance.playerData.topStage % 5);

            // �����ִ� ������������ Lock �̹��� �����ֱ�
            UnlockStage(index);
        }
        else
        {
            // ������ Chapter �����ֱ�
            OpenSelectChapter(index);

            SelectStage(0);

            // �����ִ� ������������ Lock �̹��� �����ֱ�
            UnlockStage(index);
        }
    }

    // �������� �г� ����
    public void OpenSelectChapter(int index)
    {
        // ���� index ���� ���缭 Chapter����
        for (int i = 0; i < 5; i++)
        {
            if (i == index)
                stageSelectPanels[i].SetActive(true);
            else
                stageSelectPanels[i].SetActive(false);
        }
    }

    // �������� ����
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
                // Stage�� �������� ���� (DeActive), ��Ŭ���� ����
                sBtns[chapterSelect][stageNum].SetActive(true);
                saBtns[chapterSelect][stageNum].SetActive(false);
                scBtns[chapterSelect][stageNum].SetActive(false);
                break;
            case 1:
                // Stage�� ������ (Active) 
                sBtns[chapterSelect][stageNum].SetActive(false);
                saBtns[chapterSelect][stageNum].SetActive(true);
                scBtns[chapterSelect][stageNum].SetActive(false);
                break;
            case 2:
                // Stage�� �������� ���� (DeActive), Ŭ���� ����
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
                Debug.Log("1�� �");

                break;

            case 1:
                Debug.Log("2�� �");

                break;

            case 2:
                Debug.Log("3�� �");

                break;
        }
    }
    #endregion
}