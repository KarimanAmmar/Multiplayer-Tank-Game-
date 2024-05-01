using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

namespace NGOTanks
{
    public class NetworkingManager : NetworkManager
    {
        static NetworkingManager singleton;
        [SerializeField] NetworkObject tankPrefab;
        public const string SceneName_GamePlay = "GamePlay";
        public PlayerData PlayerData; 
        public static new NetworkingManager Singleton => singleton;

        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
        }
        void Start()
        {
            OnServerStarted += NetworkingManager_onServerStarted;
        }
        #region Events
        private void NetworkingManager_onServerStarted()
        {
            SceneManager.LoadScene(SceneName_GamePlay, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        #endregion
        public void SpwanPlayerObject(ulong clientId, Vector3 startPos , Quaternion rotation)
        {
            if (!IsServer)
            {
                return;
            }

            NetworkObject playerPrefab = Instantiate(tankPrefab, startPos, rotation);
            playerPrefab.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

            //SpawnManager.InstantiateAndSpawn(tankPrefab, clientId, false, true, false, startPos, rotation);
        }
        public void UpdatePlayerData(PlayerData data)
        {
            PlayerData = data;
        }
        private void OnDestroy()
        {
            OnServerStarted -= NetworkingManager_onServerStarted;
        }
    }
}
