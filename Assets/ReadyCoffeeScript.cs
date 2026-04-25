using UnityEngine;

public class ReadyCoffeeScript : MonoBehaviour, IInteractable
{
    private DragObjectSimple dragScript;
    [Header("Окак")]
    public Transform target;

    void Start()
    {
        dragScript = GetComponent<DragObjectSimple>();
    }

    public string GetDescription()
    {
        if (dragScript != null && dragScript.isDragging) 
        {
            return "Нажмите [E] чтобы сварить кофе";
        }
        
        return "Нажмите [E] чтобы забрать кофе";
    }

    public void Interact()
    {
        dragScript.positionEat.position = target.position;
    }
}
