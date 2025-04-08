using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptImportantisimo : MonoBehaviour
{
    
    public float y = 0;
    public float speed = 10;
    void Update()
    {
        y += Time.deltaTime * speed;
        transform.rotation = Quaternion.Euler(0, y, 0);
    }
}
