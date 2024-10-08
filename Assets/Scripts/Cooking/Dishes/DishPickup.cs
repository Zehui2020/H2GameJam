using UnityEngine;
using UnityEngine.EventSystems;

public class DishPickup : Draggable
{
    public Appliance.CookedDish dish;

    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool canDrag;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitDraggable();
        canDrag = false;
    }

    public void SetCanDrag(bool drag)
    {
        canDrag = drag;
    }

    public void InitPickup(Appliance.CookedDish dish)
    {
        this.dish = dish;
        spriteRenderer.sprite = dish.dish.dishSprite;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (canDrag)
            base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (canDrag)
            base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (canDrag)
            base.OnEndDrag(eventData);
    }
}