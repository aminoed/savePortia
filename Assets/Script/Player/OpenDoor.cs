using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private SpriteRenderer doorSprite;
    [SerializeField] private bool isEnter = true;
    private string roomTag;
    private string doorTag;
    private string pathName;
    private GameObject door;
    private GameObject room;
    private string direction;
    public Vector2 roomPoint;

    private PlayerController player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent != null)
            room = other.transform.parent.gameObject;

        if (other.CompareTag("RoomArea"))
        {
            isEnter = true;
            if (roomTag == "Room" && room.GetComponent<EnemyManager>().spawnEnemy == true)
            {
                // Debug.Log("有人，关门！");
                door.GetComponent<Collider2D>().isTrigger = false;
                StartCoroutine(Close());
            }
        }
        else if (other.CompareTag("Path"))
        {
            isEnter = false;

            pathName = other.name;
            direction = pathName.Substring(4);
            door = other.transform.parent.Find("Door" + direction).gameObject;
            doorSprite = door.GetComponent<SpriteRenderer>();
            door.GetComponent<Collider2D>().isTrigger = true;
            // Debug.Log("碰到了碰到了roomtag" + room.tag);
            // Debug.Log("朝向" + direction);
            StartCoroutine(Close());
            // }
        }
        else if (other.CompareTag("Door"))
        {
            roomTag = other.transform.parent.tag;
            doorTag = other.gameObject.tag;
            // other.gameObject.GetComponent<Collider2D>().enabled = other.gameObject.GetComponent<Collider2D>().enabled;
            doorSprite = other.gameObject.GetComponent<SpriteRenderer>();
            // Debug.Log("碰到了roomtag" + roomTag);
            // Debug.Log("碰到了doortag" + doorTag);
            if (isEnter == false)
            {
                //boss房外条件不够
                if (roomTag == "boss" && player.key < 4)
                {
                    // Debug.Log("条件不够，不开！");
                    door.GetComponent<Collider2D>().isTrigger = false;
                    StartCoroutine(Close());
                }
                else
                {
                    // Debug.Log("欢迎！");
                    //开门
                    door.GetComponent<Collider2D>().isTrigger = true;
                    StartCoroutine(Open());
                }
            }
            else
            {
                //房里有敌人，不开
                if (roomTag == "Room" && room.GetComponent<EnemyManager>().spawnEnemy == true)
                {
                    // Debug.Log("有人，不开！");
                    door.GetComponent<Collider2D>().isTrigger = false;
                    StartCoroutine(Close());
                }
                else
                {
                    // Debug.Log("开始探险吧！");
                    StartCoroutine(Open());
                }
            }

        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("RoomArea"))
        {
            isEnter = false;
        }

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.parent != null)
            room = other.transform.parent.gameObject;
        if (other.CompareTag("RoomArea"))
        {
            isEnter = true;
            if (room.tag == "Room" && room.GetComponent<EnemyManager>().spawnEnemy == false && room.GetComponent<EnemyManager>().enemyList.Count==0)
            {
                // Debug.Log("死光啦，快出去！");
                room.transform.GetChild(4).GetComponent<Collider2D>().isTrigger = true;
                room.transform.GetChild(5).GetComponent<Collider2D>().isTrigger = true;
                room.transform.GetChild(6).GetComponent<Collider2D>().isTrigger = true;
                room.transform.GetChild(7).GetComponent<Collider2D>().isTrigger = true;
                roomPoint = room.transform.position;
            }
            if (room.tag == "start")
            {
                room.transform.GetChild(4).GetComponent<Collider2D>().isTrigger = true;
                room.transform.GetChild(5).GetComponent<Collider2D>().isTrigger = true;
                room.transform.GetChild(6).GetComponent<Collider2D>().isTrigger = true;
                room.transform.GetChild(7).GetComponent<Collider2D>().isTrigger = true;
            }
        }
        else if (other.CompareTag("Path"))
        {
            isEnter = false;
        }
    }

    IEnumerator Open()
    {
        while (doorSprite.color.a > 0)
        {
            doorSprite.color = new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, doorSprite.color.a - 0.01f);
        }

        yield return new WaitForFixedUpdate();
    }

    IEnumerator Close()
    {
        while (doorSprite.color.a <= 1)
        {
            doorSprite.color = new Color(doorSprite.color.r, doorSprite.color.g, doorSprite.color.b, doorSprite.color.a + 0.01f);
        }
        yield return new WaitForFixedUpdate();
    }
}
