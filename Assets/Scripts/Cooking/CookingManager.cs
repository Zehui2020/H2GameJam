using UnityEngine;
using UnityEngine.Events;

public class CookingManager : MonoBehaviour
{
    private CookingUIManager cookingUIManager;

    [SerializeField] private float timer;
    [SerializeField] private float maxTimer;
    [SerializeField] private bool updateTimer = true;

    private void Start()
    {
        cookingUIManager = GetComponent<CookingUIManager>();
        cookingUIManager.SetTimerSlider(0, 0);
    }

    private void Update()
    {
        if (updateTimer)
        {
            timer += Time.deltaTime;
            cookingUIManager.SetTimerSlider(timer, maxTimer);

            if (timer >= maxTimer)
                cookingUIManager.ShowFinishAlert();
        }
    }

    public void GoToMarketPlace()
    {
        PlayerStats.playerStatsInstance.GoToShop();
    }

    public bool IsRushHour()
    {
        return timer >= (maxTimer / 3) && timer <= (maxTimer / 3 * 2);
    }
}