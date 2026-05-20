using UnityEngine;
using System.Collections.Generic;

public class ProductBox : MonoBehaviour
{
    [Header("Настройки продуктов")]
    [Tooltip("Префаб продукта, который будет автоматически спавниться")]
    public GameObject productPrefab;

    [Tooltip("Количество продуктов для автоматического создания")]
    public int initialCount = 5;

    [Tooltip("Если включено, продукты будут созданы автоматически при старте")]
    public bool autoSpawn = true;

    [Header("Размещение продуктов в коробке")]
    [Tooltip("Локальное смещение первого продукта относительно коробки")]
    public Vector3 startOffset = Vector3.zero;

    [Tooltip("Расстояние между продуктами по осям")]
    public Vector3 spacing = new Vector3(0.2f, 0.2f, 0.2f);

    [Tooltip("Максимальное число продуктов в одном ряду (для сетки)")]
    public int itemsPerRow = 3;

    [Tooltip("Тип размещения: Stack — все в одной точке, Grid — сеткой")]
    public PlacementType placement = PlacementType.Grid;

    [Header("Поведение продуктов")]
    [Tooltip("Если true, заспавненные продукты будут выключены до тех пор, пока их не возьмут")]
    public bool deactivateOnSpawn = true;

    // Список продуктов, которые сейчас лежат в коробке
    public List<GameObject> productsInBox = new List<GameObject>();

    public enum PlacementType
    {
        Stack,
        Grid
    }

    private void Start()
    {
        if (autoSpawn)
        {
            ClearProducts();      // Удаляем всё, что могло остаться от ручного заполнения
            SpawnProducts();      // Создаём продукты согласно настройкам
        }
    }

    /// <summary>
    /// Создаёт указанное количество продуктов и размещает их внутри коробки.
    /// </summary>
    public void SpawnProducts()
    {
        if (productPrefab == null)
        {
            Debug.LogWarning("ProductBox: Не назначен префаб продукта (productPrefab).");
            return;
        }

        for (int i = 0; i < initialCount; i++)
        {
            // Создаём экземпляр продукта как дочерний объект коробки
            GameObject newProduct = Instantiate(productPrefab, transform);
            newProduct.name = productPrefab.name + "_" + i;

            // Рассчитываем локальную позицию в зависимости от типа размещения
            Vector3 localPos = CalculatePosition(i);
            newProduct.transform.localPosition = localPos;
            newProduct.transform.localRotation = Quaternion.identity;

            // Если нужно, прячем продукт до момента взятия
            if (deactivateOnSpawn)
                newProduct.SetActive(false);

            // Добавляем в список доступных продуктов
            productsInBox.Add(newProduct);
        }
    }

    /// <summary>
    /// Выдаёт один продукт из коробки (последний в списке).
    /// Активирует его и открепляет от коробки.
    /// </summary>
    public GameObject TakeProduct()
    {
        if (productsInBox.Count == 0)
        {
            Debug.Log("ProductBox: Коробка пуста.");
            return null;
        }

        // Берём последний продукт (можно изменить логику на первый или случайный)
        GameObject product = productsInBox[productsInBox.Count - 1];
        productsInBox.RemoveAt(productsInBox.Count - 1);

        // Делаем продукт независимым от коробки и активируем
        product.transform.SetParent(null);
        if (deactivateOnSpawn)
            product.SetActive(true);

        return product;
    }

    /// <summary>
    /// Возвращает количество оставшихся продуктов.
    /// </summary>
    public int GetProductCount()
    {
        return productsInBox.Count;
    }

    /// <summary>
    /// Удаляет все созданные продукты и очищает список.
    /// </summary>
    public void ClearProducts()
    {
        // Уничтожаем дочерние объекты, которые являются продуктами
        for (int i = productsInBox.Count - 1; i >= 0; i--)
        {
            if (productsInBox[i] != null)
            {
                if (Application.isPlaying)
                    Destroy(productsInBox[i]);
                else
                    DestroyImmediate(productsInBox[i]);
            }
        }
        productsInBox.Clear();
    }

    // Вычисление позиции продукта в зависимости от индекса и типа размещения
    private Vector3 CalculatePosition(int index)
    {
        Vector3 pos;
        if (placement == PlacementType.Stack)
        {
            // Все продукты в одной точке, можно сдвигать только по вертикали
            pos = startOffset + new Vector3(0, index * spacing.y, 0);
        }
        else // Grid
        {
            int row = index / itemsPerRow;
            int col = index % itemsPerRow;
            pos = startOffset + new Vector3(col * spacing.x, row * spacing.y, 0);
        }
        return pos;
    }

    // Для удобства можно вызвать SpawnProducts() из редактора (контекстное меню)
    [ContextMenu("Spawn Products Now")]
    private void SpawnProductsEditor()
    {
        ClearProducts();
        SpawnProducts();
    }
}