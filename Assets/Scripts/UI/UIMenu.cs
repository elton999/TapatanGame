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
        GameManagment.Instance.PlayerTurn = GameManagment.Players.PLAYER_1;
        SceneManager.LoadScene("Tapatan", LoadSceneMode.Single);
    }

    public void StartGamePlayerVsCPU(){
        GameManagment.Instance.CurrentStatus = GameManagment.Status.PLAYING;
        GameManagment.Instance.CurrentGameType = GameManagment.GameType.PLAYER_VS_CPU;
        GameManagment.Instance.PlayerTurn = GameManagment.Players.PLAYER_1;
        SceneManager.LoadScene("Tapatan", LoadSceneMode.Single);
    }

    public void GoToMenu(){
        GameManagment.Instance.CurrentStatus = GameManagment.Status.MENU;
        SceneManager.LoadScene("menu", LoadSceneMode.Single);
    }

    public void Restart(){
        GameManagment.Instance.CurrentStatus = GameManagment.Status.PLAYING;
        GameManagment.Instance.PlayerTurn = GameManagment.Players.PLAYER_1;
        SceneManager.LoadScene("Tapatan", LoadSceneMode.Single);
    }

    public void ChangeFullScreemWindowed(){
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void QuitGame(){
        Application.Quit();
    }

}
