using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseWindow : MonoBehaviour
{
    [SerializeField] private protected Button _closeButton;
    public static BaseWindow CurrentWindow { get; private set; }
    public bool IsHidden { get; private set; } = true;

    private protected virtual void Awake()
    {
        EventsBus.Subscribe<OnOpenWindow>(OnOpenWindow);
    }

    private protected virtual void OnDestroy()
    {
        EventsBus.Unsubscribe<OnOpenWindow>(OnOpenWindow);
    }

    public virtual void Open()
    {
        CurrentWindow = this;
        IsHidden = false;
        Initialize();
        Debug.Log("______Initialized Inventory");
        DOTween.Kill(transform);
        transform.DOScale(1, 0.3f);
    }

    public virtual void Hide()
    {
        CurrentWindow = null; // TODO: add condition if we transfer from window to window
        IsHidden = true;
        DOTween.Kill(transform);
        transform.DOScale(0, 0f);
    }

    public abstract void Initialize();

    private void OnOpenWindow(OnOpenWindow data)
    {
        if (data.Window != this)
            return;

        Open();
    }
}
