using DesignPatterns.ObjectPool;
using System.Linq;
using UnityEngine;

public class IngredientPickup : PooledObject
{
    public Ingredient ingredient;

    [SerializeField] private SpriteRenderer ingredientSR;
    private Camera mainCamera;

    public void InitPickup(Ingredient ingredient)
    {
        mainCamera = Camera.main;
        this.ingredient = ingredient;
        ingredientSR.sprite = ingredient.ingrendientSprite;
    }

    private void Update()
    {
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
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, ingredientSR.size.x);
        foreach (Collider2D col in cols)
        {
            if (!col.TryGetComponent<Appliance>(out Appliance appliance))
                continue;

            appliance.AddIngredient(ingredient);
            break;
        }

        Destroy(gameObject);
    }
}