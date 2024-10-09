using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Appliance;


//Decription: Handle Customer Behaviours

public class CustomerEntity : MonoBehaviour
{
    [SerializeField] private GameObject orderUI;
    [SerializeField] private Image patienceMeter;
    [SerializeField] private RectTransform requestContainer;
    [SerializeField] private CustomerDialogueHandler customerDialogueHandler;

    public enum CustomerState
    {
        Entering,
        Waiting,
        Leaving,
        HasLeft,
        TotalStates
    }


    //Variables
    private Vector3 placementPos;
    private Vector3 exitPos;
    private float counter;
    private CustomerState customerState;
    private float patienceCounter;
    private float maxPatience;
    private float wrongFoodCounter;
    private bool saidImpatientRemark;
    [SerializeField] private List<Appliance.CookedDish> requestedDishes;

    //to change image correct and wrong
    private List<int> imageItemsIndexList;

    public void Init(Vector3 _placementPos, Vector3 _exitPos, List<Appliance.CookedDish> _requestedDishes, CustomerScriptableObject _customerData)
    {
        //Set Positions
        placementPos = _placementPos;
        exitPos = _exitPos;
        
        //Assign Requested Dishes
        requestedDishes = _requestedDishes;

        //Init Variables
        counter = 0;
        customerState = CustomerState.Entering;
        SetOrderLayer(2);
        imageItemsIndexList = new List<int>();

        saidImpatientRemark = false;

        orderUI.SetActive(false);

        //assign customer data
        patienceCounter = _customerData.currentPatienceLevel;
        maxPatience = 100;
        wrongFoodCounter = _customerData.wrongOrderLeeway;
    }

    // Update is called once per frame
    public bool UpdateCustomer()
    {
        switch (customerState)
        {
            case CustomerState.Entering:
                //move towards placement pos
                transform.position = Vector3.MoveTowards(transform.position, placementPos, 3 * Time.deltaTime);
                //reach  placement position
                if (transform.position == placementPos)
                {
                    //Wait
                    customerState = CustomerState.Waiting;
                    SetOrderLayer(3);
                    //Show food choices and patience
                    orderUI.SetActive(true);

                    //dialogue
                    int chance = Random.Range(1, 101);
                    int currentRep = PlayerStats.playerStatsInstance.currentReputation;
                    //negative / positive remarks 35%
                    if (chance >= 1 && chance <= 35 &&
                        (currentRep <= -5 || currentRep >= 8))
                    {
                        //negative
                        if (currentRep <= -5)
                        {
                            customerDialogueHandler.InitNewDialogue(CustomerDialogueController.DialogueType.NegPriceRemarks);
                        }
                        //positive
                        else
                        {
                            customerDialogueHandler.InitNewDialogue(CustomerDialogueController.DialogueType.PosPriceRemarks);
                        }
                    }
                    //commen on new facility
                    else if (chance >= 36 && chance <= 70 && PlayerStats.playerStatsInstance.dayCounter == 1 && PlayerStats.playerStatsInstance.currentGeneration > 0)
                    {
                        customerDialogueHandler.InitNewDialogue(CustomerDialogueController.DialogueType.VenueRemarks);
                    }
                    //normal hellos
                    else
                    {
                        customerDialogueHandler.InitNewDialogue(CustomerDialogueController.DialogueType.NormalGreetingRemarks);
                    }
                    
                }
                break;

            case CustomerState.Waiting:
                //Patience Tick Down
                patienceCounter -= Time.deltaTime;

                //UI
                //Patience Meter
                patienceMeter.fillAmount = patienceCounter / maxPatience;
                //Patience Meter Color
                if (patienceCounter > maxPatience / 2)
                {
                    //green to yellow
                    patienceMeter.color = Color.Lerp(Color.green, Color.yellow, 1 - (patienceCounter - (maxPatience / 2)) / (maxPatience / 2));
                }
                else
                {
                    //yellow to red
                    patienceMeter.color = Color.Lerp(Color.yellow, Color.red, 1 - patienceCounter / (maxPatience / 2));
                }

                //if below 25% max patience and have not said remark yet
                if (!saidImpatientRemark && patienceCounter <= maxPatience * 0.25f)
                {
                    saidImpatientRemark = true;
                    //dialogue
                    customerDialogueHandler.InitNewDialogue(CustomerDialogueController.DialogueType.ImpatientRemarks);
                }

                //Reach Patience Limit
                if (patienceCounter <= 0)
                {
                    //Leave
                    customerState = CustomerState.Leaving;
                    SetOrderLayer(2);

                    //Mark all remaining requests as wrong
                    SetRequestedItemsWrong();

                    //Angry Review
                    customerDialogueHandler.InitNewDialogue(CustomerDialogueController.DialogueType.NegReviewRemarks);

                    //Reputation Decrease
                    PlayerStats.playerStatsInstance.LoseRep(3);
                }

                break;

            case CustomerState.Leaving:
                //move towards exit position
                transform.position = Vector3.MoveTowards(transform.position, exitPos, 3 * Time.deltaTime);
                //reach exit position
                if (transform.position == exitPos)
                {
                    customerState = CustomerState.HasLeft;
                }
                break;

            case CustomerState.HasLeft:
                return false;

            default: 
                break;
        }
        return true;
    }


