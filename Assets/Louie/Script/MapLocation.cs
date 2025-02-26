using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Louie
{
    public class MapLocation
    {
        public int x;
        public int z;

        public MapLocation(int _x, int _z)
        {
            x = _x;
            z = _z;
        }
        public Vector2 ToVecter()
        {
            return new Vector2(x, z);
        }

        //<Ssummary>
        // operator + 정의, 람다식
        // </summary>
        // <param name="a"></param>
        // <param name="B"></param>
        // <returns></returns>

        public static MapLocation operator +(MapLocation a, MapLocation b) => new MapLocation(a.x + b.x, a.z + b.z);

        //public static MapLocation operator +(MapLocation a, MapLocation b)
        //{
        //    return new MapLocation(a.x + b.x , a.z + b.z);
        //}

    }

    public class Maze : MonoBehaviour
    {
        public List<MapLocation> directions = new List<MapLocation>()
        {
            new MapLocation(1,0), new MapLocation(0,1), new MapLocation(-1,0), new MapLocation(0, -1)
        };

        public int width = 30;
        public int depth = 30;
        public byte[,] map;
        public int scale = 6;
        public int CurX = 0;
        public int CurZ = 0;
        public int[] Ran;
        public int[] StartPostion;
        public int[] EndPostion;
        public int[] NavPostion;
        public bool isEndPostionAlive = false;

        private void Start()
        {
            InitialIseMap();
            Generate();
            DrawMap();
            StartCoroutine(SpwanMonster());
            Invoke("NaviGation", 3f);
        }

        void InitialIseMap()
        {
            map = new byte[width, depth];
            for (int z = 0; z < depth; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    map[x, z] = 1; // 1은 벽, 0은 통로
                }
            }
        }

        public virtual void Generate()
        {
            //for (int z = 0; z < depth; z++)
            //{
            //    for(int x = 0; x < width; x++)
            //    {
            //        if (Random.Range(0, 100) < 50)
            //            map[x, z] = 0; // 1 = Wall 0 = corridor
            //    }
            //}
        }

        void DrawMap()
        {
            for (int z = 0; z < depth; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (map[x, z] == 1)
                    {
                        Vector3 pos = new Vector3(x * scale, 0, z * scale);
                        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        wall.transform.localScale = new Vector3(scale, scale, scale);
                        wall.transform.position = pos;
                    }
                    if (map[x, z] == 0)
                    {
                        Vector3 pos = new Vector3(x * scale, -(scale / 2), z * scale);
                        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Plane);
                        wall.transform.localScale = new Vector3(scale, scale, scale);
                        wall.transform.position = pos;

                    }
                }
            }
        }

        IEnumerator SpwanMonster()
        {
            for (int z = 0; z < depth; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    Ran[0] = Random.Range(0, depth);
                    Ran[1] = Random.Range(0, width);
                    while (map[Ran[0], Ran[1]] == 1)
                    {
                        Ran[0] = Random.Range(0, depth);
                        Ran[1] = Random.Range(0, width);
                    }
                    if (!isEndPostionAlive)
                    {
                        isEndPostionAlive = true;
                        EndPostion[0] = Ran[0];
                        EndPostion[1] = Ran[1];
                    }
                    yield return new WaitForSeconds(1f);
                    Vector3 pos = new Vector3(Ran[0] * scale, 0, Ran[1] * scale);
                    GameObject Monster = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    Monster.transform.localScale = new Vector3(scale, scale, scale);
                    Monster.transform.position = pos;

                }
            }
        }


        void NaviGation()
        {
            while (NavPostion[0] == EndPostion[0] && NavPostion[1] == EndPostion[1])
            {
                if (map[NavPostion[0], NavPostion[1 + 1]] == 1)
                {
                    NavPostion[1]++;
                    Vector3 pos = new Vector3(NavPostion[0] * scale, 0, NavPostion[1] * scale);
                    GameObject Monster = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    Monster.transform.localScale = new Vector3(scale, scale, scale);
                    Monster.transform.position = pos;
                }
            }
        }

        /// <summary>
        /// 4방위 복도를 검색한다.
        /// </summary>
        /// <param name="x">여긴 X 값</param>
        /// <param name="z">여긴 B 값</param>
        /// <returns></returns>
        public int CountSquareNeIghbours(int x, int z)
        {
            int count = 0;
            if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1)
            {
                return 5;
            }
            if (map[x - 1, z] == 0)
            {
                count++;
            }
            if (map[x + 1, z] == 0)
            {
                count++;
            }
            if (map[x, z + 1] == 0)
            {
                count++;
            }
            if (map[x, z - 1] == 0)
            {
                count++;
            }
            return count;
        }
    }
}
