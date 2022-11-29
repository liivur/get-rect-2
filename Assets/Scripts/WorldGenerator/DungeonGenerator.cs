using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class DungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    [SerializeField]
    protected TileMapVisualizer tilemapVisualizer;

    protected abstract HashSet<Vector2Int> GetFloorPositions();

    public virtual void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = GetFloorPositions();
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        
    }
}
