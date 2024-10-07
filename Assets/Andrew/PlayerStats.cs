using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Script by Andrew
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats playerStatsInstance;

    public int porkCount; public int chickenCount;
    private void Awake()
    {
        if (playerStatsInstance == null)
        {
            playerStatsInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (playerStatsInstance != this)
        {
            Destroy(gameObject);
        }

        SetUp();
    }

    private void SetUp()
    {

    }

    public void AddToPlayerInventory(int numOfItems, Ingredient ingredient)
    {

    }
}
