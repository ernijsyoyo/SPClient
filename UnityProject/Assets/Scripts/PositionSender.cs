using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class PositionSender : MonoBehaviour
    {

        [SerializeField]
        private bool sendUpdates = false;

        private void Start() {
            GlobalOrigin.OnOrientationSet += GlobalOrigin_OnOrientationSet;
        }

        void OnDestroy() {
            GlobalOrigin.OnOrientationSet -= GlobalOrigin_OnOrientationSet;
        }

        private void GlobalOrigin_OnOrientationSet(System.EventArgs args) {
            sendUpdates = true;
        }

        // Update is called once per frame
        void FixedUpdate() {
            if (sendUpdates) {
                OSCSender.sendVector3(Constants.OSC_POS, gameObject.transform.position);
                OSCSender.sendVector3(Constants.OSC_ROT, gameObject.transform.rotation.eulerAngles);
            }
        }
    }
}
