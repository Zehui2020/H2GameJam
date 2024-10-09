using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientSpawner : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private IngredientPickup ingredientPickup;
    [SerializeField] private Ingredient ingredientToSpawn;
    [SerializeField] private TextMeshProUGUI ingredientCount;

    private void Start()
    {
        ingredientCount.text = PlayerStats.playerStatsInstance.GetIngredientCount(ingredientToSpawn.ingredientType).ToString();
    }

    public void AddIngredientBack()
    {
        PlayerStats.playerStatsInstance.AddToPlayerInventory(1, ingredientToSpawn);
        ingredientCount.text = PlayerStats.playerStatsInstance.GetIngredientCount(ingredientToSpawn.ingredientType).ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerStats.playerStatsInstance.GetIngredientCount(ingredientToSpawn.ingredientType) <= 0)
            return;

        IngredientPickup ingredient = Instantiate(ingredientPickup);
        ingredient.transform.position = transform.position;
        ingredient.InitPickup(ingredientToSpawn, this);

        PlayerStats.playerStatsInstance.AddToPlayerInventory(-1, ingredientToSpawn);
        ingredientCount.text = PlayerStats.playerStatsInstance.GetIngredientCount(ingredientToSpawn.ingredientType).ToString();
    }
}