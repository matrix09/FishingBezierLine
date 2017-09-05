using UnityEngine;
using System.Collections;

public class SplineContainer : MonoBehaviour {

    public static Transform root;

    void OnEnable()
    {
        root = transform;
    }

    void OnDisable()
    {
        root = null;
    }
}
