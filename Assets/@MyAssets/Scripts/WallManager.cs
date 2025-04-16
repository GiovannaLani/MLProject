using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    public List<GameObject> walls;
    public float speed = 5f;
    public float endX = 21.5f;
    public Vector3 spawnPosition = new Vector3(-21.5f, 17.62f, -7.15f);
    public Quaternion spawnRotation = new Quaternion(-0.5f, -0.5f, -0.5f, 0.5f);

    private GameObject currentWall;

    void Start()
    {
        StartCoroutine(SpawnWallLoop());
    }

    IEnumerator SpawnWallLoop()
    {
        while (true)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                GameObject prefab = walls[i];
                
                currentWall = Instantiate(prefab, spawnPosition, spawnRotation);

                while (currentWall != null && currentWall.transform.position.x < endX)
                {
                    currentWall.transform.Translate(Vector3.up * Time.deltaTime * speed);
                    yield return null;
                }
                if (currentWall != null)
                    Destroy(currentWall);

                yield return null;
            }

        }
    }
}
