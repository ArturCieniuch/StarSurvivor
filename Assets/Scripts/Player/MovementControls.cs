using UnityEngine;
using UnityEngine.InputSystem;

public class MovementControls : MonoBehaviour
{
    public float maxVelocity;
    public float acceleration;
    public float sideAcceleration;
    public float maxSideAcceleration;

    public float rotationSpeed;
    public Rigidbody rigidbody;

    public EngineParticles enginesEffect;

    private InputAction moveAction;
    private InputAction rotateAction;

    [SerializeField] private float mainEnginesMaxVolume = 0.2f;
    [SerializeField] private AudioSource forwardEngineSound;
    [SerializeField] private AudioSource backEngineSound;

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

        bool[,] sideEngines = new bool[2, 2];

        if (moveVector2.y > 0)
        {
            localVelocity += Vector3.forward * (acceleration * Time.deltaTime * Player.playerMods.accelerationMod);
            enginesEffect.FullForward();

            if (backEngineSound.volume < mainEnginesMaxVolume)
            {
                backEngineSound.volume += 1f * Time.deltaTime;
            }
        }

        if (moveVector2.y < 0)
        {
            localVelocity -= Vector3.forward * (acceleration * Time.deltaTime * Player.playerMods.accelerationMod);
            enginesEffect.FullBackward();

            if (forwardEngineSound.volume < mainEnginesMaxVolume)
            {
                forwardEngineSound.volume += 1f * Time.deltaTime;
            }
        }

        if (moveVector2.y == 0)
        {
            enginesEffect.StopMainEngines();
        }

        if (moveVector2.x > 0)
        {
            localVelocity += Vector3.right * (sideAcceleration * Time.deltaTime * Player.playerMods.sideAccelerationMod);
            sideEngines[0, 0] = true;
            sideEngines[1, 0] = true;
        }

        if (moveVector2.x < 0)
        {
            localVelocity += -Vector3.right * (sideAcceleration * Time.deltaTime * Player.playerMods.sideAccelerationMod);
            sideEngines[0, 1] = true;
            sideEngines[1, 1] = true;
        }

        if (rotationAxis != 0)
        {
            if (rotationAxis < 0)
            {
                sideEngines[0, 1] = true;
                sideEngines[1, 0] = true;
            }
            else
            {
                sideEngines[0, 0] = true;
                sideEngines[1, 1] = true;
            }

            transform.Rotate(Vector3.up, rotationAxis * rotationSpeed * Time.deltaTime);
        }

        enginesEffect.ActivateSideEngines(sideEngines);

        if (moveVector2.x == 0)
        {
            localVelocity.x = Mathf.MoveTowards(localVelocity.x, 0, sideAcceleration * Time.deltaTime * Player.playerMods.sideAccelerationMod);
        }

        if (moveVector2.y == 0)
        {
            localVelocity.z = Mathf.MoveTowards(localVelocity.z, 0, acceleration * Time.deltaTime * Player.playerMods.accelerationMod);

            if (backEngineSound.volume > 0)
            {
                backEngineSound.volume -= 1f * Time.deltaTime;
            }

            if (forwardEngineSound.volume > 0)
            {
                forwardEngineSound.volume -= 1f * Time.deltaTime;
            }
        }

        if (Mathf.Abs(localVelocity.x) > maxSideAcceleration * Player.playerMods.maxSideAccelerationMod)
        {
            localVelocity.x = maxSideAcceleration * Player.playerMods.maxSideAccelerationMod * Mathf.Sign(localVelocity.x);
        }

        if (Mathf.Abs(localVelocity.z) > maxVelocity * Player.playerMods.maxVelocityMod)
        {
            localVelocity.z = maxVelocity * Player.playerMods.maxVelocityMod * Mathf.Sign(localVelocity.z);
        }

        rigidbody.linearVelocity = transform.TransformDirection(localVelocity);
    }
}
