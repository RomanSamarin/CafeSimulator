using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasketManager : MonoBehaviour
{
    public static BasketManager Instance;

    [Header("Economy")]
    public int currentMoney = 1000; 
    public TMP_Text moneyText;      // Текст баланса (просто число)

    [Header("Basket")]
    public TMP_Text basketTotalText; // Текст ТЕКУЩЕЙ СУММЫ заказа
    public List<ProductSO> itemsInBasket = new List<ProductSO>();
    public Transform spawnPoint;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        UpdateUI();
    }

    public void AddProduct(ProductSO product)
{
    int basketTotal = GetTotalBasketPrice();
    int neededMoney = basketTotal + product.cost;

    Debug.Log($"Проверка: У нас {currentMoney}, нужно {neededMoney} (Корзина {basketTotal} + Товар {product.cost})");

    if (currentMoney >= neededMoney)
    {
        itemsInBasket.Add(product);
        UpdateUI();
        Debug.Log("Успех! Товар в корзине.");
    }
    else
    {
        Debug.LogError("Отказ! Мало денег.");
    }
}

    public void Checkout()
    {
        int totalCost = GetTotalBasketPrice();

        if (itemsInBasket.Count > 0 && currentMoney >= totalCost)
        {
            currentMoney -= totalCost; 
            
            foreach (ProductSO product in itemsInBasket)
            {
                if (product.gameObjectPrefab != null)
                {
                    Instantiate(product.gameObjectPrefab, spawnPoint.position, Quaternion.identity);
                }
            }

            itemsInBasket.Clear();
            UpdateUI(); // Обнуляем сумму заказа и обновляем кошелек
        }
    }

    private int GetTotalBasketPrice()
    {
        int total = 0;
        foreach (var item in itemsInBasket) total += item.cost;
        return total;
    }

    private void UpdateUI()
{
    Debug.Log("UpdateUI вызван!");
    
    if (moneyText != null) 
        moneyText.text = currentMoney.ToString();
    else 
        Debug.LogWarning("MoneyText не назначен в инспекторе!");

    if (basketTotalText != null) 
    {
        string total = GetTotalBasketPrice().ToString();
        basketTotalText.text = total;
        Debug.Log("Текст корзины обновлен на: " + total);
    }
    else 
    {
        Debug.LogWarning("BasketTotalText не назначен в инспекторе!");
    }
}
    
    
}
