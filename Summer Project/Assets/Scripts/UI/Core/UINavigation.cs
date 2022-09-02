using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavigation : MonoBehaviour
{
    public UIView Current
    {
        get
        {
            UIView view = null;
            _viewStack.TryPeek(out view);
            return view;
        }
    }

    public UIView RootView;

    private Stack<UIView> _viewStack = new();

    private void Awake()
    {
        if (RootView != null)
        {
            this.Push(RootView);
        }
    }

    public void Show()
    {
        if (_viewStack.TryPeek(out var topView))
        {
            topView.Show();
        }
    }

    public void Hide()
    {
        if (_viewStack.TryPeek(out var topView))
        {
            topView.Hide();
        }
    }

    public UIView Push(string viewName)
    {
        var views = Resources.FindObjectsOfTypeAll<UIView>();

        foreach (var view in views)
        {
            if (view.name == viewName)
            {
                this.Push(view);
                return view;
            }
        }

        Debug.LogWarning($"A UIView named {viewName} does not exist in the scene.");
        return null;
    }

    public void Push(UIView view)
    {
        if (_viewStack.TryPeek(out var topView))
        {
            topView.Hide();
        }

        view.Navigation = this;

        _viewStack.Push(view);
        view.Show();
    }

    public UIView Pop()
    {
        UIView popView = null;

        if (_viewStack.TryPop(out popView))
        {
            popView.Hide();
        }

        if (_viewStack.TryPeek(out var topView))
        {
            topView.Show();
        }

        return popView;
    }

    public UIView PopTo(string viewName)
    {
        UIView popView = null;
        int count = _viewStack.Count;

        while (count > 0)
        {
            popView = _viewStack.Pop();
            count--;

            if (popView.name == viewName)
            {
                break;
            }

            popView.Hide();
        }

        if (_viewStack.TryPeek(out var topView))
        {
            topView.Show();
        }

        return popView;
    }

    public UIView PopToRoot()
    {
        UIView popView = null;
        int count = _viewStack.Count;

        while (count > 1)
        {
            popView = _viewStack.Pop();
            popView.Hide();
            count --;
        }

        if (_viewStack.TryPeek(out var topView))
        {
            topView.Show();
        }

        return popView;
    }
}
