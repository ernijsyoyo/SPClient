using System;
using System.Collections;
using System.Collections.Generic;
using extOSC;
using UnityEngine;

/// <summary>
/// Handles incoming OSC messages
/// </summary>
public class OscReceiveHandler : OSCReceiveBase
{
    /// <summary>
    /// Prefab for spawning guidance arrows
    /// </summary>
    public GameObject pointerPrefab;
    /// <summary>
    /// List of spawned guidance objects
    /// </summary>
    public List<GameObject> spawned = new List<GameObject>();

    // Event raised when destinations are received
    public delegate void DestinationsReceived(oscArgs args);
    public static event DestinationsReceived OnDestinationsReceived;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    public override void ReceivedMessage(OSCMessage message)
    {
        base.ReceivedMessage(message);
        GameObject start = null;
        GameObject end = null;
        switch (message.Address)
        {
            case Constants.OSC_DEST:
                // spawn arrows
                for (int i = 0; i < message.Values.Count; i+=3) {
                    // extract their positions
                    var x = message.Values[i].FloatValue;
                    var y = message.Values[i + 1].FloatValue;
                    var z = message.Values[i + 2].FloatValue;
                    var pos = SP.TransformConversions.invertAxisZ(new Vector3(x, y - 0.2f, z)); // lower by 20cm for better visibility

                    // instantiate the prefabs
                    GameObject point = Instantiate(pointerPrefab);
                    point.transform.parent = SP.GlobalOrigin.getTransform();
                    point.transform.localPosition = pos;


                    if(i == 0) {
                        point.transform.GetComponentInChildren<MeshRenderer>().materials[0].color = Color.green;
                        start = point;
                    }
                    else { // Configure arrows to point sequentially to the next one
                        end = point;
                        start.transform.LookAt(end.transform);
                        start = end;
                    }
                }

                // Mark end arrow and raise an event
                end.transform.GetComponentInChildren<MeshRenderer>().materials[0].color = Color.red;
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
