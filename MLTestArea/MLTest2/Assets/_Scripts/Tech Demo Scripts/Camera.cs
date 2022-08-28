using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour{

    [SerializeField] float speed;
    void Update(){
        transform.position+= new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical")) * (Time.deltaTime * speed);
    }
}
