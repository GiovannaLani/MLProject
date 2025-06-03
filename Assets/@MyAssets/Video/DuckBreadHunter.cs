using UnityEngine;
using System.Collections;
using System.Linq;

public class DuckBreadHunter : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float eatDistance = 1.5f;
    public float peckDuration = 1.5f;
    public string breadTag = "PanFlotante";

    private Transform currentTarget;
    private bool isEating = false;

    void Update()
    {
        if (isEating) return;

        // Buscar pan si no hay objetivo actual
        if (currentTarget == null)
        {
            FindNearestBread();
            return;
        }

        // Calcular dirección hacia el pan (solo en XZ)
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        direction.y = 0f; // No subir ni bajar

        // Moverse hacia el pan
        Vector3 move = direction * moveSpeed * Time.deltaTime;
        transform.position += move;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation * Quaternion.Euler(0, 90f, 0); // Ajusta -90 si hace falta
        }

        // Comprobar si está cerca para comer
        if (Vector3.Distance(transform.position, currentTarget.position) < eatDistance)
        {
            StartCoroutine(EatBread(currentTarget.gameObject));
        }
    }

    void FindNearestBread()
    {
        GameObject[] breads = GameObject.FindGameObjectsWithTag(breadTag);
        if (breads.Length == 0) return;

        GameObject nearest = breads
            .OrderBy(b => Vector3.Distance(transform.position, b.transform.position))
            .FirstOrDefault();

        if (nearest != null)
        {
            currentTarget = nearest.transform;
        }
    }

    IEnumerator EatBread(GameObject bread)
    {
        isEating = true;

        float timer = 0f;
        Vector3 originalPos = transform.position;

        // Animación de picoteo
        while (timer < peckDuration)
        {
            float offset = Mathf.Sin(timer * 20f) * 0.05f;
            transform.position = originalPos + transform.up * offset;

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(bread);
        currentTarget = null;
        isEating = false;
    }
}
