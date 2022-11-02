using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = System.Random;


public class MapLocation
{
    public Vector2Int vector2;

    public MapLocation(Vector2Int pos)
    {
        vector2 = pos;
    }


    public static MapLocation operator +(MapLocation a, MapLocation b) =>
        new MapLocation(new Vector2Int(a.vector2.x + b.vector2.x, a.vector2.y + b.vector2.y));


    public override bool Equals(object obj)
    {
        if (obj == null || GetType() == obj.GetType())
        {
            return false;
        }

        else
        {
            return vector2 == ((MapLocation)obj).vector2;
        }

    }

    public override int GetHashCode()
    {
        return 0;
    }
}

public class Maze : MonoBehaviour
{
    public Vector2Int size = new Vector2Int(30, 30);
    public byte[,] map;
    public int scale = 6;
    public PlayerClass[] players;
    public int roomCount = 5;
    public Vector2Int roomCountSize = new Vector2Int(3, 6);
    public bool finishedLoadingMap = false;
    protected List<MapLocation> pillarLocations = new List<MapLocation>();

    public List<MapLocation> rooms = new List<MapLocation>();
   public List<MapLocation> directions = new List<MapLocation>()
    
    
    
    
    
    {
        new MapLocation(Vector2Int.up),
        new MapLocation(Vector2Int.down),
        new MapLocation(Vector2Int.left),
        new MapLocation(Vector2Int.right),
        /*new MapLocation(Vector2Int.right + Vector2Int.up),
        new MapLocation(Vector2Int.down + Vector2Int.left),
        new MapLocation(Vector2Int.down + Vector2Int.right),
        new MapLocation(Vector2Int.left + Vector2Int.up)*/
    };

    public DungeonTemplate TemplateData;

    public SpawnItem itemManager;
    public bool GeneratedRooms;

    public struct Pieces
    {
        public MazePieces piece;
        public GameObject model;

        public Pieces(MazePieces p, GameObject m)
        {
            piece = p;
            model = m;
        }
    }

    public Pieces[,] piecePlaces;

    public int pillarDistance;
    public float minimumStairDistance;
    

    private Vector3 playerpositionref;

    public GameObject item;
    public int maxItemCount;
    [SerializeField] private LayerMask chestLayer;

    private Transform astar;


    private bool generated;
    private bool validPath;
    private void Awake()
    {
       
    }

    private void Start()
    {
        players = FindObjectsOfType<PlayerClass>();
        BuildMaze();
    }
    
    

    // Start is called before the first frame update
    public void BuildMaze()
    {

        int loop = 0;
        InitialiseMap();
        Generate();
        //number of rooms we want -> Minimum and Maximum size 
        AddRooms(roomCount, roomCountSize.x, roomCountSize.y); 
        GenerateMap();
        GenerateMiscellaneous();
        




    }
    
    
    private void Update()
    {
        
    }

  


    public virtual void AddRooms(int count, int minSize, int maxSize)
    {
      
    }
    public virtual void Generate()
    {
      
        for (int z = 0; z < size.y; z++)
        {
            for (int x = 0; x < size.x; x++)
            {
                int randomValue = SeededRandom.Range(0, 100);
                map[x, z] = randomValue > 50 ? (byte) 1 : (byte) 0; //1 = wall 0 = corridor
            }
        }
    }

    public void InitialiseMap()
    {
      //  RNG.RandomController.GenerateRandomSeed();

      foreach (Transform child in transform)
      {
          Destroy(child.gameObject);
      }

        if (pillarDistance == 0)
        {
            pillarDistance = scale / 2;
        }
        map = new byte[size.x, size.y];
        piecePlaces = new Pieces[size.x, size.y];
        
        for (int z = 0; z < size.y; z++)
        {
            for (int x = 0; x < size.x; x++)
            {

                map[x, z] = 1;
            }
        }
    }

