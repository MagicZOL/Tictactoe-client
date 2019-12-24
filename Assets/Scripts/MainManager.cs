using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
public class MainManager : MonoBehaviour
{
    [SerializeField] SignInPanelManager signInPanelManager;
    [SerializeField] MessagePanelManager messagePanelManager;
    [SerializeField] Button startGamebutton;
    [SerializeField] Button logoutButton;

    [SerializeField] Text nameText;
    [SerializeField] Text scoreText;

    static MainManager instance;
    public static MainManager Instance
    {
        get
        {
            if (!instance)
            {
                //유니티에서 할당된 객체가 있는지 찾아본다
                instance = GameObject.FindObjectOfType(typeof(MainManager)) as MainManager;
                if (!instance)
                {
                    //새로 만든다
                    GameObject container = new GameObject();
                    container.name = "HTTPNetworkManager";
                    instance = container.AddComponent(typeof(MainManager)) as MainManager;
                }
            }
            return instance;
        }
    }

    public void SetInfo(string name, int score)
    {
        nameText.text = name;
        scoreText.text = score.ToString();

        EnableLoginButton(true);
    }

    void Start()
    {
        EnableLoginButton(false);
        GetInfo();
    }

    void EnableLoginButton(bool value)
    {
        startGamebutton.interactable = value;
        logoutButton.interactable = value;
    }
    void GetInfo()
    {
        HTTPNetworkManager.Instance.Info((response) =>
        {
            Debug.Log(response);

            string resultStr = response.Message;

            HTTPResponseInfo info = response.GetDataFromMessage<HTTPResponseInfo>();

            SetInfo(info.name, info.score);
        }, () =>
        {
            nameText.text = "";
            scoreText.text = "";
        }); 
    }

    public void AddScore()
    {
        startGamebutton.interactable = false;

        HTTPNetworkManager.Instance.AddScore(5, (response) =>
        {
            startGamebutton.interactable = true;

            HTTPResponseInfo info = response.GetDataFromMessage<HTTPResponseInfo>();

            SetInfo(info.name, info.score);
        }, () =>
        {
            startGamebutton.interactable = true;
        }); ;
    }

    #region UI Button events
    public void OnclickStartGame()
    {
        SceneManager.LoadScene("Gmame");
    }

    public void Logout()
    {
        logoutButton.interactable = false;
        HTTPNetworkManager.Instance.Logout((response) =>
        {
            PlayerPrefs.SetString("sid", "");

            startGamebutton.interactable = false;
            nameText.text = "";
            scoreText.text = "";
        }, () =>
        {
            logoutButton.interactable = false;
        });
    }
    #endregion

    #region 패널 관련 메소드

    public void ShowSignInPanel()
    {
        signInPanelManager.Show();
    }

    public void ShowMessagePanel(string message, Action callback = null)
    {
        messagePanelManager.Show(message, callback);
    }
    #endregion
}
