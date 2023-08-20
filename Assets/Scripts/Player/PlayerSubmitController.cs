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

    private void Awake()
    {
        var keyboardMouseMap = actionAsset.FindActionMap(actionMapName);

        iKeyAction = keyboardMouseMap.FindAction("I");
        eKeyAction = keyboardMouseMap.FindAction("E");

        iKeyAction.Enable();
        iKeyAction.performed += OnInventoryKeyPressed;

        eKeyAction.Enable();
        eKeyAction.performed += OnSubmitKeyPressed;

        keyboardMouseMap.Enable();
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
        if (_player.TryPickUpItem())
            return;

        TryToStartTrade();
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
