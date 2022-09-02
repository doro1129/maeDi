using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullSizeRect : MonoBehaviour
{
    public RectTransform CanvasRect;

    private Vector2 _lastSize = Vector2.zero;

    public void Update()
    {
        var rect = this.CanvasRect.rect;
        this.SetSize(new Vector2(rect.width, rect.height));
    }

    private void SetSize(Vector2 size)
    {
        if (this._lastSize == size)
        {
            return;
        }
        else
        {
            this._lastSize = size;
        }

        var components = GetComponentsInChildren<RectTransform>();

        foreach (var component in components)
        {
            if (component.parent == this.transform)
            {
                component.sizeDelta = size;
            }
        }
    }
}
