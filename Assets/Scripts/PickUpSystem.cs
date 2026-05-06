using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickupItem : MonoBehaviour, IInteractable
{
    public string itemName = "Коробка";
    
    private Rigidbody rb;
    private Collider itemCollider;
    private bool isHeld = false;

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
        
        rb.useGravity = false;
        rb.isKinematic = true; 
        
        if (itemCollider != null) itemCollider.enabled = false; 
    }

    public void Drop()
    {
        isHeld = false;
        PlayerInteractor.Instance.heldObject = null;
        
        rb.useGravity = true;
        rb.isKinematic = false;
        
        if (itemCollider != null) itemCollider.enabled = true; 
    }

    void Update()
    {
        if (isHeld)
        {
            Transform targetPosition = PlayerInteractor.Instance.holdArea;
            
            // 1. Плавно перемещаем в точку перед камерой
            transform.position = Vector3.Lerp(transform.position, targetPosition.position, Time.deltaTime * 15f);
            
            // 2. ИСПРАВЛЕНИЕ НАКЛОНА:
            // Берем только поворот игрока влево-вправо (ось Y). Наклоны (X и Z) жестко ставим в 0!
            Quaternion flatRotation = Quaternion.Euler(0f, targetPosition.eulerAngles.y, 0f);
            
            // Плавно выравниваем коробку
            transform.rotation = Quaternion.Lerp(transform.rotation, flatRotation, Time.deltaTime * 15f);
        }
    }
}