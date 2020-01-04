using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;

public class TicTacTocManager : MonoBehaviour
{
    [SerializeField] Button connectButton;
    [SerializeField] Button readyButton;
    [SerializeField] Text logText;

    [SerializeField] ScrollRect scrollRect; // Log 화면

    [SerializeField] GameObject cellObject;

    [SerializeField] GameOverPanelManager gameOverPanelManager;

    //화면에 있는 Cell의 정보
    public List<Cell> cells;
    //플레이어의 Maker 타입
    private MarkerType playerMarkerType;

    //현재 게임의 상태
    private enum GameState { None, PlayerTurn, OpponentTurn, GameOver }

    //Room ID와 Client ID
    ClientInfo clientInfo;

    //현재 게임 상태
    private GameState currentState;
    private GameState CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            switch(value)
            {
                case GameState.None:
                case GameState.OpponentTurn:
                case GameState.GameOver:
                    SetActiveTouchCells(false);
                        break;
                case GameState.PlayerTurn:
                    SetActiveTouchCells(true);
                    break;
            }
            currentState = value;
        }
    }
    //승리 판정
    private enum Winner { None, Player, Opponent, Tie };

    //3x3 틱택토 추후 4x4, 5x5로 바꾸고 싶으면 숫자를 바꾸면 됨
    private const int rowColNum = 3;

    //SocketID
    private SocketIOComponent socket;

    void Start()
    {
        //소켓 초기화
        InitSocket();

        //Cell 숨기기
        cellObject.SetActive(false);

        //Ready버튼 숨기기
        readyButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (CurrentState == GameState.PlayerTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                RaycastHit2D hitinfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);

                if (hitinfo && hitinfo.transform.tag == "Cell")
                {
                    Cell cell = hitinfo.transform.GetComponent<Cell>();

                    cell.MarkerType = playerMarkerType;

                    Winner winner = CheckWinner();

                    //선택된 셀의 정보 전달
                    int index = cells.IndexOf(cell);

                    JSONObject data = new JSONObject();
                    data.AddField("index", index);
                    data.AddField("roomId", clientInfo.roomId);

                    switch (winner)
                    {
                        case Winner.None:
                            {
                                //currentState를 상대턴으로 변경
                                //서버에게 상대가 게임을 진행할 수 있도록 메시지 전달
                                CurrentState = GameState.OpponentTurn;
                                socket.Emit("select", data);

                                break;
                            }
                        case Winner.Player:
                            {
                                //승리 팝업창 표시
                                //서버에게 player가 승리했음을 알림
                                CurrentState = GameState.GameOver;
                                socket.Emit("win", data);
                                Setlog("이겼음");

                                gameOverPanelManager.SetMessage("승리하였습니다.");
                                gameOverPanelManager.Show();

                                break;
                            }
                        case Winner.Tie:
                            {
                                //무승부 팝업창 표시
                                //서버에게 player가 비겼음을 알림
                                CurrentState = GameState.GameOver;
                                socket.Emit("tie", data);
                                Setlog("비겼음");

                                gameOverPanelManager.SetMessage("비겼습니다.");
                                gameOverPanelManager.Show();

                                break;
                            }
                    }
                }
            }
        }
    }

    private void InitSocket()
    {
        GameObject socketObject = GameObject.Find("SocketIO");

        //SocketIOComponent는 서버에 연결할수있게 해주는 객체
        socket = socketObject.GetComponent<SocketIOComponent>();

        //Socket.io 이벤트 등록
        socket.On("connect", EventConnect);         //서버연결
        socket.On("join", EventJoin);               //방입장
        socket.On("play", EventPlay);               //게임시작
        socket.On("selected", EventSelected);       //상대방의 플레이
        socket.On("lose", EventLose);               //졌을때
        socket.On("tie", EventTie);                 //비겼을때

        //메시지를 받으려고 할때 socket.On(이벤트이름, 실행할클라함수) 함수를 사용한다.
        //socket.On("test", Test);
    }

    #region Socket.io Events
    //서버에 연걸 되었을 때
    void EventConnect(SocketIOEvent e)
    {
        Setlog("서버에 접속함");

        //connect button은 숨김
        connectButton.gameObject.SetActive(false);
    }

    //방에 들어갔을 때
    void EventJoin(SocketIOEvent e)
    {
        //Ready 버튼 활성화
        readyButton.gameObject.SetActive(true);

        //roomId 가져오기
        string roomId = e.data.GetField("roomId").str;
        string clientId = e.data.GetField("clientId").str;

        clientInfo = new ClientInfo(roomId, clientId);

        Setlog("방 입장 : ");
        Setlog("Room ID : " + roomId);
        Setlog("client ID : " + clientId);
    }

    //게임시작
    void EventPlay(SocketIOEvent e)
    {
        Setlog("게임시작");

        //Ready button 숨기기
        readyButton.gameObject.SetActive(false);

        bool isFirst = false;
        e.data.GetField(ref isFirst, "first");

        //턴과 Marker 지정
        if (isFirst)
        {
            playerMarkerType = MarkerType.Circle;
            CurrentState = GameState.PlayerTurn;
        }
        else
        {
            playerMarkerType = MarkerType.Cross;
            CurrentState = GameState.OpponentTurn;
        }

        //cells 오브젝트 표시
        cellObject.SetActive(true);
    }

    //상대방이 셀을 선택 했을때 
    void EventSelected(SocketIOEvent e)
    {
        int index = -1;
        e.data.GetField(ref index, "index");

        MarkerType markerType = (playerMarkerType == MarkerType.Circle ? MarkerType.Cross : MarkerType.Circle);
        cells[index].GetComponent<Cell>().MarkerType = markerType;

        CurrentState = GameState.PlayerTurn;
    }

    //게임에서 졌을때
    void EventLose(SocketIOEvent e)
    {
        int index = -1;
        e.data.GetField(ref index, "index");

        MarkerType markerType = (playerMarkerType == MarkerType.Circle ? MarkerType.Cross : MarkerType.Circle);
        cells[index].GetComponent<Cell>().MarkerType = markerType;

        CurrentState = GameState.GameOver;
        Setlog("졌음");

        gameOverPanelManager.SetMessage("패배하였습니다.");
        gameOverPanelManager.Show();
    }

    //게임에서 비겼을때
    void EventTie(SocketIOEvent e)
    {
        int index = -1;
        e.data.GetField(ref index, "index");

        MarkerType markerType = (playerMarkerType == MarkerType.Circle ? MarkerType.Cross : MarkerType.Circle);
        cells[index].GetComponent<Cell>().MarkerType = markerType;

        CurrentState = GameState.GameOver;

        Setlog("비겼음");

        gameOverPanelManager.SetMessage("비겼습니다.");
        gameOverPanelManager.Show();
    }
    #endregion

    //소켓용 함수에서는 SocketIOEvent e 를 필수적으로 선언해야한다
    //void Test(SocketIOEvent e)
    //{
    //    //e.data.GetField(서버에서의 키)
    //    string msg = e.data.GetField("message").str;
    //    Debug.Log(msg);

    //    //JSONObject 테스트
    //    JSONObject data = new JSONObject();

    //    //키와 valuse 넣기
    //    data.AddField("msg", "저도 반갑습니다.");

    //    //메시지 보낼때 Emit 함수를 사용한다.
    //    socket.Emit("hello", data);
    //}

    void SetActiveTouchCells(bool active)
    {
        foreach(Cell cell in cells)
        {
            cell.SetActiveTouch(active);
        }
    }
    Winner CheckWinner()
    {
        // 1.가로체크
        for (int i = 0; i < rowColNum; i++)
        {
            //비교를 위한 첫 번째 Cell
            Cell cell = cells[rowColNum * i];
            int num = 0;

            // 첫 번째 Cell이 Player Marker Type과 다르면 for loop 빠져나옴
            //플레이어의 승패만 찾을때
            if (cell.MarkerType != playerMarkerType) continue;

            for (int j = 1; j<rowColNum; j++)
            {
                int index = i * rowColNum + j; // 0, 1, 2

                if(cell.MarkerType == cells[index].MarkerType)
                {
                    ++num;
                }

                if(cell.MarkerType != MarkerType.None && num == rowColNum-1)
                {
                    return Winner.Player;
                    //return (cell.MarkerType == playerMarkerType) ? Winner.Player : Winner.Opponent;
                }
            }
        }
        // 2.세로체크
        for (int i = 0; i<rowColNum; i++)
        {
            Cell cell = cells[i];
            int num = 0;

            if (cell.MarkerType != playerMarkerType) continue;

            for (int j=1; j<rowColNum; j++)
            {
                int index = j * rowColNum + i; // 0, 3, 6

                if(cell.MarkerType == cells[index].MarkerType)
                {
                    ++num;
                }

                if (cell.MarkerType != MarkerType.None && num == rowColNum - 1)
                {
                    return Winner.Player;
                }
            }
        }
        // 3.왼쪽 대각선 체크
        {
            if (cells[0].MarkerType == playerMarkerType)
            {
                Cell cell = cells[0];
                int num = 0;

                for (int i = 1; i < rowColNum; i++)
                {

                    int index = i * rowColNum + i; // 0, 3, 6

                    if (cell.MarkerType == cells[index].MarkerType)
                    {
                        ++num;
                    } 
                }

                if (cell.MarkerType != MarkerType.None && num == rowColNum - 1)
                {
                    return Winner.Player;
                }
            } 
        }
        // 4.오른쪽 대각선 체크
        {
            if (cells[rowColNum-1].MarkerType == playerMarkerType)
            {
                Cell cell = cells[rowColNum-1];
                int num = 0;

                for (int i = 1; i < rowColNum; i++)
                {
                    int index = i * rowColNum + rowColNum - i - 1;

                    if (cell.MarkerType == cells[index].MarkerType)
                    {
                        ++num;
                    }
                }

                if (cell.MarkerType != MarkerType.None && num == rowColNum - 1)
                {
                    return Winner.Player;
                }
            }
        }

        //5.비겼는지 체크
        {
            int num = 0;
            foreach(Cell cell in cells)
            {
                if (cell.MarkerType == MarkerType.None)
                    ++num;
            }
            if (num == 0)
            {
                return Winner.Tie;
            }
        }

        return Winner.None;
    }

    #region UI Event
    public void OnClickConnect()
    {
        //두번 클릭 금지
        connectButton.interactable = false;

        //서버에 접속
        if(socket)
            socket.Connect();
    }

    //Ready 버튼 클릭시 호출되는 함수
    public void OnClickReady()
    {
        readyButton.interactable = false;

        JSONObject data = new JSONObject();
        data.AddField("roomId", clientInfo.roomId);
        data.AddField("clientId", clientInfo.clientId);

        socket.Emit("ready", data);

        Setlog("준비완료");
    }
    #endregion

    private void Setlog(string message)
    {
        Text logText = scrollRect.content.GetComponent<Text>();
        logText.text += message + "\n";

        //함수실행 지연
        Invoke("SetScrollDown", 0.1f);
    }

    private void SetScrollDown()
    {
        scrollRect.verticalNormalizedPosition = 0;
    }
}
