//
//  ViewController.m
//  AliSDKDemo
//
//  Created by antfin on 17-10-24.
//  Copyright (c) 2017年 AntFin. All rights reserved.
//


#import <AlipaySDK/AlipaySDK.h>
#include "UnityAppController.h"
#import "WXApi.h"

#if defined (__cplusplus)
extern "C" {
#endif
    
    void _wxpay(char* appid,char* mchid,char* prepayid,char* package,char* nonceStr,char* timeStamp ,char* sign){
        [WXApi registerApp: [NSString stringWithUTF8String:appid]];
        PayReq *request     = [[PayReq alloc] init];
        request.openID      = [NSString stringWithUTF8String:appid];
        
        request.partnerId   = [NSString stringWithUTF8String:mchid];
        request.prepayId    = [NSString stringWithUTF8String:prepayid];
        request.package     =  [NSString stringWithUTF8String:package];
        request.nonceStr    = [NSString stringWithUTF8String:nonceStr];
        
        request.timeStamp   = [[NSString stringWithUTF8String:timeStamp] intValue];
        
        
        request.sign    = [NSString stringWithUTF8String:sign];
        
        [WXApi sendReq:request];
        
    }
#if defined (__cplusplus)
}
#endif


#if defined (__cplusplus)
extern "C"
{
#endif
    void IOSAlipay(char *orderstr){
         NSString *appScheme = @"shengfei";
       
        [[AlipaySDK defaultService] payOrder:[NSString stringWithUTF8String:orderstr] fromScheme:appScheme callback:^(NSDictionary *resultDic)
         {
             printf("--%s", [[resultDic objectForKey:@"resultstatus"] UTF8String]);
     
            UnitySendMessage("Main Camera", "OnIOSAlipayReslut", [[resultDic objectForKey:@"resultstatus"] UTF8String]);
        }];
    
}
#if defined (__cplusplus)
}
#endif



