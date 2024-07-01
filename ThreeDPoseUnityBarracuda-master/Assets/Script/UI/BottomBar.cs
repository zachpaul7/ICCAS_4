using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomBar : MonoBehaviour
{
    #region �⺻ �г� ����
    public GameObject[] panels;
    #endregion

    #region BottomBar ����
    public GameObject[] bBtns;
    public GameObject[] bfBtns;
    #endregion

    void Start()
    {
        SwitchTab(2);

        // �α��� �� ������ �ּ� Ǯ��
        //DataBase.instance.AddGold(0);
    }

    public void SwitchTab(int index)
    {
        UIManager.instance.barSelect = index;

        for (int i = 0; i < bBtns.Length; i++)
        {
            if (i == index)
            {
                // ���õ� �� ��Ŀ�� ��ư Ȱ��ȭ
                bfBtns[i].SetActive(true);

                // ���õ� �� �⺻ ��ư ��Ȱ��ȭ
                bBtns[i].SetActive(false);

                // ���õ� ���� �г� Ȱ��ȭ
                panels[i].SetActive(true);

                if(i == 1)
                    UIManager.instance.exUI.OpenExercisePanel(0);
                if (i == 4)
                    UIManager.instance.chUI.OpenCharacterSelect();
            }
            else
            {
                // ���õ��� ���� �� ��Ŀ�� ��ư ��Ȱ��ȭ
                bfBtns[i].SetActive(false);

                // ���õ��� ���� �� �⺻ ��ư Ȱ��ȭ
                bBtns[i].SetActive(true);

                // ���õ��� ���� ���� �г� ��Ȱ��ȭ
                panels[i].SetActive(false);
            }
        }


    }


}
