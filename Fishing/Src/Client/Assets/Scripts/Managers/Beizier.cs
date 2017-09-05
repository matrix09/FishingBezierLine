using UnityEngine;
using System.Collections;

public class Beizier : MonoBehaviour {

    /*
    B(t) = (1 - t) * p0 + t * p1;

    when it gets deeper, instead of two points, we talk about 3 points, then you get :
    B(t) = (1 - t) * ((1 - t) * p0 + t * p1) + t *((1 - t) * p1 + t * p2)
    =>
    B(t) = (1 - t) * (1 - t) * p0 + 2 * (1 - t) * t * p1 + t* t * p2;
    
    
    */


    //public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, float t)
    //{
    //    Vector3 v = Vector3.zero;

    //    v = Vector3.Lerp(p0, p1, t);


    //    return v;

    //}

    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {

        Vector3 v = Vector3.zero;

        t = Mathf.Clamp01(t);

        float oneMinusT = 1f - t;

        v = oneMinusT * oneMinusT * p0 + 2 * oneMinusT * t * p1 + t * t * p2;

        return v;

    }

    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {

        Vector3 v = Vector3.zero;

        t = Mathf.Clamp01(t);

        float oneMinusT = 1f - t;

        v = oneMinusT * oneMinusT * oneMinusT * p0 +
            3f * oneMinusT * oneMinusT * t * p1 +
            3f * oneMinusT * t * t * p2 +
            t * t * t * p3;

        return v;

    }



    public static Vector3 GetFirstDerivative (Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {

        Vector3 v = Vector3.zero;

        v = 2f * (1f - t) * (p1 - p0) +
            2f * t * (p2 - p1);


        return v;

    }

    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {

        Vector3 v = Vector3.zero;
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        v = 3f * oneMinusT * oneMinusT * (p1 - p0) +
            6f * oneMinusT * t * (p2 - p1) +
            3f * t * t * (p3 - p2);


        return v;

    }








}
