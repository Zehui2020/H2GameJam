using DesignPatterns.ObjectPool;
using System.Collections;
using System.Linq;
using UnityEngine;

public class IngredientPickup : PooledObject
{
    public Ingredient ingredient;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float detectRadius;
    private Camera mainCamera;

    private Vector3 startDragPos;
    [SerializeField] private float returnSpeed;
    private bool isReleased = false;

    private IngredientSpawner ingredientSpawner;

    public void InitPickup(Ingredient ingredient, IngredientSpawner ingredientSpawner)
    {
        mainCamera = Camera.main;
        this.ingredient = ingredient;
        startDragPos = transform.position;
        this.ingredientSpawner = ingredientSpawner;
        spriteRenderer.sprite = ingredient.ingrendientSprite;
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
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, detectRadius);

        foreach (Collider2D col in cols)
        {
            //if this is a customer to out && and there is an ingredient on the plate
            if (col.TryGetComponent<CustomerEntity>(out CustomerEntity customer) && ingredient.dishOnPlate != null)
            {
                customer.PassFood(new Appliance.CookedDish(ingredient.dishOnPlate, 0));
                Destroy(gameObject);
                break;
            }

            if (!col.TryGetComponent<IAbleToAddIngredient>(out IAbleToAddIngredient appliance))
                continue;


            if (appliance.AddIngredient(ingredient))
            {
                //if (appliance.appliance)
                Destroy(gameObject);
                break;
            }
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

        ingredientSpawner.AddIngredientBack();
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}