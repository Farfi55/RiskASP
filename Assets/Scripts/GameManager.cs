using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using map;
using player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int _nPlayers = 2;
    public List<Player> Players;

    private Queue<Player> _playerQueue = new();
    private GameState _gameState;
    private TurnPhase _turnPhase;
    private Player _currentPlayer;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        SetupGame();
        PlayGame();
    }

    private void SetupGame()
    {
        _gameState = GameState.Setup;

        for (var i = 0; i < _nPlayers; i++)
        {
            Players.Add(PlayerCreator.Instance.NewPlayer());
        }

        TerritoryRepository.Instance.RandomlyAssignTerritories(Players);
        DistributeTroops();
        Players.Shuffle();
        _playerQueue = new Queue<Player>(Players);
    }

    private void DistributeTroops()
    {
        int[] troopsPerNumberOfPlayer = { -1, -1, 50, 35, 30, 25, 20 };
        int troopsPerPlayer = troopsPerNumberOfPlayer[_nPlayers];
        
        foreach (var player in Players)
            player.RandomlyDistributeTroops(troopsPerPlayer);
    }

    private void PlayGame()
    {
        _gameState = GameState.Play;
        NextTurn();
    }


    private void NextTurn()
    {
        _turnPhase = TurnPhase.Reinforce;
        _currentPlayer = _playerQueue.Dequeue();
        StartNextTurnPhase(_currentPlayer);
        
    }

    private void StartNextTurnPhase(Player player)
    {
        switch (_turnPhase)
        {
            case TurnPhase.Reinforce:
                StartReinforcePhase(player);
                break;
            case TurnPhase.Attack:
                StartAttackPhase(player);
                break;
            case TurnPhase.Fortify:
                StartFortifyPhase(player);
                break;
            case TurnPhase.End:
            default:
                throw new ArgumentOutOfRangeException();
        }

    }


    private void StartReinforcePhase(Player player)
    {
        var troopsToDistribute = player.GetTotalTroopBonus();
        // todo: tell AI how many troops they have to distribute
        // get all reinforcements actions from AI
        // validate actions
        while (troopsToDistribute > 0)
        {
            // wait for reinforce action
            var enumerator = ActionReader.Instance.ReadNextReinforceAction();
            var reinforceAction = enumerator.Current;
            if (reinforceAction == null)
            {
                reinforceAction = enumerator.Current;
            }
        }
    }

    private void StartAttackPhase(Player player)
    {
        throw new NotImplementedException();
    }

    private void StartFortifyPhase(Player player)
    {
        throw new NotImplementedException();
    }


    private void GameOver()
    {
        throw new NotImplementedException();
    }
}

internal class ReinforceAction
{
    public Territory Territory { get; private set; }

    public int Troops { get; private set; }

    public ReinforceAction(Territory territory, int troops)
    {
        Territory = territory;
        Troops = troops;
    }
}


internal enum GameState
{
    Setup,
    Play,
    End
}

internal enum TurnPhase
{
    Reinforce,
    Attack,
    Fortify,
    End
}