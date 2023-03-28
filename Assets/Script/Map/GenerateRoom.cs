using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerateRoom : MonoBehaviour
{
    public enum Direction { up, down, left, right };
    public Direction direction;

    [Header("房间信息")]
    public GameObject roomPrefab, playerPrefab, princessPrefab, enemyPrefab;
    public int roomNum;
    public Color startColor, endColor;
    private GameObject endRoom;
    GameObject enemyGen;

    [Header("位置控制")]
    public Transform generatorPoint;
    public float xOffset;
    public float yOffset;
    public LayerMask roomLayer;

    public int maxPath;
    public List<Room> rooms = new List<Room>();
    public List<GameObject> farestRooms = new List<GameObject>();
    public List<GameObject> lessfarRooms = new List<GameObject>();
    public List<GameObject> onePathRooms = new List<GameObject>();

    public WallType wallType;

    void Awake()
    {
        for (int i = 0; i < roomNum; i++)
        {
            rooms.Add(ObjectPool.Instance.GetObjectPos(roomPrefab, generatorPoint.position).GetComponent<Room>());
            rooms[i].tag="Room";
            //改变位置
            ChangePointPos();

        }

        rooms[0].GetComponent<SpriteRenderer>().color = startColor;
        rooms[0].gameObject.tag = "start";
        endRoom = rooms[0].gameObject;

        //The final Room
        foreach (var room in rooms)
        {
            SetupRoom(room, room.transform.position);
        }
        FindFinalRoom();
        endRoom.GetComponent<SpriteRenderer>().color = endColor;
        endRoom.gameObject.tag = "boss";

        ObjectPool.Instance.GetObjectPos(princessPrefab, endRoom.transform.position);
        if (rooms != null)
        {
            foreach (var room in rooms)
            {
                if (room.gameObject.CompareTag("Room"))
                {
                    room.gameObject.GetComponent<EnemyManager>().enabled=true;
                }
            }
        }

    }


    // Update is called once per frame
    void Update()
    {

    }

    //改变生成点位置
    public void ChangePointPos()
    {
        do
        {

            direction = (Direction)UnityEngine.Random.Range(0, 4);

            switch (direction)
            {
                case Direction.up:
                    generatorPoint.position += new Vector3(0, yOffset, 0);
                    break;
                case Direction.down:
                    generatorPoint.position += new Vector3(0, -yOffset, 0);
                    break;
                case Direction.left:
                    generatorPoint.position += new Vector3(xOffset, 0, 0);
                    break;
                case Direction.right:
                    generatorPoint.position += new Vector3(-xOffset, 0, 0);
                    break;
            }
        } while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, roomLayer));
    }

    public void SetupRoom(Room newRoom, Vector3 roomPosition)
    {
        newRoom.roomUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0, yOffset, 0), 0.2f, roomLayer);
        newRoom.roomDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffset, 0), 0.2f, roomLayer);
        newRoom.roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0, 0), 0.2f, roomLayer);
        newRoom.roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0, 0), 0.2f, roomLayer);

        newRoom.UpdateRoom(xOffset, yOffset);

        switch (newRoom.pathNum)
        {
            case 1:
                if (newRoom.roomUp)
                {
                    ObjectPool.Instance.GetObjectPos(wallType.singleUp, roomPosition);
                }
                if (newRoom.roomDown)
                {
                    ObjectPool.Instance.GetObjectPos(wallType.SingleDown, roomPosition);
                }   
                if (newRoom.roomLeft)
                {
                    ObjectPool.Instance.GetObjectPos(wallType.singleLeft, roomPosition);
                } 
                if (newRoom.roomRight)
                {
                    ObjectPool.Instance.GetObjectPos(wallType.singleRight, roomPosition);
                }
                break;
            case 2:
                if (newRoom.roomLeft && newRoom.roomDown)
                {
                    ObjectPool.Instance.GetObjectPos(wallType.doubleLD, roomPosition);
                }
                if (newRoom.roomLeft && newRoom.roomRight)
                {
                    ObjectPool.Instance.GetObjectPos(wallType.doubleLR, roomPosition);
                }
                if (newRoom.roomLeft && newRoom.roomUp)
                {
                    ObjectPool.Instance.GetObjectPos(wallType.doubleLU, roomPosition);
                }
                if (newRoom.roomRight && newRoom.roomDown)
                {
                    ObjectPool.Instance.GetObjectPos(wallType.doubleRD, roomPosition);
                }
                if (newRoom.roomUp && newRoom.roomDown)
                {
                    ObjectPool.Instance.GetObjectPos(wallType.doubleUD, roomPosition);
                }
                if (newRoom.roomUp && newRoom.roomRight)
                {
                    ObjectPool.Instance.GetObjectPos(wallType.doubleUR, roomPosition);
                }
                break;
            case 3:
                if (newRoom.roomLeft && newRoom.roomRight && newRoom.roomDown)
                {
                    ObjectPool.Instance.GetObjectPos(wallType.tripleLRD, roomPosition);
                }
                if (newRoom.roomLeft && newRoom.roomUp && newRoom.roomDown)
                {
                    ObjectPool.Instance.GetObjectPos(wallType.tripleLUD, roomPosition);
                }
                if (newRoom.roomLeft && newRoom.roomUp && newRoom.roomRight)
                {
                    ObjectPool.Instance.GetObjectPos(wallType.tripleLUR, roomPosition);
                }
                if (newRoom.roomUp && newRoom.roomRight && newRoom.roomDown)
                {
                    ObjectPool.Instance.GetObjectPos(wallType.tripleURD, roomPosition);
                }
                break;
            case 4:
                if (newRoom.roomLeft && newRoom.roomRight && newRoom.roomDown && newRoom.roomUp)
                {
                    ObjectPool.Instance.GetObjectPos(wallType.fourPaths, roomPosition);
                }
                break;
        }
    }

    public void FindFinalRoom()
    {

        //Max step
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].stepToStart > maxPath)
            {
                maxPath = rooms[i].stepToStart;
            }
        }
        //Get Max & Max-1
        foreach (var room in rooms)
        {
            if (room.stepToStart == maxPath) farestRooms.Add(room.gameObject);
            if (room.stepToStart == maxPath - 1) lessfarRooms.Add(room.gameObject);
        }

        for (int i = 0; i < farestRooms.Count; i++)
        {
            if (farestRooms[i].GetComponent<Room>().pathNum == 1)
            {
                onePathRooms.Add(farestRooms[i]);
            }
        }

        for (int i = 0; i < lessfarRooms.Count; i++)
        {
            if (lessfarRooms[i].GetComponent<Room>().pathNum == 1)
            {
                onePathRooms.Add(lessfarRooms[i]);
            }
        }

        if (onePathRooms.Count != 0)
        {
            endRoom = onePathRooms[UnityEngine.Random.Range(0, onePathRooms.Count)];

        }
        else
        {
            endRoom = farestRooms[UnityEngine.Random.Range(0, farestRooms.Count)];
        }

    }
}

[System.Serializable]
public class WallType
{
    public GameObject singleLeft, singleRight, singleUp, SingleDown,
    doubleLU, doubleLR, doubleLD, doubleUR, doubleUD, doubleRD,
    tripleLUR, tripleLUD, tripleURD, tripleLRD, fourPaths;
}

