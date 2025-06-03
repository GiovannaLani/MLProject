using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadCollision : MonoBehaviour
{
    public GameObject particleEffectPrefab; // Asigna en el Inspector

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Agua")) // Asegúrate de usar el mismo tag exacto
        {
            // Instancia las partículas en la posición del impacto
            if (particleEffectPrefab != null)
            {
                Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject); // Destruye el pan
        }
    }
}