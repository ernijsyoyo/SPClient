using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SP;

[ExecuteInEditMode]
public class testGlobalRef : MonoBehaviour
{
    public bool test;
    public GameObject relTo;

    // Update is called once per frame
    void Update()
    {
        if(test)
        {
            var e = gameObject.transform.InverseTransformPoint(relTo.transform.position);
            print(e);

        }    
    }
}
