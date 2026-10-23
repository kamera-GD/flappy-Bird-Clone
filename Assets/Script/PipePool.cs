using System.Collections.Generic;
using UnityEngine;

public class PipePool : MonoBehaviour
{
    public static PipePool Instance { get; private set; }
    [Header("Pool Configuration")]
    [SerializeField] private GameObject pipePrefab;
    [SerializeField] private int poolSize = 5;

    private List<GameObject> pool = new List<GameObject>();
    private void Awake()
    {
        // Thiết lập Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        // Khởi tạo toàn bộ ống ngay khi bắt đầu game
        InitializePool();
    }
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            // 1. Tạo mới một cặp ống từ Prefab
            // (Đặt transform làm cha để gom gọn các ống vào GameObject PipePool trong Hierarchy)
            GameObject pipe = Instantiate(pipePrefab, transform);
            // 2. Tắt nó đi ngay lập tức (đưa vào trạng thái chờ)
            pipe.SetActive(false);
            // 3. Thêm vào danh sách quản lý
            pool.Add(pipe);
        }
    }
    // Hàm này được gọi từ PipeSpawner để mượn một chiếc ống đang rảnh rỗi.
    public GameObject GetPipe()
    {
        // Duyệt qua toàn bộ danh sách để tìm ống đang rảnh rỗi (đang bị tắt)
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                // Tìm thấy ống rảnh rỗi! Trả về cho bên gọi sử dụng
                return pool[i];
            }
        }
        // Trường hợp dự phòng (nếu tốc độ spawn quá nhanh vượt quá poolSize):
        // Ta tạo thêm 1 cái mới, thêm vào pool và trả về luôn (Dynamic Expand)
        GameObject newPipe = Instantiate(pipePrefab, transform);
        newPipe.SetActive(false);
        pool.Add(newPipe);
        return newPipe;
    }
}
