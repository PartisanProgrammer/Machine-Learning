using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

public class MoveToGoalAgent : Agent{
    [SerializeField] Transform targetTransform;
    [SerializeField] Rigidbody agentRigidbody;
    [SerializeField] List<Transform> Obstacles;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] bool hasMovementPenalty;
    [SerializeField] bool willRandomizeObstacle;

    float movementPenalty = 0f;

    public override void OnEpisodeBegin(){
       
        transform.localPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        targetTransform.localPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        agentRigidbody.velocity = Vector3.zero;
        if (willRandomizeObstacle){
            foreach (var obstacle in Obstacles){
                obstacle.localRotation = Quaternion.Euler(0,Random.Range(0,360),0);
                obstacle.localPosition = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
            }
        }
      
    }
    

    public override void CollectObservations(VectorSensor sensor){
        var localPosition = transform.localPosition;
        var velocity = agentRigidbody.velocity;
        sensor.AddObservation(localPosition.x);
        sensor.AddObservation(localPosition.z);
        sensor.AddObservation(velocity.x);
        sensor.AddObservation(velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions){
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        agentRigidbody.velocity += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
        
    }

    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    void OnCollisionEnter(Collision collision){
        if (hasMovementPenalty){
            movementPenalty = Mathf.Max(-1f, -StepCount * 0.001f);
        }
        if(collision.collider.CompareTag("Obstacle")){
            AddReward(-1f);
            EndEpisode();
        }
        if (collision.collider.CompareTag("Wall")){
            AddReward(-1f);
            EndEpisode();
        }
        if(collision.collider.CompareTag("Goal")){
            AddReward(1f + movementPenalty);
            EndEpisode(); 
        }
    }
}