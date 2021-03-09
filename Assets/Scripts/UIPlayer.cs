using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public Color Player1Color;
    public Color Player2Color;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Text text = this.GetComponent<RectTransform>().GetComponent<Text>();
        if(GameManagment.Instance.PlayerTurn == GameManagment.Players.PLAYER_1){
            text.color = this.Player1Color;
            text.text = "Player1";
        } else {
            text.color = this.Player2Color;
            if(GameManagment.Instance.CurrentGameType == GameManagment.GameType.PLAYER_VS_PLAYER){
                text.text = "Player2";
            } else {
                text.text = "CPU";
            }
        }
    }
}
