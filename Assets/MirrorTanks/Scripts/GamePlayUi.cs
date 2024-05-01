using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;
namespace MirrorTanks
{
    public class GamePlayUi : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI txt_gState;
        [SerializeField] TextMeshProUGUI txt_TeamName;
        [SerializeField] GameObject txtGO;
        [SerializeField] GameObject GameOverPanal;
        public void ChangeState(string Deathtxt)
        {
            StartCoroutine(SetStateActive(Deathtxt));
        }
        IEnumerator SetStateActive(string txt)
        {
            txtGO.SetActive(true);
            txt_gState.text = txt;

            yield return new WaitForSeconds(2);
            txtGO.SetActive(false);
        }
        public void gameover(string winner)
        {
            GameOverPanal.SetActive(true);
            txt_TeamName.text = winner;
        }
    }
}
