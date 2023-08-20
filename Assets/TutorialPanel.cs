using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    private void Awake()
    {
        DOTween.Kill(_rect);
        _rect.DOScale(1, 0.3f).OnComplete(() =>
         {
             _rect.DOScale(0, 0.3f).SetDelay(7f).OnComplete(() => gameObject.SetActive(false));
         });
    }

}
