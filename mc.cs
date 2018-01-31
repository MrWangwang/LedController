//-------------------------------------------------------------//
//
//      MCSDK v1.1  for C#
//
//-------------------------------------------------------------//

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;

namespace MCSDK
{
    class mc
    {
        //*****************************************************************//
        //
        //  MCSDK 数据类型定义
        //
        //*****************************************************************//

        //盒子临时编号，下面说明中boxno均指该编号
        //盒子在线时这个值不变。只要服务器不关闭，盒子重新上线下线仍是这个编号
        //typedef		int		HBOX; 
        //

        public const int MCEVENT_CONNECT = 100;//有盒子连接
        public const int MCEVENT_DISCONNECT = 101;	//有盒子断开连接

        //回调函数：盒子连接事件通知
        // 功能: 由McControl.dll回调上层程序，通知盒子的连接与断开等事件
        public delegate Boolean TOnMcEvent(
             int eventno,   //事件类型，参见MCEVENT_CONNECT等
             int boxno,     //播放盒代号
             int para1,     //备用参数
             int para2
             );


        //盒子基本信息
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct TMcBoxInfo
        {
            //用户自定义名称(又叫LedName或BoxName);
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
            public String LedName;
            //当前ip地址
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
            public String IP;
            //LED大小
            public int width, height;
            //LED磁盘剩余空间(M)
            public int diskspace;
            //当前节目名
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
            public String mcv;
        };

        //页面播放方式：
        public enum TMcPlayMode
        {
            //0-等待节目播完，播几遍
            pm_Normal,
            //1-指定时长，播完指定时间后切换到下一页
            pm_TimeLen,
            //2-全局节目 
            pm_Global,
            //3-定时播放: 到了指定时间自动弹出
            pm_Popup,
        };


        public enum TMcDateType
        {
            dt_Null = 0,	//无
            dt_Everyday = 1,	//每天
            dt_Week15 = 2,	//每周一到周五
            dt_Week6 = 3,	//周六
            dt_Week7 = 4,	//周日
            dt_Week1 = 5,	//周一
            dt_Week2 = 6,	//周二
            dt_Week3 = 7,	//周三
            dt_Week4 = 8,	//周四
            dt_Week5 = 9,	//周五
            dt_Date = 10,//指定日期
            dt_DateRange = 11,//起止日期
        };


        //定义：时间段TMcTimeItem 
        //(时间段的具体例子可以MC100软件的定时节目页中查看)
        //例子：每周三的19:00~19:30  dateype=7,begintime=”19:00:00” endtime=”19:30:00”
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct TMcTimeItem
        {
	        TMcDateType		datetype;		//定时类型
            //
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string   begindate;  //类型=10或时，指定日期,如2015-07-03
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string   enddate;	//类型=11时，指定终止日期如2015-07-10
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
	        public string	begintime;	//指定起始播放时间,如19:00:00
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
	        public string	endtime;	 //指定终止播放时间，如19:30:00
            //
	        int				tag;		 //标记，备用
        };


        //定义：单行文字动画方式
        public enum TMcTxtAniMode {
	        ani_TxtNull=0,	//静止
	        ani_TxtMoveLeft=1,//向左走字
	        ani_TxtXiangXiaHuaRu=2,//向上滑入
        };

        //单行文字框设置
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct TMcText 
        {
	        //名字（若为空，则由系统自动默认名字）
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
	        public string		name;//[40];
	        //输出位置
	        public int			left,top,width,height;
	        //边框宽度，默认0
	        public int			borderwidth;
	        //边框颜色，默认白
	        public int			bordercolor;
	        //字体（若为空，将采用盒子的默认黑体）
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string fontname;//[20];
	        //字号（默认20)
	        public int			fontsize;
	        //字色
	        public int			fontcolor;
	        //背景色
	        public int			backcolor;
	        //透明度(0-透明 255-不透明)
	        public int			alpha;
	        //动画效果 默认0-静止
	        public TMcTxtAniMode	animode;
	        //移动速度等级(0-5)，默认2
	        public int			speedLevel;
	        //每条信息显示时间(秒)  (对于水平移动文字无效，移动时间由字数和速度决定)
	        public int			playtime;
            //对齐方式(X) 0-左 1-中 2-右
            public int alignx;
            //绘制模式：默认1，若是动态文字，请用0
            public int drawmode;
        };

