using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 offset;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - GetWorldPositionFromMouse(eventData);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        transform.position = GetWorldPositionFromMouse(eventData) + offset;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        transform.position = GetWorldPositionFromMouse(eventData) + offset;
    }

    private Vector3 GetWorldPositionFromMouse(PointerEventData eventData)
    {
        Vector3 mouseScreenPosition = eventData.position;
        mouseScreenPosition.z = mainCamera.WorldToScreenPoint(transform.position).z;

        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }
}