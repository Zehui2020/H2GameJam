using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//Desciption: Controls and updates each customer entity


public class CustomerController : MonoBehaviour
{
    public static CustomerController Instance;  

    [SerializeField] private GameObject customerEntityPrefab;
    [SerializeField] private GameObject foodReqPrefab;
    [SerializeField] private Transform customerSpawnPos;
    [SerializeField] private Transform[] customerPlacementPos;
    [SerializeField] private Transform customerEndPos;
    [SerializeField] private DishList dishList;

    //store all customers
    private List<CustomerEntity> customerEntities;
    //counters to spawn next entity
    private float spawnEntityCounter;

    private bool storeClosed;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        customerEntities = new List<CustomerEntity>();

        //create list of n number of customer entities
        for (int i = 0; i< customerPlacementPos.Length; i++)
        {
            customerEntities.Add(null);
        }

        spawnEntityCounter = 5f;

        storeClosed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnEntityCounter > 0)
            spawnEntityCounter -= Time.deltaTime;

        //update customer entities
        for (int i = 0; i < customerPlacementPos.Length; i++)
        {
            //if there is an empty space
            //and spawn timer is zero
            if (customerEntities[i] == null)
            {
                if (spawnEntityCounter <= 0 && !storeClosed)
                {
                    //Debug.Log("Spawn");
                    //spawn entity
                    CustomerEntity newCustomer = Instantiate(customerEntityPrefab, customerSpawnPos).GetComponent<CustomerEntity>();

                    //Assign food
                    List<Appliance.CookedDish> cookedDishes = new();

                    //TODO: Check if have enough ingredients for that food
                    //Random between 1 or 2 foods
                    int maxFoodNo = Random.Range(1, 3);
                    for (int foodNo = 0; foodNo < 1; foodNo++)
                    {
                        //Random Dish
                        Dish reqDish = PlayerStats.playerStatsInstance.GetDishesOfThisGeneration()[Random.Range(0,3)];
                        int reqCombinationIndex = Random.Range(0, reqDish.dishCombinations.Count);
                        Appliance.CookedDish reqCookedDish = new Appliance.CookedDish(reqDish, reqCombinationIndex);

                        cookedDishes.Add(reqCookedDish);

                        //show image in player request container
                        Image newFoodReq = Instantiate(foodReqPrefab, newCustomer.GetRequestContainerTransform()).GetComponent<Image>();
                        newFoodReq.sprite = GetDishImage(reqDish.dishType);
                        newFoodReq.GetComponent<RequestImageHandler>().Init(reqCookedDish);

                        foreach (Sprite sprite in reqDish.dishCombinations[reqCombinationIndex].requirementSprites)
                        {
                            newFoodReq = Instantiate(foodReqPrefab, newCustomer.GetRequestContainerTransform()).GetComponent<Image>();
                            newFoodReq.sprite = sprite;
                        }

                        newCustomer.ForceRebuildRequestContainer();
                    }

                    //Give customer new placement position
                    newCustomer.Init(customerPlacementPos[i].position, customerEndPos.position, cookedDishes);

                    //add customer entity to list
                    customerEntities[i] = newCustomer;
                    //reset timer
                    spawnEntityCounter = 6;
                }
                else
                    continue;
                
            }
            else
            {
                //update entity like normal
                //check if customer reach the end position
                if (! customerEntities[i].UpdateCustomer())
                {
                    //can remove customer
                    Destroy(customerEntities[i].gameObject);
                    customerEntities[i] = null;
                }
            }
        }
    }

    public void CloseStore()
    {
        storeClosed = true;
    }

    private Sprite GetDishImage(Dish.DishType _type)
    {
        foreach(Dish d in dishList.listOfDishes)
        {
            if (d.dishType == _type)
            {
                return d.dishSprite;
            }
        }

        return null;
    }
}