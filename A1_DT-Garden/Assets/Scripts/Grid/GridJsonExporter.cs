using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using Grid;
using System;
using System.IO;

public class GridJsonExporter : MonoBehaviour
{
    public GridManager gridManager;

    [ContextMenu("Export")]
    public void ExportJsonFile()
    {
        List<GridTileForJson> gridTiles = new List<GridTileForJson>();

        foreach(KeyValuePair<Coordinate, GridTile> item in gridManager.tiles)
        {
            GridTileForJson gridTileForJson = new() { x =  item.Key.Position.x, z = item.Key.Position.z, section = item.Key.Section, tileType = item.Value.TileType};
            gridTiles.Add(gridTileForJson);
        }

        string json = JsonConvert.SerializeObject(gridTiles, Formatting.Indented);
        //string json = JsonConvert.SerializeObject(new Vector3(1, 1));

        Debug.Log(json);

        for (int i = 1; i <= 21; i++)
        {
            if (i > 20)
            {
                Debug.Log("Garden limit of 20 exceeded. No export made to prevent potential infinite looping");
                break;
            }
            string path = "/ExportedGardens/Garden" + i + ".json";
            if (File.Exists(Application.dataPath + path))
            {
                Debug.Log(path + " already taken");
            }
            else
            {
                File.WriteAllText(Application.dataPath + path, json);
                break;
            }
        }
        //System.IO.File.WriteAllText(Application.dataPath + "/ExportedGardens/Garden.json", json);
        //System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Garden.json", json);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
