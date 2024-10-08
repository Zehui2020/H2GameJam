using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Decription: Handle Customer Behaviours

public class CustomerEntity : MonoBehaviour
{
    [SerializeField] private GameObject orderUI;
    [SerializeField] private Image patienceMeter;
    [SerializeField] private Transform requestContainer;

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
    private List<Dish.DishType> requestedDishes;

    //to change image correct and wrong
    private List<int> imageItemsIndexList;

    public void Init(Vector3 _placementPos, Vector3 _exitPos, List<Dish.DishType> _requestedDishes)
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

        patienceCounter = 100;
        maxPatience = patienceCounter;

        orderUI.SetActive(false);
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


                //Reach Patience Limit
                if (patienceCounter <= 0)
                {
                    //Leave
                    customerState = CustomerState.Leaving;
                    SetOrderLayer(2);

                    //Mark all remaining requests as wrong
                    SetRequestedItemsWrong();

                    //Angry Review
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

    public void PassFood(Dish.DishType _type)
    {
        if (requestedDishes.Contains(_type))
        {
            int index = requestedDishes.IndexOf(_type);

            //Remove requested Dish
            requestedDishes.Remove(_type);

            //check if finish all dishes
            if (requestedDishes.Count == 0)
            {
                //leave
                customerState = CustomerState.Leaving;
            }

            //Change Image to tick or cross;
            foreach (Transform reqImage in requestContainer)
            {
                //check if correct type and not changed
                RequestImageHandler requestImageHandler = reqImage.GetComponent<RequestImageHandler>();
                if (requestImageHandler.GetDishType() == _type && !requestImageHandler.HasChanged())
                {
                    requestImageHandler.SetObjectIsCorrect(true);
                    return;
                }
            }
        }
        else
        {
            //give wrong food, minus 20% of total time
            patienceCounter -= maxPatience * 0.2f;
        }
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
