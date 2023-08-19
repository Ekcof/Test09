using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeWindow : BaseWindow
{
    [SerializeField] private ItemScrollRectManager _playerScrollManager;
    [SerializeField] private ItemScrollRectManager _traderScrollManager;

    [SerializeField] private TMPro.TextMeshProUGUI _title;
    [SerializeField] private TMPro.TextMeshProUGUI _description;

    [SerializeField] private Image _itemImage;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TMPro.TextMeshProUGUI _buyButtonText;

    [SerializeField] private TMPro.TextMeshProUGUI _playerMoneyText;
    [SerializeField] private TMPro.TextMeshProUGUI _traderMoneyText;

    private UnitBase _player;
    private UnitBase _trader;

    private ItemBase _currentItem;
    private bool _isBuying;

    private protected override void Awake()
    {
        _buyButton.onClick.RemoveAllListeners();
        _closeButton.onClick.RemoveAllListeners();
        _closeButton.onClick.AddListener(Hide);
        _buyButton.onClick.AddListener(OnTryToTrade);

        EventsBus.Subscribe<OnSelectSlot>(OnSelectSlot);
        base.Awake();
    }

    private protected override void OnDestroy()
    {
        EventsBus.Subscribe<OnSelectSlot>(OnSelectSlot);
        base.OnDestroy();
    }

    public override void Initialize()
    {
        Refresh();
    }

    public override void Hide()
    {
        base.Hide();
        _playerScrollManager.ClearInventory();
        _traderScrollManager.ClearInventory();
    }

    private void Refresh()
    {
        _currentItem = null;
        _buyButton.gameObject.SetActive(false);

        _playerScrollManager.FillScrollRect(_player);
        _traderScrollManager.FillScrollRect(_trader);

        _playerMoneyText.text = $"${_player.Money}";
        _traderMoneyText.text = $"${_trader.Money}";

        _title.text = string.Empty;
        _description.text = string.Empty;
    }

    private void OnSelectSlot(OnSelectSlot data)
    {
        if (IsHidden)
            return;

        _currentItem = data.Slot.Item;

        if (_currentItem == null || string.IsNullOrEmpty(_currentItem.Id))
        {
            Debug.Log("_______Failed to select item from slot");
            return;
        }

        _buyButton.gameObject.SetActive(true);
        _buyButtonText.text = (data.Owner == _player) ? "Sell Item" : "Buy Item";
        _isBuying = (data.Owner == _trader);

        _description.text = _currentItem.Description;
        _title.text = _currentItem.Name;
        _itemImage.sprite = ResourceManager.Instance.GetSpriteByID(_title.text);
    }

    private void OnTryToTrade()
    {
        if (_currentItem == null)
            return;



    }

}
