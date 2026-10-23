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
    private WaitForSeconds waitSpawn;

    private void Awake()
    {
        // Cache lại để tránh rác bộ nhớ (Garbage Collector) mỗi vòng lặp
        waitSpawn = new WaitForSeconds(spawnRate);
    }

    private void OnEnable()
    {
        // Lắng nghe sự kiện bắt đầu game và chim chết
        GameManager.OnGameStarted += StartSpawning;
        Bird.OnBirdDied += StopSpawning;
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= StartSpawning;
        Bird.OnBirdDied -= StopSpawning;
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
        while (true)
        {
            SpawnPipe();
            yield return waitSpawn;
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

            // 4. Bật đối tượng lên (script MoveLeft trên ống sẽ tự động trôi sang trái)
            pipe.SetActive(true);
        }
    }
}
