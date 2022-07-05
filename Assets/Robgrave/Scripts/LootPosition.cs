using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LootPosition {

    public bool Empty;
    public Vector3 GridPosition;
    public int Id;


    public LootPosition(Vector3 gridPosition, bool empty, int id)
    {
        this.Empty = empty;
        this.GridPosition = gridPosition;
        this.Id = id;
    }

}