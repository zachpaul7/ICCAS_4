using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject[] loginBtns;

    private void Start()
    {
        SoundManager.instance.PlayBGM("Main");
    }

    #region UI
    public void OpenPanel(GameObject gameObjs)
    {
        SoundManager.instance.PlaySFX("ClickBtn");
        gameObjs.SetActive(true);
    }

    public void ClosePanel(GameObject gameObjs)
    {
        SoundManager.instance.PlaySFX("CancelBtn");
        gameObjs.SetActive(false);
    }

    public void TabtoContinue()
    {
        if (PlayFabLogin.instance.isSetName == true)
        {
            DataBase.instance.playerData.nickName = PlayFabLogin.instance.username;
        }

        DataBase.instance.SaveData();

        Debug.Log("게임으로 넘어갑니다.");

        SoundManager.instance.StopBGM();
        SceneManager.LoadScene(1);
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
