using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    public GameObject[] panels;

    public void LoginPanel(int index)
    {
        switch (index)
        {
            case 0:
                panels[0].SetActive(true);
                panels[1].SetActive(false);
                break;
            case 1:
                
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
                
                break;
        }
    }
}
