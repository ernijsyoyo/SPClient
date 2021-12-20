using System.Collections;
using System.Collections.Generic;
using extOSC;
using UnityEngine;

public abstract class OSCReceiveBase : MonoBehaviour
{
	public List<string> addresses = new List<string>()
	{
		Constants.OSC_GUIDE, Constants.OSC_REC_DEST
	};

	[Header("OSC Settings")]
	public OSCReceiver Receiver;


	protected virtual void Start()
	{
        foreach (var address in addresses) {
			Receiver.Bind(address, ReceivedMessage);
		}
		print("Receiver bound" + Receiver.LocalHost + ":" + Receiver.LocalPort);
	}

	public virtual void ReceivedMessage(OSCMessage message)
	{
		Debug.LogFormat("Received: {0}", message);
	}
}
