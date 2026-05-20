using UnityEngine;

public class Stove : MonoBehaviour, IInteractable
{
    [Header("Точка для котлеты")]
    [SerializeField] private Transform cutletPlacePoint; // Пустой дочерний объект чуть выше поверхности плиты

    private Cutlet installedCutlet;

    public string GetDescription()
    {
        if (installedCutlet == null) return "Нажмите [E], чтобы положить котлету";
        
        switch (installedCutlet.currentState)
        {
            case Cutlet.CutletState.Cooking: return "Котлета жарится...";
            case Cutlet.CutletState.Cooked: return "Нажмите [E], чтобы взять готовую котлету";
            case Cutlet.CutletState.Burnt: return "Нажмите [E], чтобы выбросить сгоревшую котлету";
            default: return "";
        }
    }

    // Этот метод вызывается, если игрок нажал E с пустыми руками
    public void Interact()
    {
        if (installedCutlet != null && PlayerInteractor.Instance.heldObject == null)
        {
            installedCutlet.StopCooking();
            
            PickupItem pickup = installedCutlet.GetComponent<PickupItem>();
            Rigidbody rb = installedCutlet.GetComponent<Rigidbody>();
            Collider col = installedCutlet.GetComponent<Collider>();

            // Переводим котлету обратно в режим удержания
            if (pickup != null)
            {
                pickup.isHeld = true; 
                PlayerInteractor.Instance.heldObject = installedCutlet.gameObject;
            }

            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            if (col != null) col.enabled = false;

            installedCutlet.transform.SetParent(PlayerInteractor.Instance.holdArea);
            installedCutlet = null;
        }
    }

    public bool CanPlaceCutlet()
    {
        return installedCutlet == null;
    }

    public void PlaceAndCook(Cutlet cutlet)
    {
        installedCutlet = cutlet;

        // Отключаем физическое следование за игроком
        PickupItem pickup = cutlet.GetComponent<PickupItem>();
        if (pickup != null)
        {
            pickup.isHeld = false; 
        }

        // Отвязываем от руки игрока
        cutlet.transform.SetParent(null);

        // Фиксируем физику на плите
        Rigidbody rb = cutlet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Включаем коллайдер, чтобы по ней снова можно было кликнуть на плите
        Collider col = cutlet.GetComponent<Collider>();
        if (col != null) col.enabled = true; 

        // Переносим ровно на плиту
        cutlet.transform.position = cutletPlacePoint.position;

        // ИСПРАВЛЕНИЕ ПОВОРОТА:
        // Сохраняем исходные углы котлеты по X и Z, а вращение Y (влево-вправо) берём от плиты
        Vector3 currentAngles = cutlet.transform.eulerAngles;
        cutlet.transform.rotation = Quaternion.Euler(currentAngles.x, cutletPlacePoint.eulerAngles.y, currentAngles.z);

        // Начинаем жарить
        cutlet.StartCooking();
    }
}
