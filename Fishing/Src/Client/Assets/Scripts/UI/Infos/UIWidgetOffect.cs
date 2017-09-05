using UnityEngine;

/// <summary>
/// Simple example script of how a button can be offset visibly when the mouse hovers over it or it gets pressed.
/// </summary>
[AddComponentMenu("NGUI/Interaction/UI Widget Offset")]
public class UIWidgetOffect : MonoBehaviour {

    public enum Trigger {
        OnClick,
        OnMouseOver,
        OnMouseOut,
        OnPress,
        OnRelease,
        OnDoubleClick,
    }

    public enum eMoveTrack
    {
        MoveSingleDir,
        MoveCycleDir,
    }

    public Trigger trigger = Trigger.OnClick;
    public Transform tweenTarget;
    public GameObject eventReceiver;
    public string eventFunctionName = null;
    public Vector3 toPos = Vector3.zero;
    public float duration = 0.2f;
    public UIAtlas toggleAtlas;
    public string toggleSpriteName;
    public Vector3 toggleRotation;
    public GameObject toggleShowWidget_H;
    public GameObject toggleShowWidget_V;
    public int audioID = 0;
    public Trigger audioTrigger = Trigger.OnClick;
    public eMoveTrack eMT;

    private Vector3 mPos;

    private UIAtlas mToggleAtlas;
    private string mToggleSpriteName;
    private UISprite mToggleSprite;
    private bool mIsTo = true;
    private bool mHighlighted = false;
    private float startMoveTime = 0f;

    private void Start() {

        if (tweenTarget == null) {
            tweenTarget = transform;
        }

        mPos = tweenTarget.localPosition;
        Transform back = gameObject.transform.FindChild("Background");
        if (back) {
            mToggleSprite = back.gameObject.GetComponent<UISprite>();
            if (mToggleSprite) {
                mToggleAtlas = mToggleSprite.atlas;
                mToggleSpriteName = mToggleSprite.spriteName;
            }
        }

        if (toggleShowWidget_H)
            toggleShowWidget_H.SetActive(false);

        if (toggleShowWidget_V)
            toggleShowWidget_V.SetActive(false);
    }

    public bool TrigMove(UITweener.Method m = UITweener.Method.EaseInOut) {
        if (mToggleSprite) {
            if (!mIsTo) {
                mToggleSprite.atlas = mToggleAtlas;
                mToggleSprite.spriteName = mToggleSpriteName;

                if (toggleRotation != Vector3.zero) {
                    mToggleSprite.transform.localEulerAngles = Vector3.zero;
                }

                if (toggleShowWidget_H)
                    toggleShowWidget_H.SetActive(false);

                if (toggleShowWidget_V)
                    toggleShowWidget_V.SetActive(false);
            }
        }

        TweenPosition.Begin(tweenTarget.gameObject, duration, mIsTo ? mPos + toPos : mPos).method = m;
        if (eventReceiver) {
            eventReceiver.SendMessage(eventFunctionName, mIsTo, SendMessageOptions.DontRequireReceiver);
        }

        mIsTo = !mIsTo;
        startMoveTime = duration;
        return (!mIsTo);
    }

    public bool IsOrigPos()
    {
        return mIsTo;
    }

    public void Reset ()
    {
        mIsTo = true;
    }

    void Update() {
        if (startMoveTime > 0f && mToggleSprite != null) {
            startMoveTime -= Time.deltaTime;
            if (startMoveTime <= 0f) {
                if (!mIsTo) {
                    if (toggleAtlas != null && toggleSpriteName != "") {
                        mToggleSprite.atlas = toggleAtlas;
                        mToggleSprite.spriteName = toggleSpriteName;
                    }


                    if (toggleRotation != Vector3.zero) {
                        mToggleSprite.transform.localEulerAngles = toggleRotation;
                    }

                    if (toggleShowWidget_H)
                        toggleShowWidget_H.SetActive(true);

                    if (toggleShowWidget_V)
                        toggleShowWidget_V.SetActive(true);
                }
            }
        }
    }


    private void OnEnable() {
        if (mHighlighted) OnHover(UICamera.IsHighlighted(gameObject));
    }

    private void OnHover(bool isOver) {
        if (enabled) {
            if (((isOver && trigger == Trigger.OnMouseOver) ||
                 (!isOver && trigger == Trigger.OnMouseOut))) TrigMove();
            mHighlighted = isOver;
        }
    }

    private void OnPress(bool isPressed) {
        if (enabled) {
            if (isPressed && audioTrigger == Trigger.OnPress) {
                SendAudio();
            }

            if (((isPressed && trigger == Trigger.OnPress) ||
                 (!isPressed && trigger == Trigger.OnRelease))) TrigMove();
        }
    }

    private void OnClick() {

        if (enabled) {
            if (audioTrigger == Trigger.OnClick) {
                SendAudio();
            }

            if (trigger == Trigger.OnClick)
                TrigMove();
        }
    }

    private void OnDoubleClick() {
        if (enabled && trigger == Trigger.OnDoubleClick) TrigMove();
    }

    private void SendAudio() {
        if (audioID <= 0) return;

        gameObject.SendMessageUpwards("PlayButtonPressAudio", audioID, SendMessageOptions.DontRequireReceiver);
    }
}
