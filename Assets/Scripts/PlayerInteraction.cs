using UnityEngine;
using TMPro; // Обязательно добавляем для TMP_Text

public class PlayerInteractor : MonoBehaviour
{
    public static PlayerInteractor Instance { get; private set; }

    [Header("Настройки")]
    public Transform holdArea;
    public float interactRange = 3f;
    
    [Header("Интерфейс")]
    public TextMeshProUGUI uiText; // Используем TMP для подсказок
    
    [HideInInspector] public GameObject heldObject;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        // Каждым кадром очищаем текст интерфейса
        if (uiText != null) uiText.text = "";

        // Пускаем луч из центра камеры вперед
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactRange);

        if (hitSomething)
        {
            // Проверяем, можно ли взаимодействовать с объектом
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                // Выводим текст-описание действия на экран
                if (uiText != null) uiText.text = interactable.GetDescription();

                // Обработка нажатия клавиши взаимодействия (E)
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // 1. ЛОГИКА СТОЛА СБОРКИ БУРГЕРОВ
                    BurgerAssembly assembly = hit.collider.GetComponent<BurgerAssembly>();
                    if (assembly != null)
                    {
                        // Если в руках есть предмет — пытаемся добавить его как ингредиент
                        if (heldObject != null)
                        {
                            assembly.TryAddIngredient(heldObject);
                            return; 
                        }
                        // Если руки пусты — забираем готовый бургер
                        else
                        {
                            assembly.Interact();
                            return; 
                        }
                    }

                    // 2. ЛОГИКА АВТОМАТА НАПИТКОВ
                    DrinkMachine machine = hit.collider.GetComponent<DrinkMachine>();
                    if (machine != null && heldObject != null)
                    {
                        Cup cup = heldObject.GetComponent<Cup>();
                        if (cup != null && machine.CanPlaceCup())
                        {
                            machine.PlaceAndFillCup(cup);
                            heldObject = null; // Освобождаем руку игрока
                            return;
                        }
                    }

                    // 3. ЛОГИКА ПЛИТЫ ДЛЯ КОТЛЕТ
                    Stove stove = hit.collider.GetComponent<Stove>();
                    if (stove != null)
                    {
                        // Если в руках котлета и плита свободна — кладем жариться
                        if (heldObject != null)
                        {
                            Cutlet cutlet = heldObject.GetComponent<Cutlet>();
                            if (cutlet != null && stove.CanPlaceCutlet())
                            {
                                stove.PlaceAndCook(cutlet);
                                heldObject = null; // Освобождаем руку игрока
                                return; // Выходим, чтобы сразу не забрать обратно
                            }
                        }
                        // Если руки пусты — забираем готовую котлету с плиты
                        else
                        {
                            stove.Interact();
                            return; 
                        }
                    }

                    // 4. ЛОГИКА ШКАФА
                    if (hit.collider.GetComponent<Shelf>() != null)
                    {
                        interactable.Interact();
                    }
                    // 5. ЛОГИКА ПОДБОРА ПРЕДМЕТА (если руки пусты)
                    else if (heldObject == null)
                    {
                        interactable.Interact();
                    }
                    // 6. ЛОГИКА СБРОСА ПРЕДМЕТА (если смотрим на предмет при полных руках)
                    else
                    {
                        DropObject();
                    }
                    return; // Завершаем выполнение, чтобы не сработал код сброса ниже
                }
            }
        }

        // 7. ЛОГИКА СБРОСА В ПУСТОТУ: Если нажали E в никуда, и в руках что-то есть — бросаем
        if (Input.GetKeyDown(KeyCode.E) && heldObject != null)
        {
            DropObject();
        }
    }

    // Метод для безопасного сброса удерживаемого предмета на землю
    public void DropObject()
    {
        if (heldObject != null)
        {
            PickupItem pickup = heldObject.GetComponent<PickupItem>();
            if (pickup != null)
            {
                pickup.Drop();
            }
            heldObject = null; // Принудительно очищаем руку в самом плеере
        }
    }
}
