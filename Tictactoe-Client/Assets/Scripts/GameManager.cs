using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] SignInPanelManager signInPanelManager;
    [SerializeField] Button addScorebutton;

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
            }, () =>
            {

            });
        }
    }

    public void AddScore()
    {
        HTTPNetworkManager.Instance.AddScore(5, (response) =>
        {
            addScorebutton.interactable = true;
        }, () =>
        {
            addScorebutton.interactable = true;
        }); ;
    }

    public void ShowSignInPanel()
    {
        signInPanelManager.Show();
    }
}
