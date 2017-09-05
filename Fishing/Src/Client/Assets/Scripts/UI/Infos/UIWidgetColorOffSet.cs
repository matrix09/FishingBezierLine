using UnityEngine;
using System.Collections;

public class UIWidgetColorOffSet : MonoBehaviour
{

    public Color DesColor;
    public float duration;
    private TweenColor tc;

    private Color m_OrigColor;
    private bool mIsTo = true;

    // Use this for initialization
    void Start()
    {
        tc = gameObject.GetComponent<TweenColor>();
        m_OrigColor = tc.gameObject.GetComponent<UISprite>().color;
    }

    public bool TrigMove()
    {
        TweenColor.Begin(tc.gameObject, duration, mIsTo ? DesColor : m_OrigColor).method = UITweener.Method.EaseInOut;
        mIsTo = !mIsTo;
        return mIsTo;
    }

    public bool IsBright()
    {
        return mIsTo;
    }
}
