using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO.Ports;
using MCSDK;
using NetUtilityLib;
using System.Text;
using PingMock;
namespace Test_CSharp
{
    class Program
    {
        public string m_sExePath = "";
        //MC100测试节目的根目录 (以\结尾)
        public string m_sMcPath = "";
        //示例图片资源目录 (以\结尾)
        public string m_SamplePath = "";
        //当前正在操作的节目文件夹
        public string m_sCurMcvPath = "";
        //上传模式 0-在线 1-FTP
        public int m_upmode = 0;
        public static int m_uploadBox = 0;
        private static mc.TOnMcEvent m_event = null;//必须用static声明，否则可能会出现事件异常
        public static int pageNo = 0; //节目编号
        public static int boxNo = 0; //盒子编号
        public static String ip = "";//盒子IP
        public static String com = "";//com口
        public static SerialPort sp = null;
        public static LogClass log = new LogClass();
        public static Boolean changeImg = true;

        //构造函数
        public Program()
        {
            m_sExePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\";
            m_sMcPath = m_sExePath + "mcdata\\";
            m_SamplePath = m_sExePath + "sample\\";

            //允许多线程刷新界面
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            /**********************以服务的方式启动程序：Service1.OnStart()*********************************/
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //int result = FindMax(5, 6);
            /**----原程序注释**/

            /*Console.WriteLine("result:程序开始");
            log.WriteLogFile("程序开始启动:");
            byte[] b = new byte[1];
            //Console.ReadKey();
            com = ConfigHelper.GetAppConfig("COM");
            Console.WriteLine("读取COM口:" + com);
            log.WriteLogFile("读取COM口:"+com);
            sp = new SerialPort(com);
            Program program = null;
            try
            {
                sp.Open();
                log.WriteLogFile("打开端口成功！");
                program = new Program();
            ip = ConfigHelper.GetAppConfig("ServerIP");
            //String str = "57";
            //调用发送图片流程
            log.WriteLogFile("读取控制卡IP：" + ip); 
            program.mcInit();
            boxNo = mc.mcFindBoxByIP(ip);
            
            mc.mcConnectBox(ip, 8100);
            program.sendMvcFlow("rateLimit");
            int boxno = mc.mcFindBoxByIP(ip);
            int defaultMcv = mc.mcCreateMcv(program.m_sMcPath, "rateLimit", 160, 160, true, true);
            if (defaultMcv == 0) return;
            mc.mcSetCurMcv(defaultMcv);
            mc.mcSetDefaultMcv(boxno, "rateLimit");
                Console.WriteLine("初始化图片成功");
                log.WriteLogFile("初始化图片成功");
            }
            catch (Exception e)
            {
                Console.WriteLine("打开端口失败!:"+e.ToString());
                log.WriteLogFile("打开端口失败！" + e.ToString());
            }
            try {
                while (true)
                {
                    Console.WriteLine("开始接收无线信号数据！");
                    log.WriteLogFile("开始接收无线信号数据！");
                    //打开新的串行端口连接
                    //sp.Open();
                    //丢弃来自串行驱动程序的接受缓冲区的数据
                    sp.DiscardInBuffer();
                //丢弃来自串行驱动程序的传输缓冲区的数据
                sp.DiscardOutBuffer();
                //从串口输入缓冲区读取一些字节并将那些字节写入字节数组中指定的偏移量处
                sp.Read(b, 0, b.Length);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < b.Length; i++)
                {
                    sb.Append(Convert.ToString(b[i]) + "");
                }
                
                Console.WriteLine("接收无线信号数据:"+sb.ToString());
                log.WriteLogFile("接收无线信号数据:" + sb.ToString());
                
                program.invokeSendMvc(sb.ToString());
                    //str = "57";
                    
                    //System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                sp.Close();
                Console.WriteLine("程序错误:"+e.ToString());
                log.WriteLogFile("程序错误:"+e.ToString());
            }*/
            /******************************原程序注释结束******************************/

            //new System.EventHandler(btnConnect_Click());
            /*try
            {
                ip = ConfigHelper.GetAppConfig("ServerIP");
                program.mcInit();
                Console.WriteLine("led控制卡ip:"+ip);
            //string ip = "192.168.0.77";
            boxNo = mc.mcFindBoxByIP(ip);
            if (boxNo > 0 && mc.mcBoxIsConnect(boxNo))
            {
                Console.WriteLine("已连接");
                return;
            }
            mc.mcConnectBox(ip, 8100);
            program.setCurMcvText();
            program.sendMcv();
            m_uploadBox = program.getUploadProgress();
            //当前线程挂起500毫秒
            System.Threading.Thread.Sleep(500);
                /*while (true)
                {
                    if (m_uploadBox>=100)
                    {
                        break;
                    }
                    else
                    {
                        m_uploadBox = program.getUploadProgress();
                        Console.WriteLine("当前进度:"+m_uploadBox);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("程序错误:", e);
            }*/

        }

