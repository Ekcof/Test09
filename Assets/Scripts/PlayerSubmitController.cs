using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Responsible for player's input controls
/// </summary>
public class PlayerSubmitController : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private string actionMapName = "Main";
    [SerializeField] private InventoryWindow _inventoryWindow;
    [SerializeField] private TradeWindow _tradeWindow;
    [SerializeField] private UnitPlayer _player;

    private InputAction iKeyAction;
    private InputAction eKeyAction;
    private InputAction spaceKeyAction;

    private void Awake()
    {
        var keyboardMouseMap = actionAsset.FindActionMap(actionMapName);

        iKeyAction = keyboardMouseMap.FindAction("I");
        eKeyAction = keyboardMouseMap.FindAction("E");
        spaceKeyAction = keyboardMouseMap.FindAction("Space");

        if (iKeyAction != null)
            Debug.Log($"_________I has {iKeyAction.bindings.Count} bindings");
        iKeyAction.Enable();
        iKeyAction.performed += OnInventoryKeyPressed;

        eKeyAction.Enable();
        eKeyAction.performed += OnSubmitKeyPressed;

        spaceKeyAction.Enable();
        spaceKeyAction.performed += OnSpaceKeyPressed;

        keyboardMouseMap.Enable();
        Debug.Log("_______Submit controller has been initialized");
    }

    private void OnInventoryKeyPressed(InputAction.CallbackContext context)
    {
        if (BaseWindow.CurrentWindow is InventoryWindow)
        {
            BaseWindow.CurrentWindow.Hide();
            return;
        }
        else if (BaseWindow.CurrentWindow != null)
            return;

        EventsBus.Publish(new OnOpenWindow { Player = _player, Window = _inventoryWindow });
    }

    private void OnSubmitKeyPressed(InputAction.CallbackContext context)
    {
        Debug.Log("E key pressed");
        if (_player.TryPickUpItem())
            return;

        TryToStartTrade();
    }

    private void OnSpaceKeyPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Space key pressed");
    }

    private void TryToStartTrade()
    {
        if (_player.Trader == null)
            return;

        if (BaseWindow.CurrentWindow != null)
            BaseWindow.CurrentWindow.Hide();

        EventsBus.Publish(new OnOpenWindow { Player = _player, Window = _tradeWindow });

    }
}
