using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoffeeMachine : MonoBehaviour, IInteractable
{
    [Header("Настройки")]
    public float brewTime = 5f;
    
    [Header("UI")]
    public GameObject progressBarUI;
    public Slider progressBar;

    private CoffeeCup currentCup; // Ссылка на чашку, которая сейчас стоит в автомате
    private bool isBrewing = false; // Варится ли кофе прямо сейчас

    void Start()
    {
        if (progressBarUI != null) progressBarUI.SetActive(false);
    }

    // Текст для интерфейса (когда смотрим на саму кофемашину)
    public string GetDescription()
    {
        if (currentCup == null)
            return "Поставьте пустую чашку";

        if (isBrewing)
            return "Кофе готовится...";

        // ПРОВЕРКА: Если в чашке уже есть кофе — не даем варить!
        if (currentCup.hasCoffee)
            return "Чашка уже полная!";

        return "[E] Сварить кофе";
    }

    public void Interact()
    {
        // Разрешаем варить только если: есть чашка, она ПУСТАЯ, и процесс еще не идет
        if (currentCup != null && !currentCup.hasCoffee && !isBrewing)
        {
            StartCoroutine(BrewCoffee());
        }
    }

    private IEnumerator BrewCoffee()
    {
        isBrewing = true;
        if (progressBarUI != null) progressBarUI.SetActive(true);

        if (progressBar != null)
        {
            progressBar.minValue = 0;
            progressBar.maxValue = brewTime;
            progressBar.value = 0;
        }

        float timer = 0f;
        while (timer < brewTime)
        {
            timer += Time.deltaTime;
            if (progressBar != null) progressBar.value = timer;
            yield return null;
        }

        isBrewing = false;
        if (progressBarUI != null) progressBarUI.SetActive(false);

        // ВАЖНО: Вызываем метод из твоего скрипта CoffeeCup, чтобы налить кофе
        if (currentCup != null)
        {
            currentCup.AddCoffee(); 
        }
    }

    // --- СИСТЕМА ОБНАРУЖЕНИЯ ЧАШКИ ---
    
    // Срабатывает, когда физическая чашка касается зоны кофемашины
    private void OnTriggerEnter(Collider other)
    {
        CoffeeCup cup = other.GetComponent<CoffeeCup>();
        if (cup != null)
        {
            currentCup = cup; // Запоминаем чашку
        }
    }

    // Срабатывает, когда игрок забирает чашку из зоны
    private void OnTriggerExit(Collider other)
    {
        CoffeeCup cup = other.GetComponent<CoffeeCup>();
        if (cup != null && currentCup == cup)
        {
            currentCup = null; // Очищаем память автомата
        }
    }
}