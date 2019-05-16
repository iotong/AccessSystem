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
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using System.Threading;
   



    /// <summary>
    /// Rec管理
    /// </summary>
    public class RecController : BaseController
    {

        public RecLogic _Logic = new RecLogic();


        public string DeviceIP = null;
        public int m_lUserID = 0;
        public int m_iDeviceIndex = 0;
        private CHCNetSDK.NET_DVR_ACS_EVENT_COND m_struAcsEventCond = new CHCNetSDK.NET_DVR_ACS_EVENT_COND();
        private string MinorType = null;
        private string MajorType = null;
        private int m_lLogNum = 0;
        private int m_lGetAcsEvent = 0;
        private CHCNetSDK.RemoteConfigCallback g_fGetAcsEventCallback = null;
        private string CsTemp = null;
        string equNo, equName;

        int iStatus = -1;
        string sMsg = "获取人员进出信息错误！";

        int count = 0;

        Rec recMode = new Rec();

        protected override void Init()
        {
            this.MenuID = "A-203";
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
        public IActionResult Save(Rec model)
        {
            this.KeyID = _Logic.Save(model);
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

        #region 从设备中提取记录

        [HttpPost]
        public IActionResult getRec(string equsn,string equname,string stime,string etime,string ip ,string username,string pass,string port)
        {

            equNo = equsn;
            equName = equname;
           //2,获取时间
            DateTime sTime = Convert.ToDateTime(stime);
            DateTime eTime = Convert.ToDateTime(etime);



            //  1,获取设备
            m_lUserID = DeviceControl.GetLoginUserId(ip, username, pass,Convert.ToUInt16(port));
            if (m_lUserID > 0)
            {

                m_struAcsEventCond.Init();

                m_struAcsEventCond.dwSize = (uint)Marshal.SizeOf(m_struAcsEventCond);
                m_struAcsEventCond.dwMajor = 5;


                m_struAcsEventCond.struStartTime.dwYear = sTime.Year;
                m_struAcsEventCond.struStartTime.dwMonth = sTime.Month;
                m_struAcsEventCond.struStartTime.dwDay = sTime.Day;
                m_struAcsEventCond.struStartTime.dwHour = sTime.Hour;
                m_struAcsEventCond.struStartTime.dwMinute = sTime.Minute;
                m_struAcsEventCond.struStartTime.dwSecond = sTime.Second;
                m_struAcsEventCond.struEndTime.dwYear = eTime.Year;
                m_struAcsEventCond.struEndTime.dwMonth = eTime.Month;
                m_struAcsEventCond.struEndTime.dwDay = eTime.Day;
                m_struAcsEventCond.struEndTime.dwHour = eTime.Hour;
                m_struAcsEventCond.struEndTime.dwMinute = eTime.Minute;
                m_struAcsEventCond.struEndTime.dwSecond = eTime.Second;

                m_lLogNum = 0;
                uint dwSize = (uint)Marshal.SizeOf(m_struAcsEventCond);
                IntPtr ptrCond = Marshal.AllocHGlobal((int)dwSize);
                Marshal.StructureToPtr(m_struAcsEventCond, ptrCond, false);
                g_fGetAcsEventCallback = new CHCNetSDK.RemoteConfigCallback(ProcessGetAcsEventCallback);
                m_lGetAcsEvent = CHCNetSDK.NET_DVR_StartRemoteConfig(m_lUserID, CHCNetSDK.NET_DVR_GET_ACS_EVENT, ptrCond, (int)dwSize, g_fGetAcsEventCallback, (IntPtr)0);

                uint ierror1 = CHCNetSDK.NET_DVR_GetLastError();
                if (-1 == m_lGetAcsEvent)
                {
                    // g_formList.AddLog(m_iDeviceIndex, AcsDemoPublic.OPERATION_FAIL_T, "NET_DVR_GET_ACS_EVENT");
                    Marshal.FreeHGlobal(ptrCond);
                    //    ir = 1;
                }
                else
                {
                    //  g_formList.AddLog(m_iDeviceIndex, AcsDemoPublic.OPERATION_SUCC_T, "NET_DVR_GET_ACS_EVENT");
                    Marshal.FreeHGlobal(ptrCond);
                    //   ir = -1;
                }
                Thread.Sleep(3000);
                iStatus = 1;
                sMsg = "收集数据成功！";
            }
            else
            {
                iStatus = -1;
                sMsg = "设备连接错误！";



            }




            //3,查询数据


            return this.Success(new
            {
                status = iStatus,
                msg = sMsg


            });

        }

        private void getEvent(DateTime sTime,DateTime eTime, int userID)
        {

         //   int ir = -1;

            //return ir;
        }

        private void ProcessGetAcsEventCallback(uint dwType, IntPtr lpBuffer, uint dwBufLen, IntPtr pUserData) 
        {
            if (dwType == (uint)CHCNetSDK.NET_SDK_CALLBACK_TYPE.NET_SDK_CALLBACK_TYPE_DATA)
            {
                CHCNetSDK.NET_DVR_ACS_EVENT_CFG lpAcsEventCfg = new CHCNetSDK.NET_DVR_ACS_EVENT_CFG();
                lpAcsEventCfg = (CHCNetSDK.NET_DVR_ACS_EVENT_CFG)Marshal.PtrToStructure(lpBuffer, typeof(CHCNetSDK.NET_DVR_ACS_EVENT_CFG));
                IntPtr ptrAcsEventCfg = Marshal.AllocHGlobal(Marshal.SizeOf(lpAcsEventCfg));
                Marshal.StructureToPtr(lpAcsEventCfg, ptrAcsEventCfg, true);
                //uint i = CHCNetSDK.NET_DVR_GetLastError();
              //  count++;
                string CardNo = Encoding.UTF8.GetString(lpAcsEventCfg.struAcsEventInfo.byCardNo).TrimEnd('\0');
                if (CardNo.Length>0) { saveToDB(lpAcsEventCfg); }

               //    CHCNetSDK.PostMessage(pUserData, CHCNetSDK.WM_MSG_ADD_ACS_EVENT_TOLIST, (int)ptrAcsEventCfg, 0);
            }
            else if (dwType == (uint)CHCNetSDK.NET_SDK_CALLBACK_TYPE.NET_SDK_CALLBACK_TYPE_STATUS)
            {
                uint i = CHCNetSDK.NET_DVR_GetLastError();
                int dwStatus = Marshal.ReadInt32(lpBuffer);
                if (dwStatus == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_SUCCESS)
                {
                    //  CHCNetSDK.PostMessage(pUserData, CHCNetSDK.WM_MSG_GET_ACS_EVENT_FINISH, 0, 0);
                }
                else if (dwStatus == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_FAILED)
                {
                    //g_formList.AddLog(m_iDeviceIndex, AcsDemoPublic.OPERATION_FAIL_T, "NET_DVR_GET_ACS_EVENT failed");
                }
            }
            Console.Write(count);

        }

        private void saveToDB(CHCNetSDK.NET_DVR_ACS_EVENT_CFG lpAcsEventCfg)
        {

            string strCardNo = Encoding.UTF8.GetString(lpAcsEventCfg.struAcsEventInfo.byCardNo).TrimEnd('\0');
            recMode.CardNumber = strCardNo;
            // recMode.EquNo = Encoding.UTF8.GetString(m_struAcsEventCond.byName);
            recMode.EquNo = equNo;
            recMode.EquName = equName;
            recMode.EventType = getEventType(lpAcsEventCfg.dwMinor);
            recMode.Time = getEventTime(lpAcsEventCfg.struTime);
            recMode.Oper = _Account.UserLoginName;
            recMode.OperTime = DateTime.Now;
            recMode.Id = Guid.Empty;
            _Logic.Save(recMode);




        }

        private DateTime? getEventTime(CHCNetSDK.NET_DVR_TIME struTime)
        {
            DateTime dt;
            dt =Convert.ToDateTime(struTime.dwYear.ToString() + "-" + struTime.dwMonth.ToString() + "-" + struTime.dwDay.ToString() + " " + struTime.dwHour.ToString() + ":" + struTime.dwSecond.ToString() + ":" + struTime.dwMinute.ToString());
            return dt;
           // throw new NotImplementedException();
        }

        private string getEventType(uint dwMinor)
        {
            string EventName="刷卡验证通过";
            switch (dwMinor)
            {
                case CHCNetSDK.MINOR_FACE_VERIFY_PASS:
                    EventName = "人脸验证通过";
                    break;
                case CHCNetSDK.MINOR_FINGERPRINT_COMPARE_PASS:
                    EventName = "指纹验证通过";
                    break;     
                case CHCNetSDK.MINOR_LEGAL_CARD_PASS:
                    EventName = "刷卡验证通过";
                    break;
                case CHCNetSDK.MINOR_CARD_OUT_OF_DATE:
                    EventName = "不在规定时间内刷卡";
                    break;


            }

            return EventName;
        }
    }
}

        #endregion

