using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MirrorTanks
{
    public class ReviveController : MonoBehaviour
    {
        [SerializeField] float reviveTime = 5f;
        private float reviveProgress = 0f;
        [SerializeField] bool isReviving = false;
        [SerializeField] TextMeshProUGUI Counter;
        [SerializeField] GameObject Counterobj;

        // List to keep track of players currently in the reviving area
        private List<GameObject> playersInArea = new List<GameObject>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent<NetworkingPlayer>(out NetworkingPlayer netOther) &&
                    this.TryGetComponent<NetworkingPlayer>(out NetworkingPlayer netthis))
                {
                    if (netOther.PTeamID == netthis.PTeamID)
                    {
                        if (netthis.IsDead)
                        {
                            playersInArea.Add(other.gameObject);
                            if (!isReviving)
                            {
                                StartReviveTimer();
                            }
                        }
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent<NetworkingPlayer>(out NetworkingPlayer netOther) &&
                   this.TryGetComponent<NetworkingPlayer>(out NetworkingPlayer netthis))
                {
                    if (netOther.PTeamID == netthis.PTeamID)
                    {
                        playersInArea.Remove(other.gameObject);
                        if (playersInArea.Count == 0)
                        {
                            ResetReviveTimer();
                        }
                    }
                }
            }
        }

        private void Update()
        {
            if (isReviving)
            {
                reviveProgress += Time.deltaTime;
                int reviveProgressInt = (int)reviveProgress;
                Counter.text = reviveProgressInt.ToString();
                if (reviveProgress >= reviveTime)
                {
                    if (this.TryGetComponent<NetworkingPlayer>(out NetworkingPlayer netthis))
                    {
                        netthis.ApplyRevive();
                    }
                    ResetReviveTimer();
                }
            }
        }

        private void StartReviveTimer()
        {
            isReviving = true;
            reviveProgress = 0f;
            Counterobj.SetActive(true);
        }

        private void ResetReviveTimer()
        {
            isReviving = false;
            reviveProgress = 0f;
            Counterobj.SetActive(false);
        }
    }
}
