using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
public class SignInPanelManager : PanelManager
{
    [SerializeField] SignUpPanelManager SignUpPanelManager;
    [SerializeField] InputField usernameInputField;
    [SerializeField] InputField passwordInputField;

    [SerializeField] Button signInButton;
    [SerializeField] Button signUpButton;
    byte validationFlag = 0;

    public override void Show()
    {
        base.Show();
        signInButton.interactable = false;
    }
    public void OnclickSignUp()
    {
        SignUpPanelManager.Show();
        Hide();
    }
    public void OnclickSignIn()
    {
        signInButton.interactable = false;
        signUpButton.interactable = false;

        //로그인
        HTTPNetworkManager.Instance.SIgnIn(usernameInputField.text, passwordInputField.text, (response) =>
        {
            //쿠키값이 있으면 세션ID저장
            if(response.Headers.ContainsKey("Set-Cookie"))
            {
                //signIn성공시 쿠키값 전달
                string cookie = response.Headers["Set-Cookie"];

                int firstIndex = cookie.IndexOf('=') + 1;
                int lastIndex = cookie.IndexOf(';');

                string cookieValue = cookie.Substring(firstIndex, lastIndex - firstIndex);

                PlayerPrefs.SetString("sid", cookieValue);
            }

            //유저의 점수 표시
            //GameManager에게 GetInfo()를 호출하면서 유저이름과 스코어를 표시하는 방법이 있으나 이미 통신했는데 또 통신하는 비효율적인 상황이 생긴다.
            HTTPResponseInfo info = response.GetDataFromMessage<HTTPResponseInfo>();
            MainManager.Instance.SetInfo(info.name, info.score);
            
            //로그인창 닫기
            Hide();
        }, () =>
        {
            //TODO : 로그인창 흔들기
            signInButton.interactable = true;
            signUpButton.interactable = true;
        });
    }
    void OnvalueChangeFinalCheck()
    {
        if (validationFlag == 3)    
        {
            signInButton.interactable = true;
        }
        else
        {
            signInButton.interactable = false;
        }
    }

    public void OnValueChangeUsername(InputField inputField)
    {
        string pattern = @"^[a-zA-Z0-9]{4,12}$";

        //유효성 검사 성공시 1로 or 연산하여 값이 있다는 1을 넣어줌
        if (Regex.IsMatch(inputField.text, pattern))
        {
            validationFlag = (byte)(validationFlag | 1);
        }
        //유효성 검사 실패시 1110으로 검사한 해당 비트만 0으로 만들어줌 
        else
        {
            validationFlag = (byte)(validationFlag & ~1); // ~ : 비트 반전
        }
        OnvalueChangeFinalCheck();
    }

    public void OnValueChangePassword(InputField inputField)
    {
        string pattern = @"^[a-zA-Z0-9]{4,12}$";

        if (Regex.IsMatch(inputField.text, pattern))
        {
            validationFlag = (byte)(validationFlag | 1 << 1);
        }
        //1101
        else
        {
            validationFlag = (byte)(validationFlag & ~(1 << 1));
        }
        OnvalueChangeFinalCheck();
    }
}
