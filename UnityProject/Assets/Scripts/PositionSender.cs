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
                var pos = TransformConversions.posRelativeTo(GlobalOrigin.getTransform(), gameObject.transform);
                var rot = TransformConversions.rotRelativeTo(GlobalOrigin.getRot(), gameObject.transform.rotation);
                OSCSender.sendVector3(Constants.OSC_POS, pos);
                OSCSender.sendVector3(Constants.OSC_ROT, rot.eulerAngles);
            }
        }
    }
}
