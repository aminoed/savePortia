using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnterDialog : MonoBehaviour
{
    public GameObject enterDialog;
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.tag=="Player"){
            enterDialog.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.collider.tag=="Player"){
            enterDialog.SetActive(false);
        }
    }
    void Update () {
        if(Input.GetKeyDown(KeyCode.Tab)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
    }
    // private void OnCollisionEnter2D(Collision2D other) {
        
    // }
}
