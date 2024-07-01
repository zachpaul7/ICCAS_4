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

    #region ���� �ʿ��� ����
    public int barSelect;  // �Ʒ� �� ����
    public int chapterSelect;  // é�� ����
    public int stageSelect;  // �������� ����
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
