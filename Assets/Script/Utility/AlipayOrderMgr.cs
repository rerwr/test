using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Framework;
//using com.tenpay.util;
using UnityEngine;

namespace Assets.Script.Utility
{
    public class AlipayOrderMgr:Singleton<AlipayOrderMgr>
    {
//        public static  string RSA_PRIVATE = "MIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQCNF8LeX7sb58ZGVn73HWr4YTjXrRVLuU1RGIgYm0udPcdUBm9gNfETeKBrOhXbq2BYOhLtAypStRgicP5Ax0dRTaSU4tpbFGPnXolf/KLXOHETsVQcqqxLaNXC0tJq5/xyiizXjXWpAaljYaTlJ5mYClsXEIRJ9cnc1o8JdEc0x35lDQNODuxF0l0+g5NUuH+lwK4Hnw8OkRY/aac4sAEaRMCU7PgKyqpgoHa8cVT4KcO71HTosv6oRvhqL7OuDrbw2/ED4PSuxSt4H4zcgHKYJo8Ts8yXfKCKxUsgYVdxu9SXYWgjLGMnaKMlyyrupaNlG6pZC4XUmJmPyeKHlcTXAgMBAAECggEAKfsj9F6vocH48PzTkluidH0ZGLNbXsioBLUz6X5rpUG4iXvQr+Pc81o1ATKrRk1bwWSmNPd4JFvV7omIXWXuBnb/vX0yU19hynoKjhDxsvAMVTuyN0VhNp7e27U2/rBAISST6x8gH0VrTTLEiZqazO5n5Bj6A7eqdJcywDFk7oLsKWRvJpUQs1ThgahT97JSpUDgmxZQym409tkFZyt19fMRndZ0/hIcrS8beqfs6BNpoT2lA4+d57OCdWBqU77s4JvwNX+Ya93zkS08QtseXKfTbHq82R0W9DcCCTf+FEHraob7wFVIJcFq24zHdOt8nhQv/z2GFwWiJ5yAwh9pYQKBgQDDKGPeWf3zZZ1GVRl9ZNlFv1kTKsmlsInsYutt84dF5T7IZG4lz9e6NGOwtZ0NfokjIcxkwoH4dGXoMwjSCmWlXtlFQsL8CZ7stNS1NA42fbjzqFVWucEmppEnbG3eY3hvqwbuDlLlRGbEpmDuOfGf2t29z/E7xA1r88+5uBfNewKBgQC5FG2Wck3lfk1/OK9GuJkyIjkbTziBQDa/gk1vnlCFalziR5mePFN5Iq7Sm0pwVH8nf6CH3NKjspLyy+97+Oy/3dQHXtDOAwLb5IW+MCXCl261djX0olCcldQrQwWZWQqh1Q8J3tpTXS8gi9ASkXsfTHnrOvGTDN8UBOpZI+4xVQKBgHyDyqRVWpfu31e+eBLvQ0ki+twl0p5qcrVlr7xpQoev6kzE9xoc8BEX1/spBNlVQH4v1E79Yxt4eISb+ya4B35pc8qi+/D+2m1AOu6aFe9ia5zabAh3X+sfH0G+BqN7Z54nyrYcYXtyFvelB/c29Rj+9bdjcxCk18NmvcRK1rT9AoGAPv5G9mfdP3kno1+FuMpnyfp/+V2TW9qhR7lv5ce6nE5BYvr/vC3IM3isjB4yzdzUknsMBnIQd4r2HRFwZ3+oBP3ZhtCsvRGlTXaQVtVIZNzp33Vmk+cTDNiqKDddln4J8l70CWYCZVFYEvnDTi6Z+2MqM/gR9PzyiHvLz589GSkCgYA8QmTrfaaZDpQM+qPDmvvfgDqC8nV6UQSbSZ8FPx0m7mHILFhmh3YKsxzxIEYwO1KZ9SgR5cic2WYQFIRSfTckrsj/dxbpDdbXM4Ftx2DbF0P8cxsw92zel3XJpWnCa3+Dv+6a78q0OuQ7bZ0o5EdHjwE6XmU9tap1zlXbK5bSjg==";
//	
//        public string GetAlipayOrder()
//        {
//            string payorder="";
//
//            Debug.LogError(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "GetAlipayOrder", "test0"));
//
//            payorder = Base64.encode(Encoding.UTF8.GetBytes(RSA_PRIVATE));
//            Debug.LogError(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "GetAlipayOrder", payorder));
//
//            return payorder;
//        }

       
//        public static string  APPID = "2017101309286398";S
//	
//
//        public static  string  PID = "2088821130339800";
//
//        public static  string  TARGET_ID = "";
//
//
//        public static  string  RSA2_PRIVATE = "";
//        public static  string  RSA_PRIVATE = "MIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQCNF8LeX7sb58ZGVn73HWr4YTjXrRVLuU1RGIgYm0udPcdUBm9gNfETeKBrOhXbq2BYOhLtAypStRgicP5Ax0dRTaSU4tpbFGPnXolf/KLXOHETsVQcqqxLaNXC0tJq5/xyiizXjXWpAaljYaTlJ5mYClsXEIRJ9cnc1o8JdEc0x35lDQNODuxF0l0+g5NUuH+lwK4Hnw8OkRY/aac4sAEaRMCU7PgKyqpgoHa8cVT4KcO71HTosv6oRvhqL7OuDrbw2/ED4PSuxSt4H4zcgHKYJo8Ts8yXfKCKxUsgYVdxu9SXYWgjLGMnaKMlyyrupaNlG6pZC4XUmJmPyeKHlcTXAgMBAAECggEAKfsj9F6vocH48PzTkluidH0ZGLNbXsioBLUz6X5rpUG4iXvQr+Pc81o1ATKrRk1bwWSmNPd4JFvV7omIXWXuBnb/vX0yU19hynoKjhDxsvAMVTuyN0VhNp7e27U2/rBAISST6x8gH0VrTTLEiZqazO5n5Bj6A7eqdJcywDFk7oLsKWRvJpUQs1ThgahT97JSpUDgmxZQym409tkFZyt19fMRndZ0/hIcrS8beqfs6BNpoT2lA4+d57OCdWBqU77s4JvwNX+Ya93zkS08QtseXKfTbHq82R0W9DcCCTf+FEHraob7wFVIJcFq24zHdOt8nhQv/z2GFwWiJ5yAwh9pYQKBgQDDKGPeWf3zZZ1GVRl9ZNlFv1kTKsmlsInsYutt84dF5T7IZG4lz9e6NGOwtZ0NfokjIcxkwoH4dGXoMwjSCmWlXtlFQsL8CZ7stNS1NA42fbjzqFVWucEmppEnbG3eY3hvqwbuDlLlRGbEpmDuOfGf2t29z/E7xA1r88+5uBfNewKBgQC5FG2Wck3lfk1/OK9GuJkyIjkbTziBQDa/gk1vnlCFalziR5mePFN5Iq7Sm0pwVH8nf6CH3NKjspLyy+97+Oy/3dQHXtDOAwLb5IW+MCXCl261djX0olCcldQrQwWZWQqh1Q8J3tpTXS8gi9ASkXsfTHnrOvGTDN8UBOpZI+4xVQKBgHyDyqRVWpfu31e+eBLvQ0ki+twl0p5qcrVlr7xpQoev6kzE9xoc8BEX1/spBNlVQH4v1E79Yxt4eISb+ya4B35pc8qi+/D+2m1AOu6aFe9ia5zabAh3X+sfH0G+BqN7Z54nyrYcYXtyFvelB/c29Rj+9bdjcxCk18NmvcRK1rT9AoGAPv5G9mfdP3kno1+FuMpnyfp/+V2TW9qhR7lv5ce6nE5BYvr/vC3IM3isjB4yzdzUknsMBnIQd4r2HRFwZ3+oBP3ZhtCsvRGlTXaQVtVIZNzp33Vmk+cTDNiqKDddln4J8l70CWYCZVFYEvnDTi6Z+2MqM/gR9PzyiHvLz589GSkCgYA8QmTrfaaZDpQM+qPDmvvfgDqC8nV6UQSbSZ8FPx0m7mHILFhmh3YKsxzxIEYwO1KZ9SgR5cic2WYQFIRSfTckrsj/dxbpDdbXM4Ftx2DbF0P8cxsw92zel3XJpWnCa3+Dv+6a78q0OuQ7bZ0o5EdHjwE6XmU9tap1zlXbK5bSjg==";
//
//
//
//        private static string  ALGORITHM = "RSA";
//
//        private static string  SIGN_ALGORITHMS = "SHA1WithRSA";
//
//        private static string  SIGN_SHA256RSA_ALGORITHMS = "SHA256WithRSA";
//
//        private static string  DEFAULT_CHARSET = "UTF-8";
//
//        private static string  getAlgorithms(bool rsa2)
//        {
//            return rsa2 ? SIGN_SHA256RSA_ALGORITHMS : SIGN_ALGORITHMS;
//        }
//
//        public static string  sign(string  content, string  privateKey, bool rsa2)
//        {
//            try
//            {
//      
//                PKCS8EncodedKeySpec priPKCS8 = new PKCS8EncodedKeySpec(
//                    Base64.decode(privateKey));
//                KeyFactory keyf = KeyFactory.getInstance(ALGORITHM);
//                PrivateKey priKey = keyf.generatePrivate(priPKCS8);
//
//                java.security.Signature signature = java.security.Signature
//                    .getInstance(getAlgorithms(rsa2));
//
//                signature.initSign(priKey);
//                signature.update(content.getBytes(DEFAULT_CHARSET));
//
//                byte[] signed = signature.sign();
//
//                return Base64.encode(signed);
//            }
//            catch (Exception e)
//            {
//                e.printStackTrace();
//            }
//
//            return null;
//        }
//        /**
// * 构造支付订单参数列表
// * @param pid
// * @param app_id
// * @param target_id
// * @return
// */
//        public static Dictionary<string , string > buildOrderParamMap(string  app_id, bool rsa2)
//        {
//            Dictionary<string , string > keyValues = new Dictionary<string , string >();
//
//            keyValues.Add("app_id", app_id);
//
//            keyValues.Add("biz_content", "{\"timeout_express\":\"30m\",\"product_code\":\"QUICK_MSECURITY_PAY\",\"total_amount\":\"0.01\",\"subject\":\"1\",\"body\":\"我是测试数据\",\"out_trade_no\":\"" + getOutTradeNo() + "\"}");
//
//            keyValues.Add("charset", "utf-8");
//
//            keyValues.Add("method", "alipay.trade.app.pay");
//
//            keyValues.Add("sign_type", rsa2 ? "RSA2" : "RSA");
//
//            keyValues.Add("timestamp", "2016-07-29 16:55:53");
//
//            keyValues.Add("version", "1.0");
//
//            return keyValues;
//        }
//        /**
//* 拼接键值对
//* 
//* @param key
//* @param value
//* @param isEncode
//* @return
//*/
//        private static string  buildKeyValue(string  key,string  value, bool isEncode)
//        {
//            string Builder sb = new string Builder();
//            sb.Append(key);
//            sb.Append("=");
//            if (isEncode)
//            {
//                try
//                {
//                    sb.Append(value);
//                }
//                catch (Exception e)
//                {
//                    sb.Append(value);
//                }
//            }
//            else
//            {
//                sb.Append(value);
//            }
//            return sb.Tostring ();
//        }
//
//
//
//        /**
//     * 对支付参数信息进行签名
//     * 
//     * @param map
//     *            待签名授权信息
//     * 
//     * @return
//     */
//        public static string  GetSign(Dictionary<string , string > map, string  rsaKey, bool rsa2)
//        {
//            List<string > keys = new List<string >(map.Keys);
//            // key排序
//            keys.Sort();
//
//            string Builder authInfo = new string Builder();
//            for (int i = 0; i < keys.Count - 1; i++)
//            {
//                string  key = keys[i];
//                string  value = map[key];
//                authInfo.Append(buildKeyValue(key, value, false));
//                authInfo.Append("&");
//            }
//
//            string  tailKey = keys[keys.Count - 1];
//            string  tailValue = map[tailKey];
//            authInfo.Append(buildKeyValue(tailKey, tailValue, false));
//
//            string  oriSign = SignUtils.sign(authInfo.tostring (), rsaKey, rsa2);
//            string  encodedSign = "";
//
//            try
//            {
//                encodedSign = URLEncoder.encode(oriSign, "UTF-8");
//            }
//            catch (UnsupportedEncodingException e)
//            {
//                e.printStackTrace();
//            }
//            return "sign=" + encodedSign;
//        }
//        /**
// * 构造支付订单参数信息
// * 
// * @param map
// * 支付订单参数
// * @return
// */
//        public static string  buildOrderParam(Dictionary<string , string > map)
//        {
//            List<string > keys = new List<string >(map.Keys);
//
//            string Builder sb = new string Builder();
//            for (int i = 0; i < keys.Count - 1; i++)
//            {
//                string  key = keys[i];
//                string  value = map[key];
//                sb.Append(buildKeyValue(key, value, true));
//                sb.Append("&");
//            }
//
//            string  tailKey = keys[keys.Count - 1];
//            string  tailValue = map[tailKey];
//            sb.Append(buildKeyValue(tailKey, tailValue, true));
//
//            return sb.Tostring ();
//        }
    }

}