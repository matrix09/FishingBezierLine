using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
namespace Assets.Scripts.Managers.Controller
{

    public class NpcControl : MonoBehaviour
    {
        BaseActor ba;
        // Use this for initialization
        void Start()
        {
            ba = gameObject.GetComponent<BaseActor>();
        }

        // Update is called once per frame
        void Update()
        {
            ba.Anim.SetFloat(NameHashHelper.SpeedId, 1f);
        }
    }

}

