using UnityEngine;
using UnityEngine.Events;

public class CookingManager : MonoBehaviour
{
    private CookingUIManager cookingUIManager;

    [SerializeField] private float timer;
    [SerializeField] private float maxTimer;

    private void Start()
    {
        cookingUIManager = GetComponent<CookingUIManager>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        cookingUIManager.SetTimerSlider(timer, maxTimer);

        if (timer >= maxTimer)
            cookingUIManager.ShowFinishAlert();
    }

    public void GoToMarketPlace()
    {
        SceneLoader.Instance.LoadScene("MarketPlace");
    }
}