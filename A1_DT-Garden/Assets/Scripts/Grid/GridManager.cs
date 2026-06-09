using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private Dictionary<Coordinate, GridTile> tiles;
    public Vector2 Size;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool SetTile(Coordinate placement, GridTileType type)
    {
        // Placement out of bounds
        if (placement.Position.x < 0 || placement.Position.x > Size.x || 
            placement.Position.y < 0 || placement.Position.y > Size.y)
        {
            return false;
        }

        // Position occupied
        if (tiles.ContainsKey(placement)) return false;
        
        var tile = new GridTile(placement, type);
        tiles[placement] = tile;

        return true;
    }
}
