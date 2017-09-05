using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
using Assets.Scripts.ConsoleController.Console;
namespace Assets.Scripts.Managers.Controller
{

    public class PlayerControl : MonoBehaviour
    {


        private void OnTriggerEnter(Collider other)
        {
            int a = 0;
        }

        // Use this for initialization
        void Start()
        {
          
        }

       
        float smooth = 10f;

        float fDeltaPos = 0.05f;

        Quaternion rot = Quaternion.Euler(Vector3.zero);

        Vector3 v;

        Transform t = null;

        // Update is called once per frame
        void Update()
        {
            if(null == t)
            {
                t = GlobeHelper.CurSceneLoader.FightMgr.Major.gameObject.transform;
            }
             
            if(GlobeHelper.CurSceneLoader.FightMgr.FightUI.dir != 0)
            {
                float dir = GlobeHelper.CurSceneLoader.FightMgr.FightUI.dir;

                rot = Quaternion.AngleAxis(dir, Vector3.up);
            }
            else
            {
                rot = t.rotation;
            }

            float speed = 0f;

            if (GlobeHelper.CurSceneLoader.FightMgr.FightUI.BIsMoving)
            {
                speed = 1f;
                v = GlobeHelper.CurSceneLoader.FightMgr.FightUI.vDir * fDeltaPos;
                this.LogFormat("Navi", "v = {0}", v);
               
                if(GlobeHelper.CurSceneLoader.FightMgr.CheckCollider(GlobeHelper.CurSceneLoader.FightMgr.Major.gameObject, v))
                {
                    t.position += new Vector3(
                               v.x,
                               0f,
                               v.y

                   );
                } 
            }
          
            t.rotation = Quaternion.Lerp(t.rotation, rot, smooth * Time.deltaTime);

            GlobeHelper.CurSceneLoader.FightMgr.Major.Anim.SetFloat(NameHashHelper.SpeedId,speed);

        }

    }
}





//if (Input.GetKey("w"))
//{
//    speed = 1f;
//    GlobeHelper.CurSceneLoader.FightMgr.Major.gameObject.transform.position += new Vector3(0f, 0f, 0 + fDeltaZ);
//    rot = Quaternion.Euler(new Vector3(0, 0f, 0f));
//}
//else if(Input.GetKey("s"))
//{
//    speed = 1f;
//    GlobeHelper.CurSceneLoader.FightMgr.Major.gameObject.transform.position += new Vector3(0f, 0f, 0 - fDeltaZ);
//    rot = Quaternion.Euler(new Vector3(0, 180f, 0f));
//}
//else if (Input.GetKey("a"))
//{
//    speed = 1f;
//    GlobeHelper.CurSceneLoader.FightMgr.Major.gameObject.transform.position += new Vector3(0 - fDeltaX, 0f, 0f);
//    rot = Quaternion.Euler(new Vector3(0f, 0 - 90f, 0f));
//}
//else if(Input.GetKey("d"))
//{
//    speed = 1f;
//    GlobeHelper.CurSceneLoader.FightMgr.Major.gameObject.transform.position += new Vector3(0 + fDeltaX, 0f, 0f);
//    rot = Quaternion.Euler(new Vector3(0f, 0f + 90f, 0f));
//}
//else
//{
//    speed = 0f;
//}