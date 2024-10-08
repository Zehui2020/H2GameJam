using UnityEngine;

public class DraggableAppliance : Appliance
{
    private Draggable draggable;

    [SerializeField] private float detectRadius;
    [SerializeField] private LayerMask utensilLayer;

    public override void InitAppliance()
    {
        base.InitAppliance();
        draggable = GetComponent<Draggable>();
        draggable.InitDraggable();
        draggable.OnDragFinish += OnDragEnd;
    }

    public void OnDragEnd()
    {
        if (!canServe)
            return;

        Collider2D col = Physics2D.OverlapCircle(transform.position, detectRadius, utensilLayer);
        if (!col.TryGetComponent<Utensil>(out Utensil utensil))
            return;

        ServeFood();
        utensil.SetDish(cookedDish);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}