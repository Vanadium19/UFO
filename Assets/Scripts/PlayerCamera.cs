using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private float _startPositionY;
    private float _startPositionZ;

    private void Awake()
    {
        _startPositionY = transform.position.y;
        _startPositionZ = transform.position.z;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(_player.position.x, _startPositionY, _startPositionZ);
    }
}
