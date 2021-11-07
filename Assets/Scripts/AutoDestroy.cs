using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float timeBeforeAutoDestruction=2f;
    void Start()
    {
        Destroy(gameObject, timeBeforeAutoDestruction);
    }
}
