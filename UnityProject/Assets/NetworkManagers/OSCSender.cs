using System.Collections;
using System.Collections.Generic;
using extOSC;
using UnityEngine;

[ExecuteInEditMode]
public class OSCSender : MonoBehaviour
{
	public static OSCTransmitter Transmitter {
		get {
			if (Transmitter == null)
				Transmitter = FindObjectOfType<OSCTransmitter>();
			return Transmitter;
		}
		set { Transmitter = value; }
		}

	/// <summary>
    /// Sends a string message to a given address
    /// </summary>
    /// <param name="address"></param>
    /// <param name="msg"></param>
	public static void sendString(string address, string msg) {
		var message = new OSCMessage(address);
		message.AddValue(OSCValue.String(msg));
		Transmitter.Send(message);
	}

	public static void sendVector3(string address, Vector3 msg) {
		var message = new OSCMessage(address);
		var x = msg.x;
		var y = msg.y;
		var z = msg.z;

		message.AddValue(OSCValue.Float(x));
		message.AddValue(OSCValue.Float(y));
		message.AddValue(OSCValue.Float(z));

		Transmitter.Send(message);
	}

	public static void sendList(string address, List<string> ts)
    {
		var message = new OSCMessage(address);
        foreach (var item in ts) {
			message.AddValue(OSCValue.String(item));
		}
		Transmitter.Send(message);
	}
}
