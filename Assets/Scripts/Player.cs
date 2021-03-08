using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color Color;
    public Color SelectedColor;
    public GameManagment.Players PlayerType;
    public int CurrentPosition;
    void Start()
    {
        this.setColor();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.isMouseOver && Input.GetMouseButtonDown(0) && GameManagment.Instance.CurrentStatus == GameManagment.Status.PLAYING){
            GameManagment.Instance.SelectedPlayer = this;
            GameManagment.Instance.ValidMoviments = Board.Instance.ValideMovementsPosition[this.CurrentPosition];

            Board.Instance.setColorPlayers();

            GetComponent<SpriteRenderer>().color = this.SelectedColor;
        }
    }

    void setColor(){
        this.GetComponent<SpriteRenderer>().color = this.Color;
    }

    bool isMouseOver = false;
    private void OnMouseEnter() {
        if(GameManagment.Instance.PlayerTurn == this.PlayerType && GameManagment.Instance.CurrentStatus == GameManagment.Status.PLAYING){
            isMouseOver = true;
            GetComponent<Animator>().SetBool("mouse over", true);
        }
    }

    private void OnMouseExit() {
        isMouseOver = false;
        GetComponent<Animator>().SetBool("mouse over", false);
    }

    public List<PositionCollider> GetPossibleMovements(){
        List<PositionCollider> _valideMovements = new List<PositionCollider>();

        foreach(int movement in Board.Instance.ValideMovementsPosition[this.CurrentPosition]){
            if(
                !Board.Instance.hasAPlayerIn(GameManagment.Players.PLAYER_2, movement) &&
                !Board.Instance.hasAPlayerIn(GameManagment.Players.PLAYER_1, movement)
            ){
                _valideMovements.Add(Board.Instance.PositionCollidesList[movement]);
            }
        }

        return _valideMovements;
    }

}
