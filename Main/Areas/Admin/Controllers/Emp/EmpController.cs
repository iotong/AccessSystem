using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Main.Areas.Access.Controllers //请在此设置您得命名空间
{
    //
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Aop;
    using DbFrame;
    using DbFrame.Class;
    using Common;
    using Entitys;
    using System.Collections;
    using System.Text;
    using Logic.SysClass;
    using Entitys.SysClass;
    using Logic;
    using HzyAdmin.Areas.Admin.Controllers;
    using Common;
    using Logic.Access;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Diagnostics;

    /// <summary>
    /// Emp管理
    /// </summary>
    public class EmpController : BaseController
    {

        public EmpLogic _EmpLogic = new EmpLogic();
        public EquLogic _EquLogic = new EquLogic();
        public Sys_UserLogic _SysUserLogic = new Sys_UserLogic ();

        public Emp EmpMode = new Emp();
        public Sys_User SysUserMode = new Sys_User() ;

        int iUserCode = -1;

        public Int32 m_lGetCardCfgHandle = -1;
        private CHCNetSDK.RemoteConfigCallback g_fGetGatewayCardCallback = null;



        protected override void Init()
        {
            this.MenuID = "A-202";
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
            return _EmpLogic.GetDataSource(query, page, rows);
        }



        /// 新增人员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult getInfo(IFormCollection fc)
        {
            string rMsg = "错误";
            int rStatus = -1;
            string ip, username, userpass;
            ushort port;

            //得到设备参数  
            //  EquMode = _EquLogic.getEquData("");
                //登录设备
                //DeviceControl.GetLoginUserId();
                //获取人员数据
            DataTable dt = _EquLogic.getEquData("select * from equ where Type like  '%采集%'");
            if (dt.Rows.Count > 0)
            {
                ip = dt.Rows[0]["IpAddress"].ToString();
                username = dt.Rows[0]["LoginName"].ToString();
                userpass = dt.Rows[0]["LoginPassword"].ToString();
                port = Convert.ToUInt16(dt.Rows[0]["Port"]);
                iUserCode = DeviceControl.GetLoginUserId(ip,username,userpass,port);
                if (iUserCode > 0)
                {
                  rStatus =  getDevUserInfo(iUserCode);
                  rMsg = "数据采集成功。";

                }
                else
                {
                    rStatus = -2;
                    rMsg = "连接采集终端失败，请检查网络连接！";

                }


              
            }
            else {
                rStatus = -1;
                rMsg = "没有找到采集终端，人员信息采集失败！";
            }

           

            //  this.KeyID = _Logic.Save(EmpMode);
            return this.Success(new
            {
                status = rStatus,
                msg = rMsg,
               
            });

        }

        private int getDevUserInfo(int iUserCode)
        {
            int ireturn;
            if (-1 != m_lGetCardCfgHandle)
            {
                if (CHCNetSDK.NET_DVR_StopRemoteConfig(m_lGetCardCfgHandle))
                {
                    m_lGetCardCfgHandle = -1;
                }
            }

         
            CHCNetSDK.NET_DVR_CARD_CFG_COND struCond = new CHCNetSDK.NET_DVR_CARD_CFG_COND();
            struCond.dwSize = (uint)Marshal.SizeOf(struCond);
            ushort.TryParse("0", out struCond.wLocalControllerID);
            struCond.dwCardNum = 0xffffffff;
            struCond.byCheckCardNo = 1;

            int dwSize = Marshal.SizeOf(struCond);
            IntPtr ptrStruCond = Marshal.AllocHGlobal(dwSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);
            g_fGetGatewayCardCallback = new CHCNetSDK.RemoteConfigCallback(ProcessGetGatewayCardCallback);
            int id = Process.GetCurrentProcess().Id;
            IntPtr Handle = (IntPtr)null;
            m_lGetCardCfgHandle = CHCNetSDK.NET_DVR_StartRemoteConfig(iUserCode, CHCNetSDK.NET_DVR_GET_CARD_CFG_V50, ptrStruCond, dwSize, g_fGetGatewayCardCallback,Handle);

            if (m_lGetCardCfgHandle == -1)
            {

                Marshal.FreeHGlobal(ptrStruCond);
                ireturn =  1;

            }
            else
            {
                ireturn =  m_lGetCardCfgHandle;
            }
            Marshal.FreeHGlobal(ptrStruCond);

           return ireturn;
           // throw new NotImplementedException();
        }


   

        private void ProcessGetGatewayCardCallback(uint dwType, IntPtr lpBuffer, uint dwBufLen, IntPtr pUserData)
        {
            if (pUserData == null)
            {
                return;
            }

            if (dwType == (uint)CHCNetSDK.NET_SDK_CALLBACK_TYPE.NET_SDK_CALLBACK_TYPE_DATA)
            {
                CHCNetSDK.NET_DVR_CARD_CFG_V50 struCardCfg = new CHCNetSDK.NET_DVR_CARD_CFG_V50();
                struCardCfg = (CHCNetSDK.NET_DVR_CARD_CFG_V50)Marshal.PtrToStructure(lpBuffer, typeof(CHCNetSDK.NET_DVR_CARD_CFG_V50));
              
                string strCardNo = System.Text.Encoding.UTF8.GetString(struCardCfg.byCardNo);
                IntPtr pCardInfo = Marshal.AllocHGlobal(Marshal.SizeOf(struCardCfg));
                Marshal.StructureToPtr(struCardCfg, pCardInfo, true);
                SavetoDatabase(strCardNo, struCardCfg);
              //  CHCNetSDK.PostMessage(pUserData, 1003, (int)pCardInfo, 0);

                //AddToCardList(struCardCfg, strCardNo);
            }
            else if (dwType == (uint)CHCNetSDK.NET_SDK_CALLBACK_TYPE.NET_SDK_CALLBACK_TYPE_STATUS)
            {
                uint dwStatus = (uint)Marshal.ReadInt32(lpBuffer);
                if (dwStatus == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_SUCCESS)
                {
                
                  //  CHCNetSDK.PostMessage(pUserData, 1002, 0, 0);
                }
                else if (dwStatus == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_FAILED)
                {
                    uint dwErrorCode = (uint)Marshal.ReadInt32(lpBuffer + 1);
                    string cardNumber = Marshal.PtrToStringAnsi(lpBuffer + 2);
                  //  CHCNetSDK.PostMessage(pUserData, 1002, 0, 0);
                }
            }
            return;
        }

        private void SavetoDatabase(string strCardNo, CHCNetSDK.NET_DVR_CARD_CFG_V50 struCardCfg)
        {
            string CardNo = strCardNo.TrimEnd('\0'); ;
            string CardPass =Encoding.UTF8.GetString(struCardCfg.byCardPassword).TrimEnd('\0');
            string UserName =Encoding.GetEncoding("GB2312").GetString(struCardCfg.byName).TrimEnd('\0');
            ushort UserDepNo = struCardCfg.wDepartmentNo;
            string UserNo = struCardCfg.dwEmployeeNo.ToStr().TrimEnd('\0');

            uint CardType = struCardCfg.byCardType;
            string CardValid = struCardCfg.byCardValid.ToStr().TrimEnd('\0');
            string startTime, endTime;
            CHCNetSDK.NET_DVR_TIME_EX sTime = struCardCfg.struValid.struBeginTime;
            CHCNetSDK.NET_DVR_TIME_EX eTime = struCardCfg.struValid.struEndTime;
            startTime = string.Format("{0,4}-{1,0:D2}-{2,0:D2} {3,0:D2}:{4,0:D2}:{5,0:D2}",sTime.wYear,sTime.byMonth,sTime.byDay,sTime.byHour,sTime.byMinute,sTime.bySecond);
            endTime = string.Format("{0,4}-{1,0:D2}-{2,0:D2} {3,0:D2}:{4,0:D2}:{5,0:D2}",eTime.wYear,eTime.byMonth,eTime.byDay, eTime.byHour,eTime.byMinute, eTime.bySecond);


            string id = _EmpLogic.checkCardNumber(CardNo);

            if (id == "null")
            {
                EmpMode.Id = Guid.Empty;

                SysUserMode.User_ID = Guid.Empty;
                SysUserMode.User_Name = UserName;
                SysUserMode.User_LoginName = CardNo;
                SysUserMode.User_Pwd = CardPass;
                _SysUserLogic.Save(SysUserMode, new Sys_UserRole() { UserRole_RoleID = "40FF1844-C099-4061-98E0-CD6E544FCFD3".ToGuid() });

            } else

            { EmpMode.Id = id.ToGuid();}
           
                EmpMode.CardNumber = CardNo;
                EmpMode.CardPassword = CardPass;
                EmpMode.Number = Convert.ToInt16(UserNo);
                EmpMode.Name = UserName;
                EmpMode.CardType = CardType.ToStr();
                EmpMode.CardIdentity = CardValid;
                EmpMode.CardStartTime =Convert.ToDateTime(startTime);
                EmpMode.CardEndTime = Convert.ToDateTime(endTime);
                _EmpLogic.Save(EmpMode);

          

        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [HttpPost, AopCheckEntity]
        public IActionResult Save(Emp model)
        {

            model.Oper = this.LoginName;
            model.OperTime = DateTime.Now;
          //  Console.Write("dep:{0},depmang:{1}",dep,depmang);
            this.KeyID = _EmpLogic.Save(model);
            return this.Success(new
            {
                status = 1,
                ID = this.KeyID
            });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete(string Ids)
        {
            _EmpLogic.Delete(Ids);
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
            return this.Success(_EmpLogic.LoadForm(ID.ToGuid()));
        }

        #endregion  基本操作，增删改查



    }
}