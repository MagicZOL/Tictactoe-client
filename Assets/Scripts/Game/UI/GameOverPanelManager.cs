using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverPanelManager : PanelManager
{
    [SerializeField] Text messageText; //확인버튼
    
    //확인버튼 메시지
    public void SetMessage(string message)
    {
        messageText.text = message;
    }

    //확인버튼을 눌렀을때
    public void OnclickComfirm(Button button)
    {
        button.interactable = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

