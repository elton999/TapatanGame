using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CpuPlayer : MonoBehaviour
{
    private GameManagement _gameManagement { get => GameManagement.Instance; }
    private void Update()
    {
        if(
            _gameManagement.CurrentGameType == GameManagement.GameType.PLAYER_VS_CPU &&
            _gameManagement.PlayerTurn == GameManagement.Players.PLAYER_2 && 
            _gameManagement.CurrentStatus == GameManagement.Status.PLAYING
        )
        {
            this.CPUCheckPlayerMovements();
            if(this._CPUPossibleMovements.Count == 1 && this._CPUPossibleMovements[0].Count == 1){
                
                _gameManagement.CurrentStatus = GameManagement.Status.MOVING_PLAYER;
                _gameManagement.SelectedPlayer = this._CPUPlayerMovement;
                _gameManagement.SelectedNextPosition = Board.Instance.positions[this._player2PossibilitiesToWin[0]].GetComponent<PositionCollider>();
            
            }
            
            if(this._CPUPossibleMovements.Count > 1)
                this.CPUCheckBestMovement();
        }
    }
    
    [HideInInspector]
    List<List<PositionCollider>> _CPUPossibleMovements;
    void CPUCheckPlayerMovements(){
        List<Player> CPUPlayer = Board.Instance.Player2;
        this._CPUPossibleMovements = new List<List<PositionCollider>>();
        this._CPUPlayerMovementList = new List<Player>();

        for(int i = 0; i < CPUPlayer.Count; i++){
            List<PositionCollider> _possiblemovements = CPUPlayer[i].GetPossibleMovements();
            
            if(_possiblemovements.Count > 0){
                this._CPUPossibleMovements.Add(new List<PositionCollider>());
                this._CPUPossibleMovements[i].AddRange(CPUPlayer[i].GetPossibleMovements());
                if(i == 0)
                    this._CPUPlayerMovement = CPUPlayer[i];
                _CPUPlayerMovementList.Add(CPUPlayer[i]);
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
    private void CPUCheckBestMovement(){
        bool _FoundPlayerToMove = false;

        this.CheckMovement(GameManagement.Players.PLAYER_1);
        if(_player1PossibilitiesToWin.Count > 0){

            List<int> _positionMovements = this._CPUPossibleMovements.SelectMany(_movements => _movements)
            .Where(_movement => this._player1PossibilitiesToWin.Contains(_movement.CurrentPosition)).Select(_currentPosition => _currentPosition.CurrentPosition)
            .ToList();
            
            if(_positionMovements.Count > 0 ){
                
                foreach(Player _player in Board.Instance.Player2){
                    foreach(int _movement in Board.Instance.ValideMovementsPosition[_player.CurrentPosition]){
                        if(_positionMovements.Contains(_movement)){
                            _gameManagement.CurrentStatus = GameManagement.Status.MOVING_PLAYER;
                            _gameManagement.SelectedPlayer = _player;
                            _gameManagement.SelectedNextPosition = Board.Instance.positions[_movement].GetComponent<PositionCollider>();
                            _FoundPlayerToMove = true;
                        }
                        if(_FoundPlayerToMove)
                            break;
                    }
                }
            } else this.CheckMovementToWin_Player2();
        }

        if(!(_player1PossibilitiesToWin.Count > 0) || !_FoundPlayerToMove)
            this.CheckMovementToWin_Player2();
    }

    private void CheckMovementToWin_Player2(){
        this.CheckMovement(GameManagement.Players.PLAYER_2);
        
        if(this._player2PossibilitiesToWin.Count > 0){
            _gameManagement.CurrentStatus = GameManagement.Status.MOVING_PLAYER;
            _gameManagement.SelectedPlayer = this._CPUPlayerMovement;
            _gameManagement.SelectedNextPosition = Board.Instance.positions[this._player2PossibilitiesToWin[0]].GetComponent<PositionCollider>();
        } else {
            int playerIndex = Random.Range(0, this._CPUPlayerMovementList.Count());
            int playerMovementIndex = Random.Range(0, this._CPUPossibleMovements[playerIndex].Count());

            _gameManagement.CurrentStatus = GameManagement.Status.MOVING_PLAYER;
            _gameManagement.SelectedPlayer = this._CPUPlayerMovementList[playerIndex];
            _gameManagement.SelectedNextPosition = this._CPUPossibleMovements[playerIndex][playerMovementIndex];
        }
}

    private void CheckMovement(GameManagement.Players playerCheck){
        List<List<PositionCollider>> player1PossibleMovements = this.GetPlayer1PossibleMovements(playerCheck);

        List<int> player1_positions = new List<int>();
        List<Player> playerList = playerCheck == GameManagement.Players.PLAYER_2 ? Board.Instance.Player2 : Board.Instance.Player1;

        foreach(Player _player in playerList)
            player1_positions.Add(_player.CurrentPosition);

        for(int i = 0; i < player1PossibleMovements.Count; i++){
            if(player1PossibleMovements[i].Count > 0){
                for(int possibleMovement = 0; possibleMovement < player1PossibleMovements[i].Count; possibleMovement++){
                    int currentPosition = player1PossibleMovements[i][possibleMovement].CurrentPosition;

                    player1_positions.AddRange(player1_positions);
                    player1_positions[i] = currentPosition;
                    player1_positions.Sort();
                    
                    for(int option = 0; option < 8; option++){
                        if(player1_positions.ToArray().SequenceEqual(_gameManagement.winValidate[option])){
                            if(playerCheck == GameManagement.Players.PLAYER_1){
                                this._player1PossibilitiesToWin.Add(currentPosition);
                            }else{
                                this._player2PossibilitiesToWin.Add(currentPosition);
                                this._CPUPlayerMovement = Board.Instance.Player2[i];
                            }
                        }
                    }
                }
            }
        }
    }

    private List<List<PositionCollider>>  GetPlayer1PossibleMovements(GameManagement.Players playerCheck){
        List<List<PositionCollider>> player1PossibleMovements = new List<List<PositionCollider>>();

        this._player1PossibilitiesToWin = new List<int>();
        this._player2PossibilitiesToWin = new List<int>();

        List<Player> players = GameManagement.Players.PLAYER_1 == playerCheck ? Board.Instance.Player1 : Board.Instance.Player2;

        for(int i = 0; i < players.Count; i++){
            player1PossibleMovements.Add(new List<PositionCollider>());
            player1PossibleMovements[i].AddRange(players[i].GetPossibleMovements());
        }

        return player1PossibleMovements;
    }
}
