using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace NGOTanks
{
    public class PrefabHolder : MonoBehaviour
    {
        [SerializeField] NetworkObject playerPrefab;

        public NetworkObject PlayerPrefab => playerPrefab;
    }
}
