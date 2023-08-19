using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindow : BaseWindow
{
    [SerializeField] private UnitBase _unit;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _content;
    [SerializeField] private Button _equipButton;
    [SerializeField] private Button _dropButton;

    [SerializeField] private Image _uniformImage;
    [SerializeField] private Image _helmetImage;

    [SerializeField] private Button _helmetButton;
    [SerializeField] private Button _uniformButton;

    [SerializeField] private TMPro.TextMeshProUGUI _title;
    [SerializeField] private TMPro.TextMeshProUGUI _description;

    [SerializeField] private GameObject _prefab;
    private float _slotSizeDeltaY;

    private readonly List<ItemSlot> _slots = new();
    private ItemSlot _currenSlot;

    private protected override void Awake()
    {
        _closeButton.onClick.RemoveAllListeners();
        _dropButton.onClick.RemoveAllListeners();
        _equipButton.onClick.RemoveAllListeners();
        _helmetButton.onClick.RemoveAllListeners();
        _uniformButton.onClick.RemoveAllListeners();
        _closeButton.onClick.AddListener(Hide);
        _dropButton.onClick.AddListener(OnDropItem);
        _equipButton.onClick.AddListener(OnEquipItem);
        _helmetButton.onClick.AddListener(OnTakeOffHelmet);
        _uniformButton.onClick.AddListener(OnTakeOffUniform);
        _slotSizeDeltaY = ((RectTransform)_prefab.transform).sizeDelta.y;

        EventsBus.Subscribe<OnSelectSlot>(OnSelectSlot);

        base.Awake();
    }

    public override void Initialize()
    {
        Refresh();
    }

    public override void Hide()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i] != null && _slots[i].gameObject != null)
                Destroy(_slots[i].gameObject);
        }
        _slots.Clear();
        base.Hide();
    }

    private protected override void OnDestroy()
    {
        _closeButton.onClick.RemoveAllListeners();
        _dropButton.onClick.RemoveAllListeners();
        _equipButton.onClick.RemoveAllListeners();
        _helmetButton.onClick.RemoveAllListeners();
        _uniformButton.onClick.RemoveAllListeners();
        EventsBus.Unsubscribe<OnSelectSlot>(OnSelectSlot);
        base.OnDestroy();
    }

    private void Refresh()
    {
        _title.text = string.Empty;
        _description.text = string.Empty;
        _dropButton.gameObject.SetActive(false);
        _equipButton.gameObject.SetActive(false);

        if (_unit.Uniform != null && !string.IsNullOrEmpty(_unit.Uniform.Id))
        {
            _uniformImage.gameObject.SetActive(true);
            _uniformImage.sprite = ResourceManager.Instance.GetSpriteByID(_unit.Uniform.IconId);
        }
        else
        {
            _uniformImage.gameObject.SetActive(false);
            _uniformImage.sprite = null;
        }

        if (_unit.Helmet != null && !string.IsNullOrEmpty(_unit.Helmet.Id))
        {
            _helmetImage.gameObject.SetActive(true);
            _helmetImage.sprite = ResourceManager.Instance.GetSpriteByID(_unit.Helmet.IconId);
        }
        else
        {
            _helmetImage.gameObject.SetActive(false);
            _helmetImage.sprite = null;
        }

        _currenSlot = null;
        int childCount = _content.childCount;

        // Clear the existing slots
        foreach (var slot in _slots)
        {
            Destroy(slot.gameObject);
        }

        _slots.Clear();

        for (int i = 0; i < _unit.Items.Count; i++)
        {
            if (_unit.Items[i] == null || string.IsNullOrEmpty(_unit.Items[i].Id))
            {
                Debug.Log($"________The {_unit.Items[i]} is null. Continue");
                continue;
            }

            GameObject slotGO = Instantiate(_prefab, _content);
            ItemSlot slot = slotGO.GetComponent<ItemSlot>();
            slot.Initialize(_unit.Items[i]);

            // Position slots based on the current index
            slotGO.transform.localPosition = new Vector3(
                slotGO.transform.localPosition.x,
                -_slotSizeDeltaY * i,
                slotGO.transform.localPosition.z
            );

            _slots.Add(slot); // Add the new slot to the list
        }

        _content.sizeDelta = new Vector2(_content.sizeDelta.x, _slotSizeDeltaY * _unit.Items.Count / 2);
        DOTween.Kill(_scrollRect);
        _scrollRect.DOVerticalNormalizedPos(1f, 0.3f);
    }

    private void OnSelectSlot(OnSelectSlot data)
    {
        if (IsHidden)
            return;

        _currenSlot = data.Slot;

        _equipButton.gameObject.SetActive(_currenSlot.Item is Uniform || _currenSlot.Item is Helmet);

        _description.text = _currenSlot.Item.Description;
        _title.text = _currenSlot.Item.Name;

        _dropButton.gameObject.SetActive(true);
    }

    private void OnDropItem()
    {
        //TODO : Remove from amount if amount > 1
        if (_currenSlot == null || _currenSlot.Item == null || string.IsNullOrEmpty(_currenSlot.Item.Id))
            return;
        EventsBus.Publish<OnDropItem>(new OnDropItem { Item = _currenSlot.Item, Owner = _unit, IsDroppedDown = true });
        Refresh();
    }

    private void OnEquipItem()
    {
        if (_currenSlot == null || _currenSlot.Item == null)
            return;

        if (_currenSlot.Item is Helmet helmet)
            _unit.EquipHelmet(helmet);
        else if (_currenSlot.Item is Uniform uniform)
            _unit.EquipUniform(uniform);
        Refresh();
    }

    private void OnTakeOffHelmet()
    {
        if (_unit.Helmet == null)
            return;

        EventsBus.Publish(new OnAddItem { Obtainer = _unit, Item = _unit.Helmet });
        _unit.EquipHelmet(null);
        Refresh();
    }

    private void OnTakeOffUniform()
    {
        if (_unit.Uniform == null)
            return;

        EventsBus.Publish(new OnAddItem { Obtainer = _unit, Item = _unit.Uniform });
        _unit.EquipUniform(null);
        Refresh();
    }
}
