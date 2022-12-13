using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TileMapVisualizer tilemapVisualizer;

    protected HashSet<Vector2Int> allfloorPositions = new HashSet<Vector2Int>();

    private int clearThreshold = 250;
    private int renderThreshold = 100;
    private int renderHeight = 30;
    private int renderWidth = 50;
    private int minRoomHeight = 6;
    private int minRoomWidth = 13;
    private int offset = 4;

    private int renderedLeft = 0;
    private int renderedRight = 0;

    private int mapLowest = -4;

    IEnumerator ClearOutsideCoroutine(HashSet<Vector2Int> positions, int left, int right)
    {
        HashSet<Vector2Int> positionsToRemove = new HashSet<Vector2Int>();
        foreach (var position in positions)
        {
            if (position.x < left || position.x > right)
            {
                tilemapVisualizer.ClearPosition(position);
                positionsToRemove.Add(position);
            }
        }

        positions.ExceptWith(positionsToRemove);

        yield return null;
    }

    IEnumerator RenderLeftCoroutine(int start)
    {
        BoundsInt bounds = new BoundsInt(new Vector3Int(start - renderWidth, mapLowest, 0), new Vector3Int(renderWidth, renderHeight, 0));
        var roomList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(bounds, minRoomWidth, minRoomHeight);

        var floorPositions = CreateSimplePlatforms(roomList);
        FixGaps(floorPositions, start, start - renderWidth);

        int anchor = mapLowest;
        int? potentialAnchor = GetLowestPoint(allfloorPositions, start + 1);
        if (potentialAnchor != null)
        {
            anchor = (int)potentialAnchor;
        }
        FixSlopesLeft(floorPositions, start, start - renderWidth, anchor);
        FillDirt(floorPositions, start - renderWidth, start);
        //ConnectRoomsRight(allfloorPositions, floorPositions, start);

        floorPositions.UnionWith(GetColumn(allfloorPositions, start + 1));
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        allfloorPositions.UnionWith(floorPositions);

        yield return null;
    }

    IEnumerator RenderRightCoroutine(int start)
    {
        BoundsInt bounds = new BoundsInt(new Vector3Int(start, mapLowest, 0), new Vector3Int(renderWidth, renderHeight, 0));
        var roomList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(bounds, minRoomWidth, minRoomHeight);

        var floorPositions = CreateSimplePlatforms(roomList);
        FixGaps(floorPositions, start, start + renderWidth);

        int anchor = mapLowest;
        int? potentialAnchor = GetLowestPoint(allfloorPositions, start - 1);
        if (potentialAnchor != null)
        {
            anchor = (int)potentialAnchor;
        }
        FixSlopesRight(floorPositions, start, start + renderWidth, anchor);
        FillDirt(floorPositions, start, start + renderWidth);
        //ConnectRoomsRight(allfloorPositions, floorPositions, start);

        floorPositions.UnionWith(GetColumn(allfloorPositions, start - 1));
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        allfloorPositions.UnionWith(floorPositions);

        yield return null;
    }

    protected HashSet<Vector2Int> GetColumn(HashSet<Vector2Int> positions, int column)
    {
        HashSet<Vector2Int> columnPositions = new HashSet<Vector2Int>();
        foreach (var position in positions)
        {
            if (position.x == column)
            {
                columnPositions.Add(position);
            }
        }

        return columnPositions;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position.x);
        if (renderedLeft > transform.position.x - renderThreshold)
        {
            //Debug.Log("render left");
            RenderLeft(renderedLeft);
        }
        
        if (renderedRight < transform.position.x + renderThreshold)
        {
            //Debug.Log("render right");
            RenderRight(renderedRight);
        }

        if (renderedLeft < transform.position.x - clearThreshold)
        {
            //Debug.Log("render left");
            ClearOutside(renderedLeft + renderWidth, renderedRight);
        }

        if (renderedRight > transform.position.x + clearThreshold)
        {
            //Debug.Log("render right");
            ClearOutside(renderedLeft, renderedRight - renderWidth);
        }
    }

    public void ClearOutside(int left, int right)
    {
        renderedLeft = left;
        renderedRight = right;
        StartCoroutine(ClearOutsideCoroutine(allfloorPositions, left, right));
    }

    public int? GetLowestPoint(HashSet<Vector2Int> positions, int column)
    {
        int? lowest = null;
        foreach (var position in positions)
        {
            if (position.x == column)
            {
                if (lowest == null || lowest > position.y)
                {
                    lowest = position.y;
                }
            }
        }

        return lowest;
    }

    public Vector2Int GetNearestPoint(HashSet<Vector2Int> positions, int column, int row)
    {
        Vector2Int nearest = new Vector2Int(column, row + 50000);
        foreach (var position in positions)
        {
            if (position.x == column)
            {
                if (Math.Abs(position.y - row) < Math.Abs(nearest.y - row))
                {
                    nearest = position;
                }
            }
        }

        return nearest;
    }

    private void RenderRight(int start)
    {
        renderedRight = start + renderWidth;
        StartCoroutine(RenderRightCoroutine(start));
    }

    private void FillDirt(HashSet<Vector2Int> floorPositions, int start, int end)
    {
        for (int col = start; col < end; col++)
        {
            int? lowest = GetLowestPoint(floorPositions, col);
            for (int i = mapLowest; i < lowest; i++)
            {
                floorPositions.Add(new Vector2Int(col, i));
            }
        }
    }

    private void FixSlopesLeft(HashSet<Vector2Int> floorPositions, int start, int end, int anchor)
    {
        for (int col = Math.Max(start, end); col > Math.Min(start, end); col--)
        {
            int? lowest = GetLowestPoint(floorPositions, col);
            int i = 0;
            while ((lowest == null || lowest == anchor) && i < Math.Abs(end - start))
            {
                i++;
                lowest = GetLowestPoint(floorPositions, col - i);
            }

            anchor = FillInSlope(floorPositions, lowest, col, anchor);
        }
    }

    private void FixSlopesRight(HashSet<Vector2Int> floorPositions, int start, int end, int anchor)
    {
        for (int col = Math.Min(start, end); col < Math.Max(start, end); col++)
        {
            int? lowest = GetLowestPoint(floorPositions, col);
            int i = 0;
            while ((lowest == null || lowest == anchor) && i < Math.Abs(end - start))
            {
                i++;
                lowest = GetLowestPoint(floorPositions, col + i);                
            }

            anchor = FillInSlope(floorPositions, lowest, col, anchor);
        }
    }

    private int FillInSlope(HashSet<Vector2Int> floorPositions, int? lowest, int column, int anchor)
    {
        if (lowest == null)
        {
            floorPositions.Add(new Vector2Int(column, anchor));
        }
        else
        {
            floorPositions.Add(new Vector2Int(column, (int)anchor));
            if (lowest > anchor + 1)
            {
                floorPositions.Add(new Vector2Int(column, (int)anchor + 1));
                floorPositions.Remove(new Vector2Int(column, (int)lowest));
                anchor++;
            }
            else if (lowest < anchor - 1)
            {
                floorPositions.Add(new Vector2Int(column, (int)anchor - 1));
                floorPositions.Remove(new Vector2Int(column, (int)lowest));

                anchor--;
            }
            for (int j = 2; j < offset * 3; j++)
            {
                floorPositions.Remove(new Vector2Int(column, (int)anchor + j));
            }
        }

        return anchor;
    }

    private void FixGaps(HashSet<Vector2Int> floorPositions, int start, int end)
    {
        int? lowest;
        for (int col = Math.Min(start, end); col < Math.Max(start, end); col++)
        {
            lowest = GetLowestPoint(floorPositions, col);
            if (lowest == null)
            {
                floorPositions.Add(new Vector2Int(col, mapLowest));
            }
        }
    }
    /*
    private void ConnectRoomsRight(HashSet<Vector2Int> oldPositions, HashSet<Vector2Int> newPositions, int connectionColumn)
    {
        int lowestOld = (int) GetLowestPoint(oldPositions, connectionColumn - 1);
        int lowestNew = (int) GetLowestPoint(newPositions, connectionColumn);

        if (lowestOld.y > lowestNew.y)
        {

        } else if (lowestOld.y < lowestNew.y)
        {

        }
    }
    */
    private void RenderLeft(int start)
    {
        renderedLeft = start - renderWidth;
        StartCoroutine(RenderLeftCoroutine(start));
    }

    private HashSet<Vector2Int> CreateSimplePlatforms(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomList)
        {
            //Debug.Log(room.size);
            //Debug.Log(room.min);
            for (int col = offset; col < room.size.x - offset; col++)
            {
                Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, 0);
                floor.Add(position);
            }
        }

        return floor;
    }
}
