using System.Collections;
using UnityEngine;

public class Cup : MonoBehaviour
{
    [Header("Настройки жидкости")]
    [SerializeField] private GameObject liquidObject; // Ссылка на объект Liquid внутри стакана
    [SerializeField] private float fillDelay = 3f;     // Время наполнения (в секундах)

    public bool IsFull { get; private set; } = false;
    private bool isFilling = false;

    // Метод для начала наполнения стакана
    public void StartFilling()
    {
        if (!IsFull && !isFilling)
        {
            StartCoroutine(FillRoutine());
        }
    }

    private IEnumerator FillRoutine()
    {
        isFilling = true;
        
        // Ждем указанное время
        yield return new WaitForSeconds(fillDelay);

        if (liquidObject != null)
        {
            liquidObject.SetActive(true); // Включаем жидкость
            IsFull = true;
            Debug.Log("Стакан наполнен!");
        }

        isFilling = false;
    }
}