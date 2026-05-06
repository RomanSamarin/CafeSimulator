using UnityEngine;
using TMPro; // Обязательно добавляем для TMP_Text

public class PlayerInteractor : MonoBehaviour
{
    public static PlayerInteractor Instance { get; private set; }

    [Header("Настройки")]
    public Transform holdArea;
    public float interactRange = 3f;
    
    [Header("Интерфейс")]
    public TextMeshProUGUI uiText; // Теперь используем TMP
    
    [HideInInspector] public GameObject heldObject;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        // Очищаем текст
        if (uiText != null) uiText.text = "";

        // Пускаем луч
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactRange);

        if (hitSomething)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                // Показываем текст (GetDescription сам решит, что писать)
                if (uiText != null) uiText.text = interactable.GetDescription();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // ЛОГИКА 1: Если смотрим на шкаф
                    if (hit.collider.GetComponent<Shelf>() != null)
                    {
                        interactable.Interact();
                    }
                    // ЛОГИКА 2: Если смотрим на предмет и руки пусты
                    else if (heldObject == null)
                    {
                        interactable.Interact();
                    }
                    // ЛОГИКА 3: Если смотрим на предмет, но руки заняты — бросаем старый
                    else
                    {
                        DropObject();
                    }
                    return; // Выходим из Update, чтобы не сработал код ниже
                }
            }
        }

        // ЛОГИКА 4: Если нажали E, смотря в пустоту, и в руках что-то есть — бросаем
        if (Input.GetKeyDown(KeyCode.E) && heldObject != null)
        {
            DropObject();
        }
    }

    private void DropObject()
    {
        if (heldObject != null)
        {
            heldObject.GetComponent<PickupItem>().Drop();
        }
    }
}