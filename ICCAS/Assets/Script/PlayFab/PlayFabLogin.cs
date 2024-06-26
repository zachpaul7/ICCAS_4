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

    //public TMP_InputField emailInput, passwardInput, nameInput;
    public string emailInput, passwardInput, nameInput;
    public string myID;

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);

        // 테스트용 로그인/회원가입
        EmailLogin();
        
        
    }

    public void EmailLogin()
    {
        //InputField로 받아올때 사용
        //var request = new LoginWithEmailAddressRequest { Email = emailInput.text, Password = passwardInput.text };
        //PlayFabClientAPI.LoginWithEmailAddress(request, (result) => { print("로그인 성공"); myID = result.PlayFabId; }, (error) => print("로그인 실패"));

        //테스트용 
        var request = new LoginWithEmailAddressRequest { Email = emailInput, Password = passwardInput };
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => { print("로그인 성공"); myID = result.PlayFabId; DataBase.instance.GetUserData(); }, (error) => { print("로그인 실패"); EmailRegister(); });
    }

    public void EmailRegister()
    {
        //InputField로 받아올때 사용
        //var request = new RegisterPlayFabUserRequest { Email = emailInput.text, Password = passwardInput.text, Username = nameInput.text };
        //PlayFabClientAPI.RegisterPlayFabUser(request, (result) => print("회원가입 성공"), (error) => print("회원가입 실패"));

        //테스트용 
        var request = new RegisterPlayFabUserRequest { Email = emailInput, Password = passwardInput, Username = nameInput };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { print("회원가입 성공"); EmailLogin(); }, (error) => print("회원가입 실패"));
    }
}
