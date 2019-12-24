using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacTocManager : MonoBehaviour
{
    [SerializeField] Cell cell;

    void Start()
    {
        cell.MarkerType = MarkerType.None;
    }
}
