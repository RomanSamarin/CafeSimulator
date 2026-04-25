using UnityEngine;

public class CoffeeCup : MonoBehaviour, IInteractable
{
    public bool hasCoffee = false;  // есть ли кофе
    public GameObject coffeeVisual;  // визуал кофе
    
    private bool isInHand = false;
    private Rigidbody rb;
    private Collider col;
    private Transform handPoint;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        
        // Находим точку для руки
        GameObject hand = GameObject.Find("GlobalHandTarget");
        if (hand != null) handPoint = hand.transform;
        
        // Обновляем визуал
        if (coffeeVisual != null)
            coffeeVisual.SetActive(hasCoffee);
    }
    
    public string GetDescription()
    {
        if (!isInHand)
        {
            if (hasCoffee)
                return "Горячий кофе [E] взять";
            else
                return "Пустая чашка [E] взять";
        }
        return "Нажмите [E] чтобы бросить";
    }
    
    public void Interact()
    {
        if (!isInHand)
        {
            TakeInHand();
        }
        else
        {
            Throw();
        }
    }
    
    void TakeInHand()
    {
        isInHand = true;
        rb.isKinematic = true;
        rb.useGravity = false;
        col.isTrigger = true;
        
        if (handPoint != null)
        {
            transform.SetParent(handPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        
        Debug.Log("Взял чашку");
    }
    
    void Throw()
    {
        isInHand = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        col.isTrigger = false;
        transform.SetParent(null);
        
        if (Camera.main != null)
        {
            rb.velocity = Camera.main.transform.forward * 4f;
        }
        
        Debug.Log("Бросил чашку");
    }
    
    public void AddCoffee()
    {
        hasCoffee = true;
        if (coffeeVisual != null)
            coffeeVisual.SetActive(true);
        Debug.Log("Кофе добавлен в чашку");
    }
}