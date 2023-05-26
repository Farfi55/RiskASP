using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using player;
using UnityEngine;

namespace UI
{
    public class UIPlayerInfoManager : MonoBehaviour
    {
        GameManager _gm;
        [SerializeField] private Transform _playerInfoContainer;
        [SerializeField] private UIPlayerInfo _playerInfoPrefab;

        private readonly List<UIPlayerInfo> _playerInfos = new();

        private void Awake()
        {
            _gm = GameManager.Instance;
            _gm.Players.ForEach(CreatePlayerInfo);
            _gm.OnGamePhaseChanged += phase =>
            {
                if (phase == GamePhase.Playing)
                {
                    OrderPlayerInfos(_gm.GetPlayersInTurnOrder());
                }
            };
        }

        private void OrderPlayerInfos(List<Player> playersInTurnOrder)
        {
            for (int i = 0; i < playersInTurnOrder.Count; i++)
            {
                var player = playersInTurnOrder[i];
                var uiPlayerInfo = _playerInfos.Find(info => info.Player == player);
                uiPlayerInfo.transform.SetSiblingIndex(i);
            }
        }

        private void CreatePlayerInfo(Player player)
        {
            var uiPlayerInfo = Instantiate(_playerInfoPrefab, _playerInfoContainer);
            uiPlayerInfo.Player = player;
            _playerInfos.Add(uiPlayerInfo);
        }
    }
}
