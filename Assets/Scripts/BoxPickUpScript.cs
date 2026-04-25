using UnityEngine;

public class BoxPickUpScript : MonoBehaviour, IInteractable
{
    private bool isHolding = false;
    private Rigidbody rb;
    private Collider col;
    private Transform handTarget;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        
        GameObject hand = GameObject.Find("GlobalHandTarget");
        if (hand != null) handTarget = hand.transform;
    }
    
    public string GetDescription()
    {
        if (!isHolding) return "Нажмите [E] чтобы взять коробку";
        return "Нажмите [E] чтобы бросить";
    }
    
    public void Interact()
    {
        if (!isHolding)
        {
            isHolding = true;
            rb.isKinematic = true;
            rb.useGravity = false;
            col.isTrigger = true;
            
            if (handTarget != null)
            {
                transform.SetParent(handTarget);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
        }
        else
        {
            isHolding = false;
            rb.isKinematic = false;
            rb.useGravity = true;
            col.isTrigger = false;
            transform.SetParent(null);
            
            if (Camera.main != null)
                rb.velocity = Camera.main.transform.forward * 5f;
        }
    }
    
    void Update()
    {
        if (isHolding && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
}