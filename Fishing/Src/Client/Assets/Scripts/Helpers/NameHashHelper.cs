using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Helpers {
    public class NameHashHelper {

        public static Dictionary<int, string> animationNames = new Dictionary<int, string>();
        private static bool loaded;

        public static readonly int SpeedId = StringToHash("Speed");
        public static readonly int AgularSpeedId = StringToHash("AngularSpeed");
        public static readonly int DirectionId = StringToHash("Direction");
        public static readonly int BirthId = StringToHash("Base Layer.Birth");
        public static readonly int IdleId = StringToHash("Base Layer.Idle");
        public static readonly int LocomotionId = StringToHash("Base Layer.Locomotion");
        public static readonly int InjuredId = StringToHash("Base Layer.Injured");
        public static readonly int KnockBackId = StringToHash("Base Layer.KnockBack");
        public static readonly int FloatId = StringToHash("Base Layer.Float");
        public static readonly int VertigoId = StringToHash("Base Layer.Vertigo");
        public static readonly int InjuredWeightId = StringToHash("InjuredWeight");
        public static readonly int IdleWeightId = StringToHash("IdleWeight");
        public static readonly int ProvokeId = StringToHash("Base Layer.Provoke");
        public static readonly int ReviveId = StringToHash("Base Layer.Revive");
        public static readonly int KeepAttackCheckId = StringToHash("KeepAttackCheck");
        public static readonly int AttackWeaponEnableId = StringToHash("AttackWeaponEnable");
        public static readonly int AttackFaceEnableId = StringToHash("AttackFaceEnable");
        public static readonly int DeathId = StringToHash("Base Layer.Death");
        public static readonly int Death1Id = StringToHash("Base Layer.Death_1");
        public static readonly int IdleStateId = StringToHash("IdleState");

        public static readonly int PlayerSwipeUniqueSkill = StringToHash("Attacks.SwipeUniqueSkill");//狐狸嚎叫
        public static readonly int RushForward = StringToHash("Base Layer.Rush_Front");//玩家向前冲锋.
        public static readonly int RushBack = StringToHash("Base Layer.Rush_Back");//玩家回到原来位置.

        //transition
        public static readonly int idle_rushfront = StringToHash("Idle -> Rush_Front");//idle -> rush front
        public static readonly int attack_rushback = StringToHash("Attack0 -> Rush_Back");//Attack0 -> Rush_Back
        public static readonly int attack1_rushback = StringToHash("Attack1 -> Rush_Back");//Attack0 -> Rush_Back


        public static List<string> names = new List<string>() { "Base Layer", "Idle", "Injured", "Attacks", "Attack", "Attack00", "Attack01" };

        public static bool IsIdle( int stateId ) {
            return stateId == IdleId;
        }

        public static bool IsLocomotion( int stateId ) {
            return stateId == LocomotionId;
        }

        public static int StringToHash( string name ) {
            int id = Animator.StringToHash(name);

            //DebugHelper.Log("AllScript", "StringToHash : " + name + "\t" + id.ToString());

            if ( !animationNames.ContainsKey(id) ) {
                animationNames.Add(id, name);
            }
            return id;
        }

        public static string HashToName( int id ) {

            if ( !loaded ) {
                LoadNames();
                loaded = true;
            }

            if ( animationNames.ContainsKey(id) ) {
                return animationNames[id];
            }

            return "null:" + id;
        }

        //public class JsonNames {
        //    public string[] names;
        //}

        public static List<string> Combination( List<string> items ) {
            var result = new List<string>();
            int len = items.Count;
            int n = 1 << len;
            for ( int i = 1; i < n; i++ ) {
                string str = "";
                for ( int j = 0; j < len; j++ ) {
                    int temp = i;
                    if ( ( temp & ( 1 << j ) ) != 0 ) {
                        str += items[j] + ".";
                    }
                }
                result.Add(str);
            }

            return result;
        }


        private static void LoadNames() {

            List<string> layers = new List<string>();

            foreach ( var name in names.ToArray() ) {
                if ( name.IndexOf("Layer") != -1 ) {
                    layers.Add(name);
                }

                if ( name.EndsWith("s") ) {
                    layers.Add(name);
                }
            }

            layers = Combination(layers);

            foreach ( var layer in layers ) {

                //DebugHelper.Log(string.Format("Layers: {0}", layer));

                foreach ( var name in names.ToArray() ) {
                    names.Add(layer + name);
                    StringToHash(layer + name);
                }
            }

            // add transition name hash
            foreach ( var from in names.ToArray() ) {
                foreach ( var to in names.ToArray() ) {
                    StringToHash(string.Format("{0} -> {1}", from, to));
                }
            }

            //const string filePath = "Attributes/AnimationNames.json";
            //var txt = Resources.Load(filePath) as TextAsset;
            //if(txt != null) {
            //    string result = txt.text;

            //    var objects = SimpleJson.SimpleJson.DeserializeObject<JsonNames>(result);

            //    int count = objects.names.Length;
            //    for(int i = 0; i < count; i++) {
            //        StringToHash(objects.names[i]);
            //    }
            //}
        }
    }
}