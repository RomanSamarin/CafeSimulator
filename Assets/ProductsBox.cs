using UnityEngine;
using System.Collections.Generic;

public class ProductBox : MonoBehaviour
{
    // Список товаров, которые лежат в коробке (можно заполнить в инспекторе)
    public List<GameObject> productsInBox = new List<GameObject>();

    // Метод, который отдает один товар
    public GameObject TakeProduct()
    {
        if (productsInBox.Count > 0)
        {
            GameObject product = productsInBox[productsInBox.Count - 1]; // Берем последний
            productsInBox.RemoveAt(productsInBox.Count - 1); // Удаляем из списка
            return product;
        }
        return null; // Коробка пуста
    }
}