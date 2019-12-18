using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SignUpPanelManager : PanelManager
{
    [SerializeField] InputField usernameInputField;
    [SerializeField] InputField firstPasswordInputField;
    [SerializeField] InputField secondPasswordInputField;
    [SerializeField] InputField nameInputField;
    
    [SerializeField] SignInPanelManager signInPanelManager;

    [SerializeField] Button signUpButton;

    byte validationFlag = 0;
    public override void Show()
    {
        base.Show();
        signUpButton.interactable = false;
    }

    void SetInputFieldInteractable(bool value)
    {
        usernameInputField.interactable = value;
        firstPasswordInputField.interactable = value;
        secondPasswordInputField.interactable = value;
        nameInputField.interactable = value;
    }
    public void OnclickOK()
    {
        //Validation

        string username = usernameInputField.text;
        string password = firstPasswordInputField.text;
        string name = nameInputField.text;

        SetInputFieldInteractable(false);

        HTTPNetworkManager.Instance.SignUp(username, password, name, (response) =>
        {
            SetInputFieldInteractable(true);

            //쿠키값이 있으면
            if (response.Headers.ContainsKey("Set-Cookie"))
            {
                //signIn성공시 쿠키값 전달
                string cookie = response.Headers["Set-Cookie"];

                int firstIndex = cookie.IndexOf('=') + 1;
                int lastIndex = cookie.IndexOf(';');

                string cookieValue = cookie.Substring(firstIndex, lastIndex - firstIndex);

                PlayerPrefs.SetString("sid", cookieValue);
            }

            //유저의 점수 표시
            HTTPResponseInfo info = response.GetDataFromMessage<HTTPResponseInfo>();
            GameManager.Instance.SetInfo(info.name, info.score);

            //회원가입 창 닫기
            Hide();
        }, () =>
        {
            SetInputFieldInteractable(true);
        });
    }

    public void OnclickCancel()
    {
        signInPanelManager.Show();
        Hide();
    }

    void OnvalueChangeFinalCheck()
    {
        string firsPassword = firstPasswordInputField.text;
        string secondPassword = secondPasswordInputField.text;
        if(validationFlag == 15 && (firsPassword == secondPassword))
        {
            signUpButton.interactable = true;
        }
        else
        {
            signUpButton.interactable = false;
        }
    }

    public void OnValueChangeUsername(InputField inputField)
    {
        string pattern = @"^[a-zA-Z0-9]{4,12}$";

        //유효성 검사 성공시 1로 or 연산하여 값이 있다는 1을 넣어줌
        if(Regex.IsMatch(inputField.text, pattern))
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

    public void OnValueChangefirstPassword(InputField inputField)
    {
        string pattern = @"^[a-zA-Z0-9]{4,12}$";

        if (Regex.IsMatch(inputField.text, pattern))
        {
            validationFlag = (byte)(validationFlag | 1 << 1);
        }
        //1101
        else
        {
            validationFlag = (byte)(validationFlag & ~(1<<1));
        }
        OnvalueChangeFinalCheck();
    }

    public void OnValueChangesecondPassword(InputField inputField)
    {
        string pattern = @"^[a-zA-Z0-9]{4,12}$";

        if (Regex.IsMatch(inputField.text, pattern))
        {
            validationFlag = (byte)(validationFlag | 1 << 2);
        }
        //1011
        else
        {
            validationFlag = (byte)(validationFlag & ~(1<<2));
        }
        OnvalueChangeFinalCheck();
    }

    public void OnValueChangename(InputField inputField)
    {
        string pattern = @"^[a-zA-Z0-9]{4,12}$";

        if (Regex.IsMatch(inputField.text, pattern))
        {
            validationFlag = (byte)(validationFlag | 1 << 3);
        }
        //1000
        else
        {
            validationFlag = (byte)(validationFlag & ~(1<<3));
        }
        OnvalueChangeFinalCheck();
    }
}
