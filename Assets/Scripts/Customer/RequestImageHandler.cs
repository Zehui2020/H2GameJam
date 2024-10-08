using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestImageHandler : MonoBehaviour
{
    private bool hasChanged;
    private Dish.DishType dishType;

    public void Init(Dish.DishType _type)
    {
        foreach (Transform go in transform)
        {
            go.gameObject.SetActive(false);
        }

        hasChanged = false;
        dishType = _type;
    }

    public void SetObjectIsCorrect(bool isCorrect)
    {
        hasChanged = true;
        if (isCorrect)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public bool HasChanged()
    {
        return hasChanged;
    }

    public Dish.DishType GetDishType()
    {
        return dishType;
    }
}
