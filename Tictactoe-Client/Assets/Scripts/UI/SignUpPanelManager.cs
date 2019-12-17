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
    public void OnclickOK()
    {
        //Validation

        string username = usernameInputField.text;
        string password = firstPasswordInputField.text;
        string name = nameInputField.text;

        usernameInputField.interactable = false;
        firstPasswordInputField.interactable = false;
        secondPasswordInputField.interactable = false;
        nameInputField.interactable = false;

        HTTPNetworkManager.Instance.SignUp(username, password, name, (response) =>
        {
            usernameInputField.interactable = true;
            firstPasswordInputField.interactable = true;
            secondPasswordInputField.interactable = true;
            nameInputField.interactable = true;

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

            Debug.Log(response);
        }, () =>
        {
            usernameInputField.interactable = true;
            firstPasswordInputField.interactable = true;
            secondPasswordInputField.interactable = true;
            nameInputField.interactable = true;
        });
        Hide();
    }

    public void OnclickCancel()
    {
        signInPanelManager.Show();
        Hide();
    }

    public void OnValueChangeUsername(InputField inputField)
    {
        string pattern = @"^[a-zA-Z0-9]{4, 12}$";

        if(Regex.IsMatch(inputField.text, pattern))
        {
            validationFlag = (byte)(validationFlag | 1);
        }
        else
        {
            validationFlag = (byte)(validationFlag & 14);
        }
    }

    public void OnValueChangefirstPassword(InputField inputField)
    {
        string pattern = @"^[a-zA-Z0-9]{4, 12}$";

        if (Regex.IsMatch(inputField.text, pattern))
        {
            validationFlag = (byte)(validationFlag | 1 << 1);
        }
        else
        {
            validationFlag = (byte)(validationFlag & 13);
        }
    }

    public void OnValueChangesecondPassword(InputField inputField)
    {
        string pattern = @"^[a-zA-Z0-9]{4, 12}$";

        if (Regex.IsMatch(inputField.text, pattern))
        {
            validationFlag = (byte)(validationFlag | 1 << 2);
        }
        else
        {
            validationFlag = (byte)(validationFlag & 11);
        }
    }

    public void OnValueChangename(InputField inputField)
    {
        string pattern = @"^[a-zA-Z0-9]{4, 12}$";

        if (Regex.IsMatch(inputField.text, pattern))
        {
            validationFlag = (byte)(validationFlag | 1 << 3);
        }
        else
        {
            validationFlag = (byte)(validationFlag & 7);
        }
    }
}
