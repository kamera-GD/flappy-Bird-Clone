using UnityEngine;
using UnityEngine.InputSystem;

public class BirdRotation : MonoBehaviour
{
    [Header("Angle Settings")]
    [SerializeField] private float maxUpAngle = 30f;       // Góc ngẩng tối đa khi vừa bấm Space
    [SerializeField] private float maxDownAngle = -90f;    // Góc cắm đầu tối đa khi rơi

    [Header("Fall Speed")]
    [SerializeField] private float fallRotateSpeed = 300f; // Tốc độ chúi đầu xuống đất (độ/giây)

    private float currentAngle;
    private bool canRotate = false; // Mặc định ở Menu: KHÔNG cho xoay, giữ thẳng

    private void Awake()
    {
        currentAngle = 0f;
        transform.eulerAngles = Vector3.zero;
    }

    private void OnEnable()
    {
        // Lắng nghe sự kiện bắt đầu game và sự kiện chim chết
        GameManager.OnGameStarted += HandleGameStarted;
        Bird.OnBirdDied += HandleBirdDied;
    }

    private void OnDisable()
    {
        // Huỷ đăng ký sự kiện khi đối tượng bị vô hiệu hoá
        GameManager.OnGameStarted -= HandleGameStarted;
        Bird.OnBirdDied -= HandleBirdDied;
    }

    private void HandleGameStarted()
    {
        canRotate = true; // Bấm Play rồi mới cho phép xoay
        currentAngle = 0f;
        transform.eulerAngles = Vector3.zero;
    }

    private void HandleBirdDied() { canRotate = false; } // Chim chết: Dừng nhận input Space và ngừng xoay

    private void Update()
    {
        // Nếu đang ở Menu hoặc Chim đã chết: Không nhận phím và không tự chúi đầu
        if (!canRotate) return;

        // 1. NGẨNG LÊN NGAY LẬP TỨC KHI NHẢY:
        if (WasJumpPressed())
        {
            currentAngle = maxUpAngle;
            transform.eulerAngles = new Vector3(0f, 0f, maxUpAngle);
            return;
        }

        // 2. NẾU KHÔNG BẤM GÌ: Chúi đầu từ từ xuống đất (-90 độ)
        if (currentAngle > maxDownAngle)
        {
            currentAngle -= fallRotateSpeed * Time.deltaTime;
            currentAngle = Mathf.Max(currentAngle, maxDownAngle);

            transform.eulerAngles = new Vector3(0f, 0f, currentAngle);
        }
    }

    private bool WasJumpPressed()
    {
        bool space = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool click = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        return space || click;
    }
}
