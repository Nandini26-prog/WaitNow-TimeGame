using UnityEngine;

public class UITrail : MonoBehaviour
{
    public Transform target; // Drag the original MovingBar here
    public float delay = 0.05f; // How much it lags behind
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        // This smoothly "chases" the main bar
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, delay);
    }
}