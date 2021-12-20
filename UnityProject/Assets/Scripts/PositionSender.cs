using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    /// <summary>
    /// Send out user position during each fixed update for logging purposes
    /// </summary>
    public class PositionSender : MonoBehaviour
    {

        [SerializeField]
        public static bool sendUpdates = false;

        private void Start() {
            OscReceiveHandler.OnDestinationsReceived += GlobalOrigin_OnOrientationSet;
        }

        void OnDestroy() {
            OscReceiveHandler.OnDestinationsReceived -= GlobalOrigin_OnOrientationSet;
        }

        /// <summary>
        /// Event to flip the bool once we have received the destinations.
        /// This allows us to ensure correct starting of test start-time logging
        /// </summary>
        /// <param name="args"></param>
        private void GlobalOrigin_OnOrientationSet(System.EventArgs args) {
            sendUpdates = true;
            print("Sending position updates..");
        }

        // Update is called once per frame
        void FixedUpdate() {
            if (sendUpdates) {
                var pos = TransformConversions.posRelativeTo(GlobalOrigin.getTransform(), gameObject.transform);
                var rot = TransformConversions.rotRelativeTo(GlobalOrigin.getRot(), gameObject.transform.rotation);
                pos = TransformConversions.invertAxisZ(pos);
                OSCSender.sendVector3(Constants.OSC_POS, pos);
                OSCSender.sendVector3(Constants.OSC_ROT, rot.eulerAngles);
            }
        }
    }
}
