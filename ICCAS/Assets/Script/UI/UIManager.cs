using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public BottomBar bB;
    public ExerciseUI exUI;

    private void Awake()
    {
        instance = this;
    }

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