        //定义：图片动画类型
        public enum TMcAniMode {
	        ani_Null=0,				//静止	    
	        ani_Rand=1,				//随机	    
	        ani_DanRuDanChu=2,		//淡入淡出  
	        ani_XiangZuoHuaRu=3,	//向左滑入  
	        ani_XiangYouHuaRu=4,	//向右滑入  
	        ani_XiangShangHuaRu=5,	//向上滑入  
	        ani_XiangXiaHuaRu=6,	//向下滑入  
	        ani_SiJiaoHuaRu=7,		//四角滑入  
	        ani_JianJianFangDa=8,	//渐渐放大  
	        ani_Radar=9,			//雷达      
	        ani_SuiHuaRongRu=10,	//碎花融入  
	        ani_LingXingJieKai=11,	//菱形揭开  
	        ani_LingXingHeLong=12,	//菱形合拢  
	        ani_BaiYe=13,			//百叶      
	        ani_JaoCuo=14,			//交错      
	        ani_YouXuanJinRu=15,	//右旋进入  
	        ani_ZuoXuanJinRu=16,	//左旋进入  
	        ani_XiangShangYiDong=17,//向上移动  
	        ani_XiangZuoYiDong=18,	//向左移动  
        };

        //图片框设置
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct TMcImage
        {
            //名字（若为空，则由系统自动默认名字）
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
            public string		name;//[40];
            //输出位置
            public int			left,top,width,height;
            public int			borderwidth;
            public int			bordercolor;
            //缩放方式: 0-居中 1-拉伸(默认) 2-原始 
            public int			scaleType;
            //动画效果
            TMcAniMode		animode;
            //移动速度等级(0-5)
            public int			speedLevel;
            //每个图片显示时间(秒)
            public int			playtime;
            //背景色
            public int			backcolor;
            //透明度(0-透明 255-不透明)
            public int			alpha;
        };


        //视频框设置
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct TMcVideo
        {
	        //名字（若为空，则由系统自动默认名字）
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
            public string name;//[40];
            //输出位置
            public int left, top, width, height;
            public int borderwidth;
            public int bordercolor;
        };


        //广告框设置
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct TMcNotice
        {
	        //名字（若为空，则由系统自动默认名字）
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
            public string name;//[40];
            //输出位置
            public int left, top, width, height;
            public int borderwidth;
            public int bordercolor;
        };


        //添加时间信息框
        //fmt有如下类型：
        public enum TMcDateFmt
        {
	        mcdate_YMD,	//2013年11月28日
	        mcdate_MD,	//11月28日
	        mcdate_W,	//星期日
	        mcdate_MDW,	//11月28日 星期日
	        mcdate_hm,	//07:46
	        mcdate_hms,	//07:46:58
	        mcdate_time,	//2013年11月28日 07:46:58

        };
        //信息框设置
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct TMcInfo
        {
            //名字（若为空，则由系统自动默认名字）
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
            public string name;//[40];
            //输出位置
	        public int			left,top,width,height;
	        public int			borderwidth;
	        public int			bordercolor;
	        //字号
	        public int			fontsize;
	        //字色
	        public int			fontcolor;
	        //背景色
	        public int			backcolor;
	        //透明度(0-透明 255-不透明)
	        public int			alpha;
        };


        //FTP下载设置
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct TMcFTP
        {
	        //ftp svr IP或地址(若为空，则用默认设置)
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
	        public string		svrip;//[40];
	        //ftp端口
	        public int			port;
	        //用户名
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
	        public string		user;//[20];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
	        //密码
	        public string		pwd;//[20];
	        //字符集(GBK或UTF-8)
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string charset;//[10];
        };




        //*****************************************************************//
        //  基本调用
        //*****************************************************************//

        //--------------------------------------
        // 函数: mcInit
        // 功能: 程序启动时初始化McControl
        // 参数: SvrPort	当前SVR软件的监听端口号
        // 参数: _onevent	回调函数，当有盒子连接或断开事件时，通知上层程序
        // 参数: _debugmode	调试模式，默认0
        // 返回: bool
        //-------------------------------------
        [DllImport("McControl.dll",CharSet = CharSet.Unicode)]
        public static extern bool mcInit(int SvrPort,TOnMcEvent _onevent,int _debugmode);


        //-----------------------------------
        // 函数: mcClose
        // 功能: 程序退出时关闭McControl控制
        // 返回: 无
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public static extern void mcClose();

        //-----------------------------------
        // 函数: mcSdkVersion
        // 功能: 获取当前接口版本号
        // 备注：
        // 参数: 
        // 返回: 版本号
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static int mcSdkVersion();

        //-----------------------------------
        // 函数: mcGetBoxCount
        // 功能: 得到当前已连线播放盒个数
        // 返回: 连线的盒子数
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public static extern int mcGetBoxCount();

        //-----------------------------------
        // 函数: mcGetFirstBox
        // 功能: 返回第一个播放盒
        // 备注：用于遍历所有盒子
        // 返回: 盒子临时编号，代表特定的盒子，对盒子进行后继操作时要用到。若没有，返回0
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public static extern int mcGetFirstBox();

