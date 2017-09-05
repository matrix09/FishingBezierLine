using UnityEngine;
using System.Collections;

public class NpcContainer : MonoBehaviour {

    public static Transform root;

    void OnEnable ()
    {
        root = transform;
    }

    void OnDisable ()
    {
        root = null;
    }

	
}
