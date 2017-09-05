using UnityEngine;
using System.Collections;
using AttTypeDefine;
namespace Assets.Scripts.Managers.Camera
{

    public class PVPCamera : MonoBehaviour
    {


        public Vector3 vOffSet = Vector3.zero;

        public Vector3 vRot = Vector3.zero;

        public eCamType CamType = eCamType.eType_Follow;

        public Vector3 vSmooth = Vector3.zero;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //检测当前主角是否为空

            switch(CamType)
            {
                case eCamType.eType_Follow:
                    {
                        FollowCamera();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

        }

        Vector3 pos;
        Vector3 rot;
        void FollowCamera ()
        {

            pos = GlobeHelper.CurSceneLoader.FightMgr.Major.transform.position + vOffSet;

            transform.position = Vector3.Lerp(transform.position, pos, vSmooth.x);

            transform.rotation = Quaternion.Euler(vRot);

        }


    }

}
