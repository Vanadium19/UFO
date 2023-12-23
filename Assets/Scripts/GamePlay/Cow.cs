using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Cow : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private GameObject _deadCowPrefab;
    [SerializeField] private float _maxJumpTimer;
    [SerializeField] private float _minJumpTimer;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private float _jumpTimer = 1;
    private bool _isCatched = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }    

    private void Update()
    {
        if (_isCatched == false)
            TryJump();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCatched)
            return;

        CheckPlayerÑollision(collision);
    }

    public void SetCatched(bool value)
    {
        _isCatched = value;
        _animator.SetBool("Fly", value);
        _rigidbody.isKinematic = value;
    }

    private void TryJump()
    {
        if (_jumpTimer > 0)
        {
            _jumpTimer -= Time.deltaTime;

            if (_jumpTimer <= 0)
            {
                Jump();
                _jumpTimer = Random.Range(_minJumpTimer, _maxJumpTimer);
            }
        }
    }

    private void Jump()
    {
        _animator.SetTrigger("Jump");
        _rigidbody.velocity = (Vector3.up + transform.forward) * _jumpForce;
    }

    private void CheckPlayerÑollision(Collision collision)
    {
        var attachedRigidbody = collision.collider.attachedRigidbody;

        if (attachedRigidbody == null)
            return;

        if (attachedRigidbody.GetComponent<Player>() != null)
        {
            Instantiate(_deadCowPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
