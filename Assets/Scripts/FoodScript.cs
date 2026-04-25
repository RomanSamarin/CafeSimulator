using UnityEngine;

public class DragObjectSimple : MonoBehaviour, IInteractable
{
    [Header("Настройки")]
    public float holdDistance = 2.5f;    // расстояние от камеры
    public float smoothSpeed = 15f;      // плавность движения
    
    public bool isDragging = false;
    private Rigidbody rb;
    public Transform positionEat;
    private Collider col;
    private Camera playerCamera;
    private Vector3 targetPosition;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        playerCamera = Camera.main;
        
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
    }
    
    public string GetDescription()
    {
        if (!isDragging)
            return "Нажмите [E] чтобы взять";
        else
            return "Нажмите [E] чтобы отпустить";
    }
    
    public void Interact()
    {
        if (!isDragging)
        {
            // Взять объект
            isDragging = true;
            rb.isKinematic = true;
            rb.useGravity = false;
            col.isTrigger = true;
            Debug.Log("Взял объект");
        }
        else
        {
            // Отпустить объект
            isDragging = false;
            rb.isKinematic = false;
            rb.useGravity = true;
            col.isTrigger = false;
            
            // Небольшой толчок вперёд при отпускании
            rb.velocity = playerCamera.transform.forward * 3f;
            Debug.Log("Отпустил объект");
        }
    }
    
    void Update()
    {
        if (!isDragging) return;
        
        // Куда смотрит камера - туда и летим
        targetPosition = playerCamera.transform.position + playerCamera.transform.forward * holdDistance;
        
        // Проверка чтобы не улетел за стены
        RaycastHit hit;
        if (Physics.Linecast(playerCamera.transform.position, targetPosition, out hit))
        {
            if (hit.transform != transform)
            {
                targetPosition = hit.point - playerCamera.transform.forward * 0.1f;
            }
        }
        
        // Плавно двигаем объект
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
    }
}