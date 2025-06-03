using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadCollision : MonoBehaviour
{
    public GameObject particleEffectPrefab; // Asigna en el Inspector

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Agua")) // Aseg�rate de usar el mismo tag exacto
        {
            // Instancia las part�culas en la posici�n del impacto
            if (particleEffectPrefab != null)
            {
                Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject); // Destruye el pan
        }
    }
}