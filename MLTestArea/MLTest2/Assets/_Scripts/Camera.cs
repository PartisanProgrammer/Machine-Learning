using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour{

    [SerializeField] Transform target;
    void Update(){
        transform.LookAt(target);
    }
}