        //-----------------------------------
        // 函数: mcGetNextBox
        // 功能: 获得下一个盒子代号
        // 备注：用于遍历所有盒子
        // 参数: curbox	上一个盒子代号
        // 返回: public extern static HBOX		
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public static extern int mcGetNextBox(int curbox);

        //
        //-----------------------------------
        // 函数: mcFindBox
        // 功能: 根据盒子名字，查找盒子临时代号
        // 备注：
        // 参数: LedName	盒子名称（即大屏名称LedName,由初始设置给定）
        // 返回: 若找不到，返回0
        //-----------------------------------
        [DllImport("McControl.dll",  CharSet = CharSet.Unicode)]
        public extern static int mcFindBox(String LedName);


        //-----------------------------------
        // 函数: mcGetBoxInfo
        // 功能: 查看盒子的基本信息(名字,屏幕大小,在线状态，当前节目等)
        // 备注：
        // 参数: boxno	盒子编号
        //		 info	返回参数，返回盒子信息
        // 返回: 是否找到
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static Boolean mcGetBoxInfo(int boxno, ref TMcBoxInfo info);


        //---------------------------------------------//
        //	动态创建节目
        //---------------------------------------------//
        //typedef void*	HMCV;		//节目句柄

        //-----------------------------------
        // 函数: mcCreateMcv
        // 功能: 新建一个节目
        // 备注：如果本机就是FTP服务器，可以直接把FTP文件夹作为节目的根目录，就不用专门上传FTP了
        // 参数: 
        //       _szMcvBasePath		专门用于保存节目的本地文件夹，所有节目都保存到此处，如E:\\McData
        //       _szMcvName			节目名，40字符以内，可汉字， 保存时用这个节目名新建一个文件夹，保存所有节目数据。
        //       screenwidth		节目播放窗口大小，一般要与屏幕大小相同
        //       screenheight		
        //       _IsDeleteOld =true		如果已有这个节目，是否强制删除以前节目，默认删除
        //       _IsCopySrcFile = false		是否把节目用到的图片、视频等文件复制到节目文件夹下集中起来，便于手动上传到其它FTP服务器
        // 返回: HMCV	节目句柄，代表新建的节目 失败返回 0
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static int mcCreateMcv(string _szMcvBasePath,string _szMcvName,int screenwidth,int screenheight,bool _IsDeleteOld,bool _IsCopySrcFile);


        //-----------------------------------
        // 函数: mcLoadMcv
        // 功能: 打开一个节目，以便添加新内容
        // 备注：
        // 参数: 
        //       _szMcvPath	节目所在文件夹，如E:\\McData\\新节目1
        // 返回: HMCV	节目句柄
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static int mcLoadMcv(string _szMcvPath);

        //-----------------------------------
        // 函数: mcSaveMcv
        // 功能: 保存节目
        // 备注：发送或上传之前，要保存
        // 参数:  hmcv	节目句柄
        // 返回: 是否成功
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static bool mcSaveMcv(int hmcv);


        //-----------------------------------
        // 函数: mcSetCurMcv
        // 功能: 设置当前需要编辑的节目，以便添加文字图片
        // 备注：一般先hmcv = mcCreateMcv(),然后mcSetCurMcv(hmcv)
        // 参数: hmcv 指定节目句柄。 
        // 返回: 是否成功
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static bool mcSetCurMcv(int hmcv);

        //-----------------------------------
        // 函数: mcCloseMcv
        // 功能: 关闭节目，释放内存
        // 备注：所有新建节目或打开的节目需要close
        // 参数: 
        //       HMCV hmcv
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static void mcCloseMcv(int hmcv);



        //-----------------------------------
        // 函数: mcAddPage
        // 功能: 增加节目页
        // 备注：
        // 参数: 
        //       _szName	节目页名字，如节目页1。若为NULL，则自动起名
        //       playmode	页面播放方式: 0-正常页 1-限时页 2-全局页 3-定时弹出页
        //       playtimes	playmode=0时代表播放次数，playmode=1时代表播放时间（秒）
        //       backcolor	页面背景色(RGB)，默认0(黑色)
        // 返回: 节目页id (pageid)，供编辑函数使用，若失败返回0
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static int mcAddPage(string _szName,TMcPlayMode playmode,int playtimes,int backcolor);


        //-----------------------------------
        // 函数: mcAddPageTimer
        // 功能: 给节目页增加定时
        // 备注：您需要在创建节目页时，把页面类型指定为3(定时页) 一个节目页可以增加多个时间段
        // 参数: 
        //       int pageno				指定节目页
        //       TMcTimeItem * time		指定一个时间段，到了这个时间段，会自动弹出节目。
        // 返回: 时间段序号
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static int mcAddPageTimer(int pageno, ref TMcTimeItem time);


