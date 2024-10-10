using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenerationData")]
public class GenerationData : ScriptableObject
{
    public enum Generation
    {
        Tutorial,
        Origins,
        Renaissance,
        Morden
    }

    public Generation generation;

    public List<ApplianceData> applaincesToUpgrade;
    public List<Ingredient> ingredientsToSell;
    public List<Dish> dishesToCook;
}