using UnityEngine;

public class SpriteRotation : MonoBehaviour
{
    [SerializeField] private bool counterWise;
    private Vector3 axis;


    void Start()
    {
        axis = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (counterWise)
        {
            transform.RotateAround(gameObject.transform.position, axis, Time.deltaTime * 700);
        }
        else
        {
            transform.RotateAround(gameObject.transform.position, axis, Time.deltaTime * -700);
        }
    }
}