        //-----------------------------------
        // 函数: mcGetDefaultText
        // 功能: 填充一个默认文本设置，如字号默认20
        // 备注：为减少文本设置工作量，可以先用本函数获得一个默认设置，然后修改其中几项。
        // 参数: TMcText * text		返回默认设置
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static void mcGetDefaultText(ref TMcText text);


        //
        //-----------------------------------
        // 函数: mcAddSingleText
        // 功能: 添加单行文字框
        // 备注：
        // 参数: 
        //       pageid		指定节目页(所在节目页的id)
        //       _szTxt		文字内容 (如果有多条信息在同一个框中播出，请用回车符\n分隔)
        //       _len		文字个数(若为-1，则表示全部长度)
        //       text		文字框设置，见TMcText说明
        // 返回: 返回对象id (objid)，若失败返回0
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static int mcAddSingleText(int pageid,string _szTxt,int _len,ref TMcText text);



        //-----------------------------------
        // 函数: mcGetDefaultImg
        // 功能: 填充一个默认图片框设置，方便参数设置
        // 备注：
        // 参数: TMcImage * imgbox	返回默认设置
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static void mcGetDefaultImg(ref TMcImage imgbox);

        //-----------------------------------
        // 函数: mcAddImageBox
        // 功能: 添加图片框,返回objid
        // 备注：需再mcAddImageFile添加图片。可以添加多张图。
        // 参数: 
        //       int page		节目页id
        //       imgbox			图片框设置(位置，动画方式等)
        // 返回: 图片框id
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static int mcAddImageBox(int page, ref TMcImage imgbox);

        //-----------------------------------
        // 函数: mcAddImageFile
        // 功能: 向图片框中添加图片(每个图片将轮流在这个框中显示)
        // 备注：特大图片(4000以上)建议事先转成与屏幕相适合的小图片，否则影响播放
        // 参数: 
        //       int objid		图片框id
        //       _szImgFile		图片文件路径
        // 返回: 序号。 若失败返回0
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static int mcAddImageFile(int objid, string _szImgFile);



        //-----------------------------------
        // 函数: mcGetDefaultVideo
        // 功能: 填充一个视频框默认设置，方便参数设置
        // 备注：
        // 参数: videobox	返回默认设置
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static void mcGetDefaultVideo(ref TMcVideo videobox);

        //-----------------------------------
        // 函数: mcAddVideoBox
        // 功能: 添加视频框
        // 备注：需再用mcAddVideoFile添加视频。可以添加多个。
        // 参数: 
        //       int pageid	节目页id
        //       const TMcVideo * videobox		视频框设置
        // 返回: 视频框 id
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static int mcAddVideoBox(int pageid, ref TMcVideo videobox);

        //-----------------------------------
        // 函数: mcAddVideoFile
        // 功能: 向视频框中添加视频(每个视频将轮流在这个框中显示）
        // 备注：建议多用mp4格式，播放较稳定。
        // 参数: 
        //       int objid		视频框id
        //       _szVideoFile	视频文件 (支持mp4,avi,mpg,mkv,flv,rmvb,vob等多种)
        //       int volume     音量0~100，默认100
        // 返回: 序号。 若失败返回0
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static int mcAddVideoFile(int objid, string _szVideoFile, int volume);



        //-----------------------------------
        // 函数: mcGetDefaultNotice
        // 功能: 填充一个广告框默认设置，方便参数设置
        // 备注：
        // 参数: noticebox	返回默认设置
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static void mcGetDefaultNotice(ref TMcNotice noticebox);

        //-----------------------------------
        // 函数: mcAddNoticeBox
        // 功能: 添加广告框，用以播放多行文本，RTF文本
        // 备注：需再用mcAddNoticeRTF()等函数追加内容
        // 参数: 
        //       int pageid		节目页id
        //       * notice		广告框设置
        // 返回: 广告框id
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static int mcAddNoticeBox(int pageid, ref TMcNotice notice);

        //-----------------------------------
        // 函数: mcAddNoticeRTF
        // 功能: 为广告框添加一个rtf文本或txt纯文本
        // 备注：
        // 参数: 
        //       objid			广告框id
        //       _szRTFFile		rtf,txt文件名
        //       _animode		动画方式（默认17=向上移动，比较适合长文本）动画类型参见TMcAniMode说明
        //       backcolor		背景色，默认黑
        //       backalpha		背景透明度(0~255)，0-全透明
        //       fontcolor		字色，默认白
        //       speedLevel		移动速度(0~4)，默认2
        //       pagetime		如果是翻页显示，这个值表示每页显示时间（秒）
        // 返回: 序号。若失败返回0
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static int mcAddNoticeRTF(int objid, string _szRTFFile, TMcAniMode _animode, int backcolor, int backalpha, int fontcolor, int speedLevel, int pagetime);


