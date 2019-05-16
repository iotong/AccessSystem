using System;
using Microsoft.AspNetCore.Mvc;
using Common;
namespace Main.Areas.Access.Controllers //请在此设置您得命名空间
{
    //HzyAdmin.Areas.Admin.Controllers
    using Aop;
    using Entitys.SysClass;
    using DbFrame;
    //
    using System.Collections;
    using Logic.Access;
    using HzyAdmin.Areas.Admin.Controllers;
    using Microsoft.AspNetCore.Hosting;
    using Entitys;
    using System.Threading;

    [Area("Admin")]
    /// <summary>
    /// Equ管理
    /// </summary>
    public class EquController : BaseController
    {

        private IHostingEnvironment _IHostingEnvironment = null;
        private string _WebRootPath = string.Empty;
        public  string  EquIp, Equlogin, EquPass;
        public  int EquPort;

        public int m_iDeviceIndex = -1;
  
        private int m_iUserID = -1;
        private uint m_AysnLoginResult = 0;
        private bool LoginCallBackFlag = false;
        private bool AysnLoginFlag = false;



        CHCNetSDK.NET_DVR_DEVICEINFO_V30 m_struDeviceInfo;
        CHCNetSDK.NET_DVR_DEVICEINFO_V30 struDeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();

        public EquController(IHostingEnvironment IHostingEnvironment)
        {
            this._IHostingEnvironment = IHostingEnvironment;
            _WebRootPath = this._IHostingEnvironment.WebRootPath;
        }

        public EquLogic _Logic = new EquLogic();

        


        protected override void Init()
        {
            this.MenuID = "A-201";
        }

        public int Connect(Equ model)
        {

            int intCode = -1;

            EquIp = model.IpAddress;
            Equlogin = model.LoginName;
            EquPass = model.LoginPassword;
            EquPort = model.Port ?? 0;
            if (model.Type == null) model.Type = "人脸识别";
            model.OperDateTime = DateTime.Now;
            model.Oper = _Account.UserLoginName;
            //  intCode = 

            intCode = ConnectEqu(EquIp, Equlogin, EquPass, EquPort);
            if (intCode > 0)
            {

                model.SerialNumber = System.Text.Encoding.UTF8.GetString(struDeviceInfo.sSerialNumber).TrimEnd('\0');
                model.EquModel = struDeviceInfo.wDevType.ToStr();
                _Logic.Save(model);
                intCode = 1;
            }
            return intCode;

        }


        #region  基本操作，增删改查

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [NonAction]
        public override PagingEntity GetPagingEntity(Hashtable query, int page = 1, int rows = 20)
        {
            //获取列表
            return _Logic.GetDataSource(query, page, rows);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [HttpPost, AopCheckEntity]
        public IActionResult Save(Equ model, string UserIDList)
        {
            int iError = Connect(model);
            //string strmsg = "设备增加失败(请检查设备信息和网络再试。)";
            if (iError == 1) {

                this.KeyID = _Logic.Save(model);
                
                //strmsg = "设备增加连接成功！";



            }
            return this.Success(new
            {
                status = iError,
                ID = this.KeyID,
                //msg = strmsg
           });



        }



        /// <summary>
        /// 设备连接测试
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult getLoginUserId(Equ mode)
        {
            string Ip, LoginName, LoginPass;
            int Port,UserId;
            
            Ip = mode.IpAddress;
            LoginName = mode.LoginName;
            LoginPass = mode.LoginPassword;
            Port = mode.Port??0;
            UserId = DeviceControl.GetLoginUserId(Ip,LoginName,LoginPass,Port);
            return this.Success(new {
                status = UserId
            }
         );
        }



        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete(string Ids)
        {
            _Logic.Delete(Ids);
            return this.Success();
        }

        /// <summary>
        /// 查询根据ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LoadForm(Guid? ID)
        {
            return this.Success(_Logic.LoadForm(ID.ToGuid()));
        }

        #endregion  基本操作，增删改查

        #region 设备操作 登录，取设备信息
        public int ConnectEqu(string EquIp, string EquName, string EquPass, int EquPort)
        {

            int ireturn = -1;

            bool m_bInitSDK = CHCNetSDK.NET_DVR_Init();
            if (m_bInitSDK == false)
            {
                Console.Write("NET_DVR_Init error!");
                return ireturn;
            }


            LoginCallBackFlag = false;
            m_struDeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();

     
            struDeviceInfo.sSerialNumber = new byte[CHCNetSDK.SERIALNO_LEN];

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

            struLoginInfo.bUseAsynLogin = AysnLoginFlag;
            struLoginInfo.cbLoginResult = new CHCNetSDK.LoginResultCallBack(AsynLoginMsgCallback);
            struLoginInfo.byLoginMode = 2;

           
            struLoginInfo.sDeviceAddress = EquIp;
            struLoginInfo.sUserName = EquName;
            struLoginInfo.sPassword = EquPass;
            struLoginInfo.wPort = (ushort)EquPort;
            lUserID = CHCNetSDK.NET_DVR_Login_V40(ref struLoginInfo, ref struDeviceInfoV40);
            struDeviceInfo = struDeviceInfoV40.struDeviceV30;
            if (struLoginInfo.bUseAsynLogin)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (!LoginCallBackFlag)
                    {
                        Thread.Sleep(5);
                    }
                    else
                    {
                        break;
                    }
                }
                if (!LoginCallBackFlag)
                {
                    //   MessageBox.Show(Properties.Resources.asynloginTips, Properties.Resources.messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ireturn = 0;
                }
                if (m_AysnLoginResult == 1)
                {
                    lUserID = m_iUserID;
                    struDeviceInfoV40.struDeviceV30 = m_struDeviceInfo;
                    ireturn = lUserID;
                }
                else
                {
                    ireturn = 2;
                    //  MessageBox.Show(Properties.Resources.asynloginFailedTips, Properties.Resources.messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //  return false;
                }

            }   
            return lUserID;



        }

        public void AsynLoginMsgCallback(Int32 lUserID, UInt32 dwResult, ref CHCNetSDK.NET_DVR_DEVICEINFO_V30 lpDeviceInfo, IntPtr pUser)
        {

            if (dwResult == 1)
            {

                m_struDeviceInfo = lpDeviceInfo;

            }

            m_AysnLoginResult = dwResult;
            m_iUserID = lUserID;
            LoginCallBackFlag = true;
        }


        #endregion

    }
}