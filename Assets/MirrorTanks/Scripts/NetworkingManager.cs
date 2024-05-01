using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
namespace MirrorTanks
{
    public class NetworkingManager : NetworkManager
    {
        static NetworkingManager singleton;

        string localPlayerName;
        int localPlayerTeamID;
        Material localPlayerColor;
        List<NetworkingPlayer> Networkplayers;
        List<NetworkingPlayer> team1;
        List<NetworkingPlayer> team2;

        public static NetworkingManager Singleton => singleton;
        public bool IsServer { get; private set; }
        public bool IsClient { get; private set; }
        public bool IsHost => IsServer && IsClient;
        public string LocalPlayerName => localPlayerName;
        public int LocalPlayerTeamID => localPlayerTeamID;
        public Material LocalPlayerColor => localPlayerColor;
        public NetworkingPlayer LocalPlayer => Networkplayers.Find(x => x.isLocalPlayer);

        public List<NetworkingPlayer> Team1 => team1;
        public List<NetworkingPlayer> Team2 => team2;

        public override void Awake()
        {
            base.Awake();
            if (!singleton)
            {
                singleton = this;
            }
        }
        public override void Start()
        {
            base.Start();
            Networkplayers = new List<NetworkingPlayer>();
            team1 = new List<NetworkingPlayer>();
            team2 = new List<NetworkingPlayer>();
        }
        public override void OnStartServer()
        {
            base.OnStartServer();
            IsServer = true;
        }
        public override void OnStartClient()
        {
            base.OnStartClient();
            IsClient = true;
        }
        public void AddPlayer(NetworkingPlayer player)
        {
            if (!Networkplayers.Contains(player))
            {
                Networkplayers.Add(player);
            }
        }
        public void addPlayersToTeams(NetworkingPlayer player)
        {
            if (player.PTeamID == 1)
            {
                team1.Add(player);
            }
            else if (player.PTeamID == 2)
            {
                team2.Add(player);
            }

        }

        public void RemovePlayer(NetworkingPlayer player)
        {
            if (Networkplayers.Contains(player))
            {
                Networkplayers.Add(player);
            }
        }
        public NetworkingPlayer GetPlayerByNetId(uint netId)
        {
            return Networkplayers.Find(x => x.netId == netId);
        }
        public void UpdatePlayerName(string pName)
        {
            localPlayerName = pName;
        }
        public void UpdatePlayerTeamID(int pTeamID)
        {
            localPlayerTeamID = pTeamID;
        }
        public void UpdatePlayerColor(Material pColor)
        {
            localPlayerColor = pColor;
        }
    }
}