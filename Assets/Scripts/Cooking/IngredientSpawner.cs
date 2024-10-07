using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientSpawner : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private IngredientPickup ingredientPickup;
    [SerializeField] private Ingredient ingredientToSpawn;

    public void OnPointerDown(PointerEventData eventData)
    {
        IngredientPickup ingredient = Instantiate(ingredientPickup);
        ingredient.transform.position = transform.position;
        ingredient.InitPickup(ingredientToSpawn);
    }
}