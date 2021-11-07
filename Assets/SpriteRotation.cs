using UnityEngine;

public class SpriteRotation : MonoBehaviour
{
    [SerializeField] private Transform point;
    private Vector3 axis;


    void Start()
    {
        axis = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(point.position, axis, Time.deltaTime * 1000);
    }
}
