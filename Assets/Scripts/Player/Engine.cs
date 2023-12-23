using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Engine : MonoBehaviour
{
    [HideInInspector]
    public bool IsOverrided;

    [Header("SphereCast")]
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _sphereCastRadius;
    [SerializeField] private float _maxDistance;

    [Header("Lift")]
    [SerializeField] private float _maxForce;
    [SerializeField] private float _damping;

    private Transform _transform;
    private Rigidbody _targetBody;
    private float _altitude;
    private float _distance;
    private float _springSpeed;
    private float _oldDistance;
    private float _forceHightRange = 1f;
    private float _inputY;

    private void FixedUpdate()
    {
        if (_targetBody == null)
            return;

        var forward = _transform.forward;

        if (IsOverrided)
            ForceUpDown(forward);
        else
            Lift(forward);

        CalculateSpringSpeed();
    }

    public void Initialize(Rigidbody targetBody)
    {
        _transform = transform;
        _targetBody = targetBody;
    }

    public float GetCurrentDistance()
    {
        if (Physics.SphereCast(_transform.position, _sphereCastRadius, _transform.forward, out RaycastHit hitInfo, 
            _maxDistance, _layerMask, QueryTriggerInteraction.Ignore))
        {
            return hitInfo.distance;
        }

        return _maxDistance;
    }

    public void SetAltitude(float altitude)
    {
        _altitude = Mathf.Clamp(altitude, _sphereCastRadius, _maxDistance);
    }

    private void ForceUpDown(Vector3 forward)
    {
        float forceFactor = _inputY > 0 ? 1 : 0;      
        _targetBody.AddForce(-forward * Mathf.Max(forceFactor * _maxForce - _springSpeed * _maxForce * _damping, 0f), ForceMode.Force);
    }

    private void Lift(Vector3 forward)
    {
        if (Physics.SphereCast(_transform.position, _sphereCastRadius, forward, out RaycastHit hitInfo,
            _maxDistance, _layerMask, QueryTriggerInteraction.Ignore))
        {
            _distance = hitInfo.distance;

            var minForceHight = _altitude + _forceHightRange;
            var maxForceHight = _altitude - _forceHightRange;

            float forceFactor = Mathf.Clamp(_distance, maxForceHight, minForceHight).Remap(maxForceHight, minForceHight, 1, 0f);        
            _targetBody.AddForce(-forward * Mathf.Max(forceFactor * _maxForce - _springSpeed * _maxForce * _damping, 0f), ForceMode.Force); //_maxForce * Mathf.Clamp01(1 - distance/_maxDistance)
        }   
    }

    public void SetOverrideControls(float inputY)
    {
        _inputY = inputY;
    }

    private void CalculateSpringSpeed()
    {
        _springSpeed = (_distance - _oldDistance) * Time.fixedDeltaTime;
        _springSpeed = Mathf.Max(_springSpeed, 0f);
        _oldDistance = _distance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        var startPoint = transform.position;
        var endPoint = startPoint + _maxDistance * transform.forward;

        Gizmos.DrawLine(startPoint, endPoint);
        Gizmos.DrawWireCube(startPoint, Vector3.one * 0.2f);
        Gizmos.DrawSphere(endPoint, _sphereCastRadius);
    }
}
