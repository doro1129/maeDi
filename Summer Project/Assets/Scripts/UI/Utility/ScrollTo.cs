using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScrollTo : MonoBehaviour
{
    public float Duration = 0.5f;

    private ScrollRect _scrollRect;

    private void Awake()
    {
        this._scrollRect = this.GetComponent<ScrollRect>();
    }

    public Vector2 GetPosition(RectTransform target)
    {
        Vector2 scrollRectPosition = this._scrollRect.transform.InverseTransformPoint(this._scrollRect.content.position);
        Vector2 targetPosition = this._scrollRect.transform.InverseTransformPoint(target.position);
        Vector2 sizeDelta = target.sizeDelta / 2;
        sizeDelta.x *= -1;

        return scrollRectPosition - targetPosition - sizeDelta;
    }

    public void Scroll(RectTransform target) => this.Scroll(target, this.Duration);

    public void Scroll(RectTransform target, float duration)
    {
        var position = this.GetPosition(target);
        this._scrollRect.content.DOAnchorPos(position, duration);
    }

    public void ScrollY(RectTransform target) => this.ScrollY(target, this.Duration);

    public void ScrollY(RectTransform target, float duration)
    {
        var position = this.GetPosition(target);
        this._scrollRect.content.DOAnchorPosY(position.y, duration);
    }

    public void ScrollX(RectTransform target) => this.ScrollX(target, this.Duration);

    public void ScrollX(RectTransform target, float duration)
    {
        var position = this.GetPosition(target);
        this._scrollRect.content.DOAnchorPosX(position.x, duration);
    }
}
