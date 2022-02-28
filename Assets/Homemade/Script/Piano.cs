using System.Collections.Generic;
using Homemade.Script;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace Homemade.Script
{
    public class Piano : MonoBehaviour
    {
        public GameObject pianoHandRight;
        public GameObject pianoHandLeft;
        public Hand handRight;
        public Hand handLeft;

        public FingerManager fingerManager;

        private void Awake() {
            pianoHandLeft.SetActive(false);
            pianoHandRight.SetActive(false);
        }

        private void Start() {
            // set up of the distances of each fingers on pianoHands
            var space = fingerManager.piano.tileSize;
            var spaces = new List<float>() {1.5f, 0.5f, -0.5f, -1.5f};
            for (int i = 0; i < 8; i++) {
                var right = i >= 4 ? -1 : 1; // finger of the right or left hand ?
                fingerManager.fingers[i].transform.Translate(spaces[i % 4] * space * right, 0, 0);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // check if player's hand ?
            var hand = other.GetComponentInParent<Hand>();
            if (!hand) return;
            
            //hand.renderModelPrefab.SetActive(false);
            hand.mainRenderModel.Hide();

            if (hand.handType == SteamVR_Input_Sources.LeftHand) { // right or left hand ?
                pianoHandLeft.SetActive(true);
                fingerManager.HandIn(false);
            }
            else
            {
                pianoHandRight.SetActive(true);
                fingerManager.HandIn(true);
            }
            //Debug.Log("A hand is touching the piano:" + hand.handType); 
            
        }

        private void OnTriggerStay(Collider other)
        {
            if (pianoHandLeft.activeSelf)
            {
                var position = pianoHandLeft.transform.position;
                pianoHandLeft.transform.position = new Vector3(handLeft.transform.position.x, position.y, position.z);
            }
            if (pianoHandRight.activeSelf)
            {
                var position = pianoHandRight.transform.position;
                pianoHandRight.transform.position = new Vector3(handRight.transform.position.x, position.y, position.z);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var hand = other.GetComponentInParent<Hand>();
            if (!hand) return;
            
            hand.mainRenderModel.Show();

            if (hand.handType == SteamVR_Input_Sources.LeftHand) { // right or left hand ?
                pianoHandLeft.SetActive(false);
                fingerManager.HandOut(false);
            }
            else
            {
                pianoHandRight.SetActive(false);
                fingerManager.HandOut(true);
            }
            //Debug.Log("A hand stopped touching the piano");
        }
    }
}
