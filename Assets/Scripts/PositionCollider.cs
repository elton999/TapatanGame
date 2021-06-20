using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCollider : MonoBehaviour
{
    private GameManagement _gameManagement { get => GameManagement.Instance; } 
    void Start()
    {
        GetComponent<SpriteRenderer>().color = this.ColorOnMouseExit;   
    }

    public Color ColorOnMouseOver;
    public Color ColorOnMouseExit;
    bool isMouseOver = false;
    public int CurrentPosition;

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && this.isMouseOver && this.canClick){
            _gameManagement.SelectedNextPosition = this;
            _gameManagement.CurrentStatus = GameManagement.Status.MOVING_PLAYER;
            Board.Instance.setColorPlayers();
        }
    }
    
    bool canClick {get => 
            _gameManagement.ValidMoviments.Contains(this.CurrentPosition) && 
            (!Board.Instance.hasAPlayerIn(GameManagement.Players.PLAYER_1, this.CurrentPosition) && 
            !Board.Instance.hasAPlayerIn(GameManagement.Players.PLAYER_2, this.CurrentPosition)) &&
            _gameManagement.IsPlaying;
    }
    private void OnMouseOver() { 
        if(_gameManagement.SelectedPlayer != null && this.canClick)
            GetComponent<SpriteRenderer>().color = this.ColorOnMouseOver;

        this.isMouseOver = true;   
    }

    private void OnMouseExit() {
        GetComponent<SpriteRenderer>().color = this.ColorOnMouseExit;
        this.isMouseOver = false;
    }
    
}
