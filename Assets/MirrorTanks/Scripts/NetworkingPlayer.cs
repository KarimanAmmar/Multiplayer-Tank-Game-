using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace MirrorTanks
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

        // sync vars networking variables
        [SyncVar(hook = nameof(OnNameUpdated))] string pName;
        [SyncVar(hook = nameof(OnHealthUpdated))] int hp;
        [SyncVar(hook = nameof(OnColorUpdated))] Color pColor;
        [SyncVar(hook = nameof(OnTeamIDUpdated))] int pTeamID;


        bool isDead = false;
        GamePlayUi gameplayUI;

        public int PTeamID => pTeamID;
        public GamePlayUi GamePlayUI => gameplayUI;
        public bool IsDead { get => isDead; set => isDead = value; }

        private void Awake()
        {
            gameplayUI = FindObjectOfType<GamePlayUi>();
        }
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            mainCameraTrans = Camera.main.transform;
            NetworkingManager.Singleton.AddPlayer(this);
        }
        public override void OnStartServer()
        {
            base.OnStartServer();
            hp = MaxHealth;
        }
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            CmdUpdatePlayerName(NetworkingManager.Singleton.LocalPlayerName);
            CmdUpdatePlayerTeamId(NetworkingManager.Singleton.LocalPlayerTeamID);
            CmdUpdatePlayerColor(NetworkingManager.Singleton.LocalPlayerColor.color);
        }
        void Update()
        {
            if (isLocalPlayer && !IsDead)
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
                if (shoot)
                {
                    CmdShoot();
                }

            }
            //look at
            uiRoot.LookAt(mainCameraTrans);
        }

        #region Command server logic
        [Command]
        void CmdUpdatePlayerName(string PlayerName)
        {
            pName = PlayerName;
        }
        [Command]
        void CmdUpdatePlayerTeamId(int PlayerTeamID)
        {
            pTeamID = PlayerTeamID;
        }
        [Command]
        void CmdUpdatePlayerColor(Color color)
        {
            pColor = color;
        }
        [Command]
        void CmdShoot()
        {
            Bullet bullet = Instantiate(bulletPrefab, BulletSpawningPosition.position, cannonPivot.rotation);
            bullet.Init(netId);
            bullet.BulletTeamID(pTeamID);
            RpcShoot(bullet.transform.position, bullet.transform.rotation);
        }

        [Server]
        public void ApplyDamage(int damage, uint OwnerBulletId)
        {
            hp -= damage;
            hp = Mathf.Max(hp, 0);

            if (hp == 0 && !isDead)
            {
                RpcDie(OwnerBulletId);
            }
        }
        [Server]
        public void ApplyRevive()
        {
            hp = MaxHealth / 2;
            RpcRevive();
        }
        #endregion

        #region Client Logic
        [ClientRpc]
        void RpcShoot(Vector3 position, Quaternion rotation)
        {
            if (!NetworkingManager.Singleton.IsHost)
            {
                Instantiate(bulletPrefab, position, rotation);
            }
        }
        [ClientRpc]
        void RpcDie(uint KilledNetId)
        {
            isDead = true;
            reviveAreaCollider.enabled = true;
            checkIfGameOver();

            NetworkingPlayer netPlayer = NetworkingManager.Singleton.GetPlayerByNetId(KilledNetId);
            if (netPlayer)
            {
                gameplayUI.ChangeState($"{netPlayer.pName} Killed {this.pName}");
            }
        }
        [ClientRpc]
        void RpcRevive()
        {
            isDead = false;
            reviveAreaCollider.enabled = false;
        }
        #endregion

        #region updates
        void OnNameUpdated(string oldVal, string newVal)
        {
            pName = newVal;
            txt_pName.text = pName;
        }
        void OnTeamIDUpdated(int oldVal, int newVal)
        {
            pTeamID = newVal;
            NetworkingManager.Singleton.addPlayersToTeams(this);
        }
        void OnColorUpdated(Color oldVal, Color newVal)
        {
            pColor = newVal;
            this.GetComponent<Renderer>().material.color = pColor;
        }
        void OnHealthUpdated(int oldval, int newval)
        {
            hp = newval;
            img_hp.transform.localScale = new Vector3(hp * 1.0f / MaxHealth, 1, 1);
        }
        public void checkIfGameOver()
        {
            if (NetworkingManager.Singleton.Team1.All(player => player.IsDead == true))
            {
                gameplayUI.gameover("Team 2 is the winner");
            }
            else if (NetworkingManager.Singleton.Team2.All(player => player.IsDead == true))
            {
                gameplayUI.gameover("Team 1 is the winner");
            }
        }
        #endregion

        private void OnDestroy()
        {
            NetworkingManager.Singleton.RemovePlayer(this);
        }
    }
}