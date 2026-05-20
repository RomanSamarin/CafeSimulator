using UnityEngine;

public class BurgerAssembly : MonoBehaviour, IInteractable
{
    // Этапы сборки: Пусто -> Положили нижнюю булку -> Положили котлету -> Положили сыр -> Готово (верхняя булка)
    public enum AssemblyStage { Empty, BottomBun, Cutlet, Cheese, Completed }
    
    [Header("Текущий этап сборки")]
    public AssemblyStage currentStage = AssemblyStage.Empty;

    [Header("Визуальные части бургера на столе")]
    [SerializeField] private GameObject visualBottomBun;
    [SerializeField] private GameObject visualCutlet;
    [SerializeField] private GameObject visualCheese;
    [SerializeField] private GameObject visualTopBun;

    [Header("Префаб готового бургера для выдачи")]
    [SerializeField] private GameObject completedBurgerPrefab;
    [SerializeField] private Transform spawnPoint; // Откуда игрок заберет готовый бургер

    void Start()
    {
        // При старте игры принудительно скрываем все части бургера на столе
        UpdateVisuals();
    }

    public string GetDescription()
    {
        switch (currentStage)
        {
            case AssemblyStage.Empty: 
                return "Положите нижнюю булочку";
            case AssemblyStage.BottomBun: 
                return "Добавьте готовую котлету";
            case AssemblyStage.Cutlet: 
                return "Добавьте сыр";
            case AssemblyStage.Cheese: 
                return "Накройте верхней булочкой";
            case AssemblyStage.Completed: 
                return "Нажмите [E], чтобы взять готовый бургер";
            default: 
                return "";
        }
    }

    // Вызывается из PlayerInteractor, если у игрока пустые руки
    public void Interact()
    {
        if (currentStage == AssemblyStage.Completed && PlayerInteractor.Instance.heldObject == null)
        {
            // Спавним целый бургер
            GameObject newBurger = Instantiate(completedBurgerPrefab, spawnPoint.position, spawnPoint.rotation);
            
            PickupItem pickup = newBurger.GetComponent<PickupItem>();
            if (pickup != null)
            {
                pickup.isHeld = true;
                PlayerInteractor.Instance.heldObject = newBurger;
                newBurger.GetComponent<Rigidbody>().isKinematic = true;
                newBurger.GetComponent<Rigidbody>().useGravity = false;
                newBurger.GetComponent<Collider>().enabled = false;
                newBurger.transform.SetParent(PlayerInteractor.Instance.holdArea);
            }

            // Очищаем стол для нового бургера
            currentStage = AssemblyStage.Empty;
            UpdateVisuals();
        }
    }

    // Вызывается из PlayerInteractor, если в руках игрока есть предмет
    public void TryAddIngredient(GameObject ingredient)
    {
        // ШАГ 1: Кладем нижнюю булочку
        if (currentStage == AssemblyStage.Empty && ingredient.name.Contains("Bun_Bottom"))
        {
            currentStage = AssemblyStage.BottomBun;
            DestroyIngredient(ingredient);
            return; // Сразу выходим, чтобы не сработали другие шаги в этот кадр
        }

        // ШАГ 2: Кладем готовую котлету
        if (currentStage == AssemblyStage.BottomBun)
        {
            Cutlet cutlet = ingredient.GetComponent<Cutlet>();
            // Проверяем, что это котлета, и она ИМЕННО приготовлена (не сырая, не сгоревшая)
            if (cutlet != null && cutlet.currentState == Cutlet.CutletState.Cooked)
            {
                currentStage = AssemblyStage.Cutlet;
                DestroyIngredient(ingredient);
                return;
            }
        }

        // ШАГ 3: Кладем сыр
        if (currentStage == AssemblyStage.Cutlet && ingredient.name.Contains("Cheese"))
        {
            currentStage = AssemblyStage.Cheese;
            DestroyIngredient(ingredient);
            return;
        }

        // ШАГ 4: Накрываем верхней булочкой
        if (currentStage == AssemblyStage.Cheese && ingredient.name.Contains("Bun_Top"))
        {
            currentStage = AssemblyStage.Completed;
            DestroyIngredient(ingredient);
            return;
        }
    }

    private void DestroyIngredient(GameObject ingredient)
    {
        PlayerInteractor.Instance.heldObject = null;
        Destroy(ingredient);
        
        // Обновляем отображение слоев бургера сразу после уничтожения предмета из рук
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        // Четкая логика включения слоев: они остаются видимыми, если этап сборки прошел дальше них
        if (visualBottomBun != null) visualBottomBun.SetActive(currentStage == AssemblyStage.BottomBun || currentStage == AssemblyStage.Cutlet || currentStage == AssemblyStage.Cheese || currentStage == AssemblyStage.Completed);
        if (visualCutlet != null) visualCutlet.SetActive(currentStage == AssemblyStage.Cutlet || currentStage == AssemblyStage.Cheese || currentStage == AssemblyStage.Completed);
        if (visualCheese != null) visualCheese.SetActive(currentStage == AssemblyStage.Cheese || currentStage == AssemblyStage.Completed);
        if (visualTopBun != null) visualTopBun.SetActive(currentStage == AssemblyStage.Completed);
    }
}
