using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float timeBeforeAutoDestruction=1f;
    void Start()
    {
        Destroy(gameObject, 2f);
    }
}
