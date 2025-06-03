using UnityEngine;

public class BreadRainSpawner : MonoBehaviour
{
    public GameObject[] breadPrefabs; // 3 tipos de pan
    public GameObject[] breadPrefabsFlotar; // 3 tipos de pan
    public float spawnRate = 0.5f;    // Tiempo entre panes (en segundos)
    public float spawnRateFlotar = 2f;    // Tiempo entre panes (en segundos)
    public Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f); // Área en X-Z
    public float spawnHeight = 15f;   // Altura desde la que caen

    void Start()
    {
        InvokeRepeating(nameof(SpawnBread), 0f, spawnRate);
        InvokeRepeating(nameof(SpawnBreadFlotar), 0f, spawnRateFlotar);
    }

    void SpawnBread()
    {
        if (breadPrefabs.Length == 0)
            return;

        // Posición aleatoria dentro del área X-Z
        Vector3 randomPos = new Vector3(
            transform.position.x + Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            transform.position.y +spawnHeight,
            transform.position.z + Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
        );

        GameObject prefab = breadPrefabs[Random.Range(0, breadPrefabs.Length)];
        GameObject bread = Instantiate(prefab, randomPos, Random.rotation);

        Rigidbody rb = bread.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 randomTorque = new Vector3(
                Random.Range(-50f, 50f),
                Random.Range(-50f, 50f),
                Random.Range(-50f, 50f)
            );
            rb.AddTorque(randomTorque);
        }
    }

    void SpawnBreadFlotar()
    {
        if (breadPrefabs.Length == 0)
            return;

        // Posición aleatoria dentro del área X-Z
        Vector3 randomPos = new Vector3(
            transform.position.x + Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            transform.position.y + spawnHeight,
            transform.position.z + Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
        );

        GameObject prefab = breadPrefabsFlotar[Random.Range(0, breadPrefabs.Length)];
        GameObject bread = Instantiate(prefab, randomPos, Random.rotation);

        Rigidbody rb = bread.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 randomTorque = new Vector3(
                Random.Range(-50f, 50f),
                Random.Range(-50f, 50f),
                Random.Range(-50f, 50f)
            );
            rb.AddTorque(randomTorque);
        }
    }


}
