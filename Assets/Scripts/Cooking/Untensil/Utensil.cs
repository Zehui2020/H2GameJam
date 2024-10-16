using UnityEngine;
using UnityEngine.EventSystems;

public class Utensil : Draggable
{
    public enum UtensilType
    {
        Plate,
        Bowl
    }

    public UtensilType utensilType;
    [SerializeField] private float releaseRadius;
    [SerializeField] private LayerMask customerLayer;
    [SerializeField] private Appliance.CookedDish dish = null;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        InitDraggable();
    }

    public void SetDish(Appliance.CookedDish cookedDish)
    {
        dish = cookedDish;
        spriteRenderer.sprite = cookedDish.dish.dishSprite;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, releaseRadius, customerLayer);
        foreach (Collider2D col in cols)
        {
            // Check if collide with appliance & can serve
            if (col.TryGetComponent<Appliance>(out Appliance appliance))
            {
                Appliance.CookedDish dish = appliance.GetCookedDish(this);
                if (dish != null)
                {
                    switch (appliance.applianceData.type)
                    {
                        case ApplianceData.ApplianceType.Pot:
                            AudioManager.Instance.Play("SoupPour");
                            break;
                    }

                    Debug.Log("CALLED");
                    SetDish(dish);
                    return;
                }
            }

            // Check if collide with customer
            CustomerEntity customer = col.GetComponent<CustomerEntity>();
            if (customer == null || dish == null)
                return;

            customer.PassFood(dish);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, releaseRadius);
    }
}