using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowCatcher : MonoBehaviour
{
    [SerializeField] private float _catchDistance;
    [SerializeField] private float _catchRadius;
    [SerializeField] private GameObject _lightEffect;
    [SerializeField] private LayerMask _layerMask;    
    [SerializeField] private float _catchTime;

    private PlayerInput _input;
    private bool _isCatchActionActive;
    private float _catchTimer = -1f;
    private Transform _catchedCow;
    private Vector3 _catchedCowStartPosition;
    private Vector3 _catchedCowStartScale;

    private void Update()
    {
        CalculateCatchTimer();

        if (_catchedCow != null)
            UpdateCowPosition();
    }

    private void FixedUpdate()
    {
        if (_isCatchActionActive == false)
            return;

        if (_catchedCow != null)
            return;

        TryCatchCow();
    }

    private void OnDisable()
    {
        _input.CatchPressed -= OnCatchPressed;
        _input.CatchReleased -= OnCatchReleased;
    }

    public void SetInput(PlayerInput input)
    {
        _input = input;
        _input.CatchPressed += OnCatchPressed;
        _input.CatchReleased += OnCatchReleased;
    }

    private void OnCatchPressed()
    {
        SetCatchAction(true);
    }

    private void OnCatchReleased()
    {
        if (_catchedCow != null)
            return;

        SetCatchAction(false);
    }

    private void CalculateCatchTimer()
    {
        if (_catchTimer > 0)
        {
            _catchTimer -= Time.deltaTime / _catchTime;

            if (_catchTimer <= 0)
            {
                Destroy(_catchedCow.gameObject);
                _catchedCow = null;
                SetCatchAction(false);
            }
        }
    }

    private void TryCatchCow()
    {
        var colliders = Physics.OverlapSphere(transform.position + transform.forward * _catchDistance, _catchRadius, _layerMask, QueryTriggerInteraction.Ignore);

        foreach (var collider in colliders)
        {
            var cow = collider.GetComponentInParent<Cow>();

            if (cow != null)
            {
                CatchCow(cow);
                break;
            }
        }
    }

    private void CatchCow(Cow cow)
    {
        _catchedCow = cow.transform;
        _catchedCow.SetParent(transform);
        cow.SetCatched(true);
        _catchedCowStartPosition = cow.transform.localPosition;
        _catchedCowStartScale = cow.transform.localScale;
        _catchTimer = 1f;
    }

    private void UpdateCowPosition()
    {
        _catchedCow.localPosition = Vector3.Lerp(Vector3.zero, _catchedCowStartPosition, _catchTimer);
        _catchedCow.localScale = Vector3.Lerp(Vector3.zero, _catchedCowStartScale, _catchTimer);
    }

    private void SetCatchAction(bool value)
    {
        _isCatchActionActive = value;
        _lightEffect.SetActive(value);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position + transform.forward * _catchDistance, _catchRadius);
    }
}
