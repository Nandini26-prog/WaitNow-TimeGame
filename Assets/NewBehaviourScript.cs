using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class WaitNowGame : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform movingBar;
    [SerializeField] private RectTransform targetZone;
    [SerializeField] private RectTransform container;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text feedbackText; // New: To show "PERFECT!" or "NICE!"
    [SerializeField] private GameObject gameOverUI;

    [Header("Audio")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip perfectSound;
    [SerializeField] private AudioClip missSound;
    private AudioSource audioSource;

    [Header("Game Settings")]
    public float speed = 600f;
    public float speedMultiplier = 1.05f; // Slightly lower so it doesn't get impossible too fast

    private bool movingRight = true;
    private bool isGameOver = false;
    private int score = 0;
    
    private float leftBound;
    private float rightBound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameOverUI.SetActive(false);
        feedbackText.text = ""; // Hide feedback initially
        UpdateScoreText();
        
        // Dynamic bounds calculation
        float containerWidth = container.rect.width;
        float barWidth = movingBar.rect.width;
        rightBound = (containerWidth / 2f) - (barWidth / 2f);
        leftBound = -rightBound;
    }

    void Update()
    {
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R)) RestartGame();
            return;
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
        float movement = speed * Time.deltaTime;

        if (movingRight)
        {
            currentPos.x += movement;
            if (currentPos.x >= rightBound) { currentPos.x = rightBound; movingRight = false; }
        }
        else
        {
            currentPos.x -= movement;
            if (currentPos.x <= leftBound) { currentPos.x = leftBound; movingRight = true; }
        }

        movingBar.anchoredPosition = currentPos;
    }

    void CheckTiming()
    {
        float barX = movingBar.anchoredPosition.x;
        float targetX = targetZone.anchoredPosition.x;
        float distance = Mathf.Abs(barX - targetX);
        float zoneWidth = targetZone.rect.width / 2f;

        // Check if we hit at all
        if (distance <= zoneWidth)
        {
            // PERFECT HIT: Within 20% of the center
            if (distance <= zoneWidth * 0.2f)
            {
                ApplyHit(5, "PERFECT!", perfectSound, Color.yellow);
            }
            else // NORMAL HIT
            {
                ApplyHit(1, "NICE!", hitSound, Color.white);
            }
            
            speed *= speedMultiplier;
        }
        else
        {
            TriggerGameOver();
        }
    }

    void ApplyHit(int points, string message, AudioClip clip, Color textColor)
    {
        score += points;
        UpdateScoreText();
        audioSource.PlayOneShot(clip);
        
        StopAllCoroutines(); // Stop previous feedback animations
        StartCoroutine(ShowFeedback(message, textColor));
    }

    IEnumerator ShowFeedback(string text, Color color)
    {
        feedbackText.text = text;
        feedbackText.color = color;
        feedbackText.transform.localScale = Vector3.one;

        // Simple "Punch" Animation
        float timer = 0;
        while (timer < 0.2f)
        {
            feedbackText.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.5f, timer / 0.2f);
            timer += Time.deltaTime;
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        feedbackText.text = "";
    }

    void TriggerGameOver()
    {
        isGameOver = true;
        audioSource.PlayOneShot(missSound);
        gameOverUI.SetActive(true);
    }

    void UpdateScoreText() => scoreText.text = $"SCORE: {score}";

    void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

// using UnityEngine;
// using TMPro; // Required for TextMeshPro UI
// using UnityEngine.SceneManagement; // Required to restart the game

// public class WaitNowGame : MonoBehaviour
// {
//     [Header("UI References")]
//     public RectTransform movingBar;
//     public RectTransform targetZone;
//     public TMP_Text scoreText;       // Connects to your Score Text
//     public GameObject gameOverUI;    // Connects to your Game Over Text

//     [Header("Game Settings")]
//     public float speed = 600f;
//     private float leftBound = -450f;
//     private float rightBound = 450f;

//     // State variables
//     private bool movingRight = true;
//     private bool isGameOver = false;
//     private int score = 0;           // Tracks the player's score

//     void Start()
//     {
//         // Make sure the Game Over text is hidden when the game starts
//         gameOverUI.SetActive(false);
//         scoreText.text = "SCORE: 0";
//     }

//     void Update()
//     {
//         if (isGameOver)
//         {
//             // If the game is over, listen for the 'R' key to restart
//             if (Input.GetKeyDown(KeyCode.R))
//             {
//                 // This reloads the current scene from scratch
//                 SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//             }
//             return; // Stops the rest of the code so the bar stops moving
//         }

//         MoveTheBar();

//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             CheckTiming();
//         }
//     }

//     void MoveTheBar()
//     {
//         Vector2 currentPos = movingBar.anchoredPosition;

//         if (movingRight)
//         {
//             currentPos.x += speed * Time.deltaTime;
//             if (currentPos.x >= rightBound) movingRight = false;
//         }
//         else
//         {
//             currentPos.x -= speed * Time.deltaTime;
//             if (currentPos.x <= leftBound) movingRight = true;
//         }

//         movingBar.anchoredPosition = currentPos;
//     }

//     void CheckTiming()
//     {
//         float barX = movingBar.anchoredPosition.x;
//         float targetX = targetZone.anchoredPosition.x;
//         float safeZone = targetZone.rect.width / 2f;

//         if (Mathf.Abs(barX - targetX) <= safeZone)
//         {
//             // PERFECT HIT!
//             score++; 
//             scoreText.text = "SCORE: " + score; // Update the UI text
//             speed += 100f; // Make it faster!
//         }
//         else
//         {
//             // MISSED!
//             isGameOver = true;
//             gameOverUI.SetActive(true); // Turn on the Game Over text
//         }
//     }
// }