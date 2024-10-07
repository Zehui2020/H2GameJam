using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerEntity : MonoBehaviour
{
    private Vector3 placementPos;
    private Vector3 endPos;

    private float counter;

    public void Init(Vector3 _placementPos, Vector3 _endPos)
    {
        placementPos = _placementPos;
        endPos = _endPos;
        counter = 0;
    }

    // Update is called once per frame
    public void UpdateCustomer()
    {
        //move towards placement pos
        counter += Time.deltaTime / 5;

        transform.position = Vector3.Lerp(transform.position, placementPos, counter);
    }
}
