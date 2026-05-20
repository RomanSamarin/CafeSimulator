using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickupItem : MonoBehaviour, IInteractable
{
    public string itemName = "Предмет";
    
    [HideInInspector] public bool isHeld = false; 

    private Rigidbody rb;
    private Collider itemCollider;
    
    // Переменная для сохранения исходного наклона по X
    private float initialRotationX;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();
    }

    public string GetDescription()
    {
        return $"Взять: {itemName}";
    }

    public void Interact()
    {
        if (PlayerInteractor.Instance.heldObject != null) return;
        PickUp();
    }

    private void PickUp()
    {
        isHeld = true;
        PlayerInteractor.Instance.heldObject = this.gameObject;
        
        // ЗАПОМИНАЕМ ИСХОДНЫЙ НАКЛОН: Сохраняем текущий угол X перед тем, как взять в руки
        initialRotationX = transform.eulerAngles.x;

        rb.useGravity = false;
        rb.isKinematic = true; 
        
        if (itemCollider != null) itemCollider.enabled = false; 
    }

    public void Drop()
    {
        isHeld = false; 
        transform.SetParent(null); 

        if (PlayerInteractor.Instance.heldObject == this.gameObject)
        {
            PlayerInteractor.Instance.heldObject = null;
        }
        
        rb.isKinematic = false;
        rb.useGravity = true;
        
        rb.AddForce(Camera.main.transform.forward * 2f, ForceMode.Impulse);
        
        if (itemCollider != null) itemCollider.enabled = true; 
    }

    void Update()
    {
        if (isHeld)
        {
            Transform targetPosition = PlayerInteractor.Instance.holdArea;
            
            // 1. Плавно перемещаем в точку перед камерой
            transform.position = Vector3.Lerp(transform.position, targetPosition.position, Time.deltaTime * 15f);
            
            // 2. КОМБИНИРОВАННЫЙ ПОВОРОТ:
            // Берем сохраненный X, Y берем от направления игрока, Z выставляем от точки holdArea
            Quaternion targetRotation = Quaternion.Euler(
                initialRotationX, 
                targetPosition.eulerAngles.y, 
                targetPosition.eulerAngles.z
            );
            
            // Плавно разворачиваем предмет
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 15f);
        }
    }
}
