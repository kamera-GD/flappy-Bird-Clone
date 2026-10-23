using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Kênh sự kiện: Thông báo cho toàn bộ các script khác khi người chơi bắt đầu game
    public static event Action OnGameStarted;

    [Header("UI Panels")]
    [SerializeField] private GameObject menuPanel;

    private void Start()
    {
        // Khi vừa mở game: Luôn hiển thị Menu
        if (menuPanel != null)
        {
            menuPanel.SetActive(true);
        }
    }

    // Hàm này được gọi khi người chơi bấm nút Play
    public void StartGame()
    {
        // 1. Tắt Menu
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }

        // 2. Phát thông báo: Bắt đầu game!
        OnGameStarted?.Invoke();
    }
}
