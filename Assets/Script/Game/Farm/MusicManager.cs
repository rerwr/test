using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Game
{
    public class MusicManager : SingletonMonoBehaviour<MusicManager>
    {
        private float volumn = 1.0f;    //音量
        private bool isMute = false;    //是否静音

        //属性
        public float Volumn
        {
            get { return volumn; }
            set
            {
                volumn = value;
                if (BGM_Audiosource != null)
                {
                    BGM_Audiosource.volume = volumn;
                }
            }
        }
        public bool IsMute
        {
            get { return isMute; }
            set
            {
                isMute = value;
                if (BGM_Audiosource != null)
                {
                    BGM_Audiosource.mute = isMute;
                }
            }
        }

        private AudioSource BGM_Audiosource;
        private Dictionary<string, AudioClip> AudioList=new Dictionary<string, AudioClip>();    //存放加载过的音效

        public void PlayBGM(string sfxName)  //播放背景音乐
        {
         
            ResourceMgr.Instance.LoadResource(sfxName,(res,succ)=> 
            {
                if (succ)
                {
                    if (BGM_Audiosource == null)
                    {
                        GameObject soundObj = new GameObject("BGM");
                        soundObj.transform.position = new Vector3(0, 0, -136.5f);
                        BGM_Audiosource = soundObj.AddComponent<AudioSource>();
                        BGM_Audiosource.minDistance = 10.0f;
                        BGM_Audiosource.maxDistance = 30.0f;
                        BGM_Audiosource.volume = Volumn;
                        BGM_Audiosource.loop = true;//循环播放
                        if (isMute)
                        {
                            BGM_Audiosource.mute = true;

                        }
                    }
                    BGM_Audiosource.clip = (AudioClip)res.UnityObj;
                    BGM_Audiosource.Play();
                }
                else
                {
                    Debug.LogError("加载clip失败："+sfxName);
                }
            });
        }

        public void Playsfx(string sfxName)  //播放音效
        {
            if (isMute) return; //静音不播放音效
            
            if (AudioList.ContainsKey(sfxName) == false)
            {
                AudioClip clip = (AudioClip)Resources.Load(sfxName);
                if (clip != null)
                {
                    AudioList.Add(sfxName,clip);
                }
                else
                {
                    Debug.Log("加载clip失败：" + sfxName);
                    return;
                }
            }

            GameObject soundObj = new GameObject("Sfx"+sfxName);
            soundObj.transform.position = new Vector3(0, 0, -100f);

            AudioSource audioSource = soundObj.AddComponent<AudioSource>();
            audioSource.minDistance = 10.0f;
            audioSource.maxDistance = 30.0f;
            audioSource.volume = Volumn;
            audioSource.clip = AudioList[sfxName];
            audioSource.Play();
            Destroy(soundObj, audioSource.clip.length);
        }
    }

    public class AudioNames
    {
        public const string BGM1 = "AudioClips/BGM1";                //登录界面背景音乐
        public const string BGM2 = "AudioClips/BGM2";                //主场景背景音乐
        public const string CloseBtn = "AudioClips/CloseBtn";        //关闭按钮
        public const string Debug = "AudioClips/Debug";              //除虫
        public const string Fertilizer = "AudioClips/Fertilizer";    //施肥
        public const string GetMoney = "AudioClips/GetMoney";        //获得金币，或其他
        public const string HarvestPlant = "AudioClips/HarvestPlant";//收获一个果实
        public const string OnClick1 = "AudioClips/OnClick1";        //点击1
        public const string OnClick2 = "AudioClips/OnClick2";        //点击2
        public const string OnClick3 = "AudioClips/OnClick3";        //点击3
        public const string OnClick4 = "AudioClips/OnClick4";        //点击4
        public const string OnClick5 = "AudioClips/OnClick5";        //点击5
        public const string OnClick6 = "AudioClips/OnClick6";        //点击6
        public const string ProduceSucc = "AudioClips/ProduceSucc";  //工厂生产成功
        public const string Water = "AudioClips/Water";              //浇水
        public const string Weed = "AudioClips/Weed";                //除草
        public const string OpenFactory = "AudioClips/OpenFactory";  //打开工厂
        public const string OpenAudio = "AudioClips/OpenAudio";
        public const string dog = "AudioClips/DOG";
        
    }
}
