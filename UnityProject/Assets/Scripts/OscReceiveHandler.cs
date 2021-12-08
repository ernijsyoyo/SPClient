using System;
using System.Collections;
using System.Collections.Generic;
using extOSC;
using UnityEngine;

public class OscReceiveHandler : OSCReceiveBase
{
    public GameObject cubePrefab;
    public delegate void DestinationsReceived(oscArgs args);
    public static event DestinationsReceived OnDestinationsReceived;

    public delegate void NavigationReceived(oscArgs args);
    public static event NavigationReceived OnNavigationReceived;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    public override void ReceivedMessage(OSCMessage message)
    {
        base.ReceivedMessage(message);
        switch (message.Address)
        {
            case Constants.OSC_DEST:
                // spawn cubes
                for (int i = 0; i < message.Values.Count; i+=3)
                {
                    var x = message.Values[i].FloatValue;
                    var y = message.Values[i + 1].FloatValue;
                    var z = message.Values[i + 2].FloatValue;
                    var pos = new Vector3(x, y, z);

                    GameObject point = Instantiate(cubePrefab);
                    point.transform.position = pos;
                    point.transform.localScale = (new Vector3(0.1f, 0.1f, 0.1f));
                }
                OnDestinationsReceived?.Invoke(new oscArgs(message));
                break;
            default:
                break;
        }
    }

    public class oscArgs : EventArgs {
        public OSCMessage msg;
        public oscArgs(OSCMessage msg) {
            this.msg = msg;
        }
    }
}
