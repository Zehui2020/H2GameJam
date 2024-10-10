using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI repText;
    [SerializeField] public TextMeshProUGUI moneyText;


    // Start is called before the first frame update
    void Start()
    {
        PlayerStats playerStats = PlayerStats.playerStatsInstance;
        //set reputation and money
        if (playerStats != null )
        {
            repText.text = "Reputation: " + playerStats.currentReputation;
            moneyText.text = "Money: " + playerStats.currentMoney;
        }
        else
        {
            repText.text = "Reputation: Null";
            moneyText.text = "Money: Null";
        }
    }
}
