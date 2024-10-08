using UnityEngine;
using UnityEngine.EventSystems;

public class Utensil : Draggable
{
    [SerializeField] private float releaseRadius;
    [SerializeField] private LayerMask serveAreaLayer;

    private void Start()
    {
        InitDraggable();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (Physics2D.OverlapCircle(transform.position, releaseRadius, serveAreaLayer))
        {
            //CustomerController.Instance.
            Destroy(gameObject);
        }
    }
}