using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace Common
{


    public class DeviceControl
    {
        
          public static int GetLoginUserId(string EquIp, string EquName, string EquPass, int EquPort)
          {

            int ireturn = -1;

            bool m_bInitSDK = CHCNetSDK.NET_DVR_Init();
            
            if (m_bInitSDK == false)
            {
                Console.Write("NET_DVR_Init error!");
                return ireturn;
            }



            CHCNetSDK.NET_DVR_NETCFG_V50 struNetCfg = new CHCNetSDK.NET_DVR_NETCFG_V50();
            struNetCfg.Init();
            CHCNetSDK.NET_DVR_DEVICECFG_V40 struDevCfg = new CHCNetSDK.NET_DVR_DEVICECFG_V40();
            struDevCfg.sDVRName = new byte[CHCNetSDK.NAME_LEN];
            struDevCfg.sSerialNumber = new byte[CHCNetSDK.SERIALNO_LEN];
            struDevCfg.byDevTypeName = new byte[CHCNetSDK.DEV_TYPE_NAME_LEN];
            CHCNetSDK.NET_DVR_USER_LOGIN_INFO struLoginInfo = new CHCNetSDK.NET_DVR_USER_LOGIN_INFO();
            CHCNetSDK.NET_DVR_DEVICEINFO_V40 struDeviceInfoV40 = new CHCNetSDK.NET_DVR_DEVICEINFO_V40();
            struDeviceInfoV40.struDeviceV30.sSerialNumber = new byte[CHCNetSDK.SERIALNO_LEN];
            uint dwReturned = 0;
            int lUserID = -1;

        //    struLoginInfo.bUseAsynLogin = AysnLoginFlag;
        //    struLoginInfo.cbLoginResult = new CHCNetSDK.LoginResultCallBack(AsynLoginMsgCallback);
            struLoginInfo.byLoginMode = 2;
            struLoginInfo.sDeviceAddress = EquIp;
            struLoginInfo.sUserName = EquName;
            struLoginInfo.sPassword = EquPass;
            struLoginInfo.wPort = (ushort)EquPort;
            lUserID = CHCNetSDK.NET_DVR_Login_V40(ref struLoginInfo, ref struDeviceInfoV40);
            if (lUserID > 0)
            {
                ireturn = lUserID;

            }
            return ireturn;
        }


        //// Asynchronous callback function
        //public void AsynLoginMsgCallback(Int32 lUserID, UInt32 dwResult, ref CHCNetSDK.NET_DVR_DEVICEINFO_V30 lpDeviceInfo, IntPtr pUser)
        //{

        //    if (dwResult == 1)
        //    {

        //        m_struDeviceInfo = lpDeviceInfo;

        //    }

        //    m_AysnLoginResult = dwResult;
        //}
    
    }
}