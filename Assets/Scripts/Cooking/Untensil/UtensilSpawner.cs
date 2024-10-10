using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UtensilSpawner : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Utensil utensilPrefab;
    [SerializeField] private List<Transform> utensilSpawnPoint;

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (Transform spawnPoint in utensilSpawnPoint)
        {
            if (spawnPoint.childCount == 0)
            {
                Instantiate(utensilPrefab, spawnPoint);
                break;
            }
        }
    }
}