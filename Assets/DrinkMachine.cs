using UnityEngine;

// Добавляем интерфейс IInteractable
public class DrinkMachine : MonoBehaviour, IInteractable
{
    [Header("Точка для стакана")]
    [SerializeField] private Transform cupPlacePoint; 

    private Cup installedCup; 

    // Реализация метода интерфейса для подсказки на экране
    public string GetDescription()
    {
        if (installedCup == null)
        {
            return "Нажмите [E], чтобы поставить стакан";
        }
        else if (!installedCup.IsFull)
        {
            return "Напиток наливается...";
        }
        else
        {
            return "Напиток готов!";
        }
    }

    // Реализация метода взаимодействия интерфейса (на случай пустых рук)
    public void Interact()
    {
        // Если игрок нажал E с пустыми руками на полный стакан, можно написать тут логику взятия
    }

    public bool CanPlaceCup()
    {
        return installedCup == null;
    }

    public void PlaceAndFillCup(Cup cup)
    {
        installedCup = cup;

        // Отвязываем от камеры/рук игрока
        cup.transform.SetParent(null);

        // Отключаем физику, чтобы стакан не улетел
        Rigidbody rb = cup.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Ставим ровно на точку автомата
        cup.transform.position = cupPlacePoint.position;
        cup.transform.rotation = cupPlacePoint.rotation;

        // Запускаем наливание (скрипт Cup.cs)
        cup.StartFilling();
    }
}
