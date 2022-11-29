using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap;
    [SerializeField]
    private TileBase floorTile, innerCornerLeft, innerCornerRight, outerCornerLeft, outerCornerRight, middleDirt;

    private Dictionary<String, TileBase> GetTileDefinitionMap()
    {
        return new Dictionary<String, TileBase>() {
            {"11111111", middleDirt},
            {"11111110", innerCornerLeft},
            {"10111111", innerCornerRight},
            {"01111100", outerCornerLeft},
            {"00111100", outerCornerLeft},
            {"00011111", outerCornerRight},
            {"00011110", outerCornerRight},
        };
    }

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    private TileBase GetTile(HashSet<Vector2Int> positions, Vector2Int position)
    {
        string neighbourString = "";
        foreach (var direction in Direction2D.eightDirectionsList)
        {
            if (positions.Contains(position + direction))
            {
                neighbourString += "1";
            } else
            {
                neighbourString += "0";
            }
        }

        Dictionary<String, TileBase> tileDefinitionMap = GetTileDefinitionMap();
        //Debug.Log(neighbourString);
        if (!tileDefinitionMap.ContainsKey(neighbourString))
        {
            //Debug.Log("floor");
            return floorTile;
        }

        //Debug.Log(tileDefinitionMap[neighbourString]);
        return tileDefinitionMap[neighbourString];
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        HashSet<Vector2Int> positionSet = (HashSet<Vector2Int>) positions;
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, GetTile(positionSet, position), position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        //Debug.Log(position);
        var tilePosition = tilemap.WorldToCell((Vector3Int) position);
        //Debug.Log(tilePosition);
        tilemap.SetTile(tilePosition, tile);
        //tilemap.SetTile((Vector3Int) position, tile);
    }

    public void ClearAll()
    {
        floorTilemap.ClearAllTiles();
    }

    public void ClearPosition(Vector2Int position)
    {
        var tilePosition = floorTilemap.WorldToCell((Vector3Int)position);
        floorTilemap.SetTile(tilePosition, null);

        //floorTilemap.SetTile((Vector3Int)position, null);
    }
}
