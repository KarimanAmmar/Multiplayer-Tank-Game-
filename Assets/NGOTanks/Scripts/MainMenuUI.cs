using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NGOTanks
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField if_playerName;
        [SerializeField] TMP_Dropdown dd_Team;
        [SerializeField] TMP_Dropdown dd_Type;

        public void OnStartServerClicked()
        {
            NetworkingManager.Singleton.StartServer();
        }
        public void OnStartHostClicked()
        {
            if (canUpdatePlayerData())
            {
                NetworkingManager.Singleton.StartHost();
            }
        }
        public void OnStartClientClicked()
        {
            if (canUpdatePlayerData())
            {
                NetworkingManager.Singleton.StartClient();
            }
        }
        bool canUpdatePlayerData()
        {
            if (string.IsNullOrEmpty(if_playerName.text))
            {
                return false;
            }

            Team team = (Team)System.Enum.Parse(typeof(Team), dd_Team.captionText.text);
            PlayerClass playerClass = (PlayerClass)System.Enum.Parse(typeof(PlayerClass), dd_Type.captionText.text);

            PlayerData pData = new PlayerData
            {
                playerName = if_playerName.text,
                TeamID = team,
                playerClass = playerClass,
            };
            NetworkingManager.Singleton.UpdatePlayerData(pData);

            return true;
        }
    }
}
