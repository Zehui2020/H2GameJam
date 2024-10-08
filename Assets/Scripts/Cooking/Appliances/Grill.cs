using System.Collections.Generic;
using UnityEngine;

public class Grill : Appliance
{
    [SerializeField] private List<Transform> foodSpawnPos;
    [SerializeField] private DishPickup dishPickup;
}