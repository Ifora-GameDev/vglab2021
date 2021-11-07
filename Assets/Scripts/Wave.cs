using UnityEngine;

namespace Teist
{
    //Permet d'afficher �a dans l'inspector
    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemies;
        public float rate = 1;

        public Waypoints path;
        public bool isLerp;

            //A MODIFIER SI ON VEUT POUVOIR CHANGER LE PATTERN EN FONCTION DE LA DIRECTION
            //VOIR AVEC GAMEMANAGER
            /*
            public bool spawnRight = false;
            public bool spawnLeft = false;
            public bool spawnTop = false;
            public bool spawnBottom = false;
            */

        public bool isTimed = false;
        public float timeNextWave = 10f;
    }
}