        public void run()
        {
            Console.WriteLine("result:程序开始");
            log.WriteLogFile("程序开始启动:");
            byte[] b = new byte[1];
            //Console.ReadKey();
            com = ConfigHelper.GetAppConfig("COM");
            Console.WriteLine("读取COM口:" + com);
            log.WriteLogFile("读取COM口:" + com);
            sp = new SerialPort(com);
            Program program = null;
            try
            {
                sp.Open();
                log.WriteLogFile("打开端口成功！");
                program = new Program();
                ip = ConfigHelper.GetAppConfig("ServerIP");
                //String str = "57";
                //调用发送图片流程
                log.WriteLogFile("读取控制卡IP：" + ip);
                program.mcInit();
                boxNo = mc.mcFindBoxByIP(ip);

                mc.mcConnectBox(ip, 8100);
                program.sendMvcFlow("rateLimit");
                int boxno = mc.mcFindBoxByIP(ip);
                int defaultMcv = mc.mcCreateMcv(program.m_sMcPath, "rateLimit", 160, 160, true, true);
                if (defaultMcv == 0) return;
                mc.mcSetCurMcv(defaultMcv);
                mc.mcSetDefaultMcv(boxno, "rateLimit");
                Console.WriteLine("初始化图片成功");
                log.WriteLogFile("初始化图片成功");
            }
            catch (Exception e)
            {
                Console.WriteLine("打开端口失败!:" + e.ToString());
                log.WriteLogFile("打开端口失败！" + e.ToString());
            }
            try
            {
                while (true)
                {
                    Console.WriteLine("开始接收无线信号数据！");
                    log.WriteLogFile("开始接收无线信号数据！");
                    //打开新的串行端口连接
                    //sp.Open();
                    //丢弃来自串行驱动程序的接受缓冲区的数据
                    sp.DiscardInBuffer();
                    //丢弃来自串行驱动程序的传输缓冲区的数据
                    sp.DiscardOutBuffer();
                    //从串口输入缓冲区读取一些字节并将那些字节写入字节数组中指定的偏移量处
                    sp.Read(b, 0, b.Length);

                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < b.Length; i++)
                    {
                        sb.Append(Convert.ToString(b[i]) + "");
                    }

                    Console.WriteLine("接收无线信号数据:" + sb.ToString());
                    log.WriteLogFile("接收无线信号数据:" + sb.ToString());

                    program.invokeSendMvc(sb.ToString());
                    //str = "57";

                    //System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                sp.Close();
                Console.WriteLine("程序错误:" + e.ToString());
                log.WriteLogFile("程序错误:" + e.ToString());
            }
        }

        //调用发送图片
        public void invokeSendMvc(String sign)
        {
            if (sign.Equals("57"))//9的ascii码,检测有车出入
            {
                Console.WriteLine("检测有车出入，更新Led图片");
                log.WriteLogFile("检测有车辆驶出，显示有车图片");
                sendMvcFlow("carAlarm");
                System.Threading.Thread.Sleep(15000);
                changeImg = true;
            }
            else//无车出入，显示当前限速
            {
                if (changeImg)
                {
                    Console.WriteLine("无车出入，更新Led图片");
                    log.WriteLogFile("无车出入，显示限速图片");
                    sendMvcFlow("rateLimit");
                    changeImg = false;
                    //System.Threading.Thread.Sleep(5000);
                }
                else
                {
                    Console.WriteLine("无车出入，当前显示为无车图片，无需更换");
                    log.WriteLogFile("无车出入，当前显示为无车图片，无需更换");
                }

            }
        }

