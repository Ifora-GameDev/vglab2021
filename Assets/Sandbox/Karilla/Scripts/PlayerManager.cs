using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int life = 5;

    public static event Action OnPlayerDie;
    public void GetHit(int damage)
    {
        Debug.Log("ouch! j'ai perdu " + damage);
        life -= damage;

        if (life <= 0)
        {
            OnPlayerDie?.Invoke();
            Debug.Log("enemy " + name + "has dieded");
            Destroy(gameObject);
        }
    }
}
