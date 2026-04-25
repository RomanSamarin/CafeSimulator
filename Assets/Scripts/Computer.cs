using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Computer : MonoBehaviour, IInteractable
{
    public GameObject PanelComputer;
    public bool isActivePanel;
    public GameObject InteractionText;
    public MonoBehaviour playerScript;
    // Start is called before the first frame update
    void Start()
    {
        isActivePanel = false;
    }

    // Update is called once per frame
    public string GetDescription()
    {
        if(isActivePanel == false) return "Нажмите [E] для изпользования компьютера";
        return  "Компьютер уже включен";
    }
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape )&& isActivePanel == true)
        {
            isActivePanel = false;
            PanelComputer.SetActive(false);
            InteractionText.SetActive(true);
            playerScript.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void Interact()
    {
        isActivePanel = true;
        PanelComputer.SetActive(true);
        InteractionText.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerScript.enabled = false;;
    }
}
