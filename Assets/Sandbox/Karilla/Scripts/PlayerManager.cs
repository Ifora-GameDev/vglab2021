using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private bool _isInDanger = false;

    public static event Action OnPlayerHit;
    public static event Action OnPlayerDie;

    private void OnEnable()
    {
        HackController.OnShipInDanger += SetPlayerInDanger;
    }

    private void OnDisable()
    {
        HackController.OnShipInDanger -= SetPlayerInDanger;
    }

    private void SetPlayerInDanger(bool isInDanger)
    {
        _isInDanger = isInDanger;
    }

    public void GetHit(int damage)
    {
        Debug.Log("Player hit");
        
        if(_isInDanger)
        {
            OnPlayerDie?.Invoke();
            Destroy(gameObject);
            return;
        }

        OnPlayerHit?.Invoke();
    }
}
