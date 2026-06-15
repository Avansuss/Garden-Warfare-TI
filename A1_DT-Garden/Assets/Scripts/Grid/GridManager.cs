using System;
using System.Collections.Generic;
using Grid;
using JetBrains.Annotations;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    Dictionary<Coordinate, GridTile> tiles;
    
    private Dictionary<Coordinate, GameObject> tileObjects;
    private Dictionary<Coordinate, GridTile> oldtiles;
    
    public Vector2Int Size;
    
    public GameObject inactiveObject;
    public GameObject emptyObject;
    public GameObject grassObject;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tiles = new();
        tileObjects = new();
        oldtiles = new();
        
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                for (int s = 0; s < 5; s++)
                {
                    Coordinate coord = new(x, y, (Section)s);
                    if (!tiles.ContainsKey(coord) && (Section)s != Section.Full)
                    {
                        SetTile(coord, GridTileType.Inactive);
                    }
                }
            }
        }
    }

    void RedrawGrid()
    {
        foreach (var tile in tiles)
        {
            // Skip if its the same as the previous redraw
            if (oldtiles.ContainsKey(tile.Key))
            {
                if(oldtiles[tile.Key].TileType == tile.Value.TileType) continue;
                Destroy(tileObjects[tile.Key]);
            }
            // Only add the new tile after the check has been done
            oldtiles[tile.Key] = tile.Value;
            
            var tileObj = Instantiate(TileTypeToObject(tile.Value.TileType), transform, true);
            tileObj.transform.position = tile.Key.Position;
            tileObj.transform.eulerAngles = new Vector3(0, tile.Key.GetAngle() + 90, 0);
            
            tileObjects[tile.Key] = tileObj;
        }
    }

    public GameObject TileTypeToObject(GridTileType type)
    {
        switch (type)
        {
            case GridTileType.Inactive:
                return inactiveObject;
            case GridTileType.Empty:
                return emptyObject;
            case GridTileType.Grass:
                return grassObject;
            default:
                return emptyObject;
        }
    }

    public bool SetTile(Coordinate coordinate, GridTileType type, bool isAi=false)
    {
        coordinate.Position.y = 1;
        
        // Placement out of bounds
        if (coordinate.Position.x < 0 || coordinate.Position.x > Size.x || 
            coordinate.Position.z < 0 || coordinate.Position.z > Size.y)
        {
            return false;
        }

        // Check if tile already occupies a space
        
        if (FindCoordinate(coordinate, out var foundTile))
        {
            if (foundTile?.Value.TileType == type) return false;
            tiles.Remove(foundTile?.Key);
        }
        
        var tile = new GridTile(coordinate, type);
        
        // Tiletype forbidden for AIs
        if (isAi && (tile.TileType == GridTileType.Inactive || tile.TileType == GridTileType.Empty)) return false;
 
        tiles[coordinate] = tile;
        RedrawGrid();
        return true;
    }

    private bool FindCoordinate(Coordinate coordinate, out KeyValuePair<Coordinate, GridTile>? tile)
    {
        tile = null;
        foreach (var checkTile in tiles)
        {
            if (coordinate.IsEqualTo(checkTile.Key))
            {
                tile = checkTile;
                return true;
            }
        }

        return false;
    }
}