    private bool SpawnBossDoor(int index, Vector3 position)
    {
        if (Vector3.Distance(position, Vector3.zero) > minimumStairDistance)
        {
            if (index == 1)
            {
                generated = true;
            }
        }
        
        else
        {
            generated = false;
        }
        
        return generated;
    }
    public void GenerateMap()
    {

       

        //Instantiate(TemplateData.BossStairRoom, algorithm.goalLocation, Quaternion.identity);
        for (int z = 0; z < size.y; z++)
        {
            for (int x = 0; x < size.x; x++)
                if (map[x, z] == 1)
                {
                    /*Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.GetComponent<Renderer>().material.color = Color.white;
                    wall.transform.localScale *= scale;
                    wall.transform.position = pos;*/

                    piecePlaces[x, z].piece = MazePieces.Wall;
                    piecePlaces[x, z].model = null;

                }

                else if (Search2D(x, z, new[] {5, 0, 5, 1, 0, 1, 5, 0, 5}))
                {
                    var pos = new Vector3(x * scale, 0, z * scale);
                    var piece = Instantiate(TemplateData.VerticalStraight.prefab, pos, Quaternion.identity);

                    piece.transform.Rotate(TemplateData.VerticalStraight.rotation);
                    piece.transform.parent = gameObject.transform;
                    piecePlaces[x, z].piece = MazePieces.V_Straight;
                    piecePlaces[x, z].model = piece;
                }

                else if (Search2D(x, z, new[] {5, 1, 5, 0, 0, 0, 5, 1, 5}))
                {
                    var pos = new Vector3(x * scale, 0, z * scale);
                    var piece = Instantiate(TemplateData.HorizontalStraight.prefab, pos, Quaternion.identity);
                    piece.transform.parent = gameObject.transform;
                    piece.transform.Rotate(TemplateData.HorizontalStraight.rotation);
                    piecePlaces[x, z].piece = MazePieces.H_Straight;
                    piecePlaces[x, z].model = piece;
                }

                else if (Search2D(x, z, new[] {1, 0, 1, 0, 0, 0, 1, 0, 1}))
                {
                    var pos = new Vector3(x * scale, 0, z * scale);
                    var piece = Instantiate(TemplateData.CrossRoad.prefab, pos, Quaternion.identity);
                    piece.transform.parent = gameObject.transform;
                    piece.transform.Rotate(TemplateData.CrossRoad.rotation);
                    piecePlaces[x, z].piece = MazePieces.CRoad;
                    piecePlaces[x, z].model = piece;
                }

                else if (Search2D(x, z, new[] {5, 1, 5, 0, 0, 1, 5, 1, 5}))
                {
                    var pos = new Vector3(x * scale, 0, z * scale);
                    GameObject piece = null;
                    if (!generated && (SpawnBossDoor(SeededRandom.Range(0, 2), pos)))
                    {
                       piece  = Instantiate(TemplateData.BossRoom, pos, Quaternion.identity);
                       
                    }

                    else
                    {
                        piece = Instantiate(TemplateData.EndpieceRight.prefab, pos, Quaternion.identity);
                      
                    }

                     
                    piece.transform.parent = gameObject.transform;
                    piece.transform.Rotate(TemplateData.EndpieceRight.rotation);
                    piecePlaces[x, z].piece = MazePieces.DeadEnd_Right;
                    piecePlaces[x, z].model = piece;
                }

                else if (Search2D(x, z, new[] {5, 1, 5, 1, 0, 0, 5, 1, 5}))
                {
                    var pos = new Vector3(x * scale, 0, z * scale);
                    GameObject piece = null;
                    piece = !generated && (SpawnBossDoor(SeededRandom.Range(0, 2), pos))
                        ? Instantiate(TemplateData.BossRoom, pos, Quaternion.identity)
                        : Instantiate(TemplateData.EndpieceLeft.prefab, pos, Quaternion.identity);

                    piece.transform.parent = gameObject.transform;
                    piece.transform.Rotate(TemplateData.EndpieceLeft.rotation);
                    piecePlaces[x, z].piece = MazePieces.DeadEnd_Left;
                    piecePlaces[x, z].model = piece;

                }

                else if (Search2D(x, z, new[] {5, 1, 5, 1, 0, 1, 5, 0, 5}))
                {
                    var pos = new Vector3(x * scale, 0, z * scale);
                    GameObject piece = null;
                    piece = !generated && (SpawnBossDoor(SeededRandom.Range(0, 2), pos))
                        ? Instantiate(TemplateData.BossRoom, pos, Quaternion.identity)
                        : Instantiate(TemplateData.Endpiece.prefab, pos, Quaternion.identity);

                    piece.transform.parent = gameObject.transform;
                    piece.transform.Rotate(TemplateData.Endpiece.rotation);
                    piecePlaces[x, z].piece = MazePieces.DeadEnd;
                    piecePlaces[x, z].model = piece;

                }

                else if (Search2D(x, z, new[] {5, 0, 5, 1, 0, 1, 5, 1, 5}))
                {
                    var pos = new Vector3(x * scale, 0, z * scale);
                    GameObject piece = null;
                    piece = !generated && (SpawnBossDoor(SeededRandom.Range(0, 2), pos))
                        ? Instantiate(TemplateData.BossRoom, pos, Quaternion.identity)
                        : Instantiate(TemplateData.EndpieceUpsideDown.prefab, pos, Quaternion.identity);

                    piece.transform.parent = gameObject.transform;
                    piece.transform.Rotate(TemplateData.EndpieceUpsideDown.rotation);
                    piecePlaces[x, z].piece = MazePieces.DeadEnd_UpisdeDown;
                    piecePlaces[x, z].model = piece;

                }

                else if (Search2D(x, z, new[] {5, 1, 5, 0, 0, 1, 1, 0, 5}))
                {
                    var pos = new Vector3(x * scale, 0, z * scale);
                    var piece = Instantiate(TemplateData.LeftUpCorner.prefab, pos, Quaternion.identity);
                    piece.transform.Rotate(TemplateData.LeftUpCorner.rotation);
                    piece.name = "UL";
                    piece.transform.parent = gameObject.transform;
                    piecePlaces[x, z].piece = MazePieces.Left_Up_Corner;
                    piecePlaces[x, z].model = piece;
                }

                else if (Search2D(x, z, new[] {5, 1, 5, 1, 0, 0, 5, 0, 1}))
                {
                    var pos = new Vector3(x * scale, 0, z * scale);
                    var piece = Instantiate(TemplateData.RightUpCorner.prefab, pos, Quaternion.identity);
                    piece.transform.Rotate(TemplateData.RightUpCorner.rotation);
                    piece.name = "UR";
                    piece.transform.parent = gameObject.transform;
                    piecePlaces[x, z].piece = MazePieces.Right_Up_Corner;
                    piecePlaces[x, z].model = piece;
                }

                else if (Search2D(x, z, new[] {5, 0, 1, 1, 0, 0, 5, 1, 5}))
                {
                    var pos = new Vector3(x * scale, 0, z * scale);
                    var piece = Instantiate(TemplateData.RightDownCorner.prefab, pos, Quaternion.identity);
                    piece.transform.Rotate(TemplateData.LeftDownCorner.rotation);
                    piece.name = "LL";
                    piece.transform.parent = gameObject.transform;
                    piecePlaces[x, z].piece = MazePieces.Left_Down_Corner;
                    piecePlaces[x, z].model = piece;
                }

                else if (Search2D(x, z, new[] {1, 0, 5, 5, 0, 1, 5, 1, 5}))
                {
                    var pos = new Vector3(x * scale, 0, z * scale);
                    var piece = Instantiate(TemplateData.LeftDownCorner.prefab, pos, Quaternion.identity);
                    piece.transform.Rotate(TemplateData.RightDownCorner.rotation);
                    piece.name = "LR";
                    piece.transform.parent = gameObject.transform;
                    piecePlaces[x, z].piece = MazePieces.Right_Down_Corner;
                    piecePlaces[x, z].model = piece;
                }

                else if (Search2D(x, z, new[] {1, 0, 1, 0, 0, 0, 5, 1, 5}))
                {
                    var pos = new Vector3(x * scale, 0, z * scale);
                    var piece = Instantiate(TemplateData.T_intersectionUpsideDown.prefab, pos, Quaternion.identity);
                    piece.transform.Rotate(TemplateData.T_intersectionUpsideDown.rotation);
                    piece.name = "UD";
                    piece.transform.parent = gameObject.transform;
                    piecePlaces[x, z].piece = MazePieces.T_Junction_Upside_Down;
                    piecePlaces[x, z].model = piece;
                }

                else if (Search2D(x, z, new[] {1, 0, 5, 0, 0, 1, 1, 0, 5}))
                {
                    var pos = new Vector3(x * scale, 0, z * scale);
                    var piece = Instantiate(TemplateData.T_intersectionRight.prefab, pos, Quaternion.identity);
                    piece.transform.Rotate(TemplateData.T_intersectionRight.rotation);
                    piece.name = "-|";
                    piece.transform.parent = gameObject.transform;
                    piecePlaces[x, z].piece = MazePieces.T_Junction_Right;
                    piecePlaces[x, z].model = piece;
                }

                else if (Search2D(x, z, new[] {5, 0, 1, 1, 0, 0, 5, 0, 1}))
                {
                    var pos = new Vector3(x * scale, 0, z * scale);
                    var piece = Instantiate(TemplateData.T_intersectionLeft.prefab, pos, Quaternion.identity);
                    piece.transform.Rotate(TemplateData.T_intersectionLeft.rotation);
                    piece.name = "|-";
                    piece.transform.parent = gameObject.transform;
                    piecePlaces[x, z].piece = MazePieces.T_Junction_Left;
                    piecePlaces[x, z].model = piece;
                }

                else if (Search2D(x, z, new[] {5, 1, 5, 0, 0, 0, 1, 0, 1}))
                {
                    var pos = new Vector3(x * scale, 0, z * scale);
                    var piece = Instantiate(TemplateData.T_intersection.prefab, pos, Quaternion.identity);
                    piece.transform.Rotate(TemplateData.T_intersection.rotation);
                    piece.transform.parent = gameObject.transform;
                    piece.name = "TJ";
                    piecePlaces[x, z].piece = MazePieces.TJunction;
                    piecePlaces[x, z].model = piece;
                }

                else if (map[x, z] == 0 && CountSquareNeighbours(x, z) > 1 && CountDiagonalNeighbours(x, z) >= 1 ||
                         CountSquareNeighbours(x, z) > 1 && CountDiagonalNeighbours(x, z) > 1)
                {
                    //Debug.Log("success");
                    GameObject floor = Instantiate(TemplateData.FloorPiece.prefab, transform, true);
                    
                    
                    floor.transform.position = new Vector3(x * scale, 0, z * scale);

                    Vector2Int floorPos = new Vector2Int((int)floor.transform.position.x, (int)floor.transform.position.z);
                    rooms.Add(new MapLocation(floorPos));
                    
                    GameObject ceiling = Instantiate(TemplateData.CeilingPiece.prefab, transform, true);
                    ceiling.transform.position =
                        new Vector3(floor.transform.position.x, 9f, floor.transform.position.z);

                    piecePlaces[x, z].piece = MazePieces.Room;
                    piecePlaces[x, z].model = floor;

                    
                    GameObject pillars;
                    LocateWalls(x, z);


                    if (top)
                    {
                        GameObject wall = Instantiate(TemplateData.WallPieceTop.prefab);
                        wall.transform.position = new Vector3(x * scale, 0, z * scale);
                        wall.transform.Rotate(TemplateData.WallPieceTop.rotation);
                        wall.name = "top wall";
                        wall.transform.parent = gameObject.transform;


                        if (map[x + 1, z] == 0 && map[x + 1, z + 1] == 0 &&
                            !pillarLocations.Contains(new MapLocation(new Vector2Int(x + pillarDistance, z + pillarDistance))))
                        {
                            pillars = Instantiate(TemplateData.Pillar.prefab);
                            pillars.transform.position =
                                new Vector3((x * scale) + pillarDistance, 0, (z * scale) + pillarDistance);
                            pillars.transform.parent = wall.transform;
                            pillars.name = "Top Right";

                            Vector2Int pos = new Vector2Int(x + pillarDistance, z + pillarDistance);
                            pillarLocations.Add(new MapLocation(pos));
                            // GeneratePillar(x, z, 270, "TRight");
                        }

                        if (map[x - 1, z] == 0 && map[x + 1, z + 1] == 0 &&
                            !pillarLocations.Contains(
                                new MapLocation(new Vector2Int((x - 1) + pillarDistance, z + pillarDistance))))
                        {
                            pillars = Instantiate(TemplateData.Pillar.prefab);
                            pillars.transform.position =
                                new Vector3(((x - 1) * scale) + pillarDistance, 0, (z * scale) + pillarDistance);
                            pillars.transform.parent = wall.transform;
                            pillars.name = "Top Left";
                            Vector2Int pos = new Vector2Int((x - 1) + pillarDistance, z + pillarDistance);
                            pillarLocations.Add(new MapLocation(pos));
                            // GeneratePillar(x, z, 180, "TLeft");
                        }



                    }

                    if (bottom)
                    {
                        GameObject wall = Instantiate(TemplateData.WallPieceBottom.prefab);
                        wall.transform.position = new Vector3(x * scale, 0, z * scale);
                        wall.transform.Rotate(TemplateData.WallPieceBottom.rotation);
                        wall.name = "bottom wall";
                        wall.transform.parent = gameObject.transform;

                        if (map[x + 1, z] == 0 && map[x + 1, z - 1] == 0 &&
                            !pillarLocations.Contains(
                                new MapLocation(new Vector2Int(x + pillarDistance, (z - 1) + pillarDistance))))
                        {
                            pillars = Instantiate(TemplateData.Pillar.prefab);
                            pillars.transform.position =
                                new Vector3((x * scale) + pillarDistance, 0, ((z - 1) * scale) + pillarDistance);
                            pillars.transform.parent = wall.transform;
                            pillars.name = "Bottom Right";
                            Vector2Int pos = new Vector2Int(x + pillarDistance, (z - 1) + pillarDistance);
                            pillarLocations.Add(new MapLocation(pos));
                            // GeneratePillar(x, z, 0, "BRight");
                        }

                        if (map[x - 1, z - 1] == 0 && map[x - 1, z] == 0 &&
                            !pillarLocations.Contains(
                                new MapLocation(new Vector2Int((x - 1) + pillarDistance, (z - 1) + pillarDistance))))
                        {
                            //GeneratePillar(x, z, 0, "BLeft");
                            pillars = Instantiate(TemplateData.Pillar.prefab);
                            pillars.transform.position = new Vector3(((x - 1) * scale) + pillarDistance, 0,
                                ((z - 1) * scale) + pillarDistance);
                            pillars.transform.parent = wall.transform;
                            pillars.name = "Bottom Left";
                            Vector2Int pos = new Vector2Int((x - 1) + pillarDistance, (z - 1) + pillarDistance);
                            pillarLocations.Add(new MapLocation(pos));
                        }


                    }

                    if (right)
                    {
                        GameObject wall = Instantiate(TemplateData.WallPieceRight.prefab);
                        wall.transform.position = new Vector3(x * scale, 0, z * scale);
                        wall.transform.Rotate(TemplateData.WallPieceRight.rotation);

                        wall.name = "right wall";
                        wall.transform.parent = gameObject.transform;



                        if (map[x + 1, z + 1] == 0 && map[x, z + 1] == 0 &&
                            !pillarLocations.Contains(
                                new MapLocation(new Vector2Int((x + 1) + pillarDistance, (z - 1) + pillarDistance))))
                        {
                            pillars = Instantiate(TemplateData.Pillar.prefab);
                            pillars.transform.position = new Vector3(((x + 1) * scale) + pillarDistance, 0,
                                ((z - 1) * scale) + pillarDistance);
                            pillars.transform.parent = wall.transform;
                            pillars.name = "Right Bottom";
                            Vector2Int pos = new Vector2Int((x + 1) + pillarDistance, (z - 1) + pillarDistance);
                            pillarLocations.Add(new MapLocation(pos));
                            // GeneratePillar(x, z, 0, "RightTop");

                        }

                        if (map[x, z - 1] == 0 && map[x + 1, z - 1] == 0 &&
                            !pillarLocations.Contains(
                                new MapLocation(new Vector2Int(x + pillarDistance, (z - 1) + pillarDistance))))
                        {
                            // GeneratePillar(x, z, 270, "RightBottom");


                            pillars = Instantiate(TemplateData.Pillar.prefab);
                            pillars.transform.position =
                                new Vector3((x * scale) + pillarDistance, 0, ((z - 1) * scale) + pillarDistance);
                            pillars.transform.parent = wall.transform;
                            pillars.name = "Right Top";
                            Vector2Int pos = new Vector2Int(x + pillarDistance, (z - 1) + pillarDistance);
                            pillarLocations.Add(new MapLocation(pos));


                        }


                    }

                    if (left)
                    {
                        GameObject wall = Instantiate(TemplateData.WallPieceLeft.prefab);
                        wall.transform.position = new Vector3(x * scale, 0, z * scale);
                        wall.transform.Rotate(TemplateData.WallPieceLeft.rotation);
                        wall.name = "left wall";
                        wall.transform.parent = gameObject.transform;

                        if (map[x - 1, z + 1] == 0 && map[x, z + 1] == 0 &&
                            !pillarLocations.Contains(
                                new MapLocation(new Vector2Int((x - 1) + pillarDistance, z + pillarDistance))))
                        {
                            pillars = Instantiate(TemplateData.Pillar.prefab);
                            pillars.transform.position =
                                new Vector3(((x - 1) * scale) + pillarDistance, 0, (z * scale) + pillarDistance);
                            pillars.transform.parent = wall.transform;
                            pillars.name = "Left Top";
                            Vector2Int pos = new Vector2Int((x - 1) + pillarDistance, z + pillarDistance);
                            pillarLocations.Add(new MapLocation(pos));
                            // GeneratePillar(x, z, 180, "LeftTop");

                        }

                        if (map[x - 1, z - 1] == 0 && map[x, z - 1] == 0 &&
                            !pillarLocations.Contains(
                                new MapLocation(new Vector2Int((x - 1) + pillarDistance, (z - 1) + pillarDistance))))
                        {
                            pillars = Instantiate(TemplateData.Pillar.prefab);
                            pillars.transform.position = new Vector3(((x - 1) * scale) + pillarDistance, 0,
                                ((z - 1) * scale) + pillarDistance);
                            pillars.transform.parent = wall.transform;
                            pillars.name = "Left Bottom";
                            Vector2Int pos = new Vector2Int((x - 1) + pillarDistance, (z - 1) + pillarDistance);
                            pillarLocations.Add(new MapLocation(pos));
                            //  GeneratePillar(x, z, 90, "LeftBottom");

                        }


                    }

                }

        }


        for (int z = 0; z < size.y; z++)
        {
            for (int x = 0; x < size.x; x++)
            {
                
                if(piecePlaces[x, z].piece != MazePieces.Room) continue;
                GameObject doorway;
                LocateDoors(x, z);

                if (top)
                {
                    if (CanGenerateWall(x, z))
                    {
                        doorway = Instantiate(TemplateData.DoorWayUp.prefab, transform);
                        doorway.transform.position = new Vector3(x * scale, 0, z * scale);
                        doorway.transform.Rotate(TemplateData.DoorWayUp.rotation);
                        doorway.name = "Top Doorway";
                        doorway.transform.Translate(0, 0.01f, 0);
                    }
                }

                if (bottom)
                {
                    if (CanGenerateWall(x, z))
                    {
                        doorway = Instantiate(TemplateData.DoorWayDown.prefab, transform);
                        doorway.transform.position = new Vector3(x * scale, 0, z * scale);
                        doorway.transform.Rotate(TemplateData.DoorWayDown.rotation);
                        doorway.name = "Bottom Doorway";
                        doorway.transform.Translate(0, 0.01f, 0);
                    }
                }

                if (right)
                {
                    if (CanGenerateWall(x, z))
                    {
                        doorway = Instantiate(TemplateData.DoorWayRight.prefab, transform);
                        doorway.transform.position = new Vector3(x * scale, 0, z * scale);
                        doorway.transform.Rotate(TemplateData.DoorWayRight.rotation);
                        doorway.name = "Right Doorway";
                        doorway.transform.Translate(0, 0.01f, 0);
                    }
                }


                if (left)
                {
                    if (CanGenerateWall(x, z))
                    {
                        doorway = Instantiate(TemplateData.DoorWayLeft.prefab, transform);
                        doorway.transform.position = new Vector3(x * scale, 0, z * scale);
                        doorway.transform.Rotate(TemplateData.DoorWayLeft.rotation);
                        doorway.name = "Left Doorway";
                        doorway.transform.Translate(0, 0.01f, 0);
                    }
                }


            }
        }

       
    }



