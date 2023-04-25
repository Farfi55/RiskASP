using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actions;
using Extensions;
using Map;
using player;
using TurnPhases;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private TerritoryRepository _tr;
    private ContinentRepository _cr;
    private BattleSimulator _bs;

    public int NPlayers => _nPlayers;
    [SerializeField] private int _nPlayers = 2;
    
    public List<Player> Players;

    private Queue<Player> _playerQueue = new();
    
    public Player CurrentPlayer => _currentPlayer;
    private Player _currentPlayer;
    
    public int Turn => _turn;
    private int _turn;

    public IPhase CurrentPhase => _currentPhase;
    private IPhase _currentPhase;

    
    public ReinforcePhase ReinforcePhase { get; private set; }
    public AttackPhase AttackPhase { get; private set; }
    public FortifyPhase FortifyPhase { get; private set; }
    public EmptyPhase EmptyPhase { get; private set; }

      
    public Action<IPhase, IPhase> OnTurnPhaseChanged;
    public Action<Player, Player> OnPlayerTurnChanged;
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("There is more than one GameManager in the scene");
            Destroy(gameObject);
        }
        else
            Instance = this;

        _tr = TerritoryRepository.Instance;
        _cr = ContinentRepository.Instance;
        SetupPhases();
    }

    private void SetupPhases()
    {
        ReinforcePhase = new ReinforcePhase(this, _cr, _tr);
        AttackPhase = new AttackPhase(this, _cr, _tr, _bs);
        FortifyPhase = new FortifyPhase(this, _cr, _tr);
        EmptyPhase = new EmptyPhase();
        SetTurnPhase(EmptyPhase);
    }


    private void Start()
    {
        SetupGame();
        NextTurn();
    }

    private void SetupGame()
    {
        for (var i = Players.Count; i < NPlayers; i++)
        {
            Players.Add(PlayerCreator.Instance.NewPlayer());
        }
        

        _tr.RandomlyAssignTerritories(Players);
        DistributeTroops();
        EnqueuePlayers();
        _turn = 0;
    }

    private void EnqueuePlayers()
    {
        _playerQueue = new Queue<Player>();
        
        var playerOrder = Enumerable.Range(0, NPlayers).ToList();
        playerOrder.Shuffle();
        foreach (var i in playerOrder) 
            _playerQueue.Enqueue(Players[i]);
    }

    private void DistributeTroops()
    {
        int[] troopsPerNumberOfPlayer = { -1, -1, 50, 35, 30, 25, 20 };
        int troopsPerPlayer = troopsPerNumberOfPlayer[NPlayers];
        
        foreach (var player in Players)
            player.RandomlyDistributeTroops(troopsPerPlayer);
    }
    
    public void NextTurnPhase()
    {
        _currentPhase.End(_currentPlayer);
        IPhase nextTurnPhase = _currentPhase switch
        {
            global::TurnPhases.ReinforcePhase => AttackPhase,
            global::TurnPhases.AttackPhase => FortifyPhase,
            global::TurnPhases.FortifyPhase => EmptyPhase,
            global::TurnPhases.EmptyPhase => EmptyPhase,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        SetTurnPhase(nextTurnPhase);

        
        if (_currentPhase == EmptyPhase)
        { 
            NextTurn();
        }
    }
    
    public void HandlePlayerAction(PlayerAction action)
    {
        if (action.Player != _currentPlayer)
            throw new ArgumentException("PlayerAction is not from current player");

        _currentPhase.OnAction(_currentPlayer, action);
    }

    private void NextTurn()
    {
        var oldPlayer = _currentPlayer;
        do
        {
            _currentPlayer = _playerQueue.Dequeue();
        } while (_currentPlayer.IsDead());
        
        _playerQueue.Enqueue(_currentPlayer);
        
        _turn++;
        SetTurnPhase(ReinforcePhase);
        OnPlayerTurnChanged?.Invoke(oldPlayer, _currentPlayer);
    }

    private void StartTurnPhase() => _currentPhase.Start(_currentPlayer);

    private void SetTurnPhase(IPhase phase)
    {
        var oldPhase = _currentPhase;
        _currentPhase = phase;
        StartTurnPhase();
        OnTurnPhaseChanged?.Invoke(oldPhase, _currentPhase);
    }
    
    
    private void GameOver()
    {
        throw new NotImplementedException();
    }

}
