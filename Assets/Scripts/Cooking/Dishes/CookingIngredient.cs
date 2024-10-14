using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CookingIngredient : Draggable, IAbleToAddIngredient
{
    [Header("Appliance Stats")]
    [SerializeField] private Appliance appliance;
    [SerializeField] protected List<Ingredient.IngredientType> ingredients;
    [SerializeField] private Sprite burntSprite;
    [SerializeField] private float detectRadius;
    [SerializeField] private LayerMask utensilLayer;
    [SerializeField] private Utensil.UtensilType acceptedUtensil;

    private ApplianceUIManager applianceUIManager;
    private SpriteRenderer spriteRenderer;

    private float cookingTimer;
    private Coroutine cookingRoutine;
    private Coroutine burnRoutine;
    protected bool canServe = false;

    [SerializeField] private Appliance.CookedDish cookedDish;
    [SerializeField] private Ingredient.IngredientType allowedIngredientType;
    [SerializeField] private Sprite alternateIngredientSprite;

    private void Awake()
    {
        InitDraggable();
        applianceUIManager = GetComponent<ApplianceUIManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        OnReachOriginalPosition += ResumeCooking;
    }

    public void InitCookingIngredient(Appliance appliance, Appliance.CookedDish cookedDish)
    {
        this.appliance = appliance;
        this.cookedDish = cookedDish;
        canServe = false;

        cookingRoutine = StartCoroutine(StartCooking());
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        StopCooking();
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        OnDragEnd();
    }

    public void OnDragEnd()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, detectRadius, utensilLayer);

        if (col == null)
            return;

        if (col.CompareTag("Trash"))
        {
            StopCooking();
            Destroy(gameObject);
        }

        if (col.TryGetComponent<Utensil>(out Utensil utensil) && canServe)
        {
            if (utensil.utensilType != acceptedUtensil)
                return;

            utensil.SetDish(cookedDish);
            Destroy(gameObject);
        }
    }

    public bool AddIngredient(Ingredient ingredient)
    {
        if (!CanPutIngredient(ingredient.ingredientType) || ingredients.Count >= 1)
            return false;

        ingredients.Add(ingredient.ingredientType);
        applianceUIManager.AddIngredientUI(ingredient);
        spriteRenderer.sprite = alternateIngredientSprite;

        if (burnRoutine != null)
        {
            StopCoroutine(burnRoutine);
            burnRoutine = null;
            applianceUIManager.StopBurning();

            cookingTimer -= appliance.applianceData.cookDuration * 0.35f;
            if (cookingTimer <= 0)
                cookingTimer = 0;
        }

        if (cookingRoutine == null)
            cookingRoutine = StartCoroutine(StartCooking());
        else
        {
            cookingTimer -= appliance.applianceData.cookDuration * 0.35f;
            if (cookingTimer <= 0)
                cookingTimer = 0;
        }

        if (ingredients.Count >= 1)
            applianceUIManager.HideAddIngredientUI();

        return true;
    }

    public void StopCooking()
    {
        if (cookingRoutine != null)
        {
            StopCoroutine(cookingRoutine);
            cookingRoutine = null;
        }

        if (burnRoutine != null)
        {
            StopCoroutine(burnRoutine);
            burnRoutine = null;
            applianceUIManager.StopBurning();
        }
    }

    public void ResumeCooking()
    {
        cookingRoutine = StartCoroutine(StartCooking());
    }

    public void ServeFood()
    {
        if (!canServe)
            return;

        Destroy(gameObject);
    }

    public Appliance.CookedDish GetCookedDish(Utensil utensil)
    {
        if (!canServe)
            return null;

        if (utensil.utensilType != cookedDish.dish.utensil)
        {
            PlayerStats.playerStatsInstance.ShowPopup("Wrong Utensil!", transform, NotificationPopup.PopupType.TextNotification);
            return null;
        }

        ServeFood();
        return cookedDish;
    }

    private IEnumerator StartCooking()
    {
        while (true)
        {
            cookingTimer += Time.deltaTime * (appliance.applianceData.cookSpeed + appliance.cookingSpeedModifier.GetTotalModifier());
            applianceUIManager.SetCookingSlider(cookingTimer, appliance.applianceData.cookDuration);
            applianceUIManager.SetCookingSliderActive(true);

            if (cookingTimer >= appliance.applianceData.cookDuration)
            {
                applianceUIManager.SetCookingSliderActive(false);
                CookFood();
                burnRoutine = StartCoroutine(BurnRoutine());
                cookingRoutine = null;
                cookingTimer = appliance.applianceData.cookDuration;
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator BurnRoutine()
    {
        yield return new WaitForSeconds(appliance.applianceData.burnGracePeriod);

        float burnTimer = 0;

        while (burnTimer < appliance.applianceData.burnDuration)
        {
            burnTimer += Time.deltaTime;
            applianceUIManager.SetBurnWarningSpeed(burnTimer / appliance.applianceData.burnDuration);

            if (burnTimer >= appliance.applianceData.burnDuration)
            {
                BurnFood();
                yield break;
            }

            yield return null;
        }

        burnRoutine = null;
    }

    public bool CookFood()
    {
        canServe = true;
        applianceUIManager.OnFoodCooked();
        return true;
    }

    private void BurnFood()
    {
        cookingRoutine = null;
        applianceUIManager.StopBurning();
        if (spriteRenderer != null)
            spriteRenderer.sprite = burntSprite;
        canServe = false;
    }

    private bool CanPutIngredient(Ingredient.IngredientType newIngredient)
    {
        if (ingredients.Count >= 1)
            return false;

        // Check for allowed ingredient type
        if (newIngredient.Equals(allowedIngredientType))
        {
            return true;
        }

        PlayerStats.playerStatsInstance.ShowPopup("Wrong Ingredient!", transform, NotificationPopup.PopupType.TextNotification);
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}