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
    [SerializeField]
    private TileBase rulesTile;

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

    private TileBase GetTile(HashSet<Vector2Int> positions, Vector2Int position, Dictionary<String, TileBase> tileDefinitionMap)
    {
        return rulesTile;
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
        Dictionary<String, TileBase> tileDefinitionMap = GetTileDefinitionMap();
        HashSet<Vector2Int> positionSet = (HashSet<Vector2Int>) positions;
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, GetTile(positionSet, position, tileDefinitionMap), position);
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
