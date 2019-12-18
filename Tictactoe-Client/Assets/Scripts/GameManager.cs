using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] SignInPanelManager signInPanelManager;
    [SerializeField] Button addScorebutton;
    [SerializeField] Button logoutButton;

    [SerializeField] Text nameText;
    [SerializeField] Text scoreText;
    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (!instance)
            {
                //유니티에서 할당된 객체가 있는지 찾아본다
                instance = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
                if (!instance)
                {
                    //새로 만든다
                    GameObject container = new GameObject();
                    container.name = "HTTPNetworkManager";
                    instance = container.AddComponent(typeof(GameManager)) as GameManager;
                }
            }
            return instance;
        }
    }

    public void SetInfo(string name, int score)
    {
        nameText.text = name;
        scoreText.text = score.ToString();
    }

    void Start()
    {
        GetInfo();
    }

    void GetInfo()
    {
        string sid = PlayerPrefs.GetString("sid");
        if(sid.Equals(""))
        {
            // 로그인창 표시
            signInPanelManager.Show();
        }
        else
        {
            HTTPNetworkManager.Instance.Info( (response) =>
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
    }

    public void AddScore()
    {
        addScorebutton.interactable = false;

        HTTPNetworkManager.Instance.AddScore(5, (response) =>
        {
            addScorebutton.interactable = true;

            HTTPResponseInfo info = response.GetDataFromMessage<HTTPResponseInfo>();

            SetInfo(info.name, info.score);
        }, () =>
        {
            addScorebutton.interactable = true;
        }); ;
    }

    public void Logout()
    {
        logoutButton.interactable = false;
        HTTPNetworkManager.Instance.Logout((response) =>
        {
            PlayerPrefs.SetString("sid", "");

            addScorebutton.interactable = false;
            nameText.text = "";
            scoreText.text = "";
        }, () =>
        {
            logoutButton.interactable = false;
        });
    }

    public void ShowSignInPanel()
    {
        signInPanelManager.Show();
    }
}
