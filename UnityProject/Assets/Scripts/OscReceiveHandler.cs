using System;
using System.Collections;
using System.Collections.Generic;
using extOSC;
using UnityEngine;

public class OscReceiveHandler : OSCReceiveBase
{
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
        switch (message.Address)
        {
            case Constants.OSC_REC_DEST:
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
