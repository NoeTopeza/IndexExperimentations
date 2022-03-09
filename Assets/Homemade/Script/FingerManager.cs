using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Homemade.Script
{
    public class FingerManager : MonoBehaviour
    {
        private SteamVR_Action_Skeleton.ChangeHandler FingerHandler;
        // SteamVR_Action_Pose.changeTolerance set to 0.6f directly in its class
        // I haven't found a way to do it safely from this file
        public PianoRampe piano;
        
        public SteamVR_Action_Skeleton skeletonLeft = SteamVR_Input.GetSkeletonAction("SkeletonLeftHand");
        public SteamVR_Action_Skeleton skeletonRight = SteamVR_Input.GetSkeletonAction("SkeletonRightHand");

        public GameObject fingerL0;
        public GameObject fingerL1;
        public GameObject fingerL2;
        public GameObject fingerL3;
        
        public GameObject fingerR0;
        public GameObject fingerR1;
        public GameObject fingerR2;
        public GameObject fingerR3;
        
        [NonSerialized] public List<GameObject> fingers;
        private List<bool> fingerCurled;
        //private List<string> routineName = new List<string>() {
        //    "Left0", "Left1", "Left2", "Left3", "Right0", "Right1", "Right2", "Right3"
        //};

        // Threshold for the finger to be considered as pressing a key
        [SerializeField]
        private float thresholdIn = 0.4f;
        [SerializeField]
        private float thresholdOut = 0.38f;

        private void Awake()
        {
            
            fingers = new List<GameObject> {
                fingerL0, fingerL1, fingerL2, fingerL3,
                fingerR0, fingerR1, fingerR2, fingerR3
            };
            fingerCurled = new List<bool> {
                false, false, false, false,
                false, false, false, false
            };
        }

        /// <summary>
        /// HandIn and HandOut are both used in the Piano script to activate the finger inputs
        /// </summary>
        public void HandIn(bool right) {
            //Debug.Log("Hand in, adding fingerPress");
            if (!right)
                skeletonLeft.onChange += fingerPress;
            else
                skeletonRight.onChange += fingerPress;
        }
        /// <summary>
        /// HandIn and HandOut are both used in the Piano script to activate the finger inputs
        /// </summary>
        public void HandOut(bool right) {
            if (!right)
                skeletonLeft.onChange -= fingerPress;
            else
                skeletonRight.onChange -= fingerPress;
        }

        void whichFinger(float _finger, int which, bool right)
        {
            which += right ? 4 : 0; // which finger among fingers?
            if (_finger >= thresholdIn && !fingerCurled[which])
            {
                fingerCurled[which] = true;
                //Debug.Log($"a finger is pressed: {which}");
                StartCoroutine(piano.playSound(fingers[which].transform.position));
                fingers[which].GetComponent<Renderer>().material.color = Color.red;
            }
            else if (_finger < thresholdOut && fingerCurled[which])
            {
                fingerCurled[which] = false;
                fingers[which].GetComponent<Renderer>().material.color = Color.white;
                //Debug.Log($"a finger is raised: {which}");
            }
        }
        
        private void fingerPress(SteamVR_Action_Skeleton skeleton)
        {
            // right: say if skeleton belongs to right Hand, if false its the left Hand
            bool right = skeleton.Equals(skeletonRight);
            
            // No solution more generic were found ..
            whichFinger(skeleton.indexCurl, 0, right);
            whichFinger(skeleton.middleCurl, 1, right);
            whichFinger(skeleton.ringCurl, 2, right);
            whichFinger(skeleton.pinkyCurl, 3, right);
        }
    }
}
