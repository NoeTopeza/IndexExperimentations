using System;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Pool;

namespace Homemade.Script
{
    /// <summary>
    /// Will be used at some point play add AudioSource from a pool and play them
    /// Might never be used if I'm doing something else instead 
    /// </summary>
    public class PianoSound : MonoBehaviour
    {
        public static IObjectPool<AudioSource> pianoAudio;
        public AudioSource audioSource;
        //[SerializeField] private int audioPool = 20; 
        
        private void Awake()
        {
            for (int i = 0; i < 20; i++)
            {
                
            }
        }
    }
}
