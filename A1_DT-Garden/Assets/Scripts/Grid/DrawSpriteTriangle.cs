using UnityEngine;

public class DrawSpriteTriangle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Mesh triangle = new Mesh();

        Vector3[] vertices = new Vector3[3];
        Vector2[] uv = new Vector2[3];
        int[] triangles = new int[3];

        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0.5f, 0.5f, 0);
        vertices[2] = new Vector3(0.5f, -0.5f, 0);

        float cellRotation = transform.parent.eulerAngles.y;

        if (cellRotation > 45)
        {
            if (cellRotation > 135)
            {
                if (cellRotation > 225) // north
                {
                    uv[0] = new Vector2(0.5f, 0.5f);
                    uv[1] = new Vector2(0, 1);
                    uv[2] = new Vector2(1, 1);
                }
                else // west
                {
                    uv[0] = new Vector2(0.5f, 0.5f);
                    uv[1] = new Vector2(0, 0);
                    uv[2] = new Vector2(0, 1);
                }

            }
            else // south
            {
                uv[0] = new Vector2(0.5f, 0.5f);
                uv[1] = new Vector2(1, 0);
                uv[2] = new Vector2(0, 0);
            }
        }
        else // east
        {
            uv[0] = new Vector2(0.5f, 0.5f);
            uv[1] = new Vector2(1, 1);
            uv[2] = new Vector2(1, 0);
        }

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        triangle.vertices = vertices;
        triangle.uv = uv;
        triangle.triangles = triangles;

        GetComponent<MeshFilter>().mesh = triangle;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
