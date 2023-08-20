
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{

    public float movementSpeed = 10f;
    public float damping = 10f;

    [SerializeField] private InputActionAsset actionAsset;
    private const string actionMapName = "Main";
    [SerializeField] private Direction _direction;
    [SerializeField] private UnitBase _unitBase;
    private InputAction wKeyAction;
    private InputAction sKeyAction;
    private InputAction aKeyAction;
    private InputAction dKeyAction;

    private Rigidbody2D rb;

    private void Start()
    {
        var keyboardMouseMap = actionAsset.FindActionMap(actionMapName);
        wKeyAction = keyboardMouseMap.FindAction("W");
        sKeyAction = keyboardMouseMap.FindAction("S");
        aKeyAction = keyboardMouseMap.FindAction("A");
        dKeyAction = keyboardMouseMap.FindAction("D");

        wKeyAction.Enable();
        sKeyAction.Enable();
        aKeyAction.Enable();
        dKeyAction.Enable();

        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 movement = Vector2.zero;

        // Doesn't allow movement if UI window is active
        if (BaseWindow.CurrentWindow == null)
        {
            float horizontalInput = dKeyAction.ReadValue<float>() - aKeyAction.ReadValue<float>();
            float verticalInput = wKeyAction.ReadValue<float>() - sKeyAction.ReadValue<float>();

            movement = new Vector2(horizontalInput, verticalInput);
            movement.Normalize();
        }

        if (movement != Vector2.zero)
        {
            rb.velocity = movement * movementSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        _unitBase.SetAnimation(movement != Vector2.zero, GetDirection(movement));

        Vector3 currentVelocity = rb.velocity;
        Vector3 oppositeForce = -currentVelocity * damping;
        rb.AddForce(oppositeForce, ForceMode2D.Force);
    }

    private Direction GetDirection(Vector2 movement)
    {
        if (System.Math.Abs(movement.y) > System.Math.Abs(movement.x))
        {
            if (movement.y > 0)
                return Direction.North;
            else
                return Direction.South;
        }
        else if (System.Math.Abs(movement.y) < System.Math.Abs(movement.x))
        {
            if (movement.x > 0)
                return Direction.East;
            else
                return Direction.West;
        }
        return Direction.None;
    }
}
