using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScrollTo))]
public class ScrollSnap : MonoBehaviour, IPointerMoveHandler, IPointerUpHandler, IPointerDownHandler
{
    public int Current = 0;
    public GameObject CircleObject;
    public RectTransform CircleGroup;

    private ScrollRect targetScroll;
    private ScrollTo scrollTo;

    private void Awake()
    {
        this.targetScroll = this.GetComponent<ScrollRect>();
        this.scrollTo = this.GetComponent<ScrollTo>();

        this.InitCircleObjects();
    }

    private void Update()
    {
        this.ActiveCircle();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        this.Current = this.GetScrollIndex();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.ScrollIndex(this.Current);
    }

    public void OnPointerDown(PointerEventData eventData)
    {}

    public void Next()
    {
        this.Current++;
        int contentLength = this.GetContentLength();
        
        if (this.Current >= contentLength)
        {
            this.Current = contentLength - 1;
        }

        this.ScrollIndex(this.Current);
    }

    public void Previous()
    {
        this.Current--;
        
        if (this.Current < 0)
        {
            this.Current = 0;
        }

        this.ScrollIndex(this.Current);
    }

    private void ScrollIndex(int index)
    {
        var child = (RectTransform)this.targetScroll.content.GetChild(index);
        this.scrollTo.ScrollX(child);
    }

    private int GetContentLength() => this.targetScroll.content.childCount;

    private int GetCircleLength() => this.CircleGroup.childCount;

    private void InitCircleObjects()
    {
        int diff = this.GetContentLength() - this.GetCircleLength();

        if (diff > 0)
        {
            for (int index = 0; index < diff; index++)
            {
                Instantiate(CircleObject, Vector3.zero, Quaternion.identity, CircleGroup);
            }
        }
        else if (diff < 0)
        {
            diff *= -1;

            for (int index = 0; index < diff; index++)
            {
                var child = this.CircleGroup.GetChild(index);
                Destroy(child.gameObject);
            }
        }
    }

    private int GetScrollIndex()
    {
        int scrollIndex = 0;
        RectTransform content = this.targetScroll.content;
        var contentX = content.anchoredPosition.x * (-1);
        int contentLength = this.GetContentLength();

        for (int index = 0; index < contentLength - 1; index++)
        {
            var child = (RectTransform)content.GetChild(index);
            var childX = child.anchoredPosition.x;

            if (contentX > childX)
            {
                scrollIndex = index + 1;
            }
        }

        return scrollIndex;
    }

    private void ActiveCircle()
    {
        int circleLength = this.GetCircleLength();

        for (int index = 0; index < circleLength; index++)
        {
            var circle = this.CircleGroup.GetChild(index);
            var image = circle.GetComponent<Image>();
            var layoutElement = circle.GetComponent<LayoutElement>();

            if (index == this.Current)
            {
                image.color = Color.white;
                layoutElement.preferredHeight = 20;
                layoutElement.preferredWidth = 20;
            }
            else
            {
                image.color = new Color(0.4705882f, 0.4705882f, 0.470588f);
                layoutElement.preferredHeight = 15;
                layoutElement.preferredWidth = 15;
            }
        }
    }
}
