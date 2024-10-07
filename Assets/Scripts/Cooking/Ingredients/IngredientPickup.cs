using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientPickup : Draggable
{
    public Ingredient ingredient;

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
    }
}