using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ProductCard : MonoBehaviour
{
    public TMP_Text displayName;
    public Image image;
    public TMP_Text cost;
    private ProductSO _productSO;
    public List<GameObject> productsOrder;
    private int NumberOfProducts = -1;

    public void Init(ProductSO productSO)
    {
        Debug.Log("ProductCard::Init(); -- productSO:" + productSO);
        if (!productSO) return;
        if (displayName) displayName.text = productSO.displayName;
        if (image) image.sprite = productSO.sprite;
        if (cost) cost.text = productSO.cost.ToString();
        this._productSO = productSO;
        // Button localButton = gameObject.GetComponentInChildren<Button>();
        // localButton.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        // Debug.Log("ProductCard::OnButtonClick(); -- _productSO:" + _productSO);
        // ProductsManager.Instance?.SpawnObjectProduct(_productSO);
        //NumberOfProducts +=1;
        //  productsOrder.Add(_productSO.gameObjectPrefab);
        if (_productSO != null)
    {
        // Просто обращаемся к корзине и передаем данные
        BasketManager.Instance.AddProduct(_productSO);
            Debug.Log("Был добавлен в корзину");
    }
    }
}
