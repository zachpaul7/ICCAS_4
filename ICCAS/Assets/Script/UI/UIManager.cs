using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    private void Awake()
    {
        instance = this;
    }

    #region 스크립트
    public BottomBar bUI;
    public ExerciseUI exUI;
    public CharacterUI chUI;
    #endregion

    #region 재화 텍스트
    public TextMeshProUGUI[] goldText;
    public TextMeshProUGUI[] clLevel0;
    public TextMeshProUGUI[] clLevel1;
    public TextMeshProUGUI[] clLevel2;
    #endregion

    #region 각종 필요한 변수
    public int barSelect;  // 아래 탭 선택
    public int chapterSelect;  // 챕터 선택
    public int stageSelect;  // 스테이지 선택
    #endregion

    #region
    #endregion

    public void OpenPanel(GameObject gameObjs)
    {
        gameObjs.SetActive(true);
    }

    public void ClosePanel(GameObject gameObjs)
    {
        gameObjs.SetActive(false);
    }
}
