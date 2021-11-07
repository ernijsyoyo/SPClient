using System.Collections;
using System.Collections.Generic;
using extOSC;
using UnityEngine;

public abstract class OSCReceiveHandler : MonoBehaviour
{
    public string Address = "/example";

	[Header("OSC Settings")]
	public OSCReceiver Receiver;


	protected virtual void Start()
	{
		Receiver.Bind(Address, ReceivedMessage);
	}

	public virtual void ReceivedMessage(OSCMessage message)
	{
		Debug.LogFormat("Received: {0}", message);
	}
}
