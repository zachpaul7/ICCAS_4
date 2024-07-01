using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject[] loginBtns;

    #region UI
    public void OpenPanel(GameObject gameObjs)
    {
        gameObjs.SetActive(true);
    }

    public void ClosePanel(GameObject gameObjs)
    {
        gameObjs.SetActive(false);
    }

    public void TabtoContinue()
    {
        //SceneManager�� �߰��ϱ�
        Debug.Log("�������� �Ѿ�ϴ�.");

        DataBase.instance.SaveJsonToPlayfab();
    }
    #endregion

    #region
    public void LoginPanel(int index)
    {
        switch (index)
        {
            case 0:
                panels[0].SetActive(true);
                panels[1].SetActive(false);
                break;
            case 1:
                PlayFabLogin.instance.EmailLogin();
                loginBtns[0].SetActive(false);
                loginBtns[1].SetActive(true);
                panels[0].SetActive(false);
                break;
        }
    }
    public void SignUpPanel(int index)
    {
        switch (index)
        {
            case 0:
                panels[0].SetActive(false);
                panels[1].SetActive(true);
                break;
            case 1:
                PlayFabLogin.instance.EmailRegister();
                loginBtns[0].SetActive(true);
                loginBtns[1].SetActive(false);
                panels[1].SetActive(false);
                break;
        }
    }
    #endregion
}
