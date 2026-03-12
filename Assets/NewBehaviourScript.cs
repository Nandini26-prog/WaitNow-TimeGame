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
    [SerializeField] private TMP_Text feedbackText; 
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject startMenuUI;

    [Header("Audio")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip perfectSound;
    [SerializeField] private AudioClip missSound;
    private AudioSource audioSource;

    [Header("Game Settings")]
    public float speed = 600f;
    public float speedMultiplier = 1.05f; 
    public float targetMoveSpeed = 250f; // Level 4 me target ke ghumne ki speed
    public UIShake screenShaker;

    private bool movingRight = true;
    private bool isGameOver = false;
    private bool hasStarted = false;
    private bool isPlayingRound = false; 
    private int score = 0;
    
    // LEVEL VARIABLES
    private int level = 1;
    private int levelProgress = 0; // Nice = +1, Perfect = +2 (Need 3 to Level Up)
    private float initialTargetWidth;
    private bool targetIsMoving = false; // Level 4 ke liye
    private bool targetMovingRight = true;

    private float leftBound;
    private float rightBound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startMenuUI.SetActive(true);
        gameOverUI.SetActive(false);
        feedbackText.text = ""; 
        UpdateScoreText();
        
        // Bounds Calculation
        float containerWidth = container.rect.width;
        float barWidth = movingBar.rect.width;
        rightBound = (containerWidth / 2f) - (barWidth / 2f);
        leftBound = -rightBound;

        // Save original width & Center it for Level 1
        initialTargetWidth = targetZone.rect.width;
        targetZone.anchoredPosition = new Vector2(0, targetZone.anchoredPosition.y);
    }

    void Update()
    {
        if (!hasStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                hasStarted = true;
                startMenuUI.SetActive(false); 
                StartCoroutine(LevelUpSequence("LET'S GO!")); // Game start animation
            }
            return; 
        }

        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R)) RestartGame();
            return;
        }

        if (isPlayingRound)
        {
            MoveTheBar();
            
            // Level 4 me target bhi move karega!
            if (targetIsMoving) MoveTheTargetZone();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckTiming();
            }
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

    void MoveTheTargetZone()
    {
        Vector2 tPos = targetZone.anchoredPosition;
        float tMove = targetMoveSpeed * Time.deltaTime;
        float tRightBound = rightBound - (targetZone.rect.width / 2f);
        float tLeftBound = leftBound + (targetZone.rect.width / 2f);

        if (targetMovingRight)
        {
            tPos.x += tMove;
            if (tPos.x >= tRightBound) { tPos.x = tRightBound; targetMovingRight = false; }
        }
        else
        {
            tPos.x -= tMove;
            if (tPos.x <= tLeftBound) { tPos.x = tLeftBound; targetMovingRight = true; }
        }
        targetZone.anchoredPosition = tPos;
    }

    void CheckTiming()
    {
        float barX = movingBar.anchoredPosition.x;
        float targetX = targetZone.anchoredPosition.x;
        float distance = Mathf.Abs(barX - targetX);
        float zoneWidth = targetZone.rect.width / 2f;

        if (distance <= zoneWidth)
        {
            // PERFECT HIT
            if (distance <= zoneWidth * 0.2f)
            {
                ApplyHit(5, "PERFECT!", perfectSound, Color.yellow);
                levelProgress += 2; // Perfect gives +2 progress
                screenShaker.TriggerShake(0.15f, 10f);
            }
            else // NORMAL HIT
            {
                ApplyHit(1, "NICE!", hitSound, Color.white);
                levelProgress += 1; // Nice gives +1 progress
            }
            
            speed *= speedMultiplier;

            // LEVEL UP CHECK (Requires 3 Progress Points)
            if (levelProgress >= 3)
            {
                level++;
                levelProgress = 0; // Reset progress for next level
                StopAllCoroutines(); // Stop the Nice/Perfect animation
                StartCoroutine(LevelUpSequence($"LEVEL {level}\nREADY?"));
            }
            else if (level >= 2)
            {
                // Agar level up nahi hua but hum Level 2+ par hain, toh target randomly jump karega on every hit!
                JumpTargetRandomly();
            }
        }
        else
        {
            TriggerGameOver();
        }
    }

    void JumpTargetRandomly()
    {
        float tWidth = targetZone.rect.width / 2f;
        float randomX = Random.Range(leftBound + tWidth, rightBound - tWidth);
        targetZone.anchoredPosition = new Vector2(randomX, targetZone.anchoredPosition.y);
    }

    void ApplyHit(int points, string message, AudioClip clip, Color textColor)
    {
        score += points;
        UpdateScoreText();
        audioSource.PlayOneShot(clip);
        
        StopAllCoroutines(); 
        StartCoroutine(ShowFeedback(message, textColor));
    }

    IEnumerator LevelUpSequence(string text)
    {
        isPlayingRound = false; // Pause the moving bar

        // Level Rules Apply Here:
        if (level == 2) {
            JumpTargetRandomly();
        } 
        else if (level == 3) {
            targetZone.sizeDelta = new Vector2(initialTargetWidth * 0.6f, targetZone.sizeDelta.y); // Width kam kardi!
            JumpTargetRandomly();
        } 
        else if (level >= 4) {
            targetIsMoving = true; // Ab target khud bhagega!
            JumpTargetRandomly();
        }

        // Animation
        feedbackText.color = Color.cyan;
        feedbackText.text = text;
        feedbackText.transform.localScale = Vector3.one * 1.5f;

        yield return new WaitForSeconds(1.5f);
        
        feedbackText.color = Color.green;
        feedbackText.text = "GO!";
        
        yield return new WaitForSeconds(0.4f);
        feedbackText.text = "";
        
        isPlayingRound = true; // Resume game!
    }

    IEnumerator ShowFeedback(string text, Color color)
    {
        feedbackText.text = text;
        feedbackText.color = color;
        feedbackText.transform.localScale = Vector3.one;

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
        isPlayingRound = false;
        audioSource.PlayOneShot(missSound);
        gameOverUI.SetActive(true);
        screenShaker.TriggerShake(0.3f, 20f);
    }

    void UpdateScoreText() => scoreText.text = $"SCORE: {score}";
    void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}