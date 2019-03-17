using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Game
{
    public  class EncryptManager
    {




        #region  方法一 C#中对字符串加密解密（对称算法）  
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private static string encryptKey=66666666+"";
        /// <summary>  
        /// DES加密字符串  
        /// </summary>  
        /// <param name="encryptString">待加密的字符串</param>  
        /// <param name="encryptKey">加密密钥,要求为8位</param>  
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>  
        public static string EncryptDES(string encryptString)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                cStream.Close();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>  
        /// DES解密字符串  
        /// </summary>  
        /// <param name="decryptString">待解密的字符串</param>  
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>  
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>  
        public static string DecryptDES(string decryptString)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                cStream.Close();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                Debug.Log("catch");
                return decryptString;
            }
        }
        #endregion
        /// <summary>
        /// 获得字符串MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5HashFromString(string str)

        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bytValue, bytHash;

            bytValue = System.Text.Encoding.UTF8.GetBytes(str);

            bytHash = md5.ComputeHash(bytValue);

            md5.Clear();

            string sTemp = "";

            for (int i = 0; i < bytHash.Length; i++)

            {

                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');

            }

            return sTemp.ToUpper();

        }
        public static string GetMD5Hash(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5Hash() fail,error:" + ex.Message);
            }
        }

        public static string GetMD5Hash(byte[] bytedata)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(bytedata);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5Hash() fail,error:" + ex.Message);
            }
        }



        #region MD5不可逆加密  
        //32位加密  
        public string GetMD5_32(string s, string _input_charset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        //16位加密   
        public static string GetMd5_16(string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }
        #endregion
        /// <summary>
        /// 包的md5加密
        /// </summary>
        /// <returns></returns>
        public static string GetSignatureMD5Hash()
        {
            try
            {
                var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var activity = player.GetStatic<AndroidJavaObject>("currentActivity");
                var PackageManager = new AndroidJavaClass("android.content.pm.PackageManager");

                var packageName = activity.Call<string>("getPackageName");

                var GET_SIGNATURES = PackageManager.GetStatic<int>("GET_SIGNATURES");
                var packageManager = activity.Call<AndroidJavaObject>("getPackageManager");
                var packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, GET_SIGNATURES);
                var signatures = packageInfo.Get<AndroidJavaObject[]>("signatures");


                if (signatures != null && signatures.Length > 0)
                {
                    byte[] bytes = signatures[0].Call<byte[]>("toByteArray");
                    string str = getSignValidString(bytes);
                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", str, "test1"));

                    return str;

                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return null;

            }
            return null;

        }


        private static String getSignValidString(byte[] paramArrayOfByte)
        {
            var MessageDigest = new AndroidJavaClass("java.security.MessageDigest");
            var localMessageDigest = MessageDigest.CallStatic<AndroidJavaObject>("getInstance", "MD5");

            localMessageDigest.Call("update", paramArrayOfByte);
            return toHexString(localMessageDigest.Call<byte[]>("digest"));
        }

        public static String toHexString(byte[] paramArrayOfByte)
        {
            if (paramArrayOfByte == null)
            {
                return null;
            }
            StringBuilder localStringBuilder = new StringBuilder(2 * paramArrayOfByte.Length);
            for (int i = 0; ; i++)
            {
                if (i >= paramArrayOfByte.Length)
                {
                    return localStringBuilder.ToString();
                }
                String str = new AndroidJavaClass("java.lang.Integer").CallStatic<String>("toString", 0xFF & paramArrayOfByte[i], 16);
                if (str.Length == 1)
                {
                    str = "0" + str;
                }
                localStringBuilder.Append(str);
            }
        }
    }
}