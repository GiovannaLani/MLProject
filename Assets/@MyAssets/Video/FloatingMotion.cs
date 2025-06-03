using UnityEngine;

public class FloatingMotion : MonoBehaviour
{
    public float amplitude = 0.001f;   // Altura del movimiento
    public float speed = 4f;         // Velocidad base del movimiento

    private Vector3 startPos;
    private float offset;
    public float speedVariation;

    void Start()
    {
        startPos = transform.position;
        offset = Random.Range(0f, 2f * Mathf.PI);              // Fase aleatoria
        amplitude = amplitude + Random.Range(0f, 0.001f);
        speedVariation = speed + Random.Range(0.8f, 1.2f)-1;     // Velocidad ligeramente distinta
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speedVariation + offset) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
