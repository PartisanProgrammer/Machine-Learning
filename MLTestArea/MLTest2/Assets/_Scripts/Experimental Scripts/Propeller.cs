using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour{
   public Rigidbody rb;

   void Awake(){
      rb = GetComponent<Rigidbody>();
   }
   
   public void SetThrottle(float input, float speed){
      Debug.Log("SetThrottle: " + input);
     // rb.velocity+= transform.up* input* speed* Time.deltaTime;
      rb.AddForce(transform.up* input* speed* Time.deltaTime);
   }

   public void ResetVelocity(){
      rb.velocity= Vector3.zero;
   }
      
   [ContextMenu("TestPropeller")]
   public void TestPropeller(){
      Debug.Log("TestPropeller");
      SetThrottle(1,1000);
   }
}
