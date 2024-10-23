using UnityEngine;
using UnityEngine.InputSystem;

public class EngineControls : MonoBehaviour
{
    public GameObject mainEngines;
    public GameObject reverseEngines;
    public GameObject leftForwardEngines;
    public GameObject rightForwardEngines;
    public GameObject leftBackEngines;
    public GameObject rightBackEngines;

    private InputAction moveAction;
    private InputAction rotateAction;

    public Rigidbody rigidbody;

    void Start()
    {
        rotateAction = InputSystem.actions.FindAction("Rotate");
    }

    void Update()
    {
        float rotateAxis = rotateAction.ReadValue<float>();

        leftForwardEngines.SetActive(rotateAxis > 0);
        rightBackEngines.SetActive(rotateAxis > 0);
        rightForwardEngines.SetActive(rotateAxis < 0);
        leftBackEngines.SetActive(rotateAxis < 0);

        mainEngines.SetActive(transform.InverseTransformDirection(rigidbody.linearVelocity).z != 0);
    }
}
