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
    [SerializeField, Range(2, 6)] private int _nPlayers = 2;

    public List<Player> Players;

    private Queue<Player> _playerQueue = new();

    public Player CurrentPlayer => _currentPlayer;
    private Player _currentPlayer;

    public TerritoryRepository TerritoryRepository => _tr;

    public GamePhase GamePhase => _gamePhase;
    private GamePhase _gamePhase = GamePhase.Setup;

    public int Turn => _turn;
    private int _turn;

    public IPhase CurrentPhase => _currentPhase;
    private IPhase _currentPhase;


    public ReinforcePhase ReinforcePhase { get; private set; }
    public AttackPhase AttackPhase { get; private set; }
    public FortifyPhase FortifyPhase { get; private set; }
    public EmptyPhase EmptyPhase { get; private set; }


    public Action<IPhase> OnPhaseStarted;
    public Action<IPhase> OnPhaseEnded;
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

        SetGamePhase(GamePhase.Setup);

        _tr = TerritoryRepository.Instance;
        _cr = ContinentRepository.Instance;
        _bs = BattleSimulator.Instance;
        SetupPhases();
        CreatePlayers();
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
        SetGamePhase(GamePhase.Playing);
        NextTurn();
    }

    private void SetupGame()
    {
        _tr.RandomlyAssignTerritories(Players);
        DistributeTroops();
        EnqueuePlayers();
        _turn = 0;
    }

    private void CreatePlayers()
    {
        if (Players.Count == 0)
            Players.AddRange(FindObjectsByType<Player>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));

        for (var i = Players.Count; i < NPlayers; i++)
            Players.Add(PlayerCreator.Instance.CreateBotPlayer());

        foreach (var player in Players)
            if (player.Name == "")
                PlayerCreator.Instance.SetUpPlayerFromRandomColor(player);
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
        int[] troopsPerNumberOfPlayer = { -1, -1, 40, 35, 30, 25, 20 };

        foreach (var player in Players)
        {
            int troopsPerPlayer = troopsPerNumberOfPlayer[NPlayers];
            player.ClearTroops();
            troopsPerPlayer = player.DistributeNTroopsPerTerritory(1, troopsPerPlayer);
            player.RandomlyDistributeTroops(troopsPerPlayer);
        }
    }

    public void NextTurnPhase()
    {
        EndTurnPhase();
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
        if (!action.IsValid())
        {
            Debug.LogWarning("Invalid action" + action);
            return;
        }

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

    private void StartTurnPhase()
    {
        _currentPhase.Start(_currentPlayer);
        OnPhaseStarted?.Invoke(_currentPhase);
    }

    private void EndTurnPhase()
    {
        _currentPhase.End(_currentPlayer);
        OnPhaseEnded?.Invoke(_currentPhase);
    }

    private void SetTurnPhase(IPhase phase)
    {
        var oldPhase = _currentPhase;
        _currentPhase = phase;
        StartTurnPhase();
        OnTurnPhaseChanged?.Invoke(oldPhase, _currentPhase);
    }

    private void SetGamePhase(GamePhase gamePhase)
    {
        _gamePhase = gamePhase;
    }


    private void GameOver()
    {
        SetGamePhase(GamePhase.Over);
        throw new NotImplementedException();
    }

    public bool IsCurrentPlayer(Player player)
    {
        return _currentPlayer == player;
    }
}

public enum GamePhase
{
    Setup,
    Playing,
    Over
}