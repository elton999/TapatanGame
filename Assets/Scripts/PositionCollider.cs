using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().color = this.ColorOnMouseExit;   
    }

    // Update is called once per frame
    public Color ColorOnMouseOver;
    public Color ColorOnMouseExit;
    bool isMouseOver = false;
    public int CurrentPosition;

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && this.isMouseOver && this.canClick){
            GameManagment.Instance.SelectedNextPosition = this;
            GameManagment.Instance.CurrentStatus = GameManagment.Status.MOVING_PLAYER;
            Board.Instance.setColorPlayers();
        }
    }
    
    bool canClick {get => 
            GameManagment.Instance.ValidMoviments.Contains(this.CurrentPosition) && 
            (!Board.Instance.hasAPlayerIn(GameManagment.Players.PLAYER_1, this.CurrentPosition) && 
            !Board.Instance.hasAPlayerIn(GameManagment.Players.PLAYER_2, this.CurrentPosition)) &&
            GameManagment.Instance.CurrentStatus == GameManagment.Status.PLAYING;
    }
    private void OnMouseOver() { 
        if(GameManagment.Instance.SelectedPlayer != null && this.canClick)
            GetComponent<SpriteRenderer>().color = this.ColorOnMouseOver;

        this.isMouseOver = true;   
    }

    private void OnMouseExit() {
        GetComponent<SpriteRenderer>().color = this.ColorOnMouseExit;
        this.isMouseOver = false;
    }
    
}
