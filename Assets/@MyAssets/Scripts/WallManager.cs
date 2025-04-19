using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    public float speed = 5f;
    public float endX = 21.5f;
    public Vector3 spawnPosition = new Vector3(-21.5f, 17.62f, -7.15f);
    public Quaternion spawnRotation = new Quaternion(-0.5f, -0.5f, -0.5f, 0.5f);

    public BoxCollider agentCollider;
    public float wallWidth = 6f;
    public float wallHeight = 10f;

    private GameObject currentWall;
    public GameObject ground;

    private Coroutine wallLoop;
    private List<GameObject> allWalls = new List<GameObject>();

    public Vector2 lastHoleSize;
    public BreadAgent agent;

    public void StartGeneratingWalls()
    {
        wallLoop = StartCoroutine(SpawnWallLoop());
    }

    public void StopAndClearWalls()
    {
        if (wallLoop != null)
        {
            StopCoroutine(wallLoop);
            wallLoop = null;
        }

        foreach (var wall in allWalls)
        {
            if (wall != null)
                Destroy(wall);
        }
        allWalls.Clear();
    }

    IEnumerator SpawnWallLoop()
    {
        while (true)
        {
            currentWall = SpawnWall();
            allWalls.Add(currentWall);
            currentWall.transform.position = ground.transform.position + spawnPosition;
            currentWall.transform.rotation = spawnRotation;

            while (currentWall != null && currentWall.transform.position.x < ground.transform.position.x + endX)
            {
                currentWall.transform.Translate(Vector3.back * Time.deltaTime * speed);
                yield return null;
            }

            if (currentWall != null)
            {
                Destroy(currentWall);
                allWalls.Remove(currentWall);
            }

            yield return null;
        }
    }

    public GameObject SpawnWall()
    {
        Vector2 holeSize = GetRandomAgentShapeHoleSize(agentCollider);
        Debug.Log("Difficulty: " + agent.difficultyLevel);
        if (agent.difficultyLevel == 1) holeSize = new Vector2(3, 3);
        if (agent.difficultyLevel == 2) holeSize += new Vector2(0.5f, 0.5f);
        lastHoleSize = holeSize;
        float totalWidth = wallWidth;
        float holeWidth = holeSize.x;
        float minSideWidth = 1f;

        float remainingWidth = totalWidth - holeWidth;

        float leftWidth = Random.Range(minSideWidth, remainingWidth - minSideWidth);
        float rightWidth = remainingWidth - leftWidth;

        Debug.Log($"Left: {leftWidth:F2}, Hole: {holeWidth:F2}, Right: {rightWidth:F2}");

        Vector3 start = -new Vector3(totalWidth / 2f, 0f, 0f);
        float wallY = wallHeight / 2f;
        float wallZ = 0f;

        GameObject wall = new GameObject("Wall");
        Rigidbody rb = wall.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        GameObject left = GameObject.CreatePrimitive(PrimitiveType.Cube);
        left.transform.localScale = new Vector3(leftWidth, wallHeight, 0.1f);
        left.transform.position = start + new Vector3(leftWidth / 2f, wallY, wallZ);
        left.tag = "wall";
        left.transform.SetParent(wall.transform);
        left.AddComponent<Wall>();
        left.GetComponent<Collider>().isTrigger = true;

        GameObject center = GameObject.CreatePrimitive(PrimitiveType.Cube);
        center.transform.localScale = new Vector3(holeWidth, wallHeight - holeSize.y, 0.1f);
        center.transform.position = start + new Vector3(leftWidth + holeWidth / 2f, (holeSize.y + wallHeight) / 2f, wallZ);
        center.tag = "wall";
        center.transform.SetParent(wall.transform);
        center.AddComponent<Wall>();
        center.GetComponent<Collider>().isTrigger = true;

        GameObject right = GameObject.CreatePrimitive(PrimitiveType.Cube);
        right.transform.localScale = new Vector3(rightWidth, wallHeight, 0.1f);
        right.transform.position = start + new Vector3(leftWidth + holeWidth + rightWidth / 2f, wallY, wallZ);
        right.tag = "wall";
        right.transform.SetParent(wall.transform);
        right.AddComponent<Wall>();
        right.GetComponent<Collider>().isTrigger = true;

        GameObject winWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        winWall.transform.localScale = new Vector3(wallWidth, wallHeight, 0.1f);
        winWall.transform.position = start + new Vector3(wallWidth / 2f, wallY, wallZ + 1.5f);
        winWall.GetComponent<Collider>().isTrigger = true;
        winWall.GetComponent<Renderer>().enabled = false;
        winWall.tag = "wallWin";
        winWall.AddComponent<WinWall>();
        winWall.transform.SetParent(wall.transform);
        return wall;
    }
    public static Vector2 GetRandomAgentShapeHoleSize(BoxCollider baseCollider, float minY = 0.4f, float maxY = 2.4f, float baseVolume = 1f, float margin = 0.05f)
    {
        float scaleY = Random.Range(minY, maxY);

        float scaleXZ = Mathf.Sqrt(baseVolume / scaleY);

        float realWidth = baseCollider.size.x * scaleXZ + margin * 2f;
        float realHeight = baseCollider.size.y * scaleY + margin * 2f;
        int num = baseCollider.gameObject.GetComponent<BreadAgent>().wallsPassed;
        return new Vector2(realWidth+0.1f , realHeight+0.1f);
    }
}
public class Wall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BreadAgent>(out BreadAgent agent))
        {
            agent.SetReward(-1f);
            agent.EndEpisode();
            agent.ChangeColor(true);
            agent.SetstatsRecorder("wall", -1);
            agent.OnLevelFailed();
        }
    }
}
public class WinWall : MonoBehaviour
{
    bool giveReward = true;
    private void OnTriggerEnter(Collider other)
    {

        if (giveReward && other.TryGetComponent<BreadAgent>(out BreadAgent agent))
        {
            giveReward = false;
            agent.wallsPassed++;

            if (agent.wallsPassed >= agent.maxWalls)
            {
                agent.SetReward(1f);
                agent.EndEpisode();
                agent.ChangeColor(false);
                agent.SetstatsRecorder("winWall", 1);
                agent.OnLevelPassedSuccessfully();
            }
            else
            {
                agent.AddReward(1f / agent.maxWalls);
                agent.ChangeColor(false);
                agent.SetstatsRecorder("winWall", 1f / agent.maxWalls);
            }
        }
    }
}
