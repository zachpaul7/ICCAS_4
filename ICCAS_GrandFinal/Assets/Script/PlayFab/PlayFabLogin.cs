using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayFabLogin : MonoBehaviour
{
    public static PlayFabLogin instance;

    public TMP_InputField loginEmail, loginPassword;
    public TMP_InputField registerEmail, registerPassword, registerName;
    //public string emailInput, passwardInput, nameInput;
    public string myID;
    public string username;
    public LoginUI loginUI;
    public bool isSetName = false;
    public bool isLogin = false;

    private void Awake()
    {
        // ½Ì±ÛÅæ ÀÎ½ºÅÏ½º ÃÊ±âÈ­
        instance = this;

        //EmailLogin();
    }

    private void DisplayPlayfabError(PlayFabError error) => Debug.LogError("error : " + error.GenerateErrorReport());

    #region ·Î±×ÀÎ & È¸¿ø°¡ÀÔ
    public void EmailLogin()
    {
        //InputField·Î ¹Þ¾Æ¿Ã¶§ »ç¿ë
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "AC580";
        }
        var request = new LoginWithEmailAddressRequest { Email = loginEmail.text, Password = loginPassword.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);

        //Å×½ºÆ®¿ë 
        //var request = new LoginWithEmailAddressRequest { Email = emailInput, Password = passwardInput };
        //PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, (error) => { print("·Î±×ÀÎ ½ÇÆÐ"); EmailRegister(); });

    }

    public void EmailRegister()
    {
        //InputField·Î ¹Þ¾Æ¿Ã¶§ »ç¿ë
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "AC580";
        }
        var request = new RegisterPlayFabUserRequest { Email = registerEmail.text, Password = registerPassword.text, Username = registerName.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);

        //Å×½ºÆ®¿ë 
        //var request = new RegisterPlayFabUserRequest { Email = emailInput, Password = passwardInput, Username = nameInput };
        //PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { print("È¸¿ø°¡ÀÔ ¼º°ø"); EmailLogin(); username = result.Username; isSetName = true; }, (error) => { print("È¸¿ø°¡ÀÔ ½ÇÆÐ");});

    }

    private void OnLoginSuccess(LoginResult result)
    {
        // ID ÀúÀå
        myID = result.PlayFabId;

        // DataBase¿¡ ÇÃ·¹ÀÌ¾î Á¤º¸ °¡Á®¿À±â
        DataBase.instance.GetUserData();

        Debug.Log("로그인 성공");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        loginUI.LoginPanel(0);
        Debug.Log("로그인 실패");
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("회원가입 성공");
        username = result.Username;

        isSetName = true;
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        loginUI.SignUpPanel(0);
        Debug.Log("회원가입 실패");
    }
    #endregion
}
