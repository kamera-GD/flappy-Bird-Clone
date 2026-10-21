using System.Collections;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float spawnRate = 1.5f;     // Cứ bao nhiêu giây thì sinh 1 cặp ống
    [SerializeField] private float spawnPosX = 10f;      // Tọa độ X ngoài mép phải màn hình để ống xuất hiện

    [Header("Height Limits")]
    [SerializeField] private float minY = -1.5f;         // Độ cao tối thiểu của khe hở
    [SerializeField] private float maxY = 2.5f;          // Độ cao tối đa của khe hở

    private Coroutine spawnCoroutine;

    private void Start()
    {
        // Bắt đầu chu kỳ sinh ống ngay khi vào game
        StartSpawning();
    }

    public void StartSpawning()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnRoutine());
        }
    }

    public void StopSpawning()
    {
        // Dùng khi chim chết: Dừng hoàn toàn việc sinh thêm ống mới
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        // Vòng lặp vô tận chạy suốt màn chơi
        while (true)
        {
            SpawnPipe();

            // Tạm dừng coroutine đúng bằng khoảng thời gian spawnRate rồi mới lặp lại
            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void SpawnPipe()
    {
        // 1. Mượn một chiếc ống đang rảnh rỗi từ PipePool
        GameObject pipe = PipePool.Instance.GetPipe();

        if (pipe != null)
        {
            // 2. Tính toán độ cao ngẫu nhiên cho khe hở
            float randomY = Random.Range(minY, maxY);

            // 3. Đặt vị trí xuất phát cho ống (ở ngoài mép phải màn hình)
            pipe.transform.position = new Vector3(spawnPosX, randomY, 0f);

            // 4. Bật đối tượng lên! (Lúc này script MoveLeft trên ống sẽ tự động làm nó trôi sang trái)
            pipe.SetActive(true);
        }
    }
}