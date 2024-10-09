using UnityEngine;
using UnityEngine.Events;

public class AnimatorProxy : MonoBehaviour
{
    public UnityEvent OnInvokeEvent;

    public void InvokeEvent()
    {
        OnInvokeEvent?.Invoke();
    }
}