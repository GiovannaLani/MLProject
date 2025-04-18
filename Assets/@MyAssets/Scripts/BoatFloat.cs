using UnityEngine;

public class BoatFloat : MonoBehaviour
{
    public float rotationSpeed = 0.5f;
    public float rotationAmount = 2.0f;
    public float bobSpeed = 0.5f;
    public float bobAmount = 0.2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Movimiento de balanceo (rotación)
        float rotX = Mathf.Sin(Time.time * rotationSpeed) * rotationAmount;
        float rotZ = Mathf.Cos(Time.time * rotationSpeed * 0.7f) * rotationAmount;

        // Movimiento de bamboleo vertical (sube y baja)
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobAmount;

        // Aplicar movimiento
        transform.position = new Vector3(startPos.x, newY, startPos.z);
        transform.rotation = Quaternion.Euler(rotX, transform.rotation.eulerAngles.y, rotZ);
    }
}