        //发送图片完整流程
        public void sendMvcFlow(String imagName)
        {
            boxNo = mc.mcFindBoxByIP(ip);
            setCurMcvImg(imagName);
            sendMcv(imagName);
            System.Threading.Thread.Sleep(500);
            int jd = getUploadProgress();
            //当前线程挂起500毫秒
            if (jd<100)
            {
                for (int i=0;i<3;i++)
                {
                    System.Threading.Thread.Sleep(500);
                    jd = getUploadProgress();
                }
                
            }
            if (jd>=100)
            {
                Console.WriteLine("上传完成");
                log.WriteLogFile("图片上传完成");
            }
            //System.Threading.Thread.Sleep(500);
            //mc.mcDisConnectBox(boxNo);
            Console.WriteLine("结束");
            log.WriteLogFile("调用图片流程完成");
        }

        //初始化
        public  Boolean mcInit()
        {
            m_event = OnMcEvent;
            return mc.mcInit(8813, m_event, 0);
        }

        //退出
        public void mcClosed()
        {
            mc.mcClose();
        }

        //事件定义
        public  Boolean OnMcEvent(
            int eventno,   //事件类型，参见MCEVENT_CONNECT等
            int boxno,     //播放盒代号
            int para1,     //备用参数
            int para2
            )
        {
            mc.TMcBoxInfo bf = new mc.TMcBoxInfo();
            if (!mc.mcGetBoxInfo(boxno, ref bf))
            {
                return false;
            }
            String srem = bf.LedName + " " + bf.IP;
            boxNo = boxno;
            //
            switch (eventno)
            {
                case mc.MCEVENT_CONNECT:
                    {
                        ps("有连接:" + srem);
                        //onConnect(boxno);
                    }
                    break;
                case mc.MCEVENT_DISCONNECT:
                    {
                        ps("连接中断:" + srem);
                        //onDisConnect(boxno);
                    }
                    break;
            }


            return true;
        }

        //打印
        public void ps(String s)
        {
            String snow = DateTime.Now.ToString("HH:mm:ss ");
            //this.textBox1.Text += snow + s + System.Environment.NewLine;
        }

        //创建图片节目
        public void setCurMcvImg(String imageName)
        {
            string mcvname = imageName;
            string mcvpath = m_sMcPath + mcvname;
            //指定节目名以及窗口大小
            int mcv = mc.mcCreateMcv(m_sMcPath, mcvname, 160, 160, true, true);
            if (mcv == 0) return;
            mc.mcSetCurMcv(mcv);
            
            //添加节目内容
            //添加一个节目页
            pageNo = mc.mcAddPage(imageName, mc.TMcPlayMode.pm_Normal, 5, 0);
            
            //图片
            mc.TMcImage img = new mc.TMcImage();
            mc.mcGetDefaultImg(ref img);
            img.left = 0;
            img.top = 0;
            img.width = 160;
            img.height = 160;
            img.playtime = 1;//播放5秒
            int imgid = mc.mcAddImageBox(pageNo, ref img);
            if (imageName.IndexOf("carAlarm") > -1)
            {
                mc.mcAddImageFile(imgid, m_SamplePath + "carAlarm.png");
                mc.mcAddImageFile(imgid, m_SamplePath + "carAlarm2.png");
            }
            else
            {
                mc.mcAddImageFile(imgid, m_SamplePath + imageName + ".png");
            }
            //保存
            if (mc.mcSaveMcv(mcv))
            {
                m_sCurMcvPath = mcvpath;
                //MessageBox.Show("ok,已保存. " + mcvpath);
                Console.WriteLine("节目已保存:"+mcvpath);
                log.WriteLogFile("节目已保存,路径:"+mcvpath);
            }
            //关闭
            mc.mcCloseMcv(mcv);
        }

        //上传图片
        public void sendMcv(String imagName)
        {
            string mcvname = imagName;
            string mcvpath = m_sMcPath + mcvname;//mcdata/test1
            //mcStartSendMcv()
            //int boxno = getCurBox();
            //int boxno = 1;
            if (boxNo == 0) return;

            //progressBar1.Value = 0;
            if (!mc.mcStartSendMcv(boxNo, mcvpath))
            {
                //MessageBox.Show("上传失败");
                Console.WriteLine("上传失败，盒子号:"+boxNo);
                log.WriteLogFile("上传失败，盒子号："+boxNo);
            }
            //lblUpload.Text = "正在上传:" + System.IO.Path.GetFileName(mcvpath);
            //timer1.Enabled = true;
            m_upmode = 0;
            m_uploadBox = boxNo;
        }

        //查询上传进度
        public int getUploadProgress()
        {
            int jd = 0;
            jd = mc.mcGetUploadProgress(m_uploadBox);
            return jd;
        }

    }
}
