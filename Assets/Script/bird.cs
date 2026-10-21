using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bird : MonoBehaviour
{

    [SerializeField] private float jumpForce = 5f;
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

    // Update is called once per frame
    private void Update()
    {
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
        // Kiểm tra xem vật đâm vào mình có script Bird hay không
        if (collision.gameObject.CompareTag("pipe"))
        {
            Debug.Log("Collision with a pipe detected!");
            isDead = true;
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

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpRequested = false;
    }

    private void addScore()
    {
        score++;
        Debug.Log("Score: " + score);
    }    

}
