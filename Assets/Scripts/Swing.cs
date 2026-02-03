using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private float _force = 500f;

    private HingeJoint _hingeJoint;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _hingeJoint = GetComponent<HingeJoint>();
        _rigidbody = GetComponent<Rigidbody>();

        _inputHandler.SpaceKeyPressed += OnHandleSpaceKey;
    }

    private void OnDestroy()
    {
        _inputHandler.SpaceKeyPressed -= OnHandleSpaceKey;
    }

    private void OnHandleSpaceKey()
    {
        _hingeJoint.useSpring = false;
        _rigidbody.AddForce(Vector3.forward * _force, ForceMode.Impulse);

        Invoke("EnableSpring", 0.2f);
    }

    private void EnableSpring()
    {
        _hingeJoint.useSpring = true;
    }
}