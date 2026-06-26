using System;
using System.Collections.Generic;
using Grid;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Dictionary<Coordinate, GridTile> tiles;
    
    private Dictionary<Coordinate, GameObject> tileObjects;
    private Dictionary<Coordinate, GridTile> oldtiles;
    
    private GridToScore gridToScore;

    public Vector2Int Size;
    public bool DisableGrid;
    private Vector3 _origin;
    private GridOverlay _gridOverlay;
    private Camera _cam;
    
    

    public List<GameObject> lstGridObjects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!DisableGrid)
        {
            _gridOverlay = this.AddComponent<GridOverlay>();
            _gridOverlay.GridManager = this;
        }
        _origin = transform.position;
        ResetGrid();
    }

    public void ResetGrid()
    {
        if (tileObjects?.Count > 0)
        {
            foreach (var tileObject in tileObjects)
            {
                Destroy(tileObject.Value);
            }
        }
        
        tiles = new();
        tileObjects = new();
        oldtiles = new();
        gridToScore = new();
        
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                for (int s = 0; s < 5; s++)
                {
                    Coordinate coord = new(x, y, (Section)s);
                    if (!tiles.TryGetValue(coord, out _) && (Section)s != Section.Full)
                    {
                        SetTile(coord, GridTileType.Empty, redraw:false);
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
            if (oldtiles.TryGetValue(tile.Key, out var foundTile))
            {
                // If the tile did previously exist, skip
                if (foundTile.TileType == tile.Value.TileType) continue;
                
                oldtiles[tile.Key] = tile.Value;
                Destroy(tileObjects[tile.Key]);
                tileObjects.Remove(tile.Key);
            }
            else
            {
                oldtiles[tile.Key] = tile.Value;
            }

            // Create the new tile
            var tileObj = Instantiate(TileTypeToObject(tile.Value.TileType), transform, true);
            tileObj.transform.position = tile.Key.Position + new Vector3(_origin.x, 0, _origin.z);
            tileObj.transform.eulerAngles = new Vector3(0, tile.Key.GetAngle(), 0);
            
            tileObjects[tile.Key] = tileObj;
        }
    }

    public void PassGridTilesToScore(int plantAmount, bool[] animalQuestions, byte[] answersDropdown)
    {
        gridToScore.CalculateScore(tiles, plantAmount, animalQuestions, answersDropdown);
    }

    public GameObject TileTypeToObject(GridTileType type)
    {
        if ((int)type >= lstGridObjects.Count) return null;

        return lstGridObjects[(int)type];
    }

    /// <summary>
    /// Shows a summary of how many times each tile exists
    /// </summary>
    /// <returns></returns>
    public Dictionary<GridTileType, int> GetGridSummary()
    {
        Dictionary<GridTileType, int> summary = new();
        for (int i = 0; i < Enum.GetNames(typeof(GridTileType)).Length; i++)
        {
            summary[(GridTileType)i] = 0;
        }
        
        foreach (var tile in tiles)
        {
            summary[tile.Value.TileType]++;
        }

        return summary;
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
        // Tiletype forbidden for AIs
        if (isAi && type == GridTileType.Inactive) return false;
        if (isAi && type == GridTileType.Empty) return false;
        
        coordinate.Position.y = _origin.y;
        
        // Placement out of bounds
        if (coordinate.Position.x < 0 || coordinate.Position.x >= Size.x || 
            coordinate.Position.z < 0 || coordinate.Position.z >= Size.y)
        {
            return false;
        }

        if (coordinate.Section == Section.Full)
        {
            var falseNum = 0;
            for (int direction = 1; direction <= 4; direction++)
            {
                var newCoordinate = new Coordinate(coordinate.Position.x, coordinate.Position.z, (Section)direction);

                // Only redraw on the last triangle to avoid redundant updates and flickering
                if (!SetTile(newCoordinate, type, redraw: direction == 4, isAi:isAi)) falseNum++;
            }

            // If all tile draw methods fail (none can be drawn on)
            if (falseNum == 4) return false;
        }
        else
        {
            // Check if tile already occupies a space
            if (tiles.TryGetValue(coordinate, out var foundTile))
            {
                if (isAi && foundTile.TileType != GridTileType.Empty) return false;
                
                if (foundTile.TileType == type) return false;
                tiles.Remove(coordinate);
            }
            
            
            var tile = new GridTile(coordinate, type);
            
            tiles[coordinate] = tile;
            if (redraw)
            {
                RedrawGrid();
            }
        }
        return true;
    }

    public Dictionary<Coordinate, GridTile> GetGrid() => tiles;
}
