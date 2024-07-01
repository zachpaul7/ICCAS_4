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

    #region ��ũ��Ʈ
    public BottomBar bUI;
    public ExerciseUI exUI;
    public CharacterUI chUI;
    #endregion

    #region ��ȭ �ؽ�Ʈ
    public TextMeshProUGUI[] goldText;
    public TextMeshProUGUI[] clLevel0;
    public TextMeshProUGUI[] clLevel1;
    public TextMeshProUGUI[] clLevel2;
    #endregion

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
