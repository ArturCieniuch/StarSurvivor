using UnityEngine;
using UnityEngine.InputSystem;

public class MovementControls : MonoBehaviour
{
    public float maxVelocity;
    public float acceleration;
    public float sideAcceleration;

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
        Vector3 velocity = rigidbody.linearVelocity;

        if (moveAction.ReadValue<Vector2>().y > 0)
        {
            velocity += transform.forward * (acceleration * Time.deltaTime);
        }

        if (moveAction.ReadValue<Vector2>().y < 0)
        {
            velocity -= transform.forward * (acceleration * Time.deltaTime);
        }

        if (moveAction.ReadValue<Vector2>().x > 0)
        {
            velocity += transform.right * (sideAcceleration * Time.deltaTime);
        }

        if (moveAction.ReadValue<Vector2>().x < 0)
        {
            velocity += -transform.right * (sideAcceleration * Time.deltaTime);
        }

        if (rotateAction.ReadValue<float>() != 0)
        {
            transform.Rotate(Vector3.up, rotateAction.ReadValue<float>() * rotationSpeed * Time.deltaTime);
        }

        if (moveAction.ReadValue<Vector2>().y == 0 && moveAction.ReadValue<Vector2>().x == 0)
        {
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, acceleration * Time.deltaTime);
        }

        var localVelocity = transform.InverseTransformDirection(velocity);

        if (localVelocity.z > maxVelocity)
        {
            localVelocity.z = maxVelocity;
            velocity = transform.TransformDirection(localVelocity);
        }

        rigidbody.linearVelocity = velocity;

        Debug.Log(transform.InverseTransformDirection(velocity) + " " + velocity.ma);
    }
}
