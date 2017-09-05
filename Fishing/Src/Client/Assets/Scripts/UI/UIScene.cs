//using Assets.Scripts.Modules;
using System.Linq;
using Assets.Scripts.Helpers;
using UnityEngine;
using AttTypeDefine;

public class UIScene : MonoBehaviour
{


    public GameObject OpenUIByName(string sceneName)
    {
        UIManager uiMgr = Helper.Manager<UIManager>();
        if (uiMgr != null)
        {
            return uiMgr.OpenUISceneByName(sceneName);
        }

        return null;
    }

    public void CloseUIByName(string sceneName)
    {
        UIManager uiMgr = Helper.Manager<UIManager>();
        if (uiMgr != null)
        {
            uiMgr.CloseUISceneByName(sceneName);
        }
    }

    public void CloseAllUIScene(string ignorSceneName)
    {
        UIManager uiMgr = Helper.Manager<UIManager>();
        if (uiMgr != null)
        {
            uiMgr.CloseAllUIScene(ignorSceneName);
        }
    }

    public void ChangeButtonBackground(GameObject uiButton, string atlasPath, string spriteName)
    {
        if (uiButton == null)
        {
            return;
        }

        var buttonBackground = uiButton.transform.FindChild("Background");
        if (buttonBackground)
        {
            var uiS = buttonBackground.gameObject.GetComponent<UISprite>();
            if (uiS)
            {
                var uiAtlas = Resources.Load(atlasPath, typeof(UIAtlas)) as UIAtlas;
                if (uiAtlas)
                {
                    uiS.atlas = uiAtlas;
                    uiS.spriteName = spriteName;
                }
            }
        }
    }

    public void ChangeUISpriteAtlas(UISprite sprite, string atlasPath, string spriteName)
    {
        if (sprite == null)
        {
            return;
        }
        var uiAtlas = Resources.Load(atlasPath, typeof(UIAtlas)) as UIAtlas;
        if (uiAtlas)
        {
            sprite.atlas = uiAtlas;
            sprite.spriteName = spriteName;
        }
    }

    public void SetButtonGray(GameObject uiButton, bool bGray)
    {
        if (uiButton == null)
        {
            return;
        }

        var uiB = uiButton.GetComponent<UIButton>();
        if (uiB)
        {
            uiB.isEnabled = !bGray;
        }
        UISprite[] sprites = null;
        UILabel[] labels = null;
        sprites = uiButton.GetComponentsInChildren<UISprite>(true);
        labels = uiButton.GetComponentsInChildren<UILabel>(true);
        if (sprites != null)
        {
            int count_s = sprites.Length;
            if (bGray) for (int i = 0; i < count_s; i++) { sprites[i].color = new Color(0f, 255 / 255f, 255 / 255f, 255 / 255f);
            }
            else for (int i = 0; i < count_s; i++) { sprites[i].color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            }

        }
        if (labels != null)
        {
            int count_l = labels.Length;
            if (bGray)
                for (int i = 0; i < count_l; i++)
                {
                    labels[i].color = new Color(170 / 255f, 170 / 255f, 170 / 255f, 255 / 255f);
                    //labels[i].effectStyle = UILabel.Effect.None;
                }
            else
                for (int i = 0; i < count_l; i++)
                {
                    labels[i].color = new Color(255 / 255f, 240 / 255f, 194 / 255f, 255 / 255f);
                    //labels[i].effectStyle = UILabel.Effect.Outline;
                    //labels[i].effectColor = new Color(29/255f,46/255f,2/255f,255/255f);
                    //labels[i].effectDistance = Vector2.one;
                }
        }
    }
    //todo
    public void ShowMainUI(bool bShow)
    {
        UIManager uiMgr = Helper.Manager<UIManager>();
        if (uiMgr != null)
        {
            //uiMgr.ShowMainUI(bShow);
        }
    }

    public T UI<T>(string nameControl, Transform transRoot = null) where T : MonoBehaviour
    {
        var names = nameControl.Split('.').ToArray().ToList();
        var trans = transRoot ?? transform;

        while (names.Count() != 0)
        {
            var s = names.First();
            var childTrans = trans.FindChild(s);

            if (childTrans == null)
            {
                for (var i = 0; i < trans.childCount; ++i)
                {
                    if (trans.GetChild(i).name.IndexOf("Anchor", System.StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        var ret = UI<T>(string.Join(".", names.ToArray()), trans.GetChild(i));
                        if (ret != null)
                        {
                            return ret;
                        }
                    }
                }
                return null;
            }
            trans = childTrans;
            names.RemoveAt(0);
        }

        if (trans)
        {
            return trans.GetComponent<T>();
        }

        return null;
    }

   

    

}