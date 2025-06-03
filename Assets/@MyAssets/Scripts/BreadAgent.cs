using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class BreadAgent : Agent
{
    public GameObject ground;
    public GameObject area;
    public GameObject bread;

    public bool useVectorObs;
    Rigidbody m_AgentRb;
    Material m_GroundMaterial;
    Renderer m_Bread;
    BreadSettings m_BreadSettings;
    int m_Selection;
    StatsRecorder m_statsRecorder;

    public int wallsPassed = 0;
    public int maxWalls = 10;

    public int difficultyLevel { get; private set; } = 2;
    public int successfulLevel = 0;
    public WallManager wallManager;

    public override void Initialize()
    {
        m_BreadSettings = FindObjectOfType<BreadSettings>();
        m_AgentRb = GetComponent<Rigidbody>();
        m_Bread = bread.GetComponent<Renderer>();
        m_GroundMaterial = m_Bread.material;
        m_statsRecorder = Academy.Instance.StatsRecorder;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (useVectorObs)
        {
            sensor.AddObservation(StepCount / (float)MaxStep);

            Vector3 scale = transform.localScale;
            sensor.AddObservation(scale.x);
            sensor.AddObservation(scale.y);
            Vector2 hole = wallManager.lastHoleSize;
            sensor.AddObservation(hole.x);
            sensor.AddObservation(hole.y);
            Debug.Log("scale: "+scale.x + " "+scale.y+" hole "+hole.x+ " "+hole.y);
        }
        
    }

    IEnumerator GoalScoredSwapGroundMaterial(Material mat, float time)
    {
        m_Bread.material = mat;
        yield return new WaitForSeconds(time);
        m_Bread.material = m_GroundMaterial;
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];
        var action1 = act[1];
        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
        }
        switch (action1)
        {
            case 1:
                grow();
                break;
            case 2:
                shrink();
                break;
        }

        transform.Rotate(rotateDir, Time.deltaTime * 150f);
        Vector3 move = dirToGo * m_BreadSettings.agentRunSpeed * Time.fixedDeltaTime;
        m_AgentRb.MovePosition(m_AgentRb.position + move);
    }

    private float baseVolume = 1f;
    void grow()
    {
        Vector3 scale = transform.localScale;
        float newY = scale.y + 0.1f;
        if (newY >= 2.4) return;
        ApplyUniformVolume(newY);
    }

    void shrink()
    {
        Vector3 scale = transform.localScale;
        float newY = Mathf.Max(0.1f, scale.y - 0.1f);
        if (newY <= 0.4) return;
        ApplyUniformVolume(newY);
    }

    void ApplyUniformVolume(float newY)
    {
        float otherAxis = Mathf.Sqrt(baseVolume / newY);
        transform.localScale = new Vector3(otherAxis, newY, otherAxis);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        AddReward(-1f / MaxStep);
        MoveAgent(actionBuffers.DiscreteActions);
    }

    public void ChangeColor(bool isWrong)
    {
        if(isWrong) StartCoroutine(GoalScoredSwapGroundMaterial(m_BreadSettings.failMaterial, 0.5f));
        else StartCoroutine(GoalScoredSwapGroundMaterial(m_BreadSettings.goalScoredMaterial, 0.5f));
    }

    public void SetstatsRecorder(string name, float points)
    {
        m_statsRecorder.Add(name, points, StatAggregationMethod.Sum);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("water"))
        {
            SetReward(-1f);
            StartCoroutine(GoalScoredSwapGroundMaterial(m_BreadSettings.failMaterial, 0.5f));
            m_statsRecorder.Add("Water", 1, StatAggregationMethod.Sum);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            discreteActionsOut[1] = 1;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            discreteActionsOut[1] = 2;
        }
    }

    public override void OnEpisodeBegin()
    {
        var agentOffset = 14.2f;
        m_Selection = Random.Range(0, 2);
        

        transform.position = new Vector3(9f + Random.Range(-5f, 5f), agentOffset - ground.transform.position.y, 9f+Random.Range(-5f, 5f)) + ground.transform.position;
        transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        transform.localScale = Vector3.one;
        
        m_AgentRb.velocity *= 0f;
        wallsPassed = 0;

        wallManager.StopAndClearWalls();
        wallManager.StartGeneratingWalls();

        var goalPos = Random.Range(0, 2);
        
        m_statsRecorder.Add("Goal/Correct", 0, StatAggregationMethod.Sum);
        m_statsRecorder.Add("Goal/Wrong", 0, StatAggregationMethod.Sum);
    }
    public void OnLevelPassedSuccessfully()
    {
        successfulLevel++;
        if (successfulLevel >= 3)
        {
            successfulLevel = 0;
            if(difficultyLevel<2) difficultyLevel ++;
        }
    }

    public void OnLevelFailed()
    {
        successfulLevel = 0;
    }
}
