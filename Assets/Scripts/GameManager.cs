using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using map;
using player;
using Turn.Phases;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private TerritoryRepository _tr;
    private ContinentRepository _cr;

    [SerializeField]
    private int _nPlayers = 2;
    public int NPlayers => _nPlayers;
    
    public List<Player> Players;

    private Queue<Player> _playerQueue = new();
    private Player _currentPlayer;
    public Player CurrentPlayer => _currentPlayer;

    private IPhase _currentPhase;
    public IPhase CurrentPhase => _currentPhase;

    public int Turn => _turn;
    private int _turn;
  
    public ReinforcePhase _reinforcePhase { get; private set; }
    public AttackPhase _attackPhase { get; private set; }
    public FortifyPhase _fortifyPhase { get; private set; }
    public EmptyPhase _emptyPhase { get; private set; }

      
    public Action<IPhase, IPhase> OnTurnPhaseChanged;
    public Action<Player, Player> OnPlayerTurnChanged;
    

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

        _tr = TerritoryRepository.Instance;
        _cr = ContinentRepository.Instance;
        SetupPhases();
    }

    private void SetupPhases()
    {
        var cr = ContinentRepository.Instance;
        var tr = TerritoryRepository.Instance;
        _reinforcePhase = new ReinforcePhase(this, cr, tr);
        _attackPhase = new AttackPhase(this, cr, tr);
        _fortifyPhase = new FortifyPhase(this, cr, tr);
        _emptyPhase = new EmptyPhase();
        SetTurnPhase(_emptyPhase);
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
            ReinforcePhase => _attackPhase,
            AttackPhase => _fortifyPhase,
            FortifyPhase => _emptyPhase,
            EmptyPhase => _emptyPhase,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        SetTurnPhase(nextTurnPhase);

        
        if (_currentPhase == _emptyPhase)
        { 
            NextTurn();
        }
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
        SetTurnPhase(_reinforcePhase);
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
