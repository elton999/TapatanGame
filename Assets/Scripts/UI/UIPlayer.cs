using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    public Color Player1Color;
    public Color Player2Color;

    private GameManagement _gameManagement { get => GameManagement.Instance; }

    void Update()
    {
        Text text = this.GetComponent<RectTransform>().GetComponent<Text>();
        if(_gameManagement.PlayerTurn == GameManagement.Players.PLAYER_1){
            text.color = this.Player1Color;
            text.text = "Player1";
        } else {
            text.color = this.Player2Color;
            if(_gameManagement.CurrentGameType == GameManagement.GameType.PLAYER_VS_PLAYER){
                text.text = "Player2";
            } else {
                text.text = "CPU";
            }
        }
    }
}
