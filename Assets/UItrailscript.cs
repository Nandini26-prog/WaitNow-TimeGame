using UnityEngine;

public class UITrail : MonoBehaviour
{
    public Transform target; 
    public float delay = 0.05f; 
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, delay);
    }
}