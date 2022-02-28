using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Homemade.Script
{
    public class PianoRampe : MonoBehaviour
    {
        [SerializeField] private Transform rampeOrigin;
        [SerializeField] private Transform rampeEnd;
        private float distMax;
        [NonSerialized] public float tileSize; 
        
        private int nbTouche;
        private float _nbTouche;

        //private AudioSource piano;
        [SerializeField] private GameObject Tile;
        
        public List<AudioClip> Notes;
        private readonly List<AudioSource> Tiles = new();
        private void Awake()
        {
            nbTouche = Notes.Count;
            _nbTouche = nbTouche;

            if (nbTouche <= 0)
                throw new ArgumentException("No sound assigned to piano !");
            
            Tile.GetComponent<Renderer>().material.mainTextureScale = new Vector2(_nbTouche, 0.5f);

            // Je dois apprendre à faire du pooling, en attendant:
            for (int i = 0; i < nbTouche; i++) {
                var sound = gameObject.AddComponent<AudioSource>();
                sound.clip = Notes[i];
                sound.volume = 0.6f;
                Tiles.Add(sound);
            }

            //piano = GetComponent<AudioSource>();
            distMax = rampeEnd.position.x - rampeOrigin.position.x;
            tileSize = distMax / _nbTouche;
        }

        private bool whichSound(int i, float scale) {
            
            if (!(scale < i)) return false;
                
            // if this is the right note to play
            //piano.clip = Notes[i];
            
            //Debug.Log($"Note played: {Notes[i].name} with d = {scale} and i = {i}");
            
            Tiles[i].Play();
            return true;
        }
        
        /// <summary>
        /// Play a piano sound of the corresponding key
        /// </summary>
        /// <param name="position">
        /// position of the finger that pressed in the world
        /// </param>
        public IEnumerator playSound(Vector3 position)
        {
            var distance = position.x - rampeOrigin.position.x;
            if (distance < 0 || distMax < distance)
                yield break;
            
            float scale = _nbTouche * distance / distMax; // to modify: use tileSize instead
            
            // Amélioration possible
            // tableau de delegate, un par touche
            // Une fois que la touche n est appuyée
            // tu call le delegate à l'offset n, évidemment il faut que tu null check
            // ensuite pour le call tu le fais dans un thread, à voir comment
            // complexité de call : Omega(1)
            // complexité spatiale : Omega(n) n étant le nb de touches

            for (int i = 1; i < nbTouche; i++)
                if (whichSound(i, scale))
                    yield break;
        }
        
    }
}
