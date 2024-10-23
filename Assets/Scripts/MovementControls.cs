using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.ParticleSystem;

public class MovementControls : MonoBehaviour
{
    public float maxVelocity;
    public float acceleration;
    public float sideAcceleration;
    public float maxSideAcceleration;

    public float rotationSpeed;
    public Rigidbody rigidbody;

    private InputAction moveAction;
    private InputAction rotateAction;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rotateAction = InputSystem.actions.FindAction("Rotate");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rigidbody.linearVelocity);

        Vector2 moveVector2 = moveAction.ReadValue<Vector2>();
        float rotationAxis = rotateAction.ReadValue<float>();

        if (moveVector2.y > 0)
        {
            localVelocity += Vector3.forward * (acceleration * Time.deltaTime);
        }

        if (moveVector2.y < 0)
        {
            localVelocity -= Vector3.forward * (acceleration * Time.deltaTime);
        }

        if (moveVector2.x > 0)
        {
            localVelocity += Vector3.right * (sideAcceleration * Time.deltaTime);
        }

        if (moveVector2.x < 0)
        {
            localVelocity += -Vector3.right * (sideAcceleration * Time.deltaTime);
        }

        if (rotationAxis != 0)
        {
            transform.Rotate(Vector3.up, rotationAxis * rotationSpeed * Time.deltaTime);
        }

        if (moveVector2.x == 0)
        {
            localVelocity.x = Mathf.MoveTowards(localVelocity.x, 0, sideAcceleration * Time.deltaTime);
        }

        if (moveVector2.y == 0)
        {
            localVelocity.z = Mathf.MoveTowards(localVelocity.z, 0, acceleration * Time.deltaTime);
        }

        if (Mathf.Abs(localVelocity.x) > maxSideAcceleration)
        {
            localVelocity.x = maxSideAcceleration * Mathf.Sign(localVelocity.x);
        }

        if (Mathf.Abs(localVelocity.z) > maxVelocity)
        {
            localVelocity.z = maxVelocity * Mathf.Sign(localVelocity.z);
        }

        rigidbody.linearVelocity = transform.TransformDirection(localVelocity);
    }
}
