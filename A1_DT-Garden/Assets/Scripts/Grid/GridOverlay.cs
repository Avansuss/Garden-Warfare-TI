using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class GridOverlay : MonoBehaviour
    {
        public GridManager GridManager;

        private Vector2 size;

        private void Start()
        {
            size = GridManager.Size;
            DrawGrid();
        }

        public void DrawGrid()
        {
            MeshFilter filter = gameObject.GetComponent<MeshFilter>();        
            var mesh = new Mesh();
            var verticies = new List<Vector3>();

            var indicies = new List<int>();
            
            for (int i = 0; i <= math.max(size.x, size.y); i++)
            {
                // Draw from horizontal line
                if (i <= size.x)
                {
                    // Vertical
                    verticies.Add(new Vector3(i, 0, 0));
                    verticies.Add(new Vector3(i, 0, size.y));
                    
                    // // //Diagonals right
                    // verticies.Add(new Vector3(i, 0, 0));
                    //
                    // var xCappedAtTopEdge = math.min(i + size.y, size.x);
                    // var yMinusRemainingSize = math.max(i - (size.x - size.y), 0); //Capped at 0 until it exceeds the y width
                    // verticies.Add(new Vector3(xCappedAtTopEdge, 0, size.y - yMinusRemainingSize));

                    // Prevent drawing out of bounds
                    if (i < size.x)
                    {
                        // Diagonals left
                        // verticies.Add(new Vector3(i + 1, 0, 0));
                        //
                        // var xCappedAtLeftEdge = math.max(i + 1 - size.y, 0);
                        // var yMinusStartingSize = math.min(size.y, i + 1);
                        // verticies.Add(new Vector3(xCappedAtLeftEdge, 0, yMinusStartingSize));
                    }
                }

                // Draw from the vertical line
                if (i <= size.y)
                {
                    verticies.Add(new Vector3(0, 0, i));
                    verticies.Add(new Vector3(size.x, 0, i));

                    // Prevent overlap or out of bounds
                    if (i != 0 && i < size.y)
                    {
                        // Diagonal top left corner
                        verticies.Add(new Vector3(0, 0, i));

                        var xCappedAtRightEdge = math.max(size.y - i, size.x);
                        
                        verticies.Add(new Vector3(xCappedAtRightEdge, 0, size.y));
                        
                        // Diagonal top right corner
                        // verticies.Add(new Vector3(size.x, 0, i));
                        // verticies.Add(new Vector3(size.x + i - size.y, 0, size.y));
                    }
                }
                
            }

            for (int i = 0; i < verticies.Count; i++)
            {
                indicies.Add(i);
            }

            mesh.vertices = verticies.ToArray(); 
            mesh.SetIndices(indicies.ToArray(), MeshTopology.Lines, 0);
            filter.mesh = mesh;

            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
            meshRenderer.material.color = Color.white;
        }
    }
}