using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomBar : MonoBehaviour
{
    #region 기본 패널 관련
    public GameObject[] panels;
    #endregion

    #region BottomBar 관련
    public GameObject[] bBtns;
    public GameObject[] bfBtns;
    #endregion

    void Start()
    {
        SwitchTab(2);

        // 로그인 씬 나누고 주석 풀기
        //DataBase.instance.AddGold(0);
    }

    public void SwitchTab(int index)
    {
        UIManager.instance.barSelect = index;

        for (int i = 0; i < bBtns.Length; i++)
        {
            if (i == index)
            {
                // 선택된 탭 포커스 버튼 활성화
                bfBtns[i].SetActive(true);

                // 선택된 탭 기본 버튼 비활성화
                bBtns[i].SetActive(false);

                // 선택된 탭의 패널 활성화
                panels[i].SetActive(true);

                if(i == 1)
                    UIManager.instance.exUI.OpenExercisePanel(0);
                if (i == 4)
                    UIManager.instance.chUI.OpenCharacterSelect();
            }
            else
            {
                // 선택되지 않은 탭 포커스 버튼 비활성화
                bfBtns[i].SetActive(false);

                // 선택되지 않은 탭 기본 버튼 활성화
                bBtns[i].SetActive(true);

                // 선택되지 않은 탭의 패널 비활성화
                panels[i].SetActive(false);
            }
        }


    }


}
