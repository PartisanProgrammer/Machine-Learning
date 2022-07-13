using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ship : Agent{
    [SerializeField] List<GameObject> asteroids;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed = 10;
    [SerializeField] float rotateSpeed = 100;
    public override void OnEpisodeBegin(){
        transform.localPosition = Vector3.zero;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        foreach (var asteroid in asteroids){
            asteroid.transform.localPosition = new Vector3(Random.Range(-75, 75), Random.Range(-75, 75), Random.Range(-75, 75));
            asteroid.transform.localScale = new Vector3(Random.Range(10f, 20f), Random.Range(10f, 20f), Random.Range(10f, 20f));
        }
        
    }

    void FixedUpdate(){
        AddReward(0.001f);
    }

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localRotation);
        sensor.AddObservation(rb.velocity);
    }

    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
       
        discreteActions[0] = Input.GetKey(KeyCode.W) ? 1 : 0;
         discreteActions[1] = Input.GetKey(KeyCode.D) ? 1 : 0; // Right
         discreteActions[2] = Input.GetKey(KeyCode.A) ? 1 : 0; //Left
         discreteActions[3] = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)? 1 : 0; // Down
         discreteActions[4] = Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift) ? 1 : 0; //Up
    }
    

    public override void OnActionReceived(ActionBuffers actions){
        float forwardMovement = actions.DiscreteActions[0];
        var turnRight = actions.DiscreteActions[1];
        var turnLeft = actions.DiscreteActions[2];
        var tiltUp = actions.DiscreteActions[3];
        var tiltDown = actions.DiscreteActions[4];

        rb.velocity += transform.up* forwardMovement * Time.deltaTime * speed;
        
        if (turnLeft == 1){
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        }
        if (turnRight == 1){
            transform.Rotate(Vector3.forward, -rotateSpeed * Time.deltaTime);
        }
        if (tiltDown == 1){
            transform.Rotate(Vector3.left, rotateSpeed * Time.deltaTime);
        }
        if (tiltUp == 1){
            transform.Rotate(Vector3.right, rotateSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision){
        AddReward(-1f);
        EndEpisode();
    }
}