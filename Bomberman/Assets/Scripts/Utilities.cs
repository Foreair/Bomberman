using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities{

    //Given a position inside a cell, returns a position in the middle of that cell
    static public Vector3 SnapToCell(Vector3 pos)
    {
        Grid grid = Object.FindObjectOfType<Grid>();
        Vector3 snappedPos = Vector3.zero;
        if (pos.x >= 0)
        {
            snappedPos.x = (int)pos.x + (grid.cellSize.x / 2);
        }
        else
        {
            snappedPos.x = (int)pos.x - (grid.cellSize.x / 2);
        }

        if (pos.y >= 0)
        {
            snappedPos.y = (int)pos.y + (grid.cellSize.y / 2);
        }
        else
        {
            snappedPos.y = (int)pos.y - (grid.cellSize.y / 2);
        }

        return snappedPos;
    }

}
