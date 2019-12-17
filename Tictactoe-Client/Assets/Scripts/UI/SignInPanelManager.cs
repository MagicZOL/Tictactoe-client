using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SignInPanelManager : PanelManager
{
    [SerializeField] SignUpPanelManager SignUpPanelManager;
    [SerializeField] InputField usernameInputField;
    [SerializeField] InputField passwordInputField;

    public void OnclickSignUp()
    {
        SignUpPanelManager.Show();
    }
    public void OnclickSignIn()
    {
        //로그인
        HTTPNetworkManager.Instance.SIgnIn(usernameInputField.text, passwordInputField.text, (response) =>
        {
            //쿠키값이 있으면
            if(response.Headers.ContainsKey("Set-Cookie"))
            {
                //signIn성공시 쿠키값 전달
                string cookie = response.Headers["Set-Cookie"];

                int firstIndex = cookie.IndexOf('=') + 1;
                int lastIndex = cookie.IndexOf(';');

                string cookieValue = cookie.Substring(firstIndex, lastIndex - firstIndex);

                PlayerPrefs.SetString("sid", cookieValue);
            }
        }, () =>
        {
            //TODO : 로그인창 흔들기
        });

        Hide();
    }

}
