using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float leftBound = -10f;

    private void Update()
    {
        // 1. Di chuyển sang trái đều đặn độc lập với tốc độ khung hình (FPS)
        transform.position += Vector3.left * speed * Time.deltaTime;

        // 2. Kiểm tra nếu đã vượt quá mép trái màn hình thì tắt đối tượng để trả về Pool
        if (transform.position.x < leftBound)
        {
            gameObject.SetActive(false);
        }
    }
}
