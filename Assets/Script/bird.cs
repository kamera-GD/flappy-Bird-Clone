using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bird : MonoBehaviour
{
    // Kênh sự kiện: Bắn thông báo cho toàn bộ các script (Rotation, Spawner, MoveLeft...) khi chim chết
    public static event Action OnBirdDied;

    [Header("Jump Settings")]
    [SerializeField] private float jumpVelocity = 6f; // Vận tốc nảy lên tức thời chuẩn Flappy Bird
    [SerializeField] private float jumpCooldown = 0.15f;

    [Header("UI")]
    [SerializeField] private ScoreDisplay scoreDisplay;

    [Header("UI Menus")]
    [SerializeField] private GameOverMenu gameOverMenu; // Kéo GameOverMenu vào đây

    private Rigidbody2D rb;
    private bool jumpRequested = false;
    private float nextJumpTime = 0f;
    private bool isPlaying = false; // Cờ kiểm tra game đã bấm Play hay chưa
    private bool isDead = false;
    private int score = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Khi mới vào game (ở Menu): Khóa trọng lực để chim không rơi, góc thẳng 0 độ
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
        }
        transform.rotation = Quaternion.identity;
    }

    private void Start()
    {
        if (scoreDisplay != null)
        {
            scoreDisplay.UpdateScore(0);
        }
    }

    private void Update()
    {
        // Chưa bấm Play hoặc chim đã chết thì KHÔNG nhận phím nhảy
        if (!isPlaying || isDead) return;

        if (WasJumpPressed() && Time.time >= nextJumpTime) jumpRequested = true;
    }

    private void FixedUpdate()
    {
        if (jumpRequested) Jump();
    }

    private void OnEnable() { GameManager.OnGameStarted += HandleGameStarted; } // Lắng nghe sự kiện bắt đầu game từ GameManager


    private void OnDisable() { GameManager.OnGameStarted -= HandleGameStarted; }

    private void HandleGameStarted()
    {
        isPlaying = true;
        isDead = false;

        // Bật lại trọng lực cho chim rơi khi đã vào trận đấu
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("pipe")) Die();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("score")) AddScore();
    }

    private bool WasJumpPressed()
    {
        bool spacePressed = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool mousePressed = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        return spacePressed || mousePressed;
    }

    private void Jump()
    {
        if (isDead) return;

        nextJumpTime = Time.time + jumpCooldown;
        rb.linearVelocity = new Vector2(0f, jumpVelocity);
        jumpRequested = false;
    }

    private void AddScore()
    {
        score++;
        Debug.Log("Score: " + score);

        if (scoreDisplay != null)
        {
            scoreDisplay.UpdateScore(score);
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        isPlaying = false;
        Debug.Log("Bird đã chết!");

        // Phát thông báo chim chết cho toàn bộ hệ thống
        OnBirdDied?.Invoke();

        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.enabled = false;
        }

        if (gameOverMenu != null)
        {
            gameOverMenu.ShowMenu(score);
        }
    }
}
