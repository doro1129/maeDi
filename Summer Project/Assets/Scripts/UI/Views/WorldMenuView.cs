using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMenuView : UIView
{
    public RectTransform ContentRect;

    public override void Reset()
    {
      ContentRect.anchoredPosition = new Vector2(0, ContentRect.anchoredPosition.y);
    }

    public void OnCloseButtonClick()
    {
      Navigation.Pop();
    }
}
