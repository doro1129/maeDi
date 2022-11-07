using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : UINavigation
{
    public static PopupManager Instance = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public UIView OpenPrefab(string prefabName)
    {
        var prefab = Resources.Load<UIView>("Popup/" + prefabName);
        var view = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);

        var rectTransform = view.GetComponent<RectTransform>();
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        this.Push(view);

        return view;
    }

    public void Close()
    {
        var view = this.Pop();
        StartCoroutine("DestroyPopup", view);
    }

    private IEnumerator DestroyPopup(UIView view)
    {
        yield return new WaitUntil(() => view.State == UIView.VisibleState.Disappeared);

        Destroy(view.gameObject);
    }
}
