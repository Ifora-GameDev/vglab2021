using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int life = 5;

    public void GetHit(int damage)
    {
        Debug.Log("ouch! j'ai perdu " + damage + "... salope");
        life -= damage;

        if (life <= 0)
        {
            Debug.Log("enemy " + name + "has dieded");
            Destroy(gameObject);
        }
    }
}
