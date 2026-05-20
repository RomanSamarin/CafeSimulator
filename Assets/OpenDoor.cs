using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour, IInteractable
{
    public Animator animator;
    public GameObject InteractionText;
    
    public string GetDescription()
    {
        Debug.Log("OpenDoor::GetDescription(); -- animator.GetBool(Open):" + animator.GetBool("Open"));
       if (animator.GetBool("Open") == false) return "Нажмите E";
       return "Нажмиите E чтобы закрыть";
    }

    public void Interact()
    {
        Debug.Log("Нажали E");
        if (animator.GetBool("Open") == false)
        {
            animator.SetBool("Open", true); 
        }
        else
        {
            animator.SetBool("Open", false);   
        }

    }

    // Start is called before the first frame update

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
