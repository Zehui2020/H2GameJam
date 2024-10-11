using UnityEngine;

public class ShopItemData : ScriptableObject
{
    [Header("Shop Item Stats")]
    public string shopItemName;
    [TextArea(3, 10)] public string shopDescription;
    public Sprite itemSprite;
    public int maximumPurchases;
    public float cost;

    public float GetCost()
    {
        return Mathf.Round(cost * 100f) / 100f;
    }
}