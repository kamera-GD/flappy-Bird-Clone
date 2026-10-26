using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform scoreContainer; // Kéo ScoreContainer vào đây
    [SerializeField] private GameObject digitPrefab;    // Kéo DigitPrefab vào đây

    [Header("Sprites 0 to 9")]
    // Mảng chứa 10 Sprite chữ số: phần tử [0] là ảnh số 0, phần tử [9] là ảnh số 9
    [SerializeField] private Sprite[] digitSprites;

    // Danh sách các Image đang hiển thị để tái sử dụng (Pooling nhẹ)
    private List<Image> spawnedDigits = new List<Image>();

    private void Start()
    {
        UpdateScore(0); // Mặc định hiển thị số 0 lúc đầu
    }

    /// <summary>
    /// Hàm này được gọi mỗi khi điểm số thay đổi
    /// </summary>
    public void UpdateScore(int score)
    {
        // 1. Chuyển số nguyên thành chuỗi. Ví dụ: 205 -> chuỗi "205"
        string scoreStr = score.ToString();

        // 2. Đảm bảo số lượng ô Image trong container bằng đúng số lượng chữ số của điểm
        while (spawnedDigits.Count < scoreStr.Length)
        {
            GameObject newDigit = Instantiate(digitPrefab, scoreContainer);
            spawnedDigits.Add(newDigit.GetComponent<Image>());
        }

        // 3. Duyệt qua từng ô Image để gán Sprite tương ứng hoặc ẩn đi
        for (int i = 0; i < spawnedDigits.Count; i++)
        {
            if (i < scoreStr.Length)
            {
                // Lấy ký tự số tại vị trí i và chuyển thành số nguyên (ví dụ: '5' -> số 5)
                int digit = scoreStr[i] - '0';

                // Gán Sprite tương ứng vào Image
                spawnedDigits[i].sprite = digitSprites[digit];
                spawnedDigits[i].gameObject.SetActive(true);
            }
            else
            {
                // Nếu dư Image (ví dụ khi reset game về 0 điểm) thì tắt bớt đi
                spawnedDigits[i].gameObject.SetActive(false);
            }
        }
    }
}