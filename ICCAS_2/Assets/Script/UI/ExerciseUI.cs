using Google.Protobuf.WellKnownTypes;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseUI : MonoBehaviour
{
    [Header("챕터, 스테이지 선택")]
    public int chapterSelect;  // 챕터 선택
    public int stageSelect;  // 스테이지 선택

    #region 챕터
    [Header("챕터 관련")]
    public GameObject[] chapterPanel;  // 챕터 패널
    public GameObject[] startLockImg;  // 챕터 잠금
    #endregion

    #region 스테이지 
    [Header("스테이지 관련")]
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
    [Header("Exercise 관련")]
    public int exerciseNum;  // 어떤 운동을 선택했는지 확인

    public TextMeshProUGUI[] stageTexts;
    public GameObject exercisePanel;
    public GameObject exerciseSelectPanel;
    public GameObject[] exerciseExplainPanel;
    public GameObject exercisePoseObj;

    [Header("캐릭터 및 적 채력바")]
    public Image characterBar;
    public Image enemyBar;
    public TextMeshProUGUI characterBarText;
    public TextMeshProUGUI enemyBarText;

    [Header("스테이지 클리어 / 실패")]
    public GameObject stageClearSuccessPanel;
    public GameObject stageClearFailedPanel;

    // 스테이지 보상 텍스트
    public bool isDead = false;
    public TextMeshProUGUI[] stageExpReward;
    public TextMeshProUGUI[] stageGoldReward;

    public TextMeshProUGUI[] stageClearReward;
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
        stageSelect = (chapterSelect * 5) + (stageNum);

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

        Debug.Log("chapterSelect = " + chapterSelect + " stageSelect = " + stageSelect);
        SetStageText(chapterSelect, stageNum);
        StageRewardText(chapterSelect, stageSelect);
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

    // 각 스테이지별 보상 텍스트 세팅
    public void StageRewardText(int chapterNum, int stageNum)
    {
        stageExpReward[chapterNum].text = DataBase.instance.playerInfo.rewardExp[stageNum].ToString();
        stageGoldReward[chapterNum].text = DataBase.instance.enemyInfos[stageNum].gold.ToString();
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

    #region 운동 준비 - 패널 세팅
    // tmxpdlwl
    public void StageClearRewardText(int stageNum)
    {
        stageClearReward[0].text = DataBase.instance.playerInfo.rewardExp[stageNum].ToString();
        stageClearReward[1].text = DataBase.instance.enemyInfos[stageNum].gold.ToString();
    }

    public void OpenExercisPanel()
    {
        for (int i = 0; i < GameManager.instance.pc.Length; i++)
        {
            if (i == DataBase.instance.playerData.cSelect)
            {
                GameManager.instance.pc[i].SetActive(true);
                GameManager.instance.pc[i].GetComponent<PlayerController>().SetStat(i);
            }
            else
            {
                GameManager.instance.pc[i].SetActive(false);
            }
        }

        for (int i = 0; i < GameManager.instance.ec.Length; i++)
        {
            if (i == stageSelect)
            {
                GameManager.instance.ec[i].SetActive(true);
                GameManager.instance.ec[i].GetComponent<EnemyController>().SetStat(i);
            }
            else
            {
                GameManager.instance.ec[i].SetActive(false);
            }
        }

        UpdateHpBar();
    }


    public void OpenExercise()
    {
        stageTexts[0].text = (chapterSelect + 1) + " - " + ((stageSelect % 5) + 1);

        OpenExercisPanel();

        exercisePanel.SetActive(true);
        exerciseSelectPanel.SetActive(true);
    }

    public void OpenExerciseSelect()
    {
        StartCoroutine(WaitOpenExerciseSelect());
    }

    IEnumerator WaitOpenExerciseSelect()
    {
        yield return YieldCache.WaitForSeconds(5f);
        VNectModel.instance.parentTransform.SetActive(false);
        exercisePoseObj.SetActive(false);
        
        yield return YieldCache.WaitForSeconds(0.5f);
        exerciseSelectPanel.SetActive(true);
    }

    public void OnClickExercise(int index)
    {
        exerciseNum = index;

        exerciseSelectPanel.SetActive(false);
        exerciseExplainPanel[index].SetActive(true);
    }
    #endregion

    #region 운동 플레이 - 공격, HP세팅

    // AI 넣기전 테스트
    public void SelectExercise(int index)
    {
        StartCoroutine(WaitDelay(index));
    }
    
    IEnumerator WaitDelay(int index)
    {
        exerciseExplainPanel[exerciseNum].SetActive(false);

        exercisePoseObj.SetActive(true);

        yield return YieldCache.WaitForSeconds(3f);

        VNectModel.instance.parentTransform.SetActive(true);
        VNectModel.instance.GetStretchingSelect(index);
    }

    public void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }

    // 공격 로직
    IEnumerator AttackCoroutine()
    {
        yield return YieldCache.WaitForSeconds(2);

        GameManager.instance.pc[DataBase.instance.playerData.cSelect].GetComponent<PlayerController>().Attack(stageSelect);
        UpdateHpBar();

        if (GameManager.instance.ec[stageSelect].GetComponent<EnemyController>().isDead == true)
        {
            yield return YieldCache.WaitForSeconds(0.4f);

            GameManager.instance.ec[stageSelect].GetComponent<EnemyController>().Dead();
        }
        else
        {
            yield return YieldCache.WaitForSeconds(2);

            GameManager.instance.ec[stageSelect].GetComponent<EnemyController>().Attack(DataBase.instance.playerData.cSelect);
            UpdateHpBar();

            if (GameManager.instance.pc[DataBase.instance.playerData.cSelect].GetComponent<PlayerController>().isDead == true)
            {
                yield return YieldCache.WaitForSeconds(0.4f);

                GameManager.instance.pc[DataBase.instance.playerData.cSelect].GetComponent<PlayerController>().Dead();
            }
        }
    }

    // HP 텍스트, Bar 업데이트
    public void UpdateHpBar()
    {
        SetHpBar(0, DataBase.instance.playerData.cSelect);
        SetHpBar(1, stageSelect);
    }

    public void SetHpBar(int index, int setNum)
    {
        switch (index)
        {
            case 0:
                float cCurHp = (float)GameManager.instance.pc[setNum].GetComponent<PlayerController>().curHp;
                int cMaxHp = GameManager.instance.pc[setNum].GetComponent<PlayerController>().maxHp;

                if (cCurHp < 0)
                {
                    characterBar.fillAmount = 0;
                    characterBarText.text = 0 + " / " + cMaxHp;
                }
                else
                {
                    characterBar.fillAmount = cCurHp / cMaxHp;
                    characterBarText.text = cCurHp + " / " + cMaxHp;
                }
                break;

            case 1:
                float eCurHp = (float)GameManager.instance.ec[setNum].GetComponent<EnemyController>().curHp;
                int eMaxHp = GameManager.instance.ec[setNum].GetComponent<EnemyController>().maxHp;

                if (eCurHp < 0)
                {
                    enemyBar.fillAmount = 0;
                    enemyBarText.text = 0 + " / " + eMaxHp;
                }
                else
                {
                    enemyBar.fillAmount = eCurHp / eMaxHp;
                    enemyBarText.text = eCurHp + " / " + eMaxHp;
                }
                break;
        }
    }
    #endregion

    #region 운동 종료 - 클리어 성공/실패
    

    public void StageClearSuccess()
    {
        // 패널 열기전에 보상 세팅하기
        StageClearRewardText(stageSelect);

        // 스테이지 클리어 패널 열기
        stageClearSuccessPanel.SetActive(true);

        // 나머지 다 끄기
        GameManager.instance.pc[DataBase.instance.playerData.cSelect].SetActive(false);
        GameManager.instance.ec[stageSelect].SetActive(false);
        exercisePanel.SetActive(false);


        // 만약 클리어한 스테이지가 최고 스테이지 였다면 최고 스테이지를 1 올리기
        if (stageSelect == DataBase.instance.playerData.topStage && stageSelect < 24)
        {
            DataBase.instance.playerData.topStage += 1;
        }
    }

    public void StageClearFailed()
    {
        // 스테이지 클리어 패널 열기
        stageClearFailedPanel.SetActive(true);

        // 나머지 다 끄기
        GameManager.instance.pc[DataBase.instance.playerData.cSelect].SetActive(false);
        GameManager.instance.ec[stageSelect].SetActive(false);
        exercisePanel.SetActive(false);
    }

    public void OnClickContinue(int index)
    {
        switch (index)
        {
            case 0:
                stageClearSuccessPanel.SetActive(false);
                StageRewardAccept(stageSelect);

                UnlockChpater();
                UnlockStage(DataBase.instance.playerData.topStage);
                SelectStage(DataBase.instance.playerData.topStage % 5);

                break;

            case 1:
                stageClearFailedPanel.SetActive(false);
                break;
        }
    }

    // 각 스테이지별 보상 수령
    public void StageRewardAccept(int stageNum)
    {
        DataBase.instance.AddPlayerLv(DataBase.instance.playerInfo.rewardExp[stageNum]);
        DataBase.instance.AddGold(DataBase.instance.enemyInfos[stageNum].gold);
    }
    #endregion
}