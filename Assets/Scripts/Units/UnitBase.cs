using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    [SerializeField] private protected Direction _direction = Direction.South;
    [SerializeField] private protected Uniform _uniform;
    [SerializeField] private protected Helmet _helmet;
    [SerializeField] private protected int _money;
    [SerializeField] private float _traderModificator = 1f;
    [SerializeField] private protected readonly List<ItemBase> _items = new List<ItemBase>();
    [SerializeField] private string _loadoutId;
    public int Money => _money;
    public List<ItemBase> Items => _items;
    public Uniform Uniform => _uniform;
    public Helmet Helmet => _helmet;
    public float TradeModificator => _traderModificator;

    [Header("Animated parts of player")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _uniformAnimator;
    [SerializeField] private Animator _helmetAnimator;

    const string _idleN = "IdleN";
    const string _idleE = "IdleE";
    const string _idleS = "IdleS";
    const string _idleW = "IdleW";

    const string _walkN = "WalkN";
    const string _walkE = "WalkE";
    const string _walkS = "WalkS";
    const string _walkW = "WalkW";

    private protected virtual void Awake()
    {
        EventsBus.Subscribe<OnDropItem>(OnDropItem);
        EventsBus.Subscribe<OnAddItem>(OnAddItem);
        EventsBus.Subscribe<OnGetPayment>(OnGetPayment);
        EventsBus.Subscribe<OnTradeItem>(OnTradeItem);

        if (!string.IsNullOrEmpty(_loadoutId))
            ApplyPresetLoadOut();
    }

    private protected virtual void OnDestroy()
    {
        EventsBus.Unsubscribe<OnDropItem>(OnDropItem);
        EventsBus.Unsubscribe<OnAddItem>(OnAddItem);
        EventsBus.Unsubscribe<OnGetPayment>(OnGetPayment);
        EventsBus.Unsubscribe<OnTradeItem>(OnTradeItem);
    }

    public void SetAnimation(bool isWalking, Direction direction)
    {
        string state = null;
        if (direction == Direction.None)
            direction = _direction;

        if (!isWalking)
        {
            switch (direction)
            {
                case Direction.North: state = _idleN; break;
                case Direction.East: state = _idleE; break;
                case Direction.South: state = _idleS; break;
                case Direction.West: state = _idleW; break;
            }
        }
        else
            switch (direction)
            {
                case Direction.North: state = _walkN; break;
                case Direction.East: state = _walkE; break;
                case Direction.South: state = _walkS; break;
                case Direction.West: state = _walkW; break;
            }

        if (!string.IsNullOrEmpty(state))
        {
            _direction = direction;
            _animator.Play(state);
            if (_uniformAnimator != null && _uniformAnimator.runtimeAnimatorController != null)
                _uniformAnimator.Play(state);
            if (_helmetAnimator != null && _helmetAnimator.runtimeAnimatorController != null)
                _helmetAnimator.Play(state);
        }
    }

    private void OnGetPayment(OnGetPayment data)
    {
        if (data.Payee != this)
            return;
        AddMoney(data.Money);
    }

    private protected virtual void AddMoney(int money)
    {
        _money = Math.Clamp(_money + money, 0, int.MaxValue);
    }

    private void OnTradeItem(OnTradeItem data)
    {
        if (data.Item == null || string.IsNullOrEmpty(data.Item.Id))
            return;

        if (data.Buyer == this)
        {
            if (!data.Item.IsStackable)
                _items.Add(data.Item);
            else if (data.Item.Amount > 1)
            {
                int amount = data.IsSellingAll ? data.Item.Amount : 1;

                var existingItem = _items.FirstOrDefault(x => x.Id == data.Item.Id);

                if (existingItem != null)
                    existingItem.AddAmount(amount);
                else
                {
                    var item = ResourceManager.Instance.CreateNewItem(data.Item.Id, amount);
                    _items.Add(item);
                }
            }
            else
                _items.Add(data.Item);
            AddMoney(-data.Price);
        }
        else if (data.Seller == this)
        {
            if (!data.Item.IsStackable)
            {
                _items.Remove(data.Item);
            }
            else if (data.Item.Amount > 1 && !data.IsSellingAll)
            {
                data.Item.AddAmount(-1);
            }
            else
            {
                _items.Remove(data.Item);
            }
            AddMoney(data.Price);
        }
    }

    public void EquipHelmet(Helmet helmet)
    {
        if (helmet == null || string.IsNullOrEmpty(helmet.Id))
        {
            _helmet = null;
            _helmetAnimator.runtimeAnimatorController = null;
            _helmetAnimator.gameObject.SetActive(false);
        }
        else
        {
            if (_helmet != null && !string.IsNullOrEmpty(_helmet.Id))
                _items.Add(_helmet);
            _items.Remove(helmet);
            _helmet = helmet;
            _helmetAnimator.runtimeAnimatorController = helmet.Animator;
            _helmetAnimator.gameObject.SetActive(true);
        }
    }

    public void EquipUniform(Uniform uniform)
    {
        if (uniform == null || string.IsNullOrEmpty(uniform.Id))
        {
            _uniform = null;
            _uniformAnimator.runtimeAnimatorController = null;
            _uniformAnimator.gameObject.SetActive(false);
        }
        else
        {
            if (_uniform != null && !string.IsNullOrEmpty(_uniform.Id))
                _items.Add(_uniform);
            _items.Remove(uniform);
            _uniform = uniform;
            _uniformAnimator.runtimeAnimatorController = uniform.Animator;
            _uniformAnimator.gameObject.SetActive(true);
        }
    }

    private void OnDropItem(OnDropItem data)
    {
        if (data.Owner != this)
            return;

        _items.Remove(data.Item);

        if (data.IsDroppedDown)
        {
            GameObject droppedItem = Instantiate(ResourceManager.Instance.DroppedItemPrefab, transform.position, Quaternion.identity);
            var container = droppedItem.GetComponent<ItemContainer>();
            container.SetItem(data.Item);
        }
        //TODO : Drop a specific amount of items of such type
    }

    private protected void OnAddItem(OnAddItem data)
    {
        if (data.Item == null || string.IsNullOrEmpty(data.Item.Id))
            return;
        if (data.Obtainer == this)
        {
            var existingItem = _items.Find(item => item.Id == data.Item.Id);

            if (existingItem != null && existingItem.IsStackable && data.Item.IsStackable)
                existingItem.SetAmount(existingItem.Amount + data.Item.Amount);
            else
                _items.Add(data.Item);
        }
    }

    private protected void ApplyPresetLoadOut()
    {
        var loadout = ResourceManager.Instance.GetLoadOut(_loadoutId);

        if (loadout.Item1 != null && loadout.Item1.Count > 1)
        {
            for (int i = 0; i < loadout.Item1.Count; i++)
            {
                _items.Add(loadout.Item1[i]);
            }
        }
        if (loadout.Item2 != null && !(string.IsNullOrEmpty(loadout.Item2.Id)))
        {
            EquipHelmet(loadout.Item2);
        }
        if (loadout.Item3 != null && !(string.IsNullOrEmpty(loadout.Item3.Id)))
        {
            EquipUniform(loadout.Item3);
        }
    }
}

public enum Direction
{
    North, East, South, West, None
}