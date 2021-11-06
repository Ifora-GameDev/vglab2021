using UnityEngine;

namespace Teist
{
    //Permet d'afficher ça dans l'inspector
    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemies;
        public float rate = 1;
        public float timeNextWave = 10f;

        public bool spawnRight = false;
        public bool spawnLeft = false;
        public bool spawnTop = false;
        public bool spawnBottom = false;
    }
}