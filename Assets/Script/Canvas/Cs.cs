using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Cs : MonoBehaviour
{
    // Start is called before the first frame update
    private string btnType;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), UnityEngine.Vector2.zero);
        if (hit.collider != null && Input.GetMouseButtonDown(0))
        {
            // Debug.Log(hit.collider.gameObject.tag);
            btnType = hit.collider.gameObject.tag;
            if (btnType == "Exit")
            {
                ExitGame();
            }
            if (btnType == "Menu")
            {
                GameMenu();
            }
            if (btnType == "Again")
            {
                PlayAgain();
            }
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        // Debug.Log("已执行退出");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
        // Debug.Log("已执行游戏");
    }

    public void GameMenu()
    {
        SceneManager.LoadScene(0);
        // Debug.Log("已执行主界面");
    }
}
