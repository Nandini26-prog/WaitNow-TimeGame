using UnityEngine;

public class UIPulse : MonoBehaviour
{
    public float pulseSpeed = 4f; 
    public float pulseSize = 1.05f; 
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Sine wave to smoothly scale up and down over time
        float currentScale = 1f + Mathf.Sin(Time.time * pulseSpeed) * (pulseSize - 1f);
        transform.localScale = originalScale * currentScale;
    }
}