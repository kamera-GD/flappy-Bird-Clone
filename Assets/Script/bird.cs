using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bird : MonoBehaviour
{
    [SerializeField] private float jumpVelocity = 6f; // Vận tốc nảy lên tức thời chuẩn Flappy Bird
    [SerializeField] private float jumpCooldown = 0.15f;

    private Rigidbody2D rb;
    private bool jumpRequested = false;
    private float nextJumpTime = 0f;
    private bool isDead = false;
    private int score = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isDead) return;

        if (WasJumpPressed() && Time.time >= nextJumpTime)
        {
            jumpRequested = true;
        }    
    }

    private void FixedUpdate()
    {
        if (jumpRequested)
        {
            Jump();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("pipe"))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("score"))
        {
            addScore();
        }
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

    private void addScore()
    {
        score++;
        Debug.Log("Score: " + score);
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("Bird đã chết!");

        PipeSpawner spawner = FindFirstObjectByType<PipeSpawner>();
        if (spawner != null)
        {
            spawner.StopSpawning();
        }

        MoveLeft[] activePipes = FindObjectsByType<MoveLeft>(FindObjectsSortMode.None);
        foreach (MoveLeft pipe in activePipes)
        {
            pipe.enabled = false;
        }

        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.enabled = false;
        }
    }
}