    private bool top;
    private bool bottom;
    private bool right;
    private bool left;

    bool CanGenerateWall(int x, int z)
    {
        Collider[] walls = new Collider[1];
        int amount = Physics.OverlapBoxNonAlloc(transform.position, Vector3.one * scale, walls, Quaternion.identity, TemplateData.WallLayer);

        return amount == 0;
    }


    void GenerateMiscellaneous()
    {
        PlacePlayer();
        if (generated)
        {
            GenerateStairCase();
            StartCoroutine(BakeNavmesh());
            itemManager.Generate();
        }

        else
        {
            BuildMaze();
        }

       
        

    }
    
    
    
    void GenerateStairCase()
    {
        
        GameObject[] stairs = GameObject.FindGameObjectsWithTag("StairSpawn");

        if (stairs.Length == 0)
        {
            BuildMaze();
        }
        
       
        validPath = true;
        foreach (var player in players)
        {
            player.gameObject.SetActive(true);
        }
                //Debug.Log(Colliders.Count);
                
    }

    
    void SpawnEnemies()
    {
        List<Vector3> enemyPositions = new List<Vector3>();
        for (int i = 0; i < 15; i++)
        {   
            int index = SeededRandom.Range(0, rooms.Count);
            int enemyIndex = SeededRandom.Range(0, TemplateData.enemies.Count);
            int RandomValue = SeededRandom.Range(0, 100);
            Vector3 initialPos = new Vector3(rooms[index].vector2.x, 0.1f, rooms[index].vector2.y);
            Vector3 FinalPos =new Vector3(rooms[index].vector2.x, 0.11f, rooms[index].vector2.y);
            
            
            if (!enemyPositions.Contains(FinalPos))
            {
                enemyPositions.Add(FinalPos);
                TemplateData.enemies[enemyIndex].Spawn(RandomValue,initialPos, FinalPos,  GameObject.FindWithTag("EnemyP").transform);
            }

            else
            {
                index = SeededRandom.Range(0, rooms.Count);
               initialPos = new Vector3(rooms[index].vector2.x, 0.1f, rooms[index].vector2.y);
               FinalPos =new Vector3(rooms[index].vector2.x, 0.11f, rooms[index].vector2.y);
               TemplateData.enemies[enemyIndex].Spawn(RandomValue,initialPos, FinalPos,  GameObject.FindWithTag("EnemyP").transform);
            }


        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator BakeNavmesh()
   {
       //yield return new WaitForFixedUpdate();
       yield return new WaitForFixedUpdate();
       NavMeshSurface[] surfaces = GetComponents<NavMeshSurface>();

        foreach (NavMeshSurface surface in surfaces)
        {
            surface.BuildNavMesh();
        }

      

        if (validPath)
        {
            //PlayerManager.instance.SpawnAllies();
            SpawnEnemies();
        }

   }
    
    private void LocateWalls(int x, int z)
    {
        top = false;
        bottom = false;
        right = false;
        left = false;
        
        if(x <= 0 || x >= size.x - 1 || z <= 0 || z >= size.y - 1) return;

        if (map[x, z + 1] == 1) top = true;

        if (map[x, z - 1] == 1) bottom = true;

        if (map[x + 1, z] == 1) right = true;

        if (map[x - 1, z] == 1) left = true;
    }

    private void LocateDoors(int x, int z)
    {
        top = false;
        bottom = false;
        right = false;
        left = false;
        
        if(x <= 0 || x >= size.x - 1 || z <= 0 || z >= size.y - 1) return;

        if (piecePlaces[x, z + 1].piece != MazePieces.Room && piecePlaces[x, z + 1].piece != MazePieces.Wall) top = true;

        if (piecePlaces[x, z - 1].piece != MazePieces.Room && piecePlaces[x, z - 1].piece != MazePieces.Wall) bottom = true;

        if (piecePlaces[x + 1, z].piece != MazePieces.Room && piecePlaces[x + 1, z].piece != MazePieces.Wall) right = true;

        if (piecePlaces[x - 1, z].piece != MazePieces.Room && piecePlaces[x - 1, z].piece != MazePieces.Wall) left = true;
    }
    
    protected virtual void PlacePlayer()
    {
        List<Vector3> possibleSpawnLocations = new List<Vector3>();
        for (int z = 0; z < size.y; z++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (map[x, z] == 0)
                {
                    if (possibleSpawnLocations.Count < 10)
                    {
                        possibleSpawnLocations.Add(new Vector3(x * scale, 0.1f, z * scale));
                    }

                    else
                    {
                        break;
                    }
                   
                }
            }
        }

        int randomA = 0, randomB = 0;
        int spawnCount = 0;
        while (randomA == randomB && spawnCount < 200)
        {
            randomA = SeededRandom.Range(0, 5);
            randomB = SeededRandom.Range(0, 5);
            spawnCount++;
        }
        
        Vector3 randomPostionA = possibleSpawnLocations[randomA];
        Vector3 randomPositionB = possibleSpawnLocations[randomB];

        players[0].transform.position = randomPostionA;
        players[1].transform.position = randomPositionB;
        finishedLoadingMap = true;
    }



        
    bool Search2D(int c, int r, int[] pattern)
    {
        //c = column
        //r = row
        
        int count = 0;
        
        //pattern state
        int pos = 0;

        for (int z = 1; z > -2; z--)
        {
            for (int x = -1; x < 2; x++)
            {
                    //think of 5 as a "wild card" 5 for the corners (anything can be there)
                    if (pattern[pos] == map[c + x, r + z] || pattern[pos] == 5)
                    {
                        count++;
                    }

                    pos++;
            }
        }

        return count == 9;
        //want 9 matches
    }

    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        
        if (x <= 0 || x >= size.x - 1 || z <= 0 || z >= size.y - 1)
        {
            return 5;
        }
        if (map[x - 1, z] == 0)
        {
            count += 1;

        }

        if (map[x + 1, z] == 0)
        {
            count ++;
        }
        if (map[x , z - 1] == 0)
        {
            count ++;
        }
        
        if (map[x , z + 1] == 0)
        {
            count ++;
        }

        return count;
    }
    
    public int CountDiagonalNeighbours(int x , int z)
    {
        int count = 0;
        
        if (x <= 0 || x >= size.x - 1 || z <= 0 || z >= size.y - 1)
        {
            return 5;
        }
        
        
        if (map[x - 1, z + 1] == 0)
        {
            count++;
        }
        
        if (map[x + 1, z + 1] == 0)
        {
            count++;
        }
        
        if (map[x + 1, z - 1] == 0)
        {
            count++;
        }
        
        if (map[x - 1, z - 1] == 0)
        {
            count++;
        }

        return count;
    }

    public int CountAllNeighbours(int x, int z)
    {
       return CountSquareNeighbours(x, z) + CountDiagonalNeighbours(x, z);
        
    }
    
    
}
