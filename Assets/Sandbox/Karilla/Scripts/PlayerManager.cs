using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int life = 5;
    
    public void GetHit(int damage)
    {
        Debug.Log("ouch! j'ai perdu " + damage);
        life -= damage;

        if (life <= 0)
        {
            Debug.Log("enemy " + name + "has dieded");
            Destroy(gameObject);
        }
    }
}
