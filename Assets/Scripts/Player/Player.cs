using System;
using UnityEngine;

[RequireComponent(typeof(ConstantForce))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private Engine _engine;
    [SerializeField] private float _forcePower;
    [SerializeField] private CowCatcher _cowCatcher;

    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private ConstantForce _constantForce;

    private void Awake()
    {
        _playerInput = gameObject.AddComponent<PlayerInput>();
        _constantForce = GetComponent<ConstantForce>();
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _cowCatcher.SetInput(_playerInput);
        _engine.Initialize(_rigidbody);
    }

    private void Update()
    {
        bool isVerticalAxisActive = !Mathf.Approximately(_playerInput.Controls.y, 0);

        if (isVerticalAxisActive)
        {
            _engine.SetAltitude(_engine.GetCurrentDistance());
            _engine.SetOverrideControls(_playerInput.Controls.y);
        }

        _engine.IsOverrided = isVerticalAxisActive;
    }

    private void FixedUpdate()
    {            
        _constantForce.force = Vector3.right * _playerInput.Controls.x * _forcePower + Physics.gravity * _rigidbody.mass;        
    }

}
