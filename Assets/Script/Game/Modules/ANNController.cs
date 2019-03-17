using System.Collections;
using System.Collections.Generic;
using Framework;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class ANNController : MonoBehaviour {

    private RectTransform tr;
    private Text ann_Text;

    public float Speed = 30.0f;
    public float offset = 100;
//    private string init = "欢迎来到圣菲花园，祝您游戏愉快!";
    public void Init()
    {
        tr = transform.Find("Text").GetComponent<RectTransform>();
        ann_Text = tr.GetComponent<Text>();
      
        if (!string.IsNullOrEmpty(AnnouncementModel.Instance.Info))
        {
            ann_Text.text = AnnouncementModel.Instance.Info;
        }
    }

    public void SetAnn(string _ann)
    {
        ann_Text.text = _ann;
        tr.position = new Vector2(150, 0);
        MTRunner.Instance.StartRunner(Wait(30));
    }

    IEnumerator Wait(float time)
    {
        yield return time;
      
        Init();
    }
    void Update()
    {
        if (tr.localPosition.x < -tr.sizeDelta.x - offset-150)
        {
            tr.localPosition = new Vector2(150,0);
        }
        tr.Translate(Vector2.left*Speed*Time.deltaTime);
    }
}
