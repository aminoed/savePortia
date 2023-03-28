using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject pathLeft, pathRight, pathUp, pathDown;
    public GameObject doorLeft, doorRight, doorUp, doorDown;

    public bool roomLeft, roomRight, roomUp, roomDown;
    public bool isSpecial=false;

    public int stepToStart;

    public int pathNum;
    // Start is called before the first frame update
    void Start()
    {
        pathLeft.SetActive(roomLeft);
        pathRight.SetActive(roomRight);
        pathUp.SetActive(roomUp);
        pathDown.SetActive(roomDown);
        doorLeft.SetActive(roomLeft);
        doorRight.SetActive(roomRight);
        doorUp.SetActive(roomUp);
        doorDown.SetActive(roomDown);
    }

    public void UpdateRoom(float xOffset, float yOffset)
    {
        stepToStart = (int)(Mathf.Abs(transform.position.x / xOffset) + Mathf.Abs(transform.position.y) / yOffset);
        if (roomUp) pathNum++;
        if (roomDown) pathNum++;
        if (roomLeft) pathNum++;
        if (roomRight) pathNum++;

    }
}
