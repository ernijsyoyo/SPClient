using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGlobalOrientation : MonoBehaviour
{

    // Start is called before the first frame update
    void Start() {
        
    }

    private void OrientationSetHandler(EventArgs args) {
        print("Position was set!");
    }
}
