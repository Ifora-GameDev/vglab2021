using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rocket : MonoBehaviour
{
    [SerializeField] private AnimationCurve _speedOverTime = null;
    [SerializeField] private float _lifeTime = 2f;

    private Rigidbody2D _rigidbody = null;
    private bool _isAutoAimed = false;
    private float _elapsedTime = 0f;

    public static event Action<GameObject> OnRocketDestroyed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _elapsedTime = 0f;
    }

    private void Update()
    {
        if(_elapsedTime > _lifeTime)
        {
            OnRocketDestroyed?.Invoke(gameObject);
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = transform.up * _speedOverTime.Evaluate(_elapsedTime/_lifeTime);

        _elapsedTime += Time.fixedDeltaTime;
    }
}
