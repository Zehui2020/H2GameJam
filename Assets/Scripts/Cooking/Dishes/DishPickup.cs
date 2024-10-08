using UnityEngine;

public class DishPickup : Draggable
{
    public Dish dish;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitDraggable();
    }

    public void InitPickup()
    {
        spriteRenderer.sprite = dish.dishSprite;
    }
}