using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 offset;
    private Camera mainCamera;
    public bool isDraggable;

    private Vector3 startDragPos;
    [SerializeField] private float returnSpeed = 10f;

    public virtual void InitDraggable()
    {
        mainCamera = Camera.main;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - GetWorldPositionFromMouse(eventData);
        startDragPos = transform.position;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        transform.position = GetWorldPositionFromMouse(eventData) + offset;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        transform.position = GetWorldPositionFromMouse(eventData) + offset;
        StartCoroutine(OnDragEnd());
    }

    private Vector3 GetWorldPositionFromMouse(PointerEventData eventData)
    {
        Vector3 mouseScreenPosition = eventData.position;
        mouseScreenPosition.z = mainCamera.WorldToScreenPoint(transform.position).z;

        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }

    private IEnumerator OnDragEnd()
    {
        while (Vector3.Distance(transform.position, startDragPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startDragPos, Time.deltaTime * returnSpeed);
            yield return null;
        }
    }
}