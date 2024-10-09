using DesignPatterns.ObjectPool;
using System.Collections;
using System.Linq;
using UnityEngine;

public class IngredientPickup : PooledObject
{
    public Ingredient ingredient;

    [SerializeField] private SpriteRenderer ingredientSR;
    private Camera mainCamera;

    private Vector3 startDragPos;
    [SerializeField] private float returnSpeed;
    private bool isReleased = false;

    public void InitPickup(Ingredient ingredient)
    {
        mainCamera = Camera.main;
        this.ingredient = ingredient;
        ingredientSR.sprite = ingredient.ingrendientSprite;
        startDragPos = transform.position;
    }

    private void Update()
    {
        if (isReleased)
            return;

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touches.Count() > 0)
            {
                Vector3 targetPos = mainCamera.ScreenToWorldPoint(Input.touches[0].position);
                transform.position = new Vector3(targetPos.x, targetPos.y, 0);
            }
            else
                OnRelease();
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 targetPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(targetPos.x, targetPos.y, 0);
            }
            else
                OnRelease();
        }
    }

    public void OnRelease()
    {
        isReleased = true;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, ingredientSR.size.x);
        foreach (Collider2D col in cols)
        {
            if (!col.TryGetComponent<Appliance>(out Appliance appliance))
                continue;

            appliance.AddIngredient(ingredient);
            Destroy(gameObject);
            break;
        }

        StartCoroutine(ReturnToStartPos());
    }

    private IEnumerator ReturnToStartPos()
    {
        while (Vector3.Distance(transform.position, startDragPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startDragPos, Time.deltaTime * returnSpeed);
            yield return null;
        }

        Destroy(gameObject);
    }
}