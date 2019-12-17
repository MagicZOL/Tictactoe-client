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

    public void OnclickSignUp()
    {
        SignUpPanelManager.Show();
        Hide();
    }
    public void OnclickSignIn()
    {
        //Validation
        if (PanelValidation() == false) return;

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
            Hide();
        }, () =>
        {
            //TODO : 로그인창 흔들기
        });
    }

    bool PanelValidation()
    {
        if (!IDValidation(usernameInputField.text))
        {
            usernameInputField.image.color = Color.red;
            return false;
        }
        if (!PWValidation(passwordInputField.text))
        {
            passwordInputField.image.color = Color.red;
            return false;
        }
        return true;
    }

    bool IDValidation(string id)
    {
        Regex regex = new Regex(@"[a-zA-Z0-9]");

        if (regex.IsMatch(id) && (id.Length > 8 && id.Length < 20))
        {
            return true;
        }
        return false;
    }

    bool PWValidation(string pw)
    {
        Regex regex = new Regex(@"[a-z]+[A-Z]+[0-9]+[~!@#$%^&*]");
        if (regex.IsMatch(pw) && (pw.Length > 8 && pw.Length < 20))
        {
            return true;
        }
        return false;
    }

    private void OnEnable()
    {
        InitInputField(usernameInputField);
        InitInputField(passwordInputField);

    }

    // InputField의 내용을 초기화
    public void InitInputField(InputField inputField)
    {
        inputField.text = "";
        inputField.image.color = Color.white;
    }
}
