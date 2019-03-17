//
//  ViewController.h
//  AliSDKDemo
//
//  Created by antfin on 17-10-24.
//  Copyright (c) 2017å¹?AntFin. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface APViewController : UIViewController

void _wxpay(char* appid,char* mchid,char* prepayid,char* package,char* nonceStr,char* timeStamp ,char* sign);
void IOSAlipay(char *orderstr);
@end

