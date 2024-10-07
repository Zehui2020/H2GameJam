using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Desciption: Controls and updates each customer entity


public class CustomerController : MonoBehaviour
{
    
    [SerializeField] private GameObject customerEntityPrefab;
    [SerializeField] private Transform customerSpawnPos;
    [SerializeField] private Transform[] customerPlacementPos;
    [SerializeField] private Transform customerEndPos;

    //store all customers
    private List<CustomerEntity> customerEntities;
    //counters to spawn next entity
    private float spawnEntityCounter;

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
                if (spawnEntityCounter <= 0)
                {
                    Debug.Log("Spawn");
                    //spawn entity
                    CustomerEntity newCustomer = Instantiate(customerEntityPrefab, customerSpawnPos).GetComponent<CustomerEntity>();

                    //Give customer new placement position
                    newCustomer.Init(customerPlacementPos[i].position, customerEndPos.position);
                    //add customer entity to list
                    customerEntities[i] = newCustomer;
                    //reset timer
                    spawnEntityCounter = 5f;
                }
                else
                    continue;
                
            }
            else
            {
                //update entity like normal
                customerEntities[i].UpdateCustomer();
            }
        }
    }
}
