using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [SerializeField] private List<Animator> recipes;
  

    public void BtnGoUp(int index)
    {

        foreach (Animator r in recipes)
        {
            r.SetBool("GoingDown", false);
        }
    }

    public void BtnGoDown(int index)
    {
        foreach(Animator r in recipes)
        {
            r.SetBool("GoingDown", false);
        }

        recipes[index].SetBool("GoingDown", true);
    }
}
