using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int money;

    [SerializeField] private Player player;

    

    // Start is called before the first frame update
    void Awake()
    {
        Enemy.OnEnDie += Enemy_OnEnDie;
    }


    //Event send when an enemy die, containing money reward information
    private void Enemy_OnEnDie(int reward)
    {
        Debug.Log(reward);
        money += reward;
    }

    // Update is called once per frame
    void Update()
    {
        //Checking player's life


        //Managing enemy's spawner
    }
}
