using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour
{
    public float speed = 1;
    public ParticleSystem particle1;
    public ParticleSystem particle2;
    public ParticleSystem particle3;
    public ParticleSystem particle4;
    public ParticleSystem particle5;
    public ParticleSystem particle6;
    private float speedz = 0;
    public float rotationSpeed = 1;
    private Quaternion toRotation = Quaternion.Euler(250, -90, 90);
    private bool rotate = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(particles());
            speedz = 1;
            speed = 2;
            rotate = true;
        }
        if(rotate)
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        transform.position =  new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y - speedz * Time.deltaTime, transform.position.z);
    }

    private IEnumerator particles()
    {
        particle1.Play();
        yield return new WaitForSeconds(0.2f);
        particle2.Play();
        yield return new WaitForSeconds(0.1f);
        particle3.Play();
        yield return new WaitForSeconds(0.4f);
        particle4.Play();
        yield return new WaitForSeconds(0.1f);
        particle5.Play();
        yield return new WaitForSeconds(0.2f);
        particle6.Play();
        yield return new WaitForSeconds(0.02f);
    }
}
