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

        draggable.OnDragStart += StopCooking;
        draggable.OnDragFinish += OnDragEnd;
        draggable.OnReachOriginalPosition += OnReachOriginalPos;
    }

    public void OnDragEnd()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, detectRadius, utensilLayer);

        if (col == null)
            return;

        Debug.Log(col.name);

        if (col.CompareTag("Trash"))
        {
            StopCooking();
            DumpIngredients();
            return;
        }

        if (!col.TryGetComponent<Utensil>(out Utensil utensil))
            return;

        if (!canServe)
            return;

        if (utensil.utensilType != cookedDish.dish.utensil)
        {
            Debug.Log("INCORRECT UTENSIL!");
            return;
        }

        ServeFood();
        utensil.SetDish(cookedDish);
    }

    public void OnReachOriginalPos()
    {
        if (ingredients.Count > 0)
            ResumeCooking();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}