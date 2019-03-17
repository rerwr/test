using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class Director : SingletonMonoBehaviour<Director>
{

    private ToggleX[] buttons;
 

    public void ResetAllbtns()
    {
        buttons = transform.GetComponentsInChildren<ToggleX>();

        for (int index = 0; index < buttons.Length; index++)
        {
            if (buttons[index])
            {
                 buttons[index].gameObject.SetActive(false);

               
            }

        }
    }

    public void StartAnima(float delta)
    {
        MTRunner.Instance.StartRunner(FadeInbtns(delta));
    }

    IEnumerator FadeInbtns(float t)
    {

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i])
            {

                buttons[i].gameObject.SetActive(true);
                buttons[i].FadeIn();
                yield return t;

            }

        }
    }
}
