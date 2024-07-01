using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    public static PlayFabLogin instance;

    //public TMP_InputField loginEmail, loginPassword;
    //public TMP_InputField registerEmail, registerPassword, registerName;
    public string emailInput, passwardInput, nameInput;
    public string myID;
    public LoginUI loginUI;

    private void Awake()
    {
        // �̱��� �ν��Ͻ� �ʱ�ȭ
        instance = this;

        // �׽�Ʈ�� �α���/ȸ������
        EmailLogin();
    }

    public void EmailLogin()
    {
        //InputField�� �޾ƿö� ���
        //if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        //{
        //    PlayFabSettings.staticSettings.TitleId = "AC580";
        //}
        //var request = new LoginWithEmailAddressRequest { Email = loginEmail.text, Password = loginPassword.text };
        //PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);

        //�׽�Ʈ�� 
        var request = new LoginWithEmailAddressRequest { Email = emailInput, Password = passwardInput };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, (error) => { print("�α��� ����"); EmailRegister(); });

    }

    public void EmailRegister()
    {
        //InputField�� �޾ƿö� ���
        //if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        //{
        //    PlayFabSettings.staticSettings.TitleId = "AC580";
        //}
        //var request = new RegisterPlayFabUserRequest { Email = registerEmail.text, Password = registerPassword.text, Username = registerName.text };
        //PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);

        //�׽�Ʈ�� 
        var request = new RegisterPlayFabUserRequest { Email = emailInput, Password = passwardInput, Username = nameInput };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { print("ȸ������ ����"); EmailLogin(); }, (error) => { print("ȸ������ ����"); });

    }

    private void OnLoginSuccess(LoginResult result)
    {
        // ID ����
        myID = result.PlayFabId; 

        // DataBase�� �÷��̾� ���� ��������
        DataBase.instance.GetUserData();

        Debug.Log("�α��� ����");

        StartCoroutine(InitInfo());
    }

    private void OnLoginFailure(PlayFabError error)
    {

        loginUI.LoginPanel(0);
        Debug.Log("�α��� ����");
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("ȸ������ ����");
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        loginUI.SignUpPanel(0);
        Debug.Log("ȸ������ ����");
    }


    IEnumerator InitInfo()
    {
        yield return YieldCache.WaitForSeconds(0.7f);

        DataBase.instance.AddGold(0);
    }
}
