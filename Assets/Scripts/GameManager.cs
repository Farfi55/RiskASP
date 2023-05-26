using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actions;
using Cards;
using Extensions;
using Map;
using player;
using TurnPhases;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private TerritoryRepository _tr;
    private CardRepository _cr;
    private BattleSimulator _bs;

    public int NPlayers => _nPlayers;
    [SerializeField, Range(2, 6)] private int _nPlayers = 2;

    public List<Player> Players;

    private Queue<Player> _playerQueue = new();
    public List<Player> GetPlayersInTurnOrder() => _playerQueue.ToList();

    public Player CurrentPlayer => _currentPlayer;
    private Player _currentPlayer;

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
    public Action<Player> OnPlayerTurnEnded;
    
    public Action<GamePhase> OnGamePhaseChanged;


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
        _cr = CardRepository.Instance;
        _bs = BattleSimulator.Instance;
        SetupPhases();
        CreatePlayers();

        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        OnPlayerTurnEnded += TryDrawCardOnTurnEnd;
        foreach (var player in Players)
        {
            player.OnEliminated += tuple => OnPlayerEliminated(tuple.eliminatedBy, tuple.eliminated);
        }
    }

    private void OnPlayerEliminated(Player eliminatedBy, Player eliminated)
    {
        var cards = new List<Card>(eliminated.Cards);
        eliminatedBy.AddCards(cards);
        eliminated.RemoveCards(cards);
        
        if(Players.Count(player => player.IsAlive()) == 1)
            GameOver();
    }

    private void SetupPhases()
    {
        ReinforcePhase = new ReinforcePhase(this, _cr);
        AttackPhase = new AttackPhase(this, _bs);
        FortifyPhase = new FortifyPhase(this);
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
        
        
        // TODO REMOVE DEBUG COD
        foreach (var player in Players)
        {
            const int cardsToDraw = 4;
            for (int i = 0; i < cardsToDraw; i++)
            {
                player.AddCard(_cr.DrawRandomCard());
            }
        }
    
    }

    private void CreatePlayers()
    {
        if (Players.Count == 0)
            Players.AddRange(FindObjectsByType<Player>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));

        var playerCreator = PlayerCreator.Instance;
        
        for (var i = Players.Count; i < NPlayers; i++) 
            Players.Add(playerCreator.CreateBotPlayer());

        foreach (var player in Players)
            if (player.Name == "")
            {
                if(player.Color == null || player.Color.name == "UNDEFINED")
                    playerCreator.SetUpPlayerFromRandomColor(player);
                else
                    playerCreator.SetUpPlayerFromColor(player, player.Color);
            }
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

    private void TryDrawCardOnTurnEnd(Player player)
    {
        if (AttackPhase.ConqueredTerritoriesCount == 0 || player.Cards.Count >= 5)
            return;

        var card = _cr.DrawRandomCard();
        player.AddCard(card);
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
        if (oldPlayer != null)
            OnPlayerTurnEnded?.Invoke(oldPlayer);

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
        OnGamePhaseChanged?.Invoke(gamePhase);
    }


    private void GameOver()
    {
        SetGamePhase(GamePhase.Over);
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