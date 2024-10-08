using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Decription: Handle Customer Behaviours

public class CustomerEntity : MonoBehaviour
{
    [SerializeField] private GameObject orderUI;
    [SerializeField] private Image patienceMeter;

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

    public void Init(Vector3 _placementPos, Vector3 _exitPos)
    {
        //Set Positions
        placementPos = _placementPos;
        exitPos = _exitPos;
        
        //Init Variables
        counter = 0;
        customerState = CustomerState.Entering;
        SetOrderLayer(2);

        patienceCounter = 1;
        maxPatience = 1;

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
}
