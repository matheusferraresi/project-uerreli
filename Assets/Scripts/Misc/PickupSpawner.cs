using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goldCoinPrefab;
    [SerializeField] private GameObject healthGlobePrefab;
    [SerializeField] private GameObject staminaGlobePrefab;

    public void DropItems() {
        int randomNum = Random.Range(0, 5);
        int randomAmmount = 0;
        GameObject prefabToSpawn = null;
        
        switch (randomNum)
        {
            case 1:
                prefabToSpawn = goldCoinPrefab;
                break;
            
            case 2:
                prefabToSpawn = healthGlobePrefab;
                break;
            
            case 3:
                prefabToSpawn = staminaGlobePrefab;
                break;
            
            case 4:
                prefabToSpawn = goldCoinPrefab;
                randomAmmount = Random.Range(1, 4);
                break;
            
            case 5:
                prefabToSpawn = healthGlobePrefab;
                randomAmmount = Random.Range(1, 2);
                break;
            
            default:
                prefabToSpawn = goldCoinPrefab;
                break;
        }

        do
        {
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            randomAmmount--;
        } while (randomAmmount == 0);
    }
}