        //-----------------------------------
        // 函数: mcTxt2Rtf
        // 功能: 协助您处理纯文本txt文件：把txt加上字体字号转换成rtf格式，然后您可以再把这个rtf用mcAddNoticeRTF()添到节目中
        // 示例：bool isok = mcTxt2Rtf(L"D:\\文档示例\\critical.txt",L"D:\\文档示例\\critical.rtf",32,L"微软雅黑",true,48,L"楷体");
        // 参数: 
        //       szTxtFile	原始纯文本txt，文件路径
        //       szRtfFile  新生的rtf的保存文件名
        //       fontsize	正文字号
        //       fontname   正文字体
        //       isTitleCenter  是否标题自动居中处理，默认false
        //       titleFontSize	标题的字号，若为0，保持与正文一致。
        //       titleFontName  标题的字体，若为空，保持与正文一致。
        // 返回: 是否成功
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static bool mcTxt2Rtf(string szTxtFile, string szRtfFile, int fontsize, string fontname, bool isTitleCenter, int titleFontSize, string titleFontName);


        //-----------------------------------
        // 函数: mcGetDefaultInfo
        // 功能: 填充一个信息框默认设置，方便参数设置
        // 备注：
        // 参数: info	返回默认设置
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static void mcGetDefaultInfo(ref TMcInfo info);

        //-----------------------------------
        // 函数: mcAddInfoBoxTime
        // 功能: 添加一个时间框
        // 备注：
        // 参数: 
        //       int pageid		节目页
        //       TMcDateFmt fmt	时间格式类型
        //       TMcInfo * info	
        // 返回: 时间信息框id
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static int mcAddInfoBoxTime(int pageid, TMcDateFmt fmt, ref TMcInfo info);


        //-----------------------------------
        // 函数: mcStartSendMcv
        // 功能: 开始上传指定节目
        // 备注：这个函数开始在线上传后返回。整个上传过程需要一定时间。请用mcGetUploadProgress()定时查询上传进度。
        // 参数: 
        //       boxno		盒子代号 (由mcGetFirstBox,mcGetNextBox获取所有在线盒子的代号)
        //      _szMcvPath	节目所在文件夹，如E:\\McData\\新节目1
        // 返回: 是否开始
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static bool mcStartSendMcv(int boxno, string _szMcvPath);

        //-----------------------------------
        // 函数: mcGetUploadProgress
        // 功能: 获得盒子当前上传的总进度
        // 备注：
        // 参数: 
        //       HBOX boxno	盒子代号
        // 返回: 进度值(0-100)
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static int mcGetUploadProgress(int boxno);
        //停止当前上传
        //-----------------------------------
        // 函数: mcStopUpload
        // 功能: 停止上传
        // 备注：
        // 参数: 
        //       HBOX boxno
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static bool mcStopUpload(int boxno);


        //
        //-----------------------------------
        // 函数: mcStartFTP
        // 功能: 开始FTP下载
        // 备注：调用后很快返回。然后要用mcGetProgressFTP()获取进度
        // 参数: 
        //       HBOX boxno		指定播放盒
        //       _szFtpPath		指定FTP服务器上的待下载文件夹，注意要用相对于FTP Root目录的路径，如 "/新节目1"  "/doc/test1" 
        //       _szDestPath	指定保存在盒子上哪个文件夹。若为0，表示节目专用的文件夹
        //       int tag		备用标记 默认tag=1时表示下载完毕的自动开始播放
        //       TMcFTP * set	FTP设置
        // 返回: 是否开始
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static bool mcStartFTP(int boxno, string _szFtpPath, string _szDestPath, int tag, ref TMcFTP set);

        //-----------------------------------
        // 函数: mcGetProgressFTP
        // 功能: 获得播放盒FTP下载进度，局域网可3秒调用一次，外网建议5~10秒以上调用一次
        // 备注：
        // 参数: 
        //       int boxno		指定盒子
        // 返回: public extern static int	
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static int mcGetProgressFTP(int boxno);
        
        //-----------------------------------
        // 函数: mcGetFTPState
        // 功能: 获得播放盒FTP下载状态
        // 备注：
        // 参数: 
        //       int boxno
        // 返回: 当前状态0-无 1-正下载 2-正常结束 3-连结中 4-连接异常/无法登录  5-停止取消 6-磁盘满  7-目录错误 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static int mcGetFTPState(int boxno);


        //-----------------------------------
        // 函数: mcStopFTP
        // 功能: 停止当前FTP下载
        // 备注：播放盒一旦开始下载，即使重启也会自动断点续传。除非你用这个命令取消下载任务。
        // 参数: 
        //       int boxno
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static bool mcStopFTP(int boxno);


