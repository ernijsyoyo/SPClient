using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SP;

[ExecuteInEditMode]
public class TestGlobalOrientation : MonoBehaviour
{
    public bool SetGlobalOrigin = false;
    void Start() {
        set();
    }
    void Update() {
        if(SetGlobalOrigin) {
            set();
        }
    }

    void set() {
        print("Setting global origin to " + gameObject.transform.position);
        SetGlobalOrigin = false;
        print(gameObject.transform);
        GlobalOrigin.setTransform(gameObject.transform);
        GlobalOrigin.setRot(gameObject.transform.rotation);
    }
}
