using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace NGOTanks
{
    public class NetworkingPlayer : NetworkBehaviour
    {
        [Header("Colliders")]
        [SerializeField] Collider reviveAreaCollider;

        [Header("Movement")]
        [SerializeField] Transform cannonPivot;
        [SerializeField] float moveSpeed;
        [SerializeField] float rotationSpeed;
        Rigidbody rb;

        [Header("UI")]
        [SerializeField] Transform uiRoot;
        Transform mainCameraTrans;
        [SerializeField] TextMeshProUGUI txt_pName;
        [SerializeField] Image img_hp;

        [Header("states")]
        [SerializeField] int MaxHealth;

        [Header("Shooting")]
        [SerializeField] Bullet bulletPrefab;
        [SerializeField] Transform BulletSpawningPosition;

        // Networked variables
        NetworkVariable<PlayerData> pData = new NetworkVariable<PlayerData>();
        NetworkVariable<int> hp = new NetworkVariable<int>();

        private void Start()
        {
            rb = GetComponent<Rigidbody>(); 
        }
        void Update()
        {
            if (IsLocalPlayer)
            {
                //Input
                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");
                bool right = Input.GetKey(KeyCode.RightArrow);
                bool left = Input.GetKey(KeyCode.LeftArrow);
                bool shoot = Input.GetKeyDown(KeyCode.Space);

                //Movement
                Vector3 movVec = new Vector3(x, 0, z) * (moveSpeed * Time.deltaTime);
                rb.Move(rb.position + movVec, rb.rotation);


                if (right || left)
                {
                    float rotationAngle = 0;
                    if (right)
                    {
                        rotationAngle += rotationSpeed * Time.deltaTime;
                    }
                    if (left)
                    {
                        rotationAngle -= rotationSpeed * Time.deltaTime;
                    }
                    cannonPivot.Rotate(transform.up, rotationAngle);
                }
                //shooting 
                //if (shoot)
                //{
                //    CmdShoot();
                //}

            }
            //look at
            uiRoot.LookAt(mainCameraTrans);
        }
        public override void OnNetworkSpawn()
        {
            pData.OnValueChanged += OnPlayerdataUpdate;
            hp.OnValueChanged += OnHealthUpdate;
            base.OnNetworkSpawn();

            if (IsLocalPlayer)
            {
                //Debug.Log(NetworkingManager.Singleton.pData.ToString());

                UpdatePlayerDataServerRPC(NetworkingManager.Singleton.PlayerData);
            }
            else
            {
                InitializePlayer();
            }
        }
        [ServerRpc]
        public void UpdatePlayerDataServerRPC(PlayerData pdata)
        {
            pData.Value = pdata;
        }

        void InitializePlayer()
        {
            txt_pName.text = pData.Value.playerName.ToString();
            OnHealthUpdate(0, hp.Value);
        }
        void OnPlayerdataUpdate(PlayerData oldVal, PlayerData newVal)
        {
            InitializePlayer();
            if(IsServer)
            {
                hp.Value = MaxHealth;
            }
            //  txt_pName.text = newVal.playerName.ToString();
        }
        void OnHealthUpdate(int oldVal, int newVal)
        {
            img_hp.transform.localScale = new Vector3(newVal * 1.0f / MaxHealth, 1, 1);
        }
    }
}
