using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color Color;
    public Color SelectedColor;
    public GameManagement.Players PlayerType;
    private GameManagement _gameManagement { get => GameManagement.Instance; }
    public int CurrentPosition;
    void Start()
    {
        this.setColor();
    }

    void Update()
    {
        if(this.isMouseOver && Input.GetMouseButtonDown(0) && _gameManagement.CurrentStatus == GameManagement.Status.PLAYING){
            _gameManagement.SelectedPlayer = this;
            _gameManagement.ValidMoviments = Board.Instance.ValideMovementsPosition[this.CurrentPosition];

            Board.Instance.setColorPlayers();

            GetComponent<SpriteRenderer>().color = this.SelectedColor;
        }
    }

    void setColor(){
        this.GetComponent<SpriteRenderer>().color = this.Color;
    }

    bool isMouseOver = false;
    private void OnMouseEnter() {
        if(_gameManagement.PlayerTurn == this.PlayerType && _gameManagement.CurrentStatus == GameManagement.Status.PLAYING){
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
                !Board.Instance.hasAPlayerIn(GameManagement.Players.PLAYER_2, movement) &&
                !Board.Instance.hasAPlayerIn(GameManagement.Players.PLAYER_1, movement)
            ){
                _valideMovements.Add(Board.Instance.PositionCollidesList[movement]);
            }
        }

        return _valideMovements;
    }

}
