/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Security.Cryptography;
using Aliyun.OSS.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Aliyun.OSS.Samples
{
    /// <summary>
    /// Sample for getting object.
    /// </summary>
    public static class GetObjectSample
    {
        static string accessKeyId = Config.AccessKeyId;
        static string accessKeySecret = Config.AccessKeySecret;
        static string endpoint = Config.Endpoint;
        static OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret);
    

        static string key = "GetObjectSample";
        static string fileToUpload = Config.FileToUpload;
        static string dirToDownload = Config.DirToDownload;

        static AutoResetEvent _event = new AutoResetEvent(false);

        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始   
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// 将 byte[] 转成 Stream  

        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
        /// <summary>
        /// 直接从云服务器上取图片
        /// </summary>
        /// <param name="key"></param>
        public static void GetObject(string key,Image img)
        {
            try
            {
                if (client.DoesObjectExist(Config.bucketName, key))
                {
                    var result = client.GetObject(Config.bucketName, key);

                    using (var requestStream = result.Content)
                    {

                        using (var fs = File.Open(Config.DirToDownload+key.GetHashCode(), FileMode.OpenOrCreate))
                        {
                            int length = 4 * 1024;
                            var buf = new byte[length];
                            do
                            {
                                length = requestStream.Read(buf, 0, length);
                                fs.Write(buf, 0, length);
                            } while (length != 0);
                            AsyncImageDownload.Instance.SetAsyncImage(key,img);
                        }
                    }

                    Debug.Log("Get object succeeded");
                }

            }
            catch (OssException ex)
            {
                Debug.Log("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}" +
                    ex.ErrorCode + ex.Message + ex.RequestId + ex.HostId);
            }
            catch (Exception ex)
            {
                Debug.Log("-------Failed with error info: {0}" + ex.Message);
            }
        }

        public static void GetObjectByRequest(string bucketName)
        {
            try
            {
                client.PutObject(bucketName, key, fileToUpload);

                var request = new GetObjectRequest(bucketName, key);
                request.SetRange(0, 100);

                var result = client.GetObject(request);

                Debug.Log("Get object succeeded, length:{0}" + result.Metadata.ContentLength);
            }
            catch (OssException ex)
            {
                Debug.Log("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}" +
                    ex.ErrorCode + ex.Message + ex.RequestId + ex.HostId);
            }
            catch (Exception ex)
            {
                Debug.Log("Failed with error info: {0}" + ex.Message);
            }
        }

        public static void AsyncGetObject(string bucketName)
        {
            const string key = "AsyncGetObject";
            try
            {
                client.PutObject(bucketName, key, fileToUpload);

                string result = "Notice user: put object finish";
                client.BeginGetObject(bucketName, key, GetObjectCallback, result.Clone());

                _event.WaitOne();
            }
            catch (OssException ex)
            {
                Debug.Log("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}" +
                    ex.ErrorCode + ex.Message + ex.RequestId + ex.HostId);
            }
            catch (Exception ex)
            {
                Debug.Log("Failed with error info: {0}" + ex.Message);
            }
        }

        private static void GetObjectCallback(IAsyncResult ar)
        {
            try
            {
                var result = client.EndGetObject(ar);

                using (var requestStream = result.Content)
                {
                    using (var fs = File.Open(dirToDownload + "/sample2.data", FileMode.OpenOrCreate))
                    {
                        int length = 4 * 1024;
                        var buf = new byte[length];
                        do
                        {
                            length = requestStream.Read(buf, 0, length);
                            fs.Write(buf, 0, length);
                        } while (length != 0);
                    }
                }

                Debug.Log(ar.AsyncState as string);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
            finally
            {
                _event.Set();
            }
        }
    }
}
