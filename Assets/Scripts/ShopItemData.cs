using UnityEngine;

public class ShopItemData : ScriptableObject
{
    [Header("Shop Item Stats")]
    public string shopItemName;
    [TextArea(3, 10)] public string shopDescription;
    public Sprite itemSprite;
    public int maximumPurchases;
    public int cost;

    public int GetCost()
    {
        return cost;
    }
}