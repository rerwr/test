
using UnityEngine;

namespace Game
{
    public class PlayerSave
    {
        
        public static bool  HasKey(string key)
        {
            return PlayerPrefs.HasKey(EncryptManager.EncryptDES(key));
        }

        public static void  SetInt(string key,int i)
        {
            PlayerPrefs.SetInt(EncryptManager.EncryptDES(key),i);
        }

        public static void SetString(string key, string i)
        {
            PlayerPrefs.SetString(EncryptManager.EncryptDES(key), EncryptManager.EncryptDES(i));
        }

        public static void SetFloat(string key, float i)
        {
            PlayerPrefs.SetFloat(EncryptManager.EncryptDES(key), i);
            
        }

        public static void Delete(string key)
        {
            PlayerPrefs.DeleteKey(EncryptManager.EncryptDES(key));

        }

        public static float GetFloat(string key)
        {
            return PlayerPrefs.GetFloat(EncryptManager.EncryptDES(key));

        }


        public static string GetString(string key)
        {
            return EncryptManager.DecryptDES( PlayerPrefs.GetString(EncryptManager.EncryptDES(key)));

        }

        public static int GetInt(string key)
        {
            return PlayerPrefs.GetInt(EncryptManager.EncryptDES(key));

        }

       
    }
}