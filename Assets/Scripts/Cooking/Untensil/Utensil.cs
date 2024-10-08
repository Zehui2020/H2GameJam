using UnityEngine;
using UnityEngine.EventSystems;

public class Utensil : Draggable
{
    [SerializeField] private float releaseRadius;
    [SerializeField] private LayerMask customerLayer;

    [SerializeField] private Appliance.CookedDish dish;

    private void Start()
    {
        InitDraggable();
    }

    public void SetDish(Appliance.CookedDish cookedDish)
    {
        dish = cookedDish;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        Collider2D col = Physics2D.OverlapCircle(transform.position, releaseRadius, customerLayer);
        if (col == null)
            return;

        CustomerEntity customer = col.GetComponent<CustomerEntity>();
        if (customer == null)
            return;

        customer.PassFood(dish);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, releaseRadius);
    }
}