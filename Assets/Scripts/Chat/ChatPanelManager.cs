using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChatPanelManager : MonoBehaviour
{
    [SerializeField] GameObject chatCellPrefab;
    [SerializeField] Transform content;

    [SerializeField] ScrollRect scrollRect;
    [SerializeField] InputField messageInputField;
    [SerializeField] Button sendButton;

    int lastSeq = 0;

    // Start is called before the first frame update
    void Start()
    {
        //3초마다 한번씩 새로운 메시지 받아오기
        StartCoroutine(GetNewMessage());
    }

    public void OnClickSendButton()
    {
        if(messageInputField.text != "")
        {
            HTTPNetworkManager.Instance.AddMessage(messageInputField.text, (response) =>
            {
                Debug.Log(response);
            }, () =>
            {

            });
        }
    }
    IEnumerator GetNewMessage()
    {
        while (true)
        {
            HTTPNetworkManager.Instance.LoadChat((response) =>
            {
                if (response.Message == "") return;

                HTTPResponseChat chat = response.GetDataFromMessage<HTTPResponseChat>();

                foreach (HTTPResponseChat.chat message in chat.objects)
                {
                    if(lastSeq != 0)
                    {
                        ChatCell chatCell = Instantiate(chatCellPrefab, content).GetComponent<ChatCell>();
                        chatCell.CachedText.text = string.Format("[{0}] {1}", message.name, message.message);
                        chatCell.transform.SetAsLastSibling(); //셀이 들어갈때 밑? 위? 인지 결정 지금함수는 밑
                    }
                    lastSeq = int.Parse(message._id);

                }

                scrollRect.verticalNormalizedPosition = 0;
            }, () =>
            {

            }, lastSeq);

            yield return new WaitForSeconds(2);
        }
    }
}
