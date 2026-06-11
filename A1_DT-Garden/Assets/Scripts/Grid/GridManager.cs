using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private Dictionary<Coordinate, GridTile> tiles;
    private Dictionary<Coordinate, GridTile> oldtiles;
    
    public Vector2Int Size;
    public GameObject inactiveObject;
    public GameObject emptyObject;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tiles = new();
        oldtiles = new();
        
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                for (int s = 0; s < 5; s++)
                {
                    Coordinate coord = new(x, y, (Section)s);
                    if (!tiles.ContainsKey(coord))
                    {
                        SetTile(coord, GridTileType.Inactive);
                    }
                }
            }
        }
        
        RedrawGrid();
    }

    void RedrawGrid()
    {
        foreach (var tile in tiles)
        {
            // Skip if its the same as the previous redraw
            if (oldtiles.ContainsKey(tile.Key) && oldtiles[tile.Key].TileType == tile.Value.TileType) continue;
            
            var tileObj = Instantiate(TileTypeToObject(tile.Value.TileType), transform, true);
            tileObj.transform.position = tile.Key.Position;
            tileObj.transform.eulerAngles = new Vector3(0, tile.Key.GetAngle(), 0);
        }

        oldtiles = tiles;
    }

    public GameObject TileTypeToObject(GridTileType type)
    {
        switch (type)
        {
            case GridTileType.Inactive:
                return inactiveObject;
            case GridTileType.Empty:
                return emptyObject;
            default:
                return emptyObject;
        }
    }

    public bool SetTile(Coordinate placement, GridTileType type, bool isAi=false)
    {
        // Placement out of bounds
        if (placement.Position.x < 0 || placement.Position.x > Size.x || 
            placement.Position.y < 0 || placement.Position.y > Size.y)
        {
            return false;
        }

        if (tiles.ContainsKey(placement))
        {
            // Position occupied
            if (tiles[placement].TileType == type) return false;
        }
        
        var tile = new GridTile(placement, type);
        
        // Tiletype forbidden for AIs
        if (isAi && (tile.TileType == GridTileType.Inactive || tile.TileType == GridTileType.Empty)) return false;

        tiles[placement] = tile;
        return true;
    }
}
