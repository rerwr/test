/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.IO;
using System.Threading;
using Aliyun.OSS.Common;
using System.Text;
using Aliyun.OSS.Util;
using UnityEngine;

namespace Aliyun.OSS.Samples
{
    /// <summary>
    /// Sample for putting object.
    /// </summary>
    public static class PutObjectSample
    {
        static string accessKeyId = Config.AccessKeyId;
        static string accessKeySecret = Config.AccessKeySecret;
        static string endpoint = Config.Endpoint;
        static OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret);

        static string fileToUpload = Config.FileToUpload;

        static AutoResetEvent _event = new AutoResetEvent(false);

        /// <summary>
        /// sample for put object to oss
        /// </summary>
        public static void PutObject(string bucketName)
        {
            PutObjectFromFile(bucketName);



            PutObjectWithDir(bucketName);

            PutObjectWithMd5(bucketName);

            PutObjectWithHeader(bucketName);

            AsyncPutObject(bucketName);
        }

        public static void PutObjectFromFile(string bucketName)
        {
            const string key = "PutObjectFromFile";
            try
            {
                client.PutObject(bucketName, key, fileToUpload);
                Debug.LogError("Put object:{0} succeeded"+ key);
            }
            catch (OssException ex)
            {
                Debug.LogError("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}"+
                    ex.ErrorCode+ ex.Message+ ex.RequestId+ ex.HostId);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed with error info: {0}"+ex.Message);
            }
        }

        public static void PutObjectFromString(string uid,byte[] bytes)
        {
            try
            {
             
                var stream = new MemoryStream(bytes);
                client.PutObject(Config.bucketName, uid, stream);
                Debug.LogError("Put object:{0} succeeded"+ uid);
            }
            catch (OssException ex)
            {
                Debug.LogError("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}"+
                    ex.ErrorCode+ ex.Message+ ex.RequestId+ex.HostId);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed with error info: {0}"+ ex.Message);
            }
        }

        public static void PutObjectWithDir(string uid)
        {
            const string key = "folder2/sub_folder/10.jpg";

            try
            {
                client.PutObject(Config.bucketName, key, fileToUpload);
                Debug.LogError("Put object:{0} succeeded"+key);
            }
            catch (OssException ex)
            {
                Debug.LogError("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}"+
                    ex.ErrorCode+ ex.Message+ex.RequestId+ ex.HostId);
                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "test", "test1"));

            }
            catch (Exception ex)
            {
                Debug.LogError("Failed with error info: {0}"+ ex.Message);

                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "test", "test1"));

            }
        }

        public static void PutObjectWithMd5(string bucketName)
        {
            const string key = "PutObjectWithMd5";

            string md5;
            using (var fs = File.Open(fileToUpload, FileMode.Open))
            {
                md5 = OssUtils.ComputeContentMd5(fs, fs.Length);
            }

            var meta = new ObjectMetadata() { ContentMd5 = md5 };
            try
            {
                client.PutObject(bucketName, key, fileToUpload, meta);

                Debug.LogError("Put object:{0} succeeded"+ key);
            }
            catch (OssException ex)
            {
                Debug.LogError(@"Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}+
                    ex.ErrorCode+ ex.Message+_ ex.RequestId+ ex.HostId");
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed with error info: {0}"+ex.Message);
            }
        }

        public static void PutObjectWithHeader(string bucketName)
        {
            const string key = "PutObjectWithHeader";
            try
            {
                using (var content = File.Open(fileToUpload, FileMode.Open))
                {
                    var metadata = new ObjectMetadata();                    
                    metadata.ContentLength = content.Length;

                    metadata.UserMetadata.Add("github-account", "qiyuewuyi");

                    client.PutObject(bucketName, key, content, metadata);

                    Debug.LogError("Put object:{0} succeeded"+key);
                }
            }
            catch (OssException ex)
            {
                Debug.LogError("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}"+
                    ex.ErrorCode+ex.Message+ ex.RequestId+ex.HostId);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed with error info: {0}"+ ex.Message);
            }
        }

        public static void AsyncPutObject(string bucketName)
        {
            const string key = "AsyncPutObject";
            try
            {
                // 1. put object to specified output stream
                using (var fs = File.Open(fileToUpload, FileMode.Open))
                {
                    var metadata = new ObjectMetadata();
                    metadata.UserMetadata.Add("mykey1", "myval1");
                    metadata.UserMetadata.Add("mykey2", "myval2");
                    metadata.CacheControl = "No-Cache";
                    metadata.ContentType = "text/html";

                    string result = "Notice user: put object finish";
                    client.BeginPutObject(bucketName, key, fs, metadata, PutObjectCallback, result.ToCharArray());

                    _event.WaitOne();
                }
            }
            catch (OssException ex)
            {
                Debug.LogError("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}"+
                    ex.ErrorCode+ex.Message+ ex.RequestId+ex.HostId);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed with error info: {0}"+ ex.Message);
            }
        }

        private static void PutObjectCallback(IAsyncResult ar)
        {
            try
            {
                client.EndPutObject(ar);

                Debug.LogError(ar.AsyncState as string);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            finally
            {
                _event.Set();
            }
        }
    }
}
