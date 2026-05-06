using UnityEngine;
using System.Collections; // Обязательно для IEnumerator
using System.Collections.Generic;

public class Shelf : MonoBehaviour, IInteractable
{
    public string shelfName = "Шкаф";
    public List<Transform> shelfSlots;
    private int currentSlotIndex = 0;

    [Header("Настройки анимации")]
    public float flySpeed = 1.5f; // Время полета в секундах
    public float arcHeight = 0.5f; // На какую высоту подлетит предмет в полете

    public string GetDescription()
    {
        if (PlayerInteractor.Instance.heldObject != null && 
            PlayerInteractor.Instance.heldObject.GetComponent<ProductBox>() != null)
        {
            return $"[E] Выложить товар на {shelfName}";
        }
        return shelfName;
    }

    public void Interact()
    {
        GameObject held = PlayerInteractor.Instance.heldObject;
        if (held == null) return;

        ProductBox box = held.GetComponent<ProductBox>();
        if (box != null)
        {
            PlaceProductFromBox(box);
        }
    }

    private void PlaceProductFromBox(ProductBox box)
    {
        if (currentSlotIndex >= shelfSlots.Count)
        {
            Debug.Log("Полка заполнена!");
            return;
        }

        GameObject productToPlace = box.TakeProduct();

        if (productToPlace != null)
        {
            // Запускаем процесс анимации
            StartCoroutine(AnimateProductFly(productToPlace, box.transform.position, shelfSlots[currentSlotIndex]));
            currentSlotIndex++;
        }
    }

    // Корутина для плавного перемещения
    private IEnumerator AnimateProductFly(GameObject product, Vector3 startPos, Transform target)
    {
        float elapsedTime = 0;
        
        // Включаем объект и отключаем физику, чтобы не мешала лететь
        product.SetActive(true);
        if (product.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        while (elapsedTime < flySpeed)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / flySpeed; // Процент завершения пути (от 0 до 1)

            // 1. Линейное перемещение между точками
            Vector3 currentPos = Vector3.Lerp(startPos, target.position, t);

            // 2. Добавляем изгиб (дугу) по высоте
            // Используем Sin(t * Пи), чтобы получить горбик: в начале 0, в середине 1, в конце 0
            float yOffset = Mathf.Sin(t * Mathf.PI) * arcHeight;
            currentPos.y += yOffset;

            product.transform.position = currentPos;

            // Плавно вращаем предмет, чтобы он встал ровно по слоту
            product.transform.rotation = Quaternion.Lerp(product.transform.rotation, target.rotation, t);

            yield return null; // Ждем следующего кадра
        }

        // Финально закрепляем в точке (чтобы не было погрешностей Lerp)
        product.transform.position = target.position;
        product.transform.rotation = target.rotation;
    }
}