    private void SetOrderLayer(int layerNo)
    {
        //2 when moving
        //3 when stationary

        GetComponent<SpriteRenderer>().sortingOrder = layerNo;
        orderUI.GetComponent<Canvas>().sortingOrder = layerNo;
    }

    public Transform GetRequestContainerTransform()
    {
        return requestContainer;
    }

    public void ForceRebuildRequestContainer()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(requestContainer);
    }

    public void PassFood(CookedDish cookedDish)
    {
        foreach (CookedDish dish in requestedDishes)
        {
            if (CompareCookedDishes(dish, cookedDish))
            {
                ServeFood(dish);
                return;
            }
        }
        //Serve wrong food
        wrongFoodCounter += 1;
        //Patience decreaase
        patienceCounter -= maxPatience * 0.2f;
        //rep go down if go down twice
        if (wrongFoodCounter >= 2) 
        {
            PlayerStats.playerStatsInstance.LoseRep(1);
        }
        //reply
        if (patienceCounter > 0)
        {
            customerDialogueHandler.InitNewDialogue(CustomerDialogueController.DialogueType.WrongItemRemarks);
        }
    }

    private void ServeFood(CookedDish cookedDish)
    {
        //Remove requested Dish
        requestedDishes.Remove(cookedDish);

        //check if finish all dishes
        if (requestedDishes.Count == 0)
        {
            //leave
            customerState = CustomerState.Leaving;

            //up reputation
            //super fast
            if (patienceCounter >= maxPatience * 0.75f)
            {
                PlayerStats.playerStatsInstance.AddRep(2);
                //Fast remark
                customerDialogueHandler.InitNewDialogue(CustomerDialogueController.DialogueType.VeryPosReviewRemarks);
            }
            else if (patienceCounter >= maxPatience * 0.25f)
            {
                PlayerStats.playerStatsInstance.AddRep(1);
                //thanks remark
                customerDialogueHandler.InitNewDialogue(CustomerDialogueController.DialogueType.PosReviewRemarks);
            }
            else
            {
                //took them long enough remark
                customerDialogueHandler.InitNewDialogue(CustomerDialogueController.DialogueType.AgitatedReviewRemarks);
            }
        }

        //Change Image to tick or cross;
        foreach (Transform reqImage in requestContainer)
        {
            //check if correct type and not changed
            RequestImageHandler requestImageHandler = reqImage.GetComponent<RequestImageHandler>();
            if (CompareCookedDishes(requestImageHandler.GetDishType(), cookedDish) && !requestImageHandler.HasChanged())
            {
                requestImageHandler.SetObjectIsCorrect(true);
                return;
            }
        }
    }

    private bool CompareCookedDishes(CookedDish dish1, CookedDish dish2)
    {
        // Null checks
        if (dish1 == null || dish2 == null)
            return false;

        // Check for different dish types
        if (dish1.dish.dishType != dish2.dish.dishType)
            return false;

        // Check if combination index matters
        if (!dish1.dish.doesCombinationIndexMatter || !dish2.dish.doesCombinationIndexMatter)
            return true;

        // Check if combination index is the same
        if (dish1.combinationIndex != dish2.combinationIndex)
            return false;

        // Combination index is the same
        return true;
    }

    private void SetRequestedItemsWrong()
    {
        foreach (Transform reqImage in requestContainer)
        {
            //check if correct type and not changed
            RequestImageHandler requestImageHandler = reqImage.GetComponent<RequestImageHandler>();
            if (!requestImageHandler.HasChanged())
            {
                requestImageHandler.SetObjectIsCorrect(false);
            }
        }
    }
}