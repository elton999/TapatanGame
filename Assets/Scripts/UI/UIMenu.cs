using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{   

    private GameManagement _gameManagement { get => GameManagement.Instance; }
    public void StartGamePlayerVsPlayer(){
        _gameManagement.CurrentStatus = GameManagement.Status.PLAYING;
        _gameManagement.CurrentGameType = GameManagement.GameType.PLAYER_VS_PLAYER;
        _gameManagement.PlayerTurn = GameManagement.Players.PLAYER_1;
        SceneManager.LoadScene("Tapatan", LoadSceneMode.Single);
    }

    public void StartGamePlayerVsCPU(){
        _gameManagement.CurrentStatus = GameManagement.Status.PLAYING;
        _gameManagement.CurrentGameType = GameManagement.GameType.PLAYER_VS_CPU;
        _gameManagement.PlayerTurn = GameManagement.Players.PLAYER_1;
        SceneManager.LoadScene("Tapatan", LoadSceneMode.Single);
    }

    public void GoToMenu(){
       _gameManagement.CurrentStatus = GameManagement.Status.MENU;
        SceneManager.LoadScene("menu", LoadSceneMode.Single);
    }

    public void Restart(){
        _gameManagement.Restart();
    }

    public void ChangeFullScreemWindowed(){
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void QuitGame(){
        Application.Quit();
    }

}
