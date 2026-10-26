using UnityEngine;
using UnityEngine.UI;              // Dùng thư viện này cho Text Legacy và Button
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [Header("UI Panel")]
    [SerializeField] private GameObject menuPanel; // Kéo bảng GameOverMenu vào đây

    [Header("Score Texts (Legacy)")]
    // Đổi kiểu dữ liệu từ TextMeshProUGUI sang Text chuẩn của Unity UI
    [SerializeField] private Text currentScoreText;  // Text Legacy hiện điểm vừa chơi
    [SerializeField] private Text previousScoreText; // Text Legacy hiện điểm ván trước
    [SerializeField] private Text bestScoreText;     // Text Legacy hiện điểm cao nhất

    [Header("Buttons")]
    [SerializeField] private Button playAgainButton; // Nút Play Again

    private const string BEST_SCORE_KEY = "Flappy_BestScore";
    private const string PREVIOUS_SCORE_KEY = "Flappy_PreviousScore";

    private void Awake()
    {
        // Gắn sự kiện bấm nút Play Again
        if (playAgainButton != null)
        {
            playAgainButton.onClick.AddListener(OnPlayAgainClicked);
        }
    }

    /// <summary>
    /// Hàm này được gọi khi chú chim chết
    /// </summary>
    public void ShowMenu(int currentScore)
    {
        // 1. Bật bảng menu hiển thị lên
        menuPanel.SetActive(true);

        // 2. Đọc điểm ván trước ra trước
        int previousScore = PlayerPrefs.GetInt(PREVIOUS_SCORE_KEY, 0);

        // 3. Đọc và tính điểm kỷ lục (Best Score)
        int bestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt(BEST_SCORE_KEY, bestScore);
        }

        // 4. Lưu điểm ván này làm "Previous" cho lần chơi sau
        PlayerPrefs.SetInt(PREVIOUS_SCORE_KEY, currentScore);
        PlayerPrefs.Save();

        // 5. Cập nhật lên các component Text Legacy
        if (currentScoreText != null)
            currentScoreText.text = "Score: " + currentScore.ToString();

        if (previousScoreText != null)
            previousScoreText.text = "Previous: " + previousScore.ToString();

        if (bestScoreText != null)
            bestScoreText.text = "Best: " + bestScore.ToString();
    }

    public void OnPlayAgainClicked()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}