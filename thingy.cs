using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thingy : MonoBehaviour
{


   public float weight = 2;
    public float weighta = 2;
    public float weightb = 2;
    void Start()
    {
        
    }

    private void Update()
    {
        Random.seed = 15;
        int w = 10;
        int h = 10;
        float[,] heights = new float[w, h];
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                heights[x, y] = 0;
            }
        }
        heights = RandomDifference(heights, weight, 2, 1);
        heights = RandomDifference(heights, weighta, 2, 0);
        heights = RandomDifference(heights, weightb, 2, 1);
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        GridMesh meshgrid = new GridMesh(heights);
        mesh.vertices = meshgrid.Points;
        mesh.triangles = meshgrid.Triangles;

        mesh.RecalculateNormals();
    }

    float[,] RandomDifference(float[,] previous, float weight, int scale, int type)
    {
        int width = previous.GetLength(0)*scale;
        int length = previous.GetLength(1)*scale;
        float[,] newGrid = new float[width, length];
        List<Vector2> RandomPoints = new List<Vector2>();
        if (type == 1)
        {
            for (int i = 0; i < (width*length/25); i++)
            {
                RandomPoints.Add(new Vector2(Random.value * width, Random.value * length));
            }
        }
       
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                float starter = previous[x / scale, y / scale];
                if (type == 0)
                {
                    starter += Random.value;
                }
                if (type == 1)
                {
                    float Max = 0;
                    foreach(Vector2 point in RandomPoints)
                    {
                        if(1/Vector2.Distance(new Vector2(x, y), point) > Max)
                        {
                            Max = 1/Vector2.Distance(new Vector2(x, y), point);
                        }                        
                    }
                    foreach (Vector2 point in RandomPoints)
                    {
                       starter+= (1/Vector2.Distance(new Vector2(x, y), point))/Max;
                    }
                }
                newGrid[x, y] = starter*weight;
            }
        }
        return newGrid;
    }

    class GridMesh
    {
        int length;
        int width;
        public int Length
        {
            get
            {
                return length;
            }
        }
        public int Width
        {
            get
            {
                return width;
            }
        }

        public int[] Triangles
        {
            get
            {
                return GenerateTriangles();
            }
        }

        public Vector3[] Points
        {
            get
            {
                return GeneratePoints();
            }
        }

        public float [,] HeightMap;

        public GridMesh(int _X, int _Y)
        {
            width = _X;
            length = _Y;
            HeightMap = new float[width, length];
        }
        public GridMesh(float[,] _grid)
        {
            width = _grid.GetLength(0);
            length= _grid.GetLength(1);
            HeightMap = _grid;
        }

        int[] GenerateTriangles()
        {
            int[] _Triangles = new int[width * length * 6];
            int GetPosition(int _w, int _h) 
            {
                return _w + (_h * width);
            }
            int Position = 0;
            for (int x = 0; x < width-1; x++)
            {
                for (int y = 0; y < length-1; y++)
                {
                    _Triangles[Position] = GetPosition(x, y);
                    _Triangles[Position+1] = GetPosition(x+1, y);
                    _Triangles[Position + 2] = GetPosition(x, y+1);
                    Position += 3;
                    _Triangles[Position] = GetPosition(x+1, y);
                    _Triangles[Position + 1] = GetPosition(x+1, y+1);
                    _Triangles[Position + 2] = GetPosition(x, y + 1);
                    Position += 3;
                }
            }
            return _Triangles;
        }

        Vector3[] GeneratePoints()
        {
            Vector3[] _Points = new Vector3[width * length];
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    _Points[x + (width * y)] = new Vector3(x, HeightMap[x, y], y);
                }
            }
            return _Points;
        }        
    }
}

