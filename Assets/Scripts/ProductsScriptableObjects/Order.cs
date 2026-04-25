using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Order : MonoBehaviour
{
    public static Order Instance;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private List<GameObject> _list;
    public void GetList(List<GameObject> list)
    {
        this._list = list;
        
    }
}

