using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGOTanks
{
    public class GamePlayManager : MonoBehaviour
    {
        [SerializeField] Transform[] startPos;
        int currentPosInd;

        private void Start()
        {
            if (NetworkingManager.Singleton.IsServer)
            {
                NetworkingManager.Singleton.SceneManager.OnLoadComplete += SceneManager_OnLoadComplete;
                if (NetworkingManager.Singleton.IsHost)
                {
                    spwanNextPlayer(NetworkingManager.Singleton.LocalClientId);
                }
            }
        }

        private void SceneManager_OnLoadComplete(ulong clientId, string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode)
        {
            if ( sceneName == NetworkingManager.SceneName_GamePlay)
            {
                spwanNextPlayer(clientId);
            }
        }
        void spwanNextPlayer(ulong clientId)
        {
            NetworkingManager.Singleton.SpwanPlayerObject(clientId, startPos[currentPosInd].position, startPos[currentPosInd].rotation);
            currentPosInd++;
            currentPosInd %= startPos.Length;
        }
        private void OnDestroy()
        {
            if (NetworkingManager.Singleton && NetworkingManager.Singleton.SceneManager != null)
            {
                NetworkingManager.Singleton.SceneManager.OnLoadComplete -= SceneManager_OnLoadComplete;
            }
        }
    }
}
