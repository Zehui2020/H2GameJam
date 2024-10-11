using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [SerializeField] private List<Animator> recipes;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BtnGoUp(int index)
    {
        recipes[index].SetBool("GoingDown", false);
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
