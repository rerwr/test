using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Game
{
    public class SpritesManager : SingletonMonoBehaviour<SpritesManager>
    {
        public Dictionary<int, Sprite> sprites = new Dictionary<int, Sprite>();

        public void Init()
        {
            Dictionary<int, string> urls = ViewURLConfig.Instance.URLs;

            foreach (var url in urls)
            {
                ResourceMgr.Instance.LoadResource(url.Value,(res,succ)=> 
                {
                    if (succ)
                    {
                        Texture2D t = (Texture2D)res.UnityObj;
                        Sprite sp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), 
                            new Vector2(0.5f, 0.5f));

                        sprites.Add(url.Key,sp);
                    }
                });
            }
        }
      
        public Sprite GetSprite(int id)
        {
            if (sprites.ContainsKey(id))
            {
                return sprites[id];
            }
            else
            {
                Debug.LogError("物品id为：" + id + " 的物品图片不存在！");
                return null;
            }
        }
    }
}