using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Appliance;

public class RequestImageHandler : MonoBehaviour
{
    private bool hasChanged;
    private CookedDish dish;

    public void Init(CookedDish cookedDish)
    {
        foreach (Transform go in transform)
        {
            go.gameObject.SetActive(false);
        }

        hasChanged = false;
        dish = cookedDish;
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

    public CookedDish GetDishType()
    {
        return dish;
    }
}