        //-----------------------------------
        // 函数: mcSetDefaultMcv
        // 功能: 指定默认播放节目
        // 备注：
        // 参数: 
        //       HBOX boxno		指定播放盒
        //       _szMcvName		指定节目名（事先已经上传过）
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static bool mcSetDefaultMcv(int boxno, string _szMcvName);


        //-----------------------------------
        // 函数: mcGetDefaultMcv
        // 功能: 获得播放盒当前正在播放的节目
        // 备注：C#调用示例:
        //    StringBuilder buf = new StringBuilder(80);
        //    mc.mcGetDefaultMcv(buf,80);
        //    string mcvname = buf.ToString();
        // 参数: 
        //       int boxno	指定播放盒
        //       namebuf	返回节目名
        //       buflen 节目名的最大长度(50)
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static bool mcGetDefaultMcv(
                   int boxno,
                   [MarshalAs(UnmanagedType.LPTStr)]
　                  StringBuilder namebuf,
                    int buflen
            );


        //-----------------------------------
        // 函数: mcDeleteMcv
        // 功能: 删除盒子中的节目，清理磁盘空间
        // 备注：
        // 参数: 
        //       int boxno		指定播放盒
        //       _szMcvName		指定节目
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static bool mcDeleteMcv(int boxno, string _szMcvName);

        //
        //-----------------------------------
        // 函数: mcSendSingleText
        // 功能: 动态推送文字：在线实时替换当前节目中的文字内容
        // 备注：
        // 参数: 
        //       int boxno		指定播放盒
        //       int objid		指定文字框id，如果不清楚其id，可为0，换用下面_szObjName指定名称。
        //       _szObjName		指定文字框名字。如果已经指定id, 此项可为空
        //       _szTxt			指定文字内容 (若有多条信息用回车符\n分割)
        // 返回:
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static bool mcSendSingleText(int boxno, int objid, string _szObjName, string _szTxt);


        //-----------------------------------
        // 函数: mcSetLedName
        // 功能: 为大屏单独起名/改名，以便管理与区分
        // 备注：
        // 参数: 
        //       int boxno
        //       _szLedName		大屏播放盒的名字,可以是汉字，不要超过20字。
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static bool mcSetLedName(int boxno, string _szLedName);

                
        //////////////////////////////////////////////////////////////////////////
        //			节目预览功能
        //
        //注意事项：
        //mcDisplay...系列函数，要在主线程中调用
        //--不要调用mcCloseMcv()关闭它,否则看不到预览. mcCloseMcv会关闭当前编辑的节目
        //--需要在定时器中调用mcDisplayDraw()完成画面刷新。定时器的间隔决定帧率。建议20ms (50FPS) 如果机器比较烂，就30ms (30FPS)
        //--可以直接预览内存中mcv,所见即所得。 中途一般不需要mcSaveMcv();只要在用户退出编辑时mcSaveMcv()即可
        //
        //---------------------------------------------//

        /*
        使用例举:

        (1)程序初始化时
        mcInit(8300,OnMcEvent,1);
        ...
        mcDisplayInit(hwnd_screenWin,0x404040);

        (2)预览
        mcSetCurMcv(..)
        mcDisplayShow();//开始预览当前节目
        TimerDisplay->Enabled=true;//开启一个定时器，每20ms调用一次

        (3)
        onTimer()
        {
	        mcDisplayDraw();//调用该函数刷新预览画面
        }

        */

        //-----------------------------------
        // 函数: mcDisplayInit
        // 功能: 初始化预览功能
        // 备注：输出窗口最好是固定大小。(如果中途调整了大小，要在resize之后，重新调用mcDisplayInit)
        //			预览功能对显卡有一定要求。有的机器显示性能很差(如有个别Atom平板电脑)，可能会初始化失败,无法预览。
        // 参数: 
        //       HWND outWindow		输出窗口的windows句柄(hwnd)，可以在任意窗口中输出（甚至是按钮，只要有hwnd）
        //       int backcolor		输出窗口的背景色。（清屏颜色）
        //       int mode			备用参数。默认0
        // 返回: 是否成功
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static bool	 mcDisplayInit(IntPtr outWindow,int backcolor,int mode);

        //-----------------------------------
        // 函数: mcDisplayShow
        // 功能: 预览当前节目
        // 备注：可用mcSetCurMcv()设定当前正在编辑的节目。 并且，预览前不要调用mcCloseMcv!否则什么也看不到。
        //		 预览是实时的，预览前不必保存节目。
        // 参数: 
        //       int pageid		如果节目有多页，可以指定一个起始页。默认0
        //       bool repeate   是否重复播放该页。（因为用户可能要一直编辑其中一页，不希望自动翻页）默认false
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static void	 mcDisplayShow(int pageid,bool repeate);


