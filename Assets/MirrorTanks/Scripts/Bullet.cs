using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MirrorTanks
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] int speed;
        [SerializeField] int damage;
        Rigidbody rb;
        uint ownerId;
        int BTeamID;
        // Start is called before the first frame update
        void Start()
        {
            if (TryGetComponent<Rigidbody>(out rb))
            {
                rb.velocity = transform.forward * speed;
            }
            Destroy(gameObject, 3);
        }

        public void Init(uint ownerNetId)
        {
            ownerId = ownerNetId;
        }
        public void BulletTeamID(int teamID)
        {
            BTeamID = teamID;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (NetworkingManager.Singleton.IsServer)
            {
                if (other.CompareTag("Player"))
                {
                    if (other.TryGetComponent<NetworkingPlayer>(out NetworkingPlayer netPlayer))
                    {
                        if (netPlayer.PTeamID != BTeamID)
                        {
                            netPlayer.ApplyDamage(damage, ownerId);
                            Destroy(gameObject);
                        }
                        else if (netPlayer.PTeamID == BTeamID)
                        {
                            return;
                        }
                    }
                }
            }
            Destroy(gameObject);
        }
    }
}
