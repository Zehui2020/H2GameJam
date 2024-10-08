using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DishList")]
public class DishList : ScriptableObject
{
    public List<Dish> listOfDishes;
}
