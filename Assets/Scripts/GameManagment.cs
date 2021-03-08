using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagment : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManagment Instance;
    void Start()
    {
        if(Instance == null)
            Instance = this;
    }

    public enum Players  {PLAYER_1, PLAYER_2};
    public enum GameType {PLAYER_VS_PLAYER, PLAYER_VS_CPU};
    public GameType CurrentGameType = GameType.PLAYER_VS_CPU;
    public enum Status {PLAYING, MOVING_PLAYER, PLAYER_1_WIN, PLAYER_2_WIN}
    public Status CurrentStatus = GameManagment.Status.PLAYING;
    public Players PlayerTurn = GameManagment.Players.PLAYER_1;
    public Player SelectedPlayer;
    public PositionCollider SelectedNextPosition;

    [HideInInspector]
    public List<int> ValidMoviments;
    void Update()
    {
        if(this.CurrentStatus == Status.PLAYING)
            this.CheckWinner();
    }

    [HideInInspector]
    public int[][] winValidate = new int[][] { 
        new int[] {0,1,2}, 
        new int[] {3,4,5},  
        new int[] {6,7,8}, 
        new int[] {0,3,6}, 
        new int[] {1,4,7}, 
        new int[] {2,5,8}, 
        new int[] {0,4,8}, 
        new int[] {2,4,6}
    };

    void CheckWinner(){
        List<int> _player1_positions = new List<int>();
        List<int> _player2_positions = new List<int>();

        List<Player> _player1List = Board.Instance.Player1;
        List<Player> _player2List = Board.Instance.Player2;

        foreach(Player _player in _player1List)
            _player1_positions.Add(_player.CurrentPosition);

        foreach(Player _player in _player2List)
            _player2_positions.Add(_player.CurrentPosition);

        _player1_positions.Sort();
        _player2_positions.Sort();
        
        for(int option = 0; option < 8; option++){
            if(_player1_positions.ToArray().SequenceEqual(this.winValidate[option]))
                this.CurrentStatus = Status.PLAYER_1_WIN;

            if(_player2_positions.ToArray().SequenceEqual(this.winValidate[option]))
                this.CurrentStatus = Status.PLAYER_2_WIN;
        }
        
    }
}
