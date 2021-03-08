using System.Globalization;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public List<Transform> positions;
    public static Board Instance;
    // Start is called before the first frame update

    public List<List<int>> ValideMovementsPosition = new List<List<int>>();
    void Start()
    {
        if(Instance == null)
            Instance = this;
        
        this.setPositions();
        this.setPlayers();
        this.setAllValidsMovements();
    }

    float speed = 15;
    bool InvokeNextPlayer = false;
    // Update is called once per frame
    void Update()
    {
        if(GameManagment.Instance.CurrentStatus == GameManagment.Status.MOVING_PLAYER){
            Player _SelectedPlayer = GameManagment.Instance.SelectedPlayer;
            PositionCollider _SelectedNextPosition = GameManagment.Instance.SelectedNextPosition;

            _SelectedPlayer.GetComponent<Transform>().position = new Vector3(
                this.learp(_SelectedPlayer.GetComponent<Transform>().position.x, _SelectedNextPosition.GetComponent<Transform>().position.x, speed * Time.deltaTime),
                this.learp(_SelectedPlayer.GetComponent<Transform>().position.y, _SelectedNextPosition.GetComponent<Transform>().position.y, speed * Time.deltaTime),
                0
                );
                if(!InvokeNextPlayer){
                    InvokeNextPlayer = true;
                    Invoke("nextPlayer", 0.35f);
                }
        }

        if(
            GameManagment.Instance.CurrentGameType == GameManagment.GameType.PLAYER_VS_CPU &&
            GameManagment.Instance.PlayerTurn == GameManagment.Players.PLAYER_2 && 
            GameManagment.Instance.CurrentStatus == GameManagment.Status.PLAYING
        )
        {
            this.CPUCheckPlayerMovements();
            if(this._CPUPossibleMovements.Count == 1 && this._CPUPossibleMovements[0].Count == 1){
                
                GameManagment.Instance.CurrentStatus = GameManagment.Status.MOVING_PLAYER;
                GameManagment.Instance.SelectedPlayer = this._CPUPlayerMovement;
                GameManagment.Instance.SelectedNextPosition = this.positions[this._player2PossibilitiesToWin[0]].GetComponent<PositionCollider>();
            } else if(this._CPUPossibleMovements.Count > 1){
                this.CPUCheckBestMovement();
            }
        }
    }

    [HideInInspector]
    List<List<PositionCollider>> _CPUPossibleMovements;
    void CPUCheckPlayerMovements(){
        List<Player> CPUPlayer = Board.Instance.Player2;
        this._CPUPossibleMovements = new List<List<PositionCollider>>();
        this._CPUPlayerMovementList = new List<Player>();
        
        var i = 0;
        foreach(Player player in CPUPlayer){
            List<PositionCollider> _possiblemovements = player.GetPossibleMovements();
            if(_possiblemovements.Count > 0){
                this._CPUPossibleMovements.Add(new List<PositionCollider>());
                this._CPUPossibleMovements[i].AddRange(player.GetPossibleMovements());
                if(i == 0)
                    this._CPUPlayerMovement = player;
                _CPUPlayerMovementList.Add(player);
                i ++;
            }
        }
    }


    [HideInInspector]
    List<int> _player1PossibilitiesToWin = new List<int>();
    [HideInInspector]
    List<int> _player2PossibilitiesToWin = new List<int>();
    Player _CPUPlayerMovement;
    List<Player> _CPUPlayerMovementList;
    void CPUCheckBestMovement(){
        
        this.checkMovementToWin(GameManagment.Players.PLAYER_1);
        if(_player1PossibilitiesToWin.Count > 0){

            List<int> _positionMovements = this._CPUPossibleMovements.SelectMany(_movements => _movements)
            .Where(_movement => this._player1PossibilitiesToWin.Contains(_movement.CurrentPosition)).Select(_currentPosition => _currentPosition.CurrentPosition)
            .ToList();
            bool _FoundPlayerToMove = false;
            if(_positionMovements.Count > 0 ){
                
                foreach(Player _player in this.Player2){
                    foreach(int _movement in this.ValideMovementsPosition[_player.CurrentPosition]){
                        if(_positionMovements.Contains(_movement)){
                            GameManagment.Instance.CurrentStatus = GameManagment.Status.MOVING_PLAYER;
                            GameManagment.Instance.SelectedPlayer = _player;
                            GameManagment.Instance.SelectedNextPosition = this.positions[_movement].GetComponent<PositionCollider>();
                            _FoundPlayerToMove = true;
                        }
                        if(_FoundPlayerToMove)
                            break;
                    }
                    if(_FoundPlayerToMove)
                        break;
                }
                if(!_FoundPlayerToMove)
                    this.CheckMovementToWin_Player2();
            } else {
                this.CheckMovementToWin_Player2();
            }

        } else { 
           this.CheckMovementToWin_Player2();
        }
    }

    void CheckMovementToWin_Player2(){
        this.checkMovementToWin(GameManagment.Players.PLAYER_2);
        if(this._player2PossibilitiesToWin.Count > 0){
            GameManagment.Instance.CurrentStatus = GameManagment.Status.MOVING_PLAYER;
            GameManagment.Instance.SelectedPlayer = this._CPUPlayerMovement;
            GameManagment.Instance.SelectedNextPosition = this.positions[this._player2PossibilitiesToWin[0]].GetComponent<PositionCollider>();
        } else {
            int _playerIndex = Random.Range(0, this._CPUPlayerMovementList.Count());
            int _playerMovementIndex = Random.Range(0, this._CPUPossibleMovements[_playerIndex].Count());

            GameManagment.Instance.CurrentStatus = GameManagment.Status.MOVING_PLAYER;
            GameManagment.Instance.SelectedPlayer = this._CPUPlayerMovementList[_playerIndex];
            GameManagment.Instance.SelectedNextPosition = this._CPUPossibleMovements[_playerIndex][_playerMovementIndex];
        }
}

    void checkMovementToWin(GameManagment.Players playerCheck){
        // check player 1
        List<List<PositionCollider>> _player1PossibleMovements = new List<List<PositionCollider>>();
        this._player1PossibilitiesToWin = new List<int>();
        this._player2PossibilitiesToWin = new List<int>();

        int i = 0;
        if(GameManagment.Players.PLAYER_1 == playerCheck){
            foreach(Player _player in this.Player1){
                if(playerCheck == GameManagment.Players.PLAYER_1){
                    _player1PossibleMovements.Add(new List<PositionCollider>());
                    _player1PossibleMovements[i].AddRange(_player.GetPossibleMovements());
                }
                i++;
            }
        }else if(GameManagment.Players.PLAYER_2 == playerCheck){
            foreach(Player _player in this.Player2){
                if(playerCheck == GameManagment.Players.PLAYER_2){
                    _player1PossibleMovements.Add(new List<PositionCollider>());
                    _player1PossibleMovements[i].AddRange(_player.GetPossibleMovements());
                }
                i++;
            }
        }
        
        // can win
        List<int> _player1_positions = new List<int>();
        List<Player> _playerList = this.Player1;
        if(playerCheck == GameManagment.Players.PLAYER_2)
            _playerList = this.Player2;

        foreach(Player _player in _playerList)
            _player1_positions.Add(_player.CurrentPosition);

        for(i = 0; i < _player1PossibleMovements.Count; i++){
           if(_player1PossibleMovements[i].Count > 0){
               for(int possibleMovement = 0; possibleMovement < _player1PossibleMovements[i].Count; possibleMovement++){
                    List<int> _playerPositions = new List<int>();
                    _player1_positions.AddRange(_player1_positions);
                    _player1_positions[i] = _player1PossibleMovements[i][possibleMovement].CurrentPosition;
                    _player1_positions.Sort();
                    for(int option = 0; option < 8; option++){
                        if(_player1_positions.ToArray().SequenceEqual(GameManagment.Instance.winValidate[option])){
                            if(playerCheck == GameManagment.Players.PLAYER_1){
                                this._player1PossibilitiesToWin.Add(_player1PossibleMovements[i][possibleMovement].CurrentPosition);
                            }else{
                                this._player2PossibilitiesToWin.Add(_player1PossibleMovements[i][possibleMovement].CurrentPosition);
                                this._CPUPlayerMovement = this.Player2[i];
                            }
                        }
                    }
               }
           }
        }
    }

    void nextPlayer(){
        GameManagment.Instance.SelectedPlayer.CurrentPosition = GameManagment.Instance.SelectedNextPosition.CurrentPosition;
        GameManagment.Instance.SelectedPlayer.GetComponent<Transform>().position = new Vector3(
            GameManagment.Instance.SelectedNextPosition.GetComponent<Transform>().position.x,
            GameManagment.Instance.SelectedNextPosition.GetComponent<Transform>().position.y,
            GameManagment.Instance.SelectedNextPosition.GetComponent<Transform>().position.z
        );
        GameManagment.Instance.SelectedNextPosition = null;
        GameManagment.Instance.SelectedPlayer = null;
        GameManagment.Instance.CurrentStatus = GameManagment.Status.PLAYING;
        InvokeNextPlayer = false;
        
        if(GameManagment.Instance.PlayerTurn == GameManagment.Players.PLAYER_1)
            GameManagment.Instance.PlayerTurn = GameManagment.Players.PLAYER_2;
        else
            GameManagment.Instance.PlayerTurn = GameManagment.Players.PLAYER_1;

        
        this.ActiveAllFreePositions();
    }

    float learp(float min, float max, float value){
        return min + (max - min) * value;   
    }

    #region start the game
    public List<GameObject> Players;
    [HideInInspector]
    public List<Player> Player1;
    [HideInInspector]
    public List<Player> Player2;

    void setPlayers(){
        this.Player1 = new List<Player>();
        this.Player2 = new List<Player>();

        this.Player2.Add(Instantiate(this.Players[1], this.positions[0].position, Quaternion.identity).GetComponent<Player>());
        this.Player2.Add(Instantiate(this.Players[1], this.positions[7].position, Quaternion.identity).GetComponent<Player>());
        this.Player2.Add(Instantiate(this.Players[1], this.positions[2].position, Quaternion.identity).GetComponent<Player>());

        this.Player1.Add(Instantiate(this.Players[0], this.positions[6].position, Quaternion.identity).GetComponent<Player>());
        this.Player1.Add(Instantiate(this.Players[0], this.positions[1].position, Quaternion.identity).GetComponent<Player>());
        this.Player1.Add(Instantiate(this.Players[0], this.positions[8].position, Quaternion.identity).GetComponent<Player>());

        this.Player1[0].CurrentPosition = 6;
        this.Player1[1].CurrentPosition = 1;
        this.Player1[2].CurrentPosition = 8;
        
        this.Player2[0].CurrentPosition = 0;
        this.Player2[1].CurrentPosition = 7;
        this.Player2[2].CurrentPosition = 2;
    }

    public void ActiveAllFreePositions(){
        
        foreach(PositionCollider positionCollider in this.PositionCollidesList)
            positionCollider.GetComponent<Transform>().gameObject.SetActive(false);
        
        List<int> _player_positions = new List<int>();

        List<Player> _player1List = this.Player1;
        List<Player> _player2List = this.Player2;

        foreach(Player _player in _player1List)
            _player_positions.Add(_player.CurrentPosition);

        foreach(Player _player in _player2List)
            _player_positions.Add(_player.CurrentPosition);
        
        IEnumerable<PositionCollider> positionsCollideDesible =  from _PositionCollider in this.PositionCollidesList where !_player_positions.Contains(_PositionCollider.CurrentPosition) select _PositionCollider;
        foreach(PositionCollider positionCollider in positionsCollideDesible)
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

    public bool hasAPlayerIn(GameManagment.Players playerType, int position){
        List<Player> _players = this.Player1;
        if(playerType == GameManagment.Players.PLAYER_2)
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
    void setAllValidsMovements(){
        
        List<int> _valideMoviments = new List<int>();
        _valideMoviments.AddRange(new int[] {1,3,4});
        this.ValideMovementsPosition.Add(_valideMoviments);

        _valideMoviments = new List<int>();
        _valideMoviments.AddRange(new int[] {0,2,4});
        this.ValideMovementsPosition.Add(_valideMoviments);

        _valideMoviments = new List<int>();
        _valideMoviments.AddRange(new int[] {1,4,5});
        this.ValideMovementsPosition.Add(_valideMoviments);

        _valideMoviments = new List<int>();
        _valideMoviments.AddRange(new int[] {0,4,6});
        this.ValideMovementsPosition.Add(_valideMoviments);

        _valideMoviments = new List<int>();
        _valideMoviments.AddRange(new int[] {0,1,2,3,5,6,7,8});
        this.ValideMovementsPosition.Add(_valideMoviments);

        _valideMoviments = new List<int>();
        _valideMoviments.AddRange(new int[] {2,4,8});
        this.ValideMovementsPosition.Add(_valideMoviments);

        _valideMoviments = new List<int>();
        _valideMoviments.AddRange(new int[] {3,4,7});
        this.ValideMovementsPosition.Add(_valideMoviments);

        _valideMoviments = new List<int>();
        _valideMoviments.AddRange(new int[] {6,4,8});
        this.ValideMovementsPosition.Add(_valideMoviments);

        _valideMoviments = new List<int>();
        _valideMoviments.AddRange(new int[] {5,4,7});
        this.ValideMovementsPosition.Add(_valideMoviments);
    }
    #endregion
}
