using UnityEngine;

public class MovimientoAgua : MonoBehaviour
{
    public float velocidadX = 0.1f;
    public float velocidadY = 0.05f;

    private Renderer rend;
    private Vector2 offset = Vector2.zero;

    void Start()
    {
        rend = GetComponent<Renderer>();

        rend.material.mainTexture.wrapMode = TextureWrapMode.Repeat;
    }

    void Update()
    {
        offset.x += velocidadX * Time.deltaTime;
        offset.y += velocidadY * Time.deltaTime;

        offset.x %= 1f;
        offset.y %= 1f;

        rend.material.mainTextureOffset = offset;
    }
}
