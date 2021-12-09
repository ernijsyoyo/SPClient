using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class testLookAt : MonoBehaviour
{
    public bool test = false;
    public GameObject lookAt;


    // Update is called once per frame
    void Update()
    {
        if(test)
        {
            test = false;
            GameObject start = null;
            GameObject end = null;
            
            // spawn cubes
            for (int i = 0; i < 12; i += 3)
            {
                var x = i;
                var y = i + 1;
                var z = i + 2;
                var pos = new Vector3(x, y, z);
                GameObject point = Instantiate(lookAt);


                if (i == 0)
                {
                    start = point;
                }
                else
                { // true after first iteration
                    end = point;
                    start.transform.LookAt(end.transform);
                    start = end;
                }
                point.transform.position = pos;
                point.transform.localScale = (new Vector3(0.1f, 0.1f, 0.1f));
            }
        }
    }    
}