        //-----------------------------------
        // 函数: mcDisplayDraw
        // 功能: 刷新预览窗口
        // 备注：刷新的频率决定了帧率。 刷的越快，画面越流畅，CPU占用率会高一点。该函数通常放在定时器中。(不要另外开线程刷，因为刷新要在主线程中进行)
        // 参数: 
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static void	 mcDisplayDraw();


        //-----------------------------------
        // 函数: mcDisplayStop
        // 功能: 停止预览当前节目。
        // 备注：
        // 参数: 
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static void	 mcDisplayStop();


        //////////////////////////////////////////////////////////////////////////

        //-----------------------------------
        // 函数: mcConnectBox
        // 功能: 与盒子建立临时连接 （由电脑主动发起连接，连到盒子）
        // 备注：该函数不耗时，会立即返回。 连接成功后，会通知MCEVENT_CONNECT消息 (要事先在mcinit()时注册TOnMcEvent接口)。
        //		临时连接最多允许512个。太多临时连接会降低连接性能。
        //		SDK默认的连接是服务器方式，电脑做为服务器监听指端口，各个盒子事先设置好自动回连。
        //
        // 参数: 
        //       const char * IP		盒子IP
        //       int port				盒子的端口(默认8100)
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Ansi)] 
        public extern static	void	 mcConnectBox(string IP,int port);


        //-----------------------------------
        // 函数: mcDisConnectBox
        // 功能: 断开与盒子的临时连接
        // 备注：
        // 参数: 
        //       HBOX boxno
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static	void	 mcDisConnectBox(int boxno);



        //-----------------------------------
        // 函数: mcFindBoxByIP
        // 功能: 根据IP查找盒子。若没找到，返回0
        // 备注：
        // 参数: 
        //       const char * IP
        // 返回: 盒子在线编号HBOX
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Ansi)] 
        public extern static	int		 mcFindBoxByIP(string IP);


        //获取盒子是否在线
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static	bool		 mcBoxIsConnect(int boxno);

        //-----------------------------------
        // 函数: mcBoxSetTime
        // 功能: 设置盒子时钟 
        // 备注：该函数会使盒子重启,需重新连接。
        // 参数: 
        //       HBOX boxno
        //       const char * stime		当前时间，格式(19个字符)：2016-06-12 12:08:16
        // 返回: 是否设置完毕
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Ansi)] 
        public extern static bool		 mcBoxSetTime(int boxno,string stime);


         
        //-----------------------------------
        // 函数: mcBoxSetAutoConnect
        // 功能: 设置盒子自动回连
        // 备注：会引发自动重连。如果设置为停止回连，就需要手动输入IP,用mcConnectBox建立连接。
        // 参数: 
        //       HBOX boxno		盒子代号
        //       const char * svrIP		服务器IP或网址 若为NULL，则终止回连
        //       int port	服务器端口
        // 返回: 是否设置完毕
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Ansi)] 
        public extern static bool		 mcBoxSetAutoConnect(int boxno,string svrIP,int port);


        //清空当前继电器设置，准备重新设置
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static void		 mcBoxClearRelay(int boxno);

        //-----------------------------------
        // 函数: mcBoxAddRelayTime
        // 功能: 增加一项继电器定时设置(可以多项). 
        // 备注：各项时间段不能互相冲突，起始时间应顺序递增
        // 参数: 
        //       int relayNo		继电器编号，支持两路(1或2)
        //       int beginHour		起始时间_小时部分
        //       int beginMin		起始时间_分钟部分
        //       int onoff			开关(0-关，1-开)
        // 返回: [DllImport("McControl.dll", CharSet = CharSet.Unicode)] public extern static void		
        // 示例： 早上7点开，晚上22点关
        //		mcBoxAddRelayTime(1,7,0,1);
        //		mcBoxAddRelayTime(1,22,0,0);
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static void		 mcBoxAddRelayTime(int boxno,int relayNo,int beginHour,int beginMin,int onoff);


        //-----------------------------------
        // 函数: mcBoxSendRelayTime
        // 功能: 发送继电器定时设置
        // 备注：在各项定时mcBoxAddRelayTime之后，调用该函数上传设置
        // 参数: 
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static bool		 mcBoxSendRelayTime(int boxno);

        //取消继电器定时设置
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static void		 mcBoxStopRelayTime(int boxno);

        //-----------------------------------
        // 函数: mcBoxReboot
        // 功能: 重启播放盒
        // 备注：重启后，需要重新连接
        // 参数: 
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static void		 mcBoxReboot(int boxno);


        //-----------------------------------
        // 函数: mcBoxSetRebootTime
        // 功能: 设置每天定时重启
        // 备注：
        // 参数: 
        //       int hour	重启时间，几点几分
        //       int min
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static void		 mcBoxSetRebootTime(int boxno,int hour,int min);


        //-----------------------------------
        // 函数: mcBoxGetMcvList
        // 功能: 获得盒子上当前节目清单
        // 备注：
        // 参数: 
        //       HBOX boxno
        //       int timeout	阻塞时间ms, 由于从盒子上读回节目清单需要一定的时间，所以需要一个超时。调用线程会阻塞。默认1500ms
        // 返回: 获取的节目个数。 -1:获取失败(超时) 0-没有节目 >=1节目数
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static int		 mcBoxGetMcvList(int boxno,int timeout);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct TMcProgramInfo
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
            public string   name;  //节目名(节目文件夹名称)
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
            public string   date;	//上传时间，如: 2016-07-12 12:01:32
	        int				size;	//大小(字节)
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
	        public string	rem;	//节目备注信息
            //
        };


        //-----------------------------------
        // 函数: mcBoxGetMcvInfo
        // 功能: 获取节目信息
        // 备注：目前只能从盒子读回节目名等信息，然后你可以显示节目清单，切换当前节目。
        //		 无法从盒子下载节目内容。 所以盒子上的节目要在本机上备份，否则没办法修改。
        // 参数: 
        //       HBOX boxno		盒子代号
        //       int index		节目序号。 (index>=0 并且index < 节目个数) (先用上面mcBoxGetMcvList()获取节目个数)
        //       TMcProgramInfo * tf
        // 返回: 是否获得， 如果index非法返回false
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static bool		 mcBoxGetMcvInfo(int boxno,int index,ref TMcProgramInfo tf);


        //////////////////////////////////////////////////////////////////////////
        //
        //	下面一系列 mcMcv.... 函数，用来访问/修改以前节目内容。
        //
        //////////////////////////////////////////////////////////////////////////

        //-----------------------------------
        // 函数: mcMcvGetPageCount
        // 功能: 返回当前正在编辑的节目的节目页个数
        // 备注：事先mcCreateMcv()或mcLoadMcv()，并且mcSetCurMcv()
        // 参数: 
        // 返回: [DllImport("McControl.dll", CharSet = CharSet.Unicode)] public extern static int	
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static int	 mcMcvGetPageCount();

        //-----------------------------------
        // 函数: mcMcvGetPageId
        // 功能: 返回指定序号的节目页的id,以便使用mcAddPageTimer,mcDelPage等函数
        // 备注：
        // 参数: 
        //       int index
        // 返回: pageid (>=1), 若找不到返回0
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static int	 mcMcvGetPageId(int index);

        //删除指定页
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static bool	 mcMcvDelPage(int pageid);

        //指定节目页，返回它里面的节目对象个数 
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static int	 mcMcvGetObjCount(int pageid);


        //-----------------------------------
        // 函数: mcMcvGetObjId
        // 功能: 返回某页中指定序号对象的id
        // 备注：
        // 参数: 
        //       int pageid
        //       int index	序号，0 ~ page.objcount-1
        //
        // 返回: objid (>=1) 若找不到，返回0
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static int	 mcMcvGetObjId(int pageid,int index);

        public enum TMcObjType {
	        mcobj_Null,		//未知
	        mcobj_Page,		//节目页
	        mcobj_TextBox,	//文字框
	        mcobj_ImageBox,	//图像框
	        mcobj_VideoBox,	//视频框
	        mcobj_NoticeBox,//广告框(图文框)
	        mcobj_InfoBox,	//信息框
	        mcobj_SoundBox,	//音乐框
	        mcobj_ClockBox,	//模拟时钟
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct TMcObjInfo 
        {
	        int			id;		//对象id
	        TMcObjType	type;	//类型
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
	        string		name;//名称
	        int			left,top,width,height;//大小
        };



        //-----------------------------------
        // 函数: mcMcvGetObjInfo
        // 功能: 获得指定对象的信息
        // 备注：
        // 参数: 
        //       int objid		对象的id，由mcMcvGetObjId()获得，或者由mcAdd....系列函数返回的id
        //       TMcObjInfo * tf
        // 返回: [DllImport("McControl.dll", CharSet = CharSet.Unicode)] public extern static bool	
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static bool	 mcMcvGetObjInfo(int objid,ref TMcObjInfo tf);

        //删除指定对象
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)] 
        public extern static void	 mcMcvDelObj(int objid);

        //-----------------------------------
        // 函数: mcBoxSetRelay   
        // 功能: 发送继电器状态
        // 备注：调用该函数上传设置  add by [zz]
        // 参数: 
        //       int relayNo		继电器编号，支持两路(1或2)
        //       int onoff			开关(0-关，1-开)
        // 返回: 
        //-----------------------------------
        [DllImport("McControl.dll", CharSet = CharSet.Unicode)]
        public extern static  void mcBoxSetRelay(int boxno, int relayNo, int onoff);
                
	
    }
}
