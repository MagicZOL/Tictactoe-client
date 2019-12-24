using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Winner : MonoBehaviour
{
    GameObject[] cell;

    Cell[][] cells;
    MarkerType[] markerTypes;
    // Start is called before the first frame update
    void Start()
    {
        cell = GameObject.FindGameObjectsWithTag("Cell");
        int k =0;
        for (int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                cells[i][j] = cell[k].GetComponent<Cell>();
                k++;
            }
        }
        //cell = FindObjectsOfType<Cell>();  
    }
    private void OnApplicationQuit()
    {
        Win();
    }
    void Win()
    {
        //가로
        for(int i=0; i<3; i++)
        {
            for (int j=0; j<3; j++)
            {
                if(j==2)
                {
                   // string gg = cell[i][j].MarkerType == cell[i][j-1].MarkerType ? cell[i][j-1].MarkerType == cell[i][j-2].MarkerType ? "Win" : null : null;
                }
                if(i==j)
                {
                     //markerTypes[i] = cell[i][j].MarkerType;
                }
            }
        }
        for(int i = markerTypes.Length; i<0; i--)
        {
            //string gg=  markerTypes[i] == markerTypes[i - 1] ? markerTypes[i - 1] == markerTypes[i - 2] ? "Win" : null : null;
        }


        //세로
        for(int j=0; j < 3; j++)
        {
            for(int i =0; i<3; i++)
            {
                if(i==2)
                {
                   // string gg = cell[i][j].MarkerType == cell[i-1][j].MarkerType ? cell[i-1][j].MarkerType == cell[i-2][j].MarkerType ? "Win" : null : null;
                }
            }
        }

        //string gg= markerType[0] == markerType[1] ? markerType[1] == markerType[2] ? "Win" : null : null;
    }
}
