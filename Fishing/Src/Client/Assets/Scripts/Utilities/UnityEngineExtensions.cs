using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Utilities {
    public static class UnityEngineExtensions {


        #region Extensions of MonoBehaviour

        public delegate void Callback();

        private static IEnumerator ProcessInvokeNextFrame( Callback callback ) {
            yield return null;
            callback();
        }

        public static void InvokeNextFrame( this MonoBehaviour mb, Callback callback ) {
            mb.StartCoroutine(ProcessInvokeNextFrame(callback));
        }

        private static IEnumerator ProcessDownload( WWW www ) {
            yield return www;
        }

        public static WWW Download( this MonoBehaviour mb, string url ) {

            var www = new WWW(url);

            mb.StartCoroutine(ProcessDownload(www));
            while ( !www.isDone ) { }

            return www;
        }

        public static T GetOrAddComponent<T>( this MonoBehaviour mb ) where T : Component {

            if ( mb == null ) {
                return null;
            }

            var comp = mb.gameObject.GetComponent<T>();
            if ( !comp ) {
                comp = mb.gameObject.AddComponent<T>();
            }

            return comp;
        }

        #endregion

        #region Extensions of GameObject

        public static GameObject FindInChildren( this GameObject go, string name, bool force = false ) {

            if ( go == null ) {
                return null;
            }

            if ( force ) {

                for ( int i = 0; i < go.transform.childCount; ++i ) {
                    var child = go.transform.GetChild(i);
                    if ( child && child.name == name ) {
                        return child.gameObject;
                    }

                    if ( child ) {
                        var obj = FindInChildren(child.gameObject, name, true);
                        if ( obj ) {
                            return obj;
                        }
                    }
                }

                return null;
            }

            return ( from x in go.GetComponentsInChildren<Transform>()
                     where x != null && x.gameObject && x.gameObject.name == name
                     select x.gameObject ).FirstOrDefault();
        }

        public static T GetOrAddComponentInChildren<T>( this GameObject mb ) where T : Component {

            if ( mb == null ) {
                return null;
            }

            var comp = mb.gameObject.GetComponentInChildren<T>();
            if ( !comp ) {
                comp = mb.gameObject.AddComponent<T>();
            }

            return comp;
        }

        public static void SetActiveRecursively( this GameObject go, bool state ) {

            if ( go == null ) {
                return;
            }

            go.SetActive(state);

            for ( int i = 0; i < go.transform.childCount; ++i ) {
                var child = go.transform.GetChild(i);
                if ( child != null && child.gameObject != null ) {
                    SetActiveRecursively(child.gameObject, state);
                }
            }
        }

        public static T GetOrAddComponent<T>( this GameObject gb ) where T : Component {

            if ( gb == null ) {
                return null;
            }

            return gb.GetComponent<T>() ?? gb.AddComponent<T>();

        }
        public static void Flashing( this GameObject gb, float timeSeconds, int type = 0 ) {

        }

        #endregion

        public static bool IsValid( this Vector3 vec ) {

            return !( float.IsNaN(vec.x) || float.IsNaN(vec.y) || float.IsNaN(vec.z) );
        }

        public static float Angle( this Vector2 pos1, Vector2 pos2 ) {
            var from = pos2 - pos1;
            var to = new Vector2(0, -1);

            float result = Vector2.Angle(from, to);
            var cross = Vector3.Cross(from, to);

            if ( cross.z > 0 )
                result = 360f - result;

            return result;
        }

        public static void LookAt2D( this Transform trans, Vector3 target ) {

            if ( trans == null ) {
                return;
            }

            var lookAtPoint = target;
            lookAtPoint.y = trans.position.y;
            trans.LookAt(lookAtPoint);
        }

        public static void LookAt2D( this Transform trans, Transform target ) {
            if ( trans == null || target == null ) {
                return;
            }

            LookAt2D(trans, target.position);
        }

        public static float EvaluateInverse( this AnimationCurve curve, float key ) {
            var min = curve.Evaluate(0);
            var max = curve.Evaluate(1);
            var last = min;
            var higher = true;
            if ( min > max ) {
                higher = false;
                min = curve.Evaluate(1);
                max = curve.Evaluate(0);
            }

            while ( min <= max ) {
                var mid = ( min + max ) / 2;

                if ( Math.Abs(last - mid) < float.Epsilon ) {
                    return mid;
                }
                var value = curve.Evaluate(mid);

                if ( Math.Abs(key - value) < float.Epsilon ) {
                    return mid;
                }

                if ( key < value ) {
                    if ( higher ) {
                        max = mid;
                    } else {
                        min = mid;
                    }
                } else {
                    if ( higher ) {
                        min = mid;
                    } else {
                        max = mid;
                    }
                }

                last = mid;
            }

            return float.NaN;
        }

        public static float Distance2D(this Vector3 from, Vector3 to) {
            from.y = 0f;
            to.y = 0f;
            return Vector3.Distance(from, to);
        }
    }
}



