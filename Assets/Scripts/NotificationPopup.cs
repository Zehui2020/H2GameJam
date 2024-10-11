using System.Collections;
using UnityEngine;
using TMPro;
using DesignPatterns.ObjectPool;
using UnityEngine.UI;

public class NotificationPopup : PooledObject
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Image icon;

    [ColorUsageAttribute(true, true)][SerializeField] private Color increaseColor;
    [ColorUsageAttribute(true, true)][SerializeField] private Color decreaseColor;

    [SerializeField] private Sprite reputationSprite;
    [SerializeField] private Sprite moneySprite;

    private Vector2 driftDir;
    private Animator animator;

    public enum PopupType
    {
        ReputationIncrease,
        ReputationDecrease,
        MoneyIncrease,
        MoneyDecrease,
        TextNotification
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetupPopup(string notification, Vector3 position, PopupType popupType)
    {
        transform.position = position;
        transform.forward = Camera.main.transform.forward;
        driftDir = Vector2.up;

        textMesh.SetText(notification);

        switch (popupType)
        {
            case PopupType.ReputationIncrease:
                icon.sprite = reputationSprite;
                textMesh.color = increaseColor;
                break;
            case PopupType.MoneyIncrease:
                icon.sprite = moneySprite;
                textMesh.color = increaseColor;
                break;
            case PopupType.ReputationDecrease:
                icon.sprite = reputationSprite;
                textMesh.color = decreaseColor;
                break;
            case PopupType.MoneyDecrease:
                icon.sprite = moneySprite;
                textMesh.color = decreaseColor;
                break;
            case PopupType.TextNotification:
                icon.gameObject.SetActive(false);
                break;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

        Deactivate();
    }

    public void SetupPopup(string text, Vector3 position, Color color, Vector2 driftDirection)
    {
        transform.position = position;
        transform.forward = Camera.main.transform.forward;
        driftDir = driftDirection;

        textMesh.color = color;
        textMesh.SetText(text);

        Deactivate();
    }

    public void Deactivate()
    {
        StartCoroutine(DeactivateRoutine());
    }

    private IEnumerator DeactivateRoutine()
    {
        float timer = 0;
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        Vector3 initialPosition = transform.position;

        Vector3 targetPosition = initialPosition + new Vector3(driftDir.x, driftDir.y, 0);

        while (timer < animationLength)
        {
            float t = timer / animationLength;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

            timer += Time.deltaTime;

            yield return null;
        }

        Release();
        gameObject.SetActive(false);
    }
}