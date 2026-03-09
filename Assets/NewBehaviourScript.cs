// using UnityEngine;

// public class WaitNowGame : MonoBehaviour
// {
//     [Header("UI References")]
//     public RectTransform movingBar;
//     public RectTransform targetZone;

//     [Header("Game Settings")]
//     public float speed = 600f;
//     private float leftBound = -450f;
//     private float rightBound = 450f;

//     // State variables
//     private bool movingRight = true;
//     private bool isGameOver = false;

//     // Update is called once per frame by the Unity Engine
//     void Update()
//     {
//         if (isGameOver) return;

//         MoveTheBar();

//         // Listen for the Spacebar press
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             CheckTiming();
//         }
//     }

//     void MoveTheBar()
//     {
//         // anchoredPosition is how Unity handles 2D UI coordinates
//         Vector2 currentPos = movingBar.anchoredPosition;

//         // Time.deltaTime ensures the speed is frame-rate independent
//         if (movingRight)
//         {
//             currentPos.x += speed * Time.deltaTime;
//             if (currentPos.x >= rightBound) movingRight = false; // Reverse direction
//         }
//         else
//         {
//             currentPos.x -= speed * Time.deltaTime;
//             if (currentPos.x <= leftBound) movingRight = true; // Reverse direction
//         }

//         // Apply the new position back to the UI element
//         movingBar.anchoredPosition = currentPos;
//     }

//     void CheckTiming()
//     {
//         float barX = movingBar.anchoredPosition.x;
//         float targetX = targetZone.anchoredPosition.x;
        
//         // The safe zone is half the width of your green rectangle
//         float safeZone = targetZone.rect.width / 2f;

//         // Calculate the absolute distance between the bar and the exact center
//         if (Mathf.Abs(barX - targetX) <= safeZone)
//         {
//             Debug.Log("PERFECT HIT! Speeding up...");
//             speed += 100f; // Increase difficulty
//         }
//         else
//         {
//             Debug.Log("MISSED! Game Over.");
//             isGameOver = true;
//         }
//     }
// }

using UnityEngine;
using TMPro; // Required for TextMeshPro UI
using UnityEngine.SceneManagement; // Required to restart the game

public class WaitNowGame : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform movingBar;
    public RectTransform targetZone;
    public TMP_Text scoreText;       // Connects to your Score Text
    public GameObject gameOverUI;    // Connects to your Game Over Text

    [Header("Game Settings")]
    public float speed = 600f;
    private float leftBound = -450f;
    private float rightBound = 450f;

    // State variables
    private bool movingRight = true;
    private bool isGameOver = false;
    private int score = 0;           // Tracks the player's score

    void Start()
    {
        // Make sure the Game Over text is hidden when the game starts
        gameOverUI.SetActive(false);
        scoreText.text = "SCORE: 0";
    }

    void Update()
    {
        if (isGameOver)
        {
            // If the game is over, listen for the 'R' key to restart
            if (Input.GetKeyDown(KeyCode.R))
            {
                // This reloads the current scene from scratch
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return; // Stops the rest of the code so the bar stops moving
        }

        MoveTheBar();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckTiming();
        }
    }

    void MoveTheBar()
    {
        Vector2 currentPos = movingBar.anchoredPosition;

        if (movingRight)
        {
            currentPos.x += speed * Time.deltaTime;
            if (currentPos.x >= rightBound) movingRight = false;
        }
        else
        {
            currentPos.x -= speed * Time.deltaTime;
            if (currentPos.x <= leftBound) movingRight = true;
        }

        movingBar.anchoredPosition = currentPos;
    }

    void CheckTiming()
    {
        float barX = movingBar.anchoredPosition.x;
        float targetX = targetZone.anchoredPosition.x;
        float safeZone = targetZone.rect.width / 2f;

        if (Mathf.Abs(barX - targetX) <= safeZone)
        {
            // PERFECT HIT!
            score++; 
            scoreText.text = "SCORE: " + score; // Update the UI text
            speed += 100f; // Make it faster!
        }
        else
        {
            // MISSED!
            isGameOver = true;
            gameOverUI.SetActive(true); // Turn on the Game Over text
        }
    }
}