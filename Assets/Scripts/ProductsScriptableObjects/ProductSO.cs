using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProduct", menuName = "Products/NewProduct")]
public class ProductSO : ScriptableObject
{
    public string displayName;
    public Sprite sprite;
    public int cost;
    public GameObject gameObjectPrefab;
    
}
