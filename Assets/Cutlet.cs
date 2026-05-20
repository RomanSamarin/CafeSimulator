using System.Collections;
using UnityEngine;

public class Cutlet : MonoBehaviour
{
    public enum CutletState { Raw, Cooking, Cooked, Burnt }
    
    [Header("Состояние")]
    public CutletState currentState = CutletState.Raw;

    [Header("Время приготовления (сек)")]
    [SerializeField] private float cookTime = 5f;
    [SerializeField] private float burnTime = 5f;

    [Header("Цвета для этапов")]
    [SerializeField] private Color rawColor = new Color(0.7f, 0.2f, 0.2f);    
    [SerializeField] private Color cookedColor = new Color(0.35f, 0.15f, 0.05f); 
    [SerializeField] private Color burntColor = new Color(0.1f, 0.1f, 0.1f);    

    private Renderer myRenderer;

    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        if (myRenderer != null) myRenderer.material.color = rawColor;
    }

    public void StartCooking()
    {
        if (currentState == CutletState.Raw)
        {
            StartCoroutine(CookRoutine());
        }
    }

    private IEnumerator CookRoutine()
    {
        currentState = CutletState.Cooking;
        float timer = 0f;

        while (timer < cookTime)
        {
            timer += Time.deltaTime;
            if (myRenderer != null)
            {
                myRenderer.material.color = Color.Lerp(rawColor, cookedColor, timer / cookTime);
            }
            yield return null;
        }

        currentState = CutletState.Cooked;
        Debug.Log("Котлета готова!");

        timer = 0f;
        while (timer < burnTime)
        {
            timer += Time.deltaTime;
            if (myRenderer != null)
            {
                myRenderer.material.color = Color.Lerp(cookedColor, burntColor, timer / burnTime);
            }
            yield return null;
        }

        currentState = CutletState.Burnt;
        Debug.Log("Котлета сгорела!");
    }

    public void StopCooking()
    {
        StopAllCoroutines();
    }
}
