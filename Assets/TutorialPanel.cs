using DG.Tweening;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    private void Awake()
    {
        DOTween.Kill(_rect);
        _rect.DOScale(1, 0.5f).OnComplete(() =>
         {
             _rect.DOScale(0, 0.3f).SetDelay(5f).OnComplete(() => gameObject.SetActive(false));
         });
    }

}
