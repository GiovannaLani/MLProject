using UnityEngine;

public class FloatingBread : MonoBehaviour
{
    public float bobbingSpeed = 1f;
    public float bobbingHeight = 0.2f;
    public float waterSurfaceY = 0.5f; // Ajusta esto al nivel del agua

    private Vector3 floatCenter;
    private bool isFloating = false;
    private Rigidbody rb;

    public GameObject particleEffectPrefab; // Asigna en el Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isFloating)
        {
            float newY = waterSurfaceY + Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
            transform.position = new Vector3(floatCenter.x, newY, floatCenter.z);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Agua") && !isFloating)
        {
            // Guardar posición X/Z al caer al agua
            floatCenter = new Vector3(transform.position.x, 0f, transform.position.z);
            if (particleEffectPrefab != null)
            {
                Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
            }
            gameObject.tag = "PanFlotante";
            // Dejarlo quieto y activamos el modo flotante
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;

            isFloating = true;
        }
    }
}
