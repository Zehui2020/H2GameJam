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
    [SerializeField] private CookingManager cookingManager;
    [SerializeField] private List<CustomerScriptableObject> customerDatas;
    [SerializeField] private Sprite[] emojiSprites; //1. Very Happy 2. Happy 3. Upset 4. Agitated 5. Angry
    [SerializeField] private float startTimerDelay = 0;
    [SerializeField] private bool canSpawn = true;
    [SerializeField] private List<Sprite> gen1Sprites;
    [SerializeField] private List<Sprite> gen2Sprites;
    [SerializeField] private List<Sprite> gen3Sprites;
    public enum CustomerEmotion
    {
        VeryPositive,
        Positive,
        Upset,
        Agitated,
        Angry
    }

    //store all customers
    private List<CustomerEntity> customerEntities;
    //counters to spawn next entity
    private float spawnEntityCounter;

    private bool storeClosed;

    private int dayDifficulty;

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

        spawnEntityCounter = startTimerDelay;
        HandleSpawnTimer();

        storeClosed = false;

        dayDifficulty = PlayerStats.playerStatsInstance.dayCounter;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSpawn)
            return;

        if (spawnEntityCounter > 0)
            spawnEntityCounter -= Time.deltaTime;

        //check if all seats are empty
        bool shopEmpty = true;
        for (int i = 0; i < customerPlacementPos.Length; i++)
        {
            if (customerEntities[i] != null)
                shopEmpty = false;
        }

        if (shopEmpty)
        {
            //spawn new customer
            spawnEntityCounter = 0;
        }

        //update customer entities
        for (int i = 0; i < customerPlacementPos.Length; i++)
        {
            //if there is an empty space
            //and spawn timer is zero
            if (customerEntities[i] == null)
            {
                if (spawnEntityCounter <= 0 && !storeClosed)
                {
                    //spawn entity
                    CustomerEntity newCustomer = Instantiate(customerEntityPrefab, customerSpawnPos).GetComponent<CustomerEntity>();

                    //Assign food
                    List<Appliance.CookedDish> cookedDishes = new();

                    //TODO: Check if have enough ingredients for that food
                    //1 or 2 foods
                    //rate is based on day difficulty and reputation
                    int chance = Random.Range(1, 11);
                    int repInc = (PlayerStats.playerStatsInstance.currentReputation <= -5 ? -2 : (PlayerStats.playerStatsInstance.currentReputation >= 8 ? 2 : 0));
                    int maxFoodNo = 1 + ((dayDifficulty == 2 && chance < 5 + repInc) || (dayDifficulty == 3 && chance < 9 + repInc) ? 1 : 0);
                    for (int foodNo = 0; foodNo < maxFoodNo; foodNo++)
                    {
                        //Random Dish
                        List<Dish> dishes = PlayerStats.playerStatsInstance.GetDishesOfThisGeneration();
                        Dish reqDish = dishes[Random.Range(0, dishes.Count)];
                        int reqCombinationIndex = Random.Range(0, reqDish.dishCombinations.Count);
                        Appliance.CookedDish reqCookedDish = new Appliance.CookedDish(reqDish, reqCombinationIndex);
                        //check if enough ingredients
                        if (CanCookDish(reqCookedDish))
                        {
                            cookedDishes.Add(reqCookedDish);

                            //show image in player request container
                            Image newFoodReq = Instantiate(foodReqPrefab, newCustomer.GetRequestContainerTransform()).GetComponent<Image>();
                            newFoodReq.sprite = GetDishImage(reqDish.dishType);
                            newFoodReq.GetComponent<RequestImageHandler>().Init(reqCookedDish);

                            // Side dishes
                            foreach (Dish sideDish in reqDish.dishCombinations[reqCombinationIndex].sideDishes)
                            {
                                Debug.Log(sideDish.dishType.ToString());
                                Appliance.CookedDish cookedSideDish = new Appliance.CookedDish(sideDish, 0);
                                cookedDishes.Add(cookedSideDish);

                                //show image in player request container
                                newFoodReq = Instantiate(foodReqPrefab, newCustomer.GetRequestContainerTransform()).GetComponent<Image>();
                                newFoodReq.sprite = GetDishImage(sideDish.dishType);
                                newFoodReq.GetComponent<RequestImageHandler>().Init(cookedSideDish);
                            }

                            // Check for requirement sprites
                            foreach (Sprite sprite in reqDish.dishCombinations[reqCombinationIndex].requirementSprites)
                            {
                                newFoodReq = Instantiate(foodReqPrefab, newCustomer.GetRequestContainerTransform()).GetComponent<Image>();
                                newFoodReq.sprite = sprite;
                            }

                            newCustomer.ForceRebuildRequestContainer();
                        }
                    }

                    //if have dishes to give this customer
                    if (cookedDishes.Count > 0)
                    {
                        //Select sprite
                        Sprite _customerSprite = null;
                        GenerationData.Generation gen = PlayerStats.playerStatsInstance.currentGeneration.generation;
                        int currDay = PlayerStats.playerStatsInstance.dayCounter;
                        //gen 1 
                        switch (gen)
                        {
                            case GenerationData.Generation.Morden:
                                {
                                    if (currDay == 1)
                                    {
                                        //50% ren, 50% Modern
                                        _customerSprite = (Random.Range(0, 2) == 0 ? gen3Sprites[Random.Range(0, gen3Sprites.Count)] : gen2Sprites[Random.Range(0, gen2Sprites.Count)]);
                                        break;
                                    }
                                    //else 100 % Modern
                                    _customerSprite = gen3Sprites[Random.Range(0, gen3Sprites.Count)];
                                    break;
                                }

                            case GenerationData.Generation.Renaissance:
                                {
                                    if (currDay == 3)
                                    {
                                        //50% ren, 50% Modern
                                        _customerSprite = (Random.Range(0, 2) == 0 ? gen3Sprites[Random.Range(0, gen3Sprites.Count)] : gen2Sprites[Random.Range(0, gen2Sprites.Count)]);
                                        break;
                                    }
                                    else if (currDay == 1)
                                    {
                                        //50% ren, 50% Origin
                                        _customerSprite = (Random.Range(0, 2) == 0 ? gen1Sprites[Random.Range(0, gen1Sprites.Count)] : gen2Sprites[Random.Range(0, gen2Sprites.Count)]);
                                        break;
                                    }
                                    //else 100% Ren
                                    _customerSprite = gen2Sprites[Random.Range(0, gen2Sprites.Count)];
                                    break;
                                }

                            default:
                                {
                                    if (currDay == 3)
                                    {
                                        //50% ren, 50% Origin
                                        _customerSprite = (Random.Range(0, 2) == 0 ? gen1Sprites[Random.Range(0, gen1Sprites.Count)] : gen2Sprites[Random.Range(0, gen2Sprites.Count)]);
                                        break;
                                    }
                                    //100% gen 1
                                    _customerSprite = gen1Sprites[Random.Range(0, gen1Sprites.Count)];
                                    break;
                                }
                        }

                        //Give customer new placement position
                        newCustomer.Init(customerPlacementPos[i].position, customerEndPos.position, cookedDishes, customerDatas[Random.Range(0, customerDatas.Count)], _customerSprite);

                        //add customer entity to list
                        customerEntities[i] = newCustomer;

                        //reset timer
                        //spawnEntityCounter = 6;
                        HandleSpawnTimer();
                    } 
                    else
                    {
                        //delete customer
                        Destroy(newCustomer.gameObject);
                    }
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
                    HandleSpawnTimer();
                }
            }
        }
    }

    public void SetCanSpawn(bool canSpawn)
    {
        this.canSpawn = canSpawn;
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

    private void HandleSpawnTimer()
    {
        //Default Value
        spawnEntityCounter = 5;

        //Rush Hour
        if (cookingManager.IsRushHour())
        {
            spawnEntityCounter -= 3;
        }
        //Reputation
        int rep = PlayerStats.playerStatsInstance.currentReputation;
        if (rep <= -5)
        {
            spawnEntityCounter += 2;
        }
        else if (rep >= 8)
        {
            spawnEntityCounter -= 1;
        }
    }

    private bool CanCookDish(Appliance.CookedDish _reqDish)
    {
        return PlayerStats.playerStatsInstance.CheckEnoughIngredients(_reqDish);
    }

    public Sprite GetEmojiSprite(CustomerEmotion _emotion)
    {
        return emojiSprites[(int)_emotion];
    }
}