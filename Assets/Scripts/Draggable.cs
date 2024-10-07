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

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - GetWorldPositionFromMouse(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = GetWorldPositionFromMouse(eventData) + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
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