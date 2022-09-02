using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UIView : MonoBehaviour
{
    public enum VisibleState
    {
        Appearing,
        Appeared,
        Disappearing,
        Disappeared,
    }

    public bool isReset = true;

    public UINavigation Navigation { get; set; }

    public VisibleState State { get; private set; } = VisibleState.Disappeared;

    public virtual void Reset()
    {
        // Reset Scrollviews
        var scrollRects = GetComponentsInChildren<ScrollRect>();

        foreach (var scrollRect in scrollRects)
        {
            scrollRect.content.anchoredPosition = Vector2.zero;
        }
    }

    public virtual void Show()
    {
        if (State == VisibleState.Appeared || State == VisibleState.Appearing)
        {
            return;
        }

        if (isReset)
        {
            this.Reset();
        }

        var canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0;
        gameObject.SetActive(true);

        State = VisibleState.Appearing;

        Sequence fadeIn = DOTween.Sequence();
        fadeIn.Append(canvasGroup.DOFade(1, 0.5f));
        fadeIn.AppendCallback(delegate { State = VisibleState.Appeared; });
    }

    public virtual void Hide()
    {
        if (State == VisibleState.Disappeared || State == VisibleState.Disappearing)
        {
            return;
        }
        var canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 1;
        // gameObject.SetActive(false);

        State = VisibleState.Disappearing;

        Sequence fadeOut = DOTween.Sequence();
        fadeOut.Append(canvasGroup.DOFade(0, 0.5f));
        fadeOut.AppendCallback(delegate
            {
                State = VisibleState.Disappeared;
                gameObject.SetActive(false);
            }
        );
    }
}
