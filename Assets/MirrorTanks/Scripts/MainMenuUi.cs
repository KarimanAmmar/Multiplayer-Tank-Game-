using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace MirrorTanks
{
    public class MainMenuUi : MonoBehaviour
    {
        [SerializeField] Material team1Material;
        [SerializeField] Material team2Material;
        [SerializeField] TMP_InputField if_playerName;
        [SerializeField] TMP_Dropdown dropdown;
        void Start()
        {
            dropdown.onValueChanged.AddListener(delegate
            {
                UpdateTeam(dropdown);
            });
            UpdateTeam(dropdown);
        }
        private void Update()
        {
            //Debug.Log(NetworkingManager.Singleton.Team1);
        }
        public void OnStartServerClicked()
        {
            NetworkingManager.Singleton.StartServer();
        }
        public void OnStartHostClicked()
        {
            if (!string.IsNullOrEmpty(if_playerName.text))
            {
                NetworkingManager.Singleton.UpdatePlayerName(if_playerName.text);
                NetworkingManager.Singleton.StartHost();
            }
        }
        public void OnStartClientClicked()
        {
            if (!string.IsNullOrEmpty(if_playerName.text))
            {
                NetworkingManager.Singleton.UpdatePlayerName(if_playerName.text);
                NetworkingManager.Singleton.StartClient();
            }
        }
        public void UpdateTeam(TMP_Dropdown choise)
        {
            string selectedValue = choise.options[choise.value].text;

            if (selectedValue == "Team 1")
            {
                NetworkingManager.Singleton.UpdatePlayerColor(team1Material);
                NetworkingManager.Singleton.UpdatePlayerTeamID(1);
            }
            else if (selectedValue == "Team 2")
            {
                NetworkingManager.Singleton.UpdatePlayerColor(team2Material);
                NetworkingManager.Singleton.UpdatePlayerTeamID(2);
            }
        }
    }
}