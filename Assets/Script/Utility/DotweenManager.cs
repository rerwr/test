using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class DotweenManager : Singleton<DotweenManager>
    {
        public static Tweener DOLocalMoveY(GameObject go)
        {
            go.transform.localPosition = new Vector3(0, 1280, 0);
            go.SetActive(true);
            Tweener t = go.transform.DOLocalMoveY(0, 0.7f, true);
            t.SetDelay(0.3f);
            t.SetEase(Ease.InOutFlash);
            t.SetAutoKill(false);
            t.PlayForward();
            return t;
        }

        public static void DoFade(GameObject BG)
        {
            Image img = BG.GetComponent<Image>();
            img.DOFade(0, 1.5f);

        }
        public static void BeginTouchBranAnim(GameObject go)
        {
            Vector3 v3 = go.transform.localScale;
            Tweener tScale;
            Tweener tColor;
            //此处不同
            tColor = go.transform.GetChild(0).GetComponent<SpriteRenderer>().DOBlendableColor(new Color(0.8f, 0.8f, 0.8f), 0.4f).SetEase(Ease.InBounce).SetAutoKill(false);
            tColor.OnComplete((() =>
            {
                tColor.PlayBackwards();
            }));
            tScale = go.transform.DOScale(new Vector3(v3.x, v3.y + 0.11f, v3.z), 0.3f).SetEase(Ease.Flash).SetAutoKill(false);
            tScale.OnComplete(() =>
            {
                tScale.PlayBackwards();
            });
            tScale.PlayForward();
            tColor.PlayForward();
        }


        public static List<Tweener> BeginTouchBranAnim(GameObject go, float strength)
        {
            Vector3 v3 = go.transform.localScale;
            Tweener tScale;
            Tweener tColor;
            List<Tweener> t = new List<Tweener>();
            //此处不同
            tScale = go.transform.DOScale(new Vector3(v3.x + strength, v3.y + strength, v3.z), 0.3f).SetEase(Ease.Flash).SetAutoKill(false);
            if (!tScale.IsPlaying() && tScale.IsComplete())
            {
                tScale.PlayForward();
            }
            t.Add(tScale);
            if (go.transform.GetComponent<SpriteRenderer>())
            {
                tColor = go.transform.GetComponent<SpriteRenderer>().DOBlendableColor(new Color(0.8f, 0.8f, 0.8f), 0.3f).SetEase(Ease.InBounce).SetAutoKill(false);
                if (!tColor.IsPlaying()&& tColor.IsComplete())
                {
                    tColor.PlayForward();

                }
                t.Add(tColor);

            }
            

            return t;
        }

        public static Tweener BeginTouchActionUI(GameObject go)
        {
            Vector3 v3 = go.transform.localScale;
            Tweener tScale;
            //            Tweener tColor;
            //            //此处不同
            //            tColor = go.transform.GetComponent<Image>().DOBlendableColor(new Color(0.8f, 0.8f, 0.8f), 0.3f).SetEase(Ease.InBounce).SetAutoKill(false);
            //            tColor.OnComplete((() =>
            //            {
            //                tColor.PlayBackwards();
            //            }));
            tScale = go.transform.DOScale(new Vector3(v3.x + 0.2f, v3.y + 0.2f, v3.z), 0.3f).SetEase(Ease.Flash).SetAutoKill(false);

            tScale.PlayForward();
            //            tColor.PlayForward();
            return tScale;
        }
    }
}
