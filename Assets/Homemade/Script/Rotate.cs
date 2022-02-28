using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace Homemade.Script
{
    public class Rotate : MonoBehaviour
    {
        public SteamVR_Action_Vector2 input;
        public Transform origin;
        public float speed = 1;
        void Update()
        {
            var orientation = new Vector3(input.axis.x, 0, input.axis.y);
            
            transform.position = origin.position;
            
            if (input.changed) {
                transform.LookAt(Vector3.ProjectOnPlane(orientation, Vector3.up));
            }
        }
    }
}
