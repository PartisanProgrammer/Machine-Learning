using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

public class Drone : Agent{
    [SerializeField] List<Propeller> propellers;
    [SerializeField] Transform targetTransform;
    [SerializeField] float speed = 0.5f;
    [SerializeField] new Rigidbody rigidbody;

    void Awake(){
        rigidbody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin(){
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = Quaternion.identity;
        foreach(Propeller propeller in propellers){
            propeller.ResetVelocity();
            Debug.Log(propeller.name);
        }
        
        targetTransform.localPosition = Random.insideUnitSphere * 5;
    }

    public override void CollectObservations(VectorSensor sensor){
      
        //Propellers
        foreach (var propeller in propellers){
            sensor.AddObservation(propeller.rb.velocity); //3 * 4
        }
      
        //Drone
        sensor.AddObservation(transform.localPosition); //3
        sensor.AddObservation(transform.localRotation); //3
        sensor.AddObservation(rigidbody.velocity); //3
      
        //Target
        sensor.AddObservation(targetTransform.localPosition); //3
    }
   
    
    public override void OnActionReceived(ActionBuffers actions){
        Debug.Log("OnActionReceived");
        var propeller0 = actions.ContinuousActions[0];
        var propeller1 = actions.ContinuousActions[1];
        var propeller2 = actions.ContinuousActions[2];
        var propeller3 = actions.ContinuousActions[3];
        propellers[0].SetThrottle(propeller0,speed);
        propellers[1].SetThrottle(propeller1,speed);
        propellers[2].SetThrottle(propeller2,speed);
        propellers[3].SetThrottle(propeller3,speed);
        // for (int i = 0; i <= propellers.Count; i++){
        //     propellers[i].SetThrottle(actions.ContinuousActions[i],speed);
        //    
        // } 
        propellers[0].SetThrottle(propeller0,speed);
    }

    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        // continuousActions[0] = Input.GetAxisRaw("Horizontal");
        // continuousActions[1] = Input.GetAxisRaw("2");
        // continuousActions[2] = Input.GetAxisRaw("3");
        // continuousActions[3] = Input.GetAxisRaw("4");
        //
        
        continuousActions[0] = Input.GetKey(KeyCode.Alpha1) ? 1 : 0;
        continuousActions[1] = Input.GetKey(KeyCode.Alpha2) ? 1 : 0;
        continuousActions[2] = Input.GetKey(KeyCode.Alpha3) ? 1 : 0;
        continuousActions[3] = Input.GetKey(KeyCode.Alpha4) ? 1 : 0;
           
    }
   
   
}