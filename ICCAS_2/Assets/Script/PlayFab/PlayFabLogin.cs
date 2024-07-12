using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayFabLogin : MonoBehaviour
{
    public static PlayFabLogin instance;

    //public TMP_InputField loginEmail, loginPassword;
    //public TMP_InputField registerEmail, registerPassword, registerName;
    public string emailInput, passwardInput, nameInput;
    public string myID;
    public string username;
    //public LoginUI loginUI;
    public bool isSetName = false;
    public bool isLogin = false;

    private void Awake()
    {
        // 싱글톤 인스턴스 초기화
        instance = this;

        EmailLogin();
    }

    private void DisplayPlayfabError(PlayFabError error) => Debug.LogError("error : " + error.GenerateErrorReport());

    #region 로그인 & 회원가입
    public void EmailLogin()
    {
        //InputField로 받아올때 사용
        //if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        //{
        //    PlayFabSettings.staticSettings.TitleId = "AC580";
        //}
        //var request = new LoginWithEmailAddressRequest { Email = loginEmail.text, Password = loginPassword.text };
        //PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);

        //테스트용 
        var request = new LoginWithEmailAddressRequest { Email = emailInput, Password = passwardInput };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, (error) => { print("로그인 실패"); EmailRegister(); });

    }

    public void EmailRegister()
    {
        //InputField로 받아올때 사용
        //if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        //{
        //    PlayFabSettings.staticSettings.TitleId = "AC580";
        //}
        //var request = new RegisterPlayFabUserRequest { Email = registerEmail.text, Password = registerPassword.text, Username = registerName.text };
        //PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);

        //테스트용 
        var request = new RegisterPlayFabUserRequest { Email = emailInput, Password = passwardInput, Username = nameInput };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { print("회원가입 성공"); EmailLogin(); username = result.Username; isSetName = true; }, (error) => { print("회원가입 실패");});

    }

    private void OnLoginSuccess(LoginResult result)
    {
        // ID 저장
        myID = result.PlayFabId;

        // DataBase에 플레이어 정보 가져오기
        DataBase.instance.GetUserData();

        Debug.Log("로그인 성공");
    }

    private void OnLoginFailure(PlayFabError error)
    {

        //loginUI.LoginPanel(0);
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
        //loginUI.SignUpPanel(0);
        Debug.Log("회원가입 실패");
    }
    #endregion
}
