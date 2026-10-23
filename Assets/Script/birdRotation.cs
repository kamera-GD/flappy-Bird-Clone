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

    private void Start()
    {
        currentAngle = 0f;
    }

    private void Update()
    {
        // 1. NGẨNG LÊN NGAY LẬP TỨC:
        // Ngay khoảnh khắc vừa bấm phím -> BẺ THẲNG GÓC LÊN 30 ĐỘ NGAY TRONG FRAME NÀY
        if (WasJumpPressed())
        {
            currentAngle = maxUpAngle;
            transform.eulerAngles = new Vector3(0f, 0f, maxUpAngle); // Ép góc trực tiếp không có độ trễ
            return; // Kết thúc frame này luôn, không cho chạy phần rơi xuống phía dưới
        }

        // 2. NẾU KHÔNG BẤM GÌ: Chúi đầu từ từ xuống đất (-90 độ)
        if (currentAngle > maxDownAngle)
        {
            currentAngle -= fallRotateSpeed * Time.deltaTime;
            currentAngle = Mathf.Max(currentAngle, maxDownAngle); // Chặn không cho vượt quá -90 độ

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