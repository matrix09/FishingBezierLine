using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIScene_FightUI : MonoBehaviour {

    public GameObject NaviKeyObj;
    public GameObject LimitArea;

    private Vector3 vOrigPos;
    //private Vector3 vPressPos;
    private Transform t;
    private Camera cam;
    private bool bIsKeyDown = false;
    Vector3 newPos = Vector3.zero;
    [HideInInspector]
    public float dir;
    [HideInInspector]
    public Vector3 vDir = Vector3.zero;
    private bool ismoving = false;
    public bool BIsMoving
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                ismoving = dFingerPress.Count > 0 ? true : false;
            }
            else
            {
                ismoving = bIsKeyDown;
            }

            return ismoving;
        }
    }
	// Use this for initialization
	void Start () {
        t = NaviKeyObj.transform;
        vOrigPos = t.position;
     
        NaviKeyObj.SetActive(false);

    }

    Vector3 v;
    float z = 0f;
    float tangant = 0f;
    float fRadius = 0.2f;
    
    void Update () {

        if (null == cam)
        {
            cam = NGUITools.FindCameraForLayer(gameObject.layer);
        }

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            TouchNavigation();
        }
        else
        {
            MouseNavigation();
        }
	}

    void PressObj ()
    {
        NaviKeyObj.SetActive(true);
        dir = 0f;
        t.position = newPos; ;
    }

    void DragObj ()
    {
        vDir = (newPos - vOrigPos).normalized;

        if ((newPos - vOrigPos).magnitude > fRadius)
        {
           
            t.position = vOrigPos + fRadius * vDir;
        }
        else
        {
            t.position = newPos;
        }

        if (newPos.y != vOrigPos.y)
        {
            tangant = (newPos.x - vOrigPos.x) / (newPos.y - vOrigPos.y);
            dir = Mathf.Rad2Deg * Mathf.Atan(tangant);
        }
        else
        {
            if (newPos.x < vOrigPos.x)
            {
                dir = 270f;
            }
            else
            {
                dir = 90f;
            }
        }

        if (newPos.y < vOrigPos.y)
        {
            float temp = 180 - (Mathf.Rad2Deg * Mathf.Atan(0 - tangant));
            dir = temp;
        }
    }

    void ReleaseObj ()
    {
        t.position = vOrigPos;
        NaviKeyObj.SetActive(false);
        dir = 0f;
    }

    void HandleTouchBegin(Touch touch)
    {
        if ((newPos - vOrigPos).magnitude <= fRadius)
        {
            dFingerPress[touch.fingerId] = new Vector2(vOrigPos.x, vOrigPos.y);
            PressObj();
        }
    }

    void ResetData(Touch touch)
    {
        dFingerPress.Clear();
        t.position = vOrigPos;
        NaviKeyObj.SetActive(false);
        dir = 0f;
    }

    void HandleTouchEnd(Touch touch)
    {
        if(dFingerPress.ContainsKey(touch.fingerId))
        {
            dFingerPress.Remove(touch.fingerId);
            ReleaseObj();
        }
    }

    void HandleTouchMoved(Touch touch)
    {
       if(dFingerPress.ContainsKey(touch.fingerId))
        {
            DragObj();
        }
    }

    void MouseNavigation ()
    {
        v = Input.mousePosition;

        z = 0 - cam.transform.position.z;

        newPos = cam.ScreenToWorldPoint(new Vector3(v.x, v.y, z));

        if (bIsKeyDown == false && true == Input.GetMouseButtonDown(0))
        {
            
            if ((newPos - vOrigPos).magnitude <= fRadius)
            {
                bIsKeyDown = true;
                PressObj();
            }
                
        }
        else if (bIsKeyDown == true && true == Input.GetMouseButton(0))
        {
            DragObj();
        }
        else if (bIsKeyDown == true && true == Input.GetMouseButtonUp(0))
        {
            bIsKeyDown = false;
            ReleaseObj();
        }
    }

    private readonly Dictionary<int, Vector2> dFingerPress = new Dictionary<int, Vector2>();
    void TouchNavigation ()
    {

        for (int i = 0; i < Input.touches.Length; i++)
        {
            Touch touch = Input.touches[i];

            v = touch.position;

            z = 0 - cam.transform.position.z;

            newPos = cam.ScreenToWorldPoint(new Vector3(v.x, v.y, z));

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    {
                        HandleTouchBegin(touch);
                        break;
                    }
                case TouchPhase.Moved:
                    {
                        HandleTouchMoved(touch);
                        break;
                    }
                //A finger is touching the screen but hasn't moved since the last frame.
                case TouchPhase.Stationary:
                    {
                        break;
                    }
                //The system cancelled tracking for the touch, as when (for example) the user puts the device to her face 
                //or more than five touches happened simultaneously. This is the final phase of a touch.
                case TouchPhase.Canceled:
                    {
                        ResetData(touch);
                        break;
                    }
                case TouchPhase.Ended:
                    {
                        HandleTouchEnd(touch);
                        break;
                    }
            }//----end switch

        }//----end for cycle
    }

    //bool GetValidAreaTouch ()
    //{
    //    bool isValid = false;
        
    //    z = 0 - cam.transform.position.z;
    //    /*
    //        x : (-length, -length/3)

    //        y : (-1f, -0.3f)
    //    */
    //    Vector3 v = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, z));
    //    if(
    //        v.x > -length && v.x < -length/3f &&
    //        v.y > -1f && v.y < -0.3f
    //      )
    //    {
    //        isValid = true;
    //    }

    //    return isValid;
    //}


    //private float length = (float)Screen.width/(float)Screen.height;
    //void OnDrawGizmos()
    //{
    //    //Gizmos.color = Color.white;
    //    ////Gizmos.DrawSphere(NaviKeyObj.transform.position, explosionRadius);
    //    //Transform tt = LimitArea.transform;

    //    //Gizmos.DrawLine(new Vector3(0 - length, 0 - 0.3f, tt.position.z),
    //    //                new Vector3(0 - length/3, 0 - 0.3f, tt.position.z));

    //    //Gizmos.DrawLine(new Vector3(0 - length, 0 - 1f, tt.position.z),
    //    //                new Vector3(0 - length / 3, 0 - 1f, tt.position.z));

    //    //Gizmos.DrawLine(new Vector3(0 - length, 0 - 0.3f, tt.position.z),
    //    //               new Vector3(0 - length, 0 - 1f, tt.position.z));

    //    //Gizmos.DrawLine(new Vector3(0 - length/3f, 0 - 0.3f, tt.position.z),
    //    //               new Vector3(0 - length/3f, 0 - 1f, tt.position.z));

    //    //Gizmos.DrawWireSphere(tt.position, 0.5f);

    //}

}
