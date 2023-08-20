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
    [SerializeField] private TMPro.TextMeshProUGUI _warningText;

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

    private protected override void OnOpenWindow(OnOpenWindow data)
    {
        if (data.Window != this)
            return;
        _player = data.Player;
        _trader = data.Player.Trader;
            base.OnOpenWindow(data);
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
        _itemImage.gameObject.SetActive(false);

        _playerScrollManager.FillScrollRect(_player);
        _traderScrollManager.FillScrollRect(_trader);

        _playerMoneyText.text = $"${_player.Money}";
        _traderMoneyText.text = $"${_trader.Money}";

        _title.text = string.Empty;
        _description.text = string.Empty;
        _warningText.text = string.Empty;
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
        _warningText.text = string.Empty;
        _itemImage.sprite = ResourceManager.Instance.GetSpriteByID(data.Slot.Item.IconId);
        _itemImage.gameObject.SetActive(true);
    }

    private void OnTryToTrade()
    {
        if (_currentItem == null)
            return;

        if (_isBuying)
        {
            var price = (int)(_currentItem.BasePrice * _trader.TradeModificator);
            if (_player.Money >= price)
            {
                EventsBus.Publish(new OnTradeItem { Buyer = _player, Seller = _trader, Item = _currentItem, Price = price });
            }
            else
            {
                _warningText.text = "You have not enough money";
            }
        }
        else
        {
            var price = (int)(_currentItem.BasePrice * _player.TradeModificator);
            if (_trader.Money >= price)
            {
                EventsBus.Publish(new OnTradeItem { Buyer = _trader, Seller = _player, Item = _currentItem, Price = price });
            }
            else
            {
                _warningText.text = "You have not enough money";
            }
        }
        Refresh();
    }

}
