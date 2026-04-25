using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPanelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject uiProductCardPrefab;
    [SerializeField] private Transform rootObject;
    public List<ProductSO> productSOs;
    public List<GameObject> buttons;

    private void Start()
    {
        buttons = new List<GameObject>();
        foreach (ProductSO productSO in productSOs)
        {
            GameObject clonePrefab = Instantiate(uiProductCardPrefab, rootObject);
            Debug.Log("UiPanelSpawner::Start(); -- clonePrefab:" + clonePrefab, clonePrefab);
            ProductCard productCard = clonePrefab.GetComponent<ProductCard>();
            productCard.Init(productSO);
            buttons.Add(clonePrefab);
        }
    }
}
