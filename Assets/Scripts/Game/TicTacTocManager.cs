using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacTocManager : MonoBehaviour
{
    //플레이어의 Maker 타입
    private MarkerType playerMarkerType;

    //현재 게임의 상태
    private enum GameState { None, PlayerTurn, OpponentTurn, GameOver }

    private GameState currentState;

    private void Start()
    {
        //임시코드
        playerMarkerType = MarkerType.Circle;
        currentState = GameState.PlayerTurn;
    }
    private void Update()
    {
        if (currentState == GameState.PlayerTurn || currentState == GameState.OpponentTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                RaycastHit2D hitinfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);

                if (hitinfo && hitinfo.transform.tag == "Cell")
                {
                    Cell cell = hitinfo.transform.GetComponent<Cell>();
                    cell.MarkerType = playerMarkerType;

                    if (currentState == GameState.PlayerTurn)
                    {
                        cell.MarkerType = playerMarkerType;
                    }
                    else
                    {
                        cell.MarkerType = (playerMarkerType == MarkerType.Circle) ? MarkerType.Cross : MarkerType.Circle;
                    }

                    currentState = (currentState == GameState.PlayerTurn) ? GameState.OpponentTurn : GameState.PlayerTurn;
                }
            }
        }
    }
}
