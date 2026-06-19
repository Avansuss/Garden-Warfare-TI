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
    public GameObject rockObject;
    
    
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
                    if (!FindCoordinate(coord, tiles, out var foundTile) && (Section)s != Section.Full)
                    {
                        SetTile(coord, GridTileType.Inactive, redraw:false);
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
            if (FindCoordinate(tile.Key, oldtiles, out var foundTile))
            {
                // If the tile did previously exist, skip
                if (foundTile!.Value.Value.TileType == tile.Value.TileType) continue;
                
                // The specific found tile coordinate has to be used due to floating point imprecision
                oldtiles[foundTile.Value.Key] = tile.Value;
            }
            else
            {
                oldtiles[tile.Key] = tile.Value;
            }

            // Have to do this seperately for the tile objects, due to...floating point imprecision
            if (FindCoordinate(tile.Key, tileObjects, out var foundObject))
            {
                // Otherwise, remove it for the list so a new one can take its place
                Destroy(tileObjects[foundObject!.Value.Key]);
                tileObjects.Remove(foundObject.Value.Key);
            }

            // Create the new tile
            var tileObj = Instantiate(TileTypeToObject(tile.Value.TileType), transform, true);
            tileObj.transform.position = tile.Key.Position;
            tileObj.transform.eulerAngles = new Vector3(0, tile.Key.GetAngle(), 0);
            
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
            case GridTileType.Rock:
                return rockObject;
            default:
                return emptyObject;
        }
    }

    /// <summary>
    /// Places down or edits a tile on a certain location. Will return false if an illegal move was made
    /// </summary>
    /// <param name="coordinate">The coordinate to set the tile to</param>
    /// <param name="type">The tile type to place on the coordinate</param>
    /// <param name="isAi">Whether the caller of this function is an AI</param>
    /// <param name="redraw">Whether to force a redraw of the grid. Don't use this if you change a lot of tiles at once</param>
    /// <returns></returns>
    public bool SetTile(Coordinate coordinate, GridTileType type, bool isAi=false, bool redraw=false)
    {
        coordinate.Position.y = 1;
        
        // Placement out of bounds
        if (coordinate.Position.x < 0 || coordinate.Position.x > Size.x || 
            coordinate.Position.z < 0 || coordinate.Position.z > Size.y)
        {
            return false;
        }

        // Check if tile already occupies a space
        if (FindCoordinate(coordinate, tiles, out var foundTile))
        {
            if (foundTile!.Value.Value.TileType == type) return false;
            tiles.Remove(foundTile?.Key);
        }

        var tile = new GridTile(coordinate, type);

        // Tiletype forbidden for AIs
        if (isAi && (tile.TileType == GridTileType.Inactive || tile.TileType == GridTileType.Empty)) return false;


        tiles[coordinate] = tile;
        if (redraw)
        {
            RedrawGrid();
        }

        return true;
    }

    /// <summary>
    /// Used to find a coordinate match since it has floating point imprecision on the vector 3's
    /// This method uses a workaround that does work with these positions
    /// </summary>
    /// <param name="coordinate">The coordinate to look for</param>
    /// <param name="tile">The found tile that matches the coordinate. Can be null</param>
    /// <param name="lookupTable"></param>
    /// <returns></returns>
    private bool FindCoordinate<T>(Coordinate coordinate, Dictionary<Coordinate, T> lookupTable, out KeyValuePair<Coordinate, T>? tile)
    {
        tile = null;
        foreach (var checkTile in lookupTable)
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
