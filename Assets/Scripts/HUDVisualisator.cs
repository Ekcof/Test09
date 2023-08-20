using UnityEngine;

public class HUDVisualisator : MonoBehaviour
{
    [SerializeField] private UnitPlayer _player;
    [SerializeField] private GameObject _iconCanPick;
    [SerializeField] private GameObject _iconCanTrade;
    [SerializeField] private TMPro.TextMeshProUGUI _moneyText;

    private bool _canPick;
    private bool _canTrade;

    private void Awake()
    {
        EventsBus.Subscribe<OnChangeMoneyAmount>(OnChangeMoneyAmount);
        EventsBus.Subscribe<OnTogglePickableZone>(OnTogglePickableZone);

    }

    private void OnDestroy()
    {
        EventsBus.Unsubscribe<OnChangeMoneyAmount>(OnChangeMoneyAmount);
        EventsBus.Unsubscribe<OnTogglePickableZone>(OnTogglePickableZone);
    }

    private void OnTogglePickableZone(OnTogglePickableZone data)
    {
        _canPick = data.IsCanPick;
        Refresh();
    }

    private void Refresh()
    { 
            _iconCanPick.SetActive(_canPick);
    }

    private void OnChangeMoneyAmount(OnChangeMoneyAmount data)
    {
        if (data.Player != _player)
            return;

        _moneyText.text = $"${data.Money}";
    }
}
