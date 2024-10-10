using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 offset;
    private Camera mainCamera;

    private Vector3 startDragPos;
    [SerializeField] private float returnSpeed = 10f;

    public event System.Action OnDragStart;
    public event System.Action OnDragging;
    public event System.Action OnDragFinish;
    public event System.Action OnReachOriginalPosition;

    public virtual void InitDraggable()
    {
        mainCamera = Camera.main;
        startDragPos = transform.position;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - GetWorldPositionFromMouse(eventData);
        OnDragStart?.Invoke();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        transform.position = GetWorldPositionFromMouse(eventData) + offset;
        OnDragging?.Invoke();
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        transform.position = GetWorldPositionFromMouse(eventData) + offset;
        StartCoroutine(OnDragEnd());
        OnDragFinish?.Invoke();
    }

    private Vector3 GetWorldPositionFromMouse(PointerEventData eventData)
    {
        Vector3 mouseScreenPosition = eventData.position;
        mouseScreenPosition.z = mainCamera.WorldToScreenPoint(transform.position).z;

        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }

    private IEnumerator OnDragEnd()
    {
        while (Vector3.Distance(transform.position, startDragPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startDragPos, Time.deltaTime * returnSpeed);
            yield return null;
        }

        OnReachOriginalPosition?.Invoke();
    }

    private void OnDisable()
    {
        OnDragStart = null;
        OnDragging = null;
        OnDragFinish = null;
        OnReachOriginalPosition = null;
    }
}