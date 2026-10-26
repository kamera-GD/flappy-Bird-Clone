using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float leftBound = -10f;

    private bool canMove = true;

    private void OnEnable()
    {
        canMove = true;
        // Đăng ký nghe sự kiện chim chết
        Bird.OnBirdDied += HandleBirdDied;
    }

    private void OnDisable() { Bird.OnBirdDied -= HandleBirdDied; } // Hủy đăng ký khi ống bị tắt (trả về pool)


    private void HandleBirdDied() { canMove = false; } // Chim chết thì đóng băng di chuyển ngay tại chỗ

    private void Update()
    {
        if (!canMove) return;

        // 1. Di chuyển sang trái đều đặn độc lập với tốc độ khung hình (FPS)
        transform.position += Vector3.left * speed * Time.deltaTime;

        // 2. Kiểm tra nếu đã vượt quá mép trái màn hình thì tắt đối tượng để trả về Pool
        if (transform.position.x < leftBound)
        {
            gameObject.SetActive(false);
        }
    }
}
