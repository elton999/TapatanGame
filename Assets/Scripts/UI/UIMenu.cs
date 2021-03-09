using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartGamePlayerVsPlayer(){
        GameManagment.Instance.CurrentStatus = GameManagment.Status.PLAYING;
        GameManagment.Instance.CurrentGameType = GameManagment.GameType.PLAYER_VS_PLAYER;
        SceneManager.LoadScene("Tapatan", LoadSceneMode.Single);
    }

    public void StartGamePlayerVsCPU(){
        GameManagment.Instance.CurrentStatus = GameManagment.Status.PLAYING;
        GameManagment.Instance.CurrentGameType = GameManagment.GameType.PLAYER_VS_CPU;
        SceneManager.LoadScene("Tapatan", LoadSceneMode.Single);
    }

    public void QuitGame(){
        Application.Quit();
    }

}
