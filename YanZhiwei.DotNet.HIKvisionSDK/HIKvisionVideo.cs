namespace YanZhiwei.DotNet.HIKvisionSDK
{
    using System;
    using System.IO;
    using System.Net;
    using YanZhiwei.DotNet2.Utilities.Result;

    /// <summary>
    /// 海康视频
    /// </summary>
    public sealed class HIKvisionVideo : IDisposable
    {
        #region Fields

        /// <summary>
        /// 视频截图文件夹
        /// </summary>
        public readonly string CaptureFolders = null;

        /// <summary>
        /// 日志截图文件夹
        /// </summary>
        public readonly string LogFolders = null;

        /// <summary>
        /// 视频设备登路密码
        /// </summary>
        public readonly string PassWord;

        /// <summary>
        /// SDK是否初始化正常
        /// </summary>
        public readonly bool SDKInitStatus = false;

        /// <summary>
        /// 视频设备登陆名称
        /// </summary>
        public readonly string UserName;

        /// <summary>
        /// 视频截图文件夹
        /// </summary>
        public readonly string VideoFolders = null;

        /// <summary>
        /// 视频设备IP，端口信息
        /// </summary>
        public readonly IPEndPoint VideoIPEndPoint = null;

        /// <summary>
        /// 视频预览
        /// </summary>
        private int previewId = -1;

        /// <summary>
        /// 用户ID数值，默认-1未登录，>=0登路状态
        /// </summary>
        private int userId = -1;

        #endregion Fields

        #region Constructors

        public HIKvisionVideo(string userName, string password, IPEndPoint videoIPEndPoint)
        {
            SDKInitStatus = CHCNetSDK.NET_DVR_Init();

            if (!SDKInitStatus)
                throw new FileLoadException("HCNetSDK初始化失败.");
            LogFolders = string.Format(@"{0}\HIKvision\Log", AppDomain.CurrentDomain.BaseDirectory);
            CaptureFolders = string.Format(@"{0}\HIKvision\Image", AppDomain.CurrentDomain.BaseDirectory);
            VideoFolders = string.Format(@"{0}\HIKvision\Video", AppDomain.CurrentDomain.BaseDirectory);
            HanlderAppFolders();

            CHCNetSDK.NET_DVR_SetLogToFile(3, LogFolders, true);
            UserName = userName;
            PassWord = password;
            VideoIPEndPoint = videoIPEndPoint;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 当前视频预览的设备通道
        /// </summary>
        public ushort CurPreviewChannelNo
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否处于视频预览状态
        /// </summary>
        public bool IsPreviewing
        {
            get
            {
                return previewId >= 0;
            }
        }

        /// <summary>
        /// 是否录像中
        /// </summary>
        public bool IsRecording
        {
            get;
            private set;
        }

        /// <summary>
        /// 登路状态
        /// </summary>
        public bool LoginStatus
        {
            get
            {
                return UserId >= 0;
            }
        }

        /// <summary>
        /// 视频预览ID
        /// </summary>
        public int PreviewId
        {
            get
            {
                return previewId;
            }

            private set
            {
                previewId = value;
            }
        }

        /// <summary>
        /// 登陆成功后，用户ID数值
        /// </summary>
        public int UserId
        {
            get
            {
                return userId;
            }

            private set
            {
                userId = value;
            }
        }

        private string LastErrorMessage
        {
            get
            {
                return string.Format("操作失败，错误代码:{0}", CHCNetSDK.NET_DVR_GetLastError());
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 视频截图，默认BMP格式
        /// </summary>
        /// <returns>操作结果</returns>
        public OperatedResult Capture()
        {
            return Capture(null);
        }

        /// <summary>
        /// 视频截图，默认BMP格式
        /// </summary>
        /// <param name="saveImageFilePath">图片格式存储全路径;eg:D:\20170913162510.jpeg</param>
        /// <param name="imageType">图片格式</param>
        /// <returns>操作结果</returns>
        public OperatedResult Capture(string saveImageFilePath, CaptureImageType imageType = CaptureImageType.BMP)
        {
            if (!IsPreviewing)
                return OperatedResult.Fail("尚未处于视频预览状态，不能进行视频截图操作.");

            string _captureImageFile = string.IsNullOrEmpty(saveImageFilePath) != true ? saveImageFilePath : string.Format(@"{0}\{1}.{2}", CaptureFolders, DateTime.Now.ToString("yyyyMMddHHmmss"), imageType.ToString().ToLower());
            bool _result = false;
            switch (imageType)
            {
                case CaptureImageType.BMP:
                    _result = CHCNetSDK.NET_DVR_CapturePicture(previewId, _captureImageFile);
                    break;

                case CaptureImageType.JPEG:
                    CHCNetSDK.NET_DVR_JPEGPARA _jpegParamter = new CHCNetSDK.NET_DVR_JPEGPARA();
                    _jpegParamter.wPicQuality = 0; //图像质量 Image quality
                    _jpegParamter.wPicSize = 0xff; //抓图分辨率 Picture size: 2- 4CIF，0xff- Auto(使用当前码流分辨率)，抓图分辨率需要设备支持，更多取值请参考SDK文档
                    _result = CHCNetSDK.NET_DVR_CaptureJPEGPicture(UserId, CurPreviewChannelNo, ref _jpegParamter, _captureImageFile);
                    break;
            }

            return _result == true ? OperatedResult.Success(_captureImageFile) : OperatedResult.Fail(LastErrorMessage);
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            if (IsPreviewing)
                CHCNetSDK.NET_DVR_StopRealPlay(PreviewId);
            if (LoginStatus)
                CHCNetSDK.NET_DVR_Logout(UserId);
            if (SDKInitStatus)
                CHCNetSDK.NET_DVR_Cleanup();
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns>操作结果</returns>
        public OperatedResult Login()
        {
            if (LoginStatus)
                return OperatedResult.Fail("已经处于登陆状态，不能进行登陆操作.");

            CHCNetSDK.NET_DVR_DEVICEINFO_V30 _deviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
            UserId = CHCNetSDK.NET_DVR_Login_V30(VideoIPEndPoint.Address.ToString(), VideoIPEndPoint.Port, UserName, PassWord, ref _deviceInfo);
            return LoginStatus == true ? OperatedResult.Success() : OperatedResult.Fail(LastErrorMessage);
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <returns>操作结果</returns>
        public OperatedResult Logout()
        {
            if (!LoginStatus)
                return OperatedResult.Fail("尚未处于登陆状态，不能进行登出操作.");

            if (CHCNetSDK.NET_DVR_Logout(UserId))
            {
                UserId = -1;
                return OperatedResult.Success();
            }
            else
            {
                return OperatedResult.Fail(LastErrorMessage);
            }
        }

        /// <summary>
        /// 视频预览
        /// </summary>
        /// <returns>操作结果</returns>
        public OperatedResult Preview(ushort channelNumber, IntPtr previewUIHandle)
        {
            if (!LoginStatus)
                return OperatedResult.Fail("请先登陆后，再进行视频预览操作.");
            if (IsPreviewing)
                return OperatedResult.Fail("已经处于视频预览状态，不能进行视频预览操作.");

            CHCNetSDK.NET_DVR_PREVIEWINFO _previewParamter = new CHCNetSDK.NET_DVR_PREVIEWINFO();
            _previewParamter.hPlayWnd = previewUIHandle;//预览窗口
            _previewParamter.lChannel = channelNumber;//预te览的设备通道
            _previewParamter.dwStreamType = 0;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
            _previewParamter.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP
            _previewParamter.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
            _previewParamter.dwDisplayBufNum = 15; //播放库播放缓冲区最大缓冲帧数
            IntPtr _pUser = new IntPtr();//用户数据
            PreviewId = CHCNetSDK.NET_DVR_RealPlay_V40(UserId, ref _previewParamter, null/*RealData*/, _pUser);
            CurPreviewChannelNo = channelNumber;
            return IsPreviewing == true ? OperatedResult.Success() : OperatedResult.Fail(LastErrorMessage);
        }

        /// <summary>
        /// 开始录像
        /// </summary>
        /// <returns>操作结果</returns>
        public OperatedResult StartRecord()
        {
            return StartRecord(null);
        }

        /// <summary>
        /// 开始录像
        /// </summary>
        /// <param name="saveVideoFilePath">视频格式存储全路径;eg:D:\20170913162510.mp4</param>
        /// <returns>操作结果</returns>
        public OperatedResult StartRecord(string saveVideoFilePath)
        {
            if (!IsPreviewing)
                return OperatedResult.Fail("尚未处于视频预览状态，不能进行开始录像操作.");
            string _videoRecordFile = string.IsNullOrEmpty(saveVideoFilePath) != true ? saveVideoFilePath : string.Format(@"{0}\{1}.mp4", VideoFolders, DateTime.Now.ToString("yyyyMMddHHmmss"));
            CHCNetSDK.NET_DVR_MakeKeyFrame(UserId, CurPreviewChannelNo);
            IsRecording = CHCNetSDK.NET_DVR_SaveRealData(PreviewId, _videoRecordFile);

            return IsRecording == true ? OperatedResult.Success() : OperatedResult.Fail(LastErrorMessage);
        }

        /// <summary>
        /// 停止视频预览
        /// </summary>
        /// <returns>操作结果</returns>
        public OperatedResult StopPreview()
        {
            if (!IsPreviewing)
                return OperatedResult.Fail("尚未处于视频预览状态，不能进行停止预览操作.");

            if (CHCNetSDK.NET_DVR_StopRealPlay(PreviewId))
            {
                PreviewId = -1;
                CurPreviewChannelNo = 0;
                return OperatedResult.Success();
            }
            else
            {
                return OperatedResult.Fail(LastErrorMessage);
            }
        }

        /// <summary>
        /// 停止录像
        /// </summary>
        /// <returns>操作结果</returns>
        public OperatedResult StopRecord()
        {
            if (!IsRecording)
                return OperatedResult.Fail("尚未处于视频录像状态，不能进行停止录像操作.");
            bool _result = CHCNetSDK.NET_DVR_StopSaveRealData(PreviewId);
            if (_result)
                IsRecording = false;
            return _result == true ? OperatedResult.Success() : OperatedResult.Fail(LastErrorMessage);
        }

        /// <summary>
        /// 处理程序文件夹是否存在，若不存在则创建
        /// </summary>
        private void HanlderAppFolders()
        {
            if (!Directory.Exists(LogFolders))
                Directory.CreateDirectory(LogFolders);
            if (!Directory.Exists(CaptureFolders))
                Directory.CreateDirectory(CaptureFolders);

            if (!Directory.Exists(VideoFolders))
                Directory.CreateDirectory(VideoFolders);
        }

        #endregion Methods
    }
}