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
                // Draw along x axis
                if (i <= size.x)
                {
                    var xRightCappedAtYSize = math.min(size.x, i + size.y);
                    var yTopCappedAtXSize = math.min(size.x - i, size.y);
                    
                    // Vertical
                    verticies.Add(new Vector3(i, 0, 0));
                    verticies.Add(new Vector3(i, 0, size.y));
                    
                    //Diagonals towards up right
                    verticies.Add(new Vector3(i, 0, 0));
                    verticies.Add(new Vector3(xRightCappedAtYSize, 0, yTopCappedAtXSize));

                    // Prevent drawing out of bounds
                    if (i < size.x)
                    {
                        // Diagonals towards up left
                        var xLeftCappedAtYSizeOffsetOne = math.max(0, i - size.y + 1);
                        var yTopCappedAtLeftSizeOffsetOne = math.min(i + 1, size.y);
                        
                        verticies.Add(new Vector3(i + 1, 0, 0));
                        verticies.Add(new Vector3(xLeftCappedAtYSizeOffsetOne, 0, yTopCappedAtLeftSizeOffsetOne));
                    }
                }

                // Draw along z axis (y in 2d)
                if (i <= size.y)
                {
                    verticies.Add(new Vector3(0, 0, i));
                    verticies.Add(new Vector3(size.x, 0, i));

                    // Prevent overlap or out of bounds
                    if (i != 0 && i < size.y)
                    {
                        var xRightCappedAtYSize = math.min(size.x, size.y - i);
                        var xLeftCappedAtYSize = math.max(0, size.x - (size.y - i));
                        var yCappedAtTop = math.min(i + size.x, size.y);
                        
                        // Diagonal top left corner
                        verticies.Add(new Vector3(0, 0, i));
                        verticies.Add(new Vector3(xRightCappedAtYSize, 0, yCappedAtTop));
                        
                        // Diagonal top right corner
                        verticies.Add(new Vector3(size.x, 0, i));
                        verticies.Add(new Vector3(xLeftCappedAtYSize, 0, yCappedAtTop));
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