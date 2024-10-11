using UnityEngine;
using UnityEngine.EventSystems;

public class Fan : Draggable
{
    [SerializeField] private LayerMask grillLayer;
    [SerializeField] private float fanRadius;
    [SerializeField] private float cookingSpeedModifier;
    [SerializeField] private float dragIdleTime;

    private float lastMovedTime = 0;
    private bool startDragging = false;

    private Appliance targetGrill;

    private void Start()
    {
        InitDraggable();
    }

    private void Update()
    {
        if (!startDragging)
            return;

        lastMovedTime += Time.deltaTime;
        if (lastMovedTime >= dragIdleTime && targetGrill != null)
            targetGrill.cookingSpeedModifier.RemoveModifier(cookingSpeedModifier);

        Collider2D grill = Physics2D.OverlapCircle(transform.position, fanRadius, grillLayer);
        if (grill == null || !grill.TryGetComponent<Appliance>(out Appliance appliance))
        {
            if (targetGrill != null)
                targetGrill.cookingSpeedModifier.RemoveModifier(cookingSpeedModifier);

            targetGrill = null;
            return;
        }

        targetGrill = appliance;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        startDragging = true;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        lastMovedTime = 0;
        if (targetGrill != null)
            targetGrill.cookingSpeedModifier.AddModifierOnce(cookingSpeedModifier);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        startDragging = false;
        if (targetGrill != null)
            targetGrill.cookingSpeedModifier.RemoveModifier(cookingSpeedModifier);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, fanRadius);
    }
}