using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardData
{
    public int[] column;
    public int[] row;
    public string[] status;
    public int CellAmount;
    //Stores board's cell properties
    public BoardData(GameManager board)
    {
        CellProperties[] cells = board.GetComponentsInChildren<CellProperties>();
        CellAmount = board.GetComponentsInChildren<CellProperties>().Length;
        column = new int[cells.Length];
        row = new int[cells.Length];
        status = new string[cells.Length];
        for(int i = 0;i < CellAmount; i++)
        {
            // store each cell's colum row and status one by one
            column[i] = cells[i].column;
            row[i] = cells[i].row;
            status[i] = cells[i].status;
        }
    }
}
