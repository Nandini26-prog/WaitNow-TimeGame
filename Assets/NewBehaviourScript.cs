using UnityEngine;

public class WaitNowGame : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform movingBar;
    public RectTransform targetZone;

    [Header("Game Settings")]
    public float speed = 600f;
    private float leftBound = -450f;
    private float rightBound = 450f;

    // State variables
    private bool movingRight = true;
    private bool isGameOver = false;

    // Update is called once per frame by the Unity Engine
    void Update()
    {
        if (isGameOver) return;

        MoveTheBar();

        // Listen for the Spacebar press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckTiming();
        }
    }

    void MoveTheBar()
    {
        // anchoredPosition is how Unity handles 2D UI coordinates
        Vector2 currentPos = movingBar.anchoredPosition;

        // Time.deltaTime ensures the speed is frame-rate independent
        if (movingRight)
        {
            currentPos.x += speed * Time.deltaTime;
            if (currentPos.x >= rightBound) movingRight = false; // Reverse direction
        }
        else
        {
            currentPos.x -= speed * Time.deltaTime;
            if (currentPos.x <= leftBound) movingRight = true; // Reverse direction
        }

        // Apply the new position back to the UI element
        movingBar.anchoredPosition = currentPos;
    }

    void CheckTiming()
    {
        float barX = movingBar.anchoredPosition.x;
        float targetX = targetZone.anchoredPosition.x;
        
        // The safe zone is half the width of your green rectangle
        float safeZone = targetZone.rect.width / 2f;

        // Calculate the absolute distance between the bar and the exact center
        if (Mathf.Abs(barX - targetX) <= safeZone)
        {
            Debug.Log("PERFECT HIT! Speeding up...");
            speed += 100f; // Increase difficulty
        }
        else
        {
            Debug.Log("MISSED! Game Over.");
            isGameOver = true;
        }
    }
}
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class NewBehaviourScript : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
