using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerAttri : MonoBehaviour
{
    public PlayerController player;
    private Slider bloodSlider;
    private Slider hungerSlider;
    private Slider keySlider;
    public GameObject GameOverBoard;
    public GameObject WinningBoard;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        bloodSlider = GameObject.Find("Canvas/Attri/Life/Slider").GetComponent<Slider>();
        hungerSlider = GameObject.Find("Canvas/Attri/Hunger/Slider").GetComponent<Slider>();
        keySlider = GameObject.Find("Canvas/Attri/Key/Slider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentState();
        if (player.life <= 0 || player.hunger <= 0)
        {
            GameOverBoard.SetActive(true);
        }
    }
    public void currentState()
    {
        bloodSlider.value = player.life;
        hungerSlider.value = player.hunger;
        keySlider.value = player.key;
    }

    public void win(){
        AudioController.instance.SuccessAudio();
        WinningBoard.SetActive(true);
    }
}
