using System.Net.Mime;
using System.Globalization;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public List<Transform> positions;
    public static Board Instance;
    public Text WinText;
    public RectTransform WinScream;
    public List<List<int>> ValideMovementsPosition = new List<List<int>>();
    private GameManagement _gameManagement { get => GameManagement.Instance; }
    void Start()
    {
        if(Instance == null)
            Instance = this;
        
        this.setPositions();
        this.setPlayers();
        this.setAllValidsMovements();
    }

    public float speed = 15;
    bool InvokeNextPlayer = false;
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer(){
        if(_gameManagement.CurrentStatus == GameManagement.Status.MOVING_PLAYER){
            Player selectedPlayer = _gameManagement.SelectedPlayer;

            Vector3 selectedNextPosition = _gameManagement.SelectedNextPosition.GetComponent<Transform>().position;
            Vector3 selectedPlayerPosition = selectedPlayer.GetComponent<Transform>().position;

            selectedPlayer.GetComponent<Transform>().position = Vector3.Lerp(
                selectedPlayerPosition,
                selectedNextPosition,
                speed * Time.deltaTime
            );

            
            if(!InvokeNextPlayer){
                InvokeNextPlayer = true;
                Invoke("nextPlayer", 0.35f);
            }
        }
    }

    void nextPlayer(){
        Player player = _gameManagement.SelectedPlayer;
        player.CurrentPosition = _gameManagement.SelectedNextPosition.CurrentPosition;
        player.GetComponent<Transform>().position = _gameManagement.SelectedNextPosition.GetComponent<Transform>().position;

        _gameManagement.SelectedNextPosition = null;
        _gameManagement.SelectedPlayer = null;
        _gameManagement.CurrentStatus = GameManagement.Status.PLAYING;
        InvokeNextPlayer = false;
        
        if(_gameManagement.PlayerTurn == GameManagement.Players.PLAYER_1)
            _gameManagement.PlayerTurn = GameManagement.Players.PLAYER_2;
        else
            _gameManagement.PlayerTurn = GameManagement.Players.PLAYER_1;

        
        this.ActiveAllFreePositions();
    }

    #region start the game
    public List<GameObject> Players;
    [HideInInspector]
    public List<Player> Player1;
    [HideInInspector]
    public List<Player> Player2;
    void setPlayers(){

        int[] player1Positions = new int[] { 6,1,8 };
        int[] player2Positions = new int[] { 0,7,2 };

        this.Player1 = new List<Player>();
        this.Player2 = new List<Player>();

        for(int i = 0; i < 3; i++){
            this.Player1.Add(Instantiate(this.Players[0], this.positions[player1Positions[i]].position, Quaternion.identity).GetComponent<Player>());
            this.Player2.Add(Instantiate(this.Players[1], this.positions[player2Positions[i]].position, Quaternion.identity).GetComponent<Player>());
            
            this.Player1[i].CurrentPosition = player1Positions[i];
            this.Player2[i].CurrentPosition = player2Positions[i];
        }
    }

    public void ActiveAllFreePositions(){
        
        foreach(PositionCollider positionCollider in this.PositionCollidesList)
            positionCollider.GetComponent<Transform>().gameObject.SetActive(false);
        
        List<int> _player_positions = new List<int>();

        foreach(Player _player in this.Player1)
            _player_positions.Add(_player.CurrentPosition);

        foreach(Player _player in this.Player2)
            _player_positions.Add(_player.CurrentPosition);
        
        IEnumerable<PositionCollider> positionsCollideDesable =  from _PositionCollider in this.PositionCollidesList where !_player_positions.Contains(_PositionCollider.CurrentPosition) select _PositionCollider;
        
        foreach(PositionCollider positionCollider in positionsCollideDesable)
            positionCollider.GetComponent<Transform>().gameObject.SetActive(true);
    }

    [HideInInspector]
    public List<PositionCollider> PositionCollidesList;
    public GameObject PositionPreFab;
    void setPositions(){
        this.PositionCollidesList = new List<PositionCollider>();

        for(var i = 0; i < this.positions.Count; i++){
            this.PositionCollidesList.Add(Instantiate(this.PositionPreFab, this.positions[i].position, Quaternion.identity).GetComponent<PositionCollider>());
            this.PositionCollidesList[i].CurrentPosition = i;
        }
    }
    #endregion

    public bool hasAPlayerIn(GameManagement.Players playerType, int position){
        List<Player> _players = this.Player1;
        if(playerType == GameManagement.Players.PLAYER_2)
            _players = this.Player2;

        IEnumerable<Player> _playerPositionsOver = from _player in _players where _player.CurrentPosition == position select _player;
        if(_playerPositionsOver.Count() > 0)
            return true;
        return false;
    }

    public void setColorPlayers(){
        foreach(Player _player in this.Player1)
            _player.GetComponent<Transform>().GetComponent<SpriteRenderer>().color = _player.Color;
        
        foreach(Player _player in this.Player2)
            _player.GetComponent<Transform>().GetComponent<SpriteRenderer>().color = _player.Color;
    }

    #region validsMovements
    private void setAllValidsMovements(){

        SetValidMovement(new int[] {1,3,4});
        SetValidMovement(new int[] {0,2,4});
        SetValidMovement(new int[] {1,4,5});
        SetValidMovement(new int[] {0,4,6});
        SetValidMovement(new int[] {0,1,2,3,5,6,7,8});
        SetValidMovement(new int[] {2,4,8});
        SetValidMovement(new int[] {3,4,7});
        SetValidMovement(new int[] {6,4,8});
        SetValidMovement(new int[] {5,4,7});
    }

    private void SetValidMovement(int[] positions){
         List<int> valideMovements = new List<int>();
        valideMovements.AddRange(positions);
        this.ValideMovementsPosition.Add(valideMovements);
    }
    #endregion
}
