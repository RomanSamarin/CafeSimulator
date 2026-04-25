using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductsManager : MonoBehaviour
{
    public static ProductsManager Instance;
    public Transform spawnPoint;
    public Transform parent;
    public List<GameObject> spawnedObject;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        spawnedObject = new List<GameObject>();
    }

    public void SpawnObjectProduct(ProductSO productSO)
    {
        Debug.Log("ProductsManager::SpawnObjectProduct(); -- productSO:" + productSO);
        GameObject clonePrefab = Instantiate(productSO.gameObjectPrefab, spawnPoint.position, spawnPoint.rotation, parent);
        Debug.Log("ProductsManager::SpawnObjectProduct(); -- clonePrefab:" + clonePrefab, clonePrefab);
        spawnedObject.Add(clonePrefab);
    }
}
