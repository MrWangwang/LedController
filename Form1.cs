using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;   

using MCSDK;

namespace Test_CSharp
{
    public partial class Form1 : Form
    {
        public string m_sExePath = "";
        //MC100测试节目的根目录 (以\结尾)
        public string m_sMcPath = "";
        //示例图片资源目录 (以\结尾)
        public string m_SamplePath = "";
        //当前正在操作的节目文件夹
        public string m_sCurMcvPath = "";
        //上传模式 0-在线 1-FTP
        public int m_upmode=0;
        public int m_uploadBox=0;
        //
        private static mc.TOnMcEvent m_event = null;//必须用static声明，否则可能会出现事件异常
        public bool m_IsInitDisplay = false;
        public Form1()
        {
            InitializeComponent();
            m_event = this.OnMcEvent;

            m_sExePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath)+"\\";
            m_sMcPath = m_sExePath+"mcdata\\";
            m_SamplePath = m_sExePath+"sample\\";

            //允许多线程刷新界面
            Control.CheckForIllegalCrossThreadCalls = false;

            //初始化
            mc.mcInit(8813, m_event, 0);//d9
            //
            this.grid1.View = View.Details;
            this.grid1.HideSelection = false;
            this.grid1.FullRowSelect = true;
            this.grid1.Columns.Add("IP", 120, HorizontalAlignment.Left); 
            this.grid1.Columns.Add("LED名称", 200, HorizontalAlignment.Left);
            this.grid1.Columns.Add("状态", 200, HorizontalAlignment.Left);

	        m_IsInitDisplay=false;
	        if (mc.mcDisplayInit(this.pnlScreen.Handle,this.pnlScreen.BackColor.ToArgb(),0))
	        {
		        m_IsInitDisplay=true;
	        }


        }

        public void ps(String s)
        {
            String snow = DateTime.Now.ToString("HH:mm:ss ");
            this.textBox1.Text += snow + s + System.Environment.NewLine;
        }

        public Boolean OnMcEvent(
             int eventno,   //事件类型，参见MCEVENT_CONNECT等
             int boxno,     //播放盒代号
             int para1,     //备用参数
             int para2
             )
        {
	        mc.TMcBoxInfo bf=new mc.TMcBoxInfo();
	        if (!mc.mcGetBoxInfo(boxno,ref bf))
	        {
                return false;
	        }
	        String srem=bf.LedName+" "+bf.IP;
	        //
	        switch (eventno) {
	        case mc.MCEVENT_CONNECT:
	        {
		        ps("有连接:"+srem);
                onConnect(boxno);
	        }
	        break;
	        case mc.MCEVENT_DISCONNECT:
	        {
                ps("连接中断:" + srem);
                onDisConnect(boxno);
	        }
	        break;
	        }


            return true;
        }

        //box数组，记录所有连线过的box (永不删除，盒子下线后也留着）
        private ArrayList m_idlist = new ArrayList();

        public void onConnect(int boxno)
        {
            //收到连接，把盒子IP名字填到列表中
            int idx = GetRow(boxno);
            if (idx < 0)
            {
                m_idlist.Add(boxno);

                mc.TMcBoxInfo bf=new mc.TMcBoxInfo();
                if (!mc.mcGetBoxInfo(boxno, ref bf))
                {
                    return;
                }
                this.grid1.BeginUpdate(); 
                ListViewItem lvi = new ListViewItem();
                lvi.Text = bf.IP;
                lvi.SubItems.Add(bf.LedName);
                lvi.SubItems.Add("在线");
                this.grid1.Items.Add(lvi);
                this.grid1.EndUpdate();
            }
            else
            {
                int r = idx;
                grid1.Items[r].SubItems[2].Text = "在线";
            }
        }


        public void onDisConnect(int boxno)
        {
            //断开连接，更新在线状态
            int idx = GetRow(boxno);
            if (idx < 0)
            {
                return;
            }
            int r = idx;
            int count = grid1.Items.Count;
            //grid1->Cells[2][r] = "";
            grid1.Items[r].SubItems[2].Text = "";
        }

        //查找boxno 在显示列表中第几行，准备输出
        int		GetRow(int boxno)
        {
	        for (int i=0;i<m_idlist.Count;i++)
	        {
                int no = (int)m_idlist[i];
		        if (no==boxno)
		        {
			        return i;
		        }
	        }
	        return -1;
        }

        int		GetBox(int row)
        {
	        if (row<0 || row>=m_idlist.Count)  return 0;
	        return (int)m_idlist[row];
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //退出时，通知底层释放
            mc.mcClose();
        }

        private void button1_Click(object sender, EventArgs e)
        {

	        string mcvname="test1";
	        string mcvpath=m_sMcPath+mcvname;
	        //指定节目名以及窗口大小
	        int mcv = mc.mcCreateMcv(m_sMcPath,mcvname,160,160,true,true);
	        if (mcv ==0 ) return;
	        mc.mcSetCurMcv(mcv);
	        //添加节目内容
	        //添加一个节目页
	        int pageno=mc.mcAddPage("",mc.TMcPlayMode.pm_Normal,1,0);
	        //添加一个文字框
	        /*mc.TMcText	text=new mc.TMcText();
	        mc.mcGetDefaultText(ref text);
	        text.left=10;
	        text.top=10;
	        text.width=150;
	        text.height=68;
	        text.backcolor=0x80FF;
	        text.alpha=255;
	        //text.fontname="楷体";
            text.fontname = "";
	        text.fontsize=32;
	        mc.mcAddSingleText(pageno,"欢迎使用 MC100 SDK",-1,ref text);
            */
	        //图片
	        mc.TMcImage	img=new mc.TMcImage();
	        mc.mcGetDefaultImg(ref img);
	        img.left=0;
	        img.top=0;
	        img.width=160;
	        img.height=160;
	        int imgid=mc.mcAddImageBox(pageno,ref img);
	        mc.mcAddImageFile(imgid,m_SamplePath+"test1.jpg");

            
	        //视频
           /* mc.TMcVideo video = new mc.TMcVideo();
	        mc.mcGetDefaultVideo(ref video);
	        video.left=250;
	        video.top=170;
	        video.width=160;
	        video.height=120;
	        int videoid=mc.mcAddVideoBox(pageno,ref video);
	        mc.mcAddVideoFile(videoid,m_SamplePath+"test2.mp4",100);

	        //时间
	        mc.TMcInfo	info = new mc.TMcInfo();
	        mc.mcGetDefaultInfo(ref info);
	        info.left=50;
	        info.top=15;
	        info.width=300;
	        info.height=32;
	        mc.mcAddInfoBoxTime(pageno,mc.TMcDateFmt.mcdate_time,ref info);
            */
	        //
	        //保存
	        if (mc.mcSaveMcv(mcv))
	        {
		        m_sCurMcvPath=mcvpath;
		        MessageBox.Show("ok,已保存. "+mcvpath);
	        }
	        //关闭
	        //mc.mcCloseMcv(mcv);//先不关闭，以便预览

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string mcvname = "test2";
            string mcvpath = m_sMcPath + mcvname;
            //指定节目名以及窗口大小
            int mcv = mc.mcCreateMcv(m_sMcPath, mcvname, 640, 360,true,true);
            if (mcv == null) return;

            mc.mcSetCurMcv(mcv);
            //添加节目内容
            //添加一个节目页
            int pageno = mc.mcAddPage("", mc.TMcPlayMode.pm_Normal, 1, 0);


            //添加RTF文本
            mc.TMcNotice notice=new mc.TMcNotice();
            mc.mcGetDefaultNotice(ref notice);
            notice.left = 0;
            notice.top = 0;
            notice.width = 640;
            notice.height = 360;
            int noticeid = mc.mcAddNoticeBox(pageno, ref notice);
            mc.mcAddNoticeRTF(noticeid, m_SamplePath + "通告.rtf", mc.TMcAniMode.ani_XiangShangYiDong,0,255,0xFFFFFF,2,10);
            //保存
            if (mc.mcSaveMcv(mcv))
            {
                m_sCurMcvPath = mcvpath;
                MessageBox.Show("ok,已保存. " + mcvpath);
            }
            //关闭
            //mc.mcCloseMcv(mcv);//先不关闭，以便预览
        }

        public int getCurBox()
        {
            int r = -1;
            if (grid1.SelectedItems.Count > 0)
            {
                r = grid1.SelectedItems[0].Index;
            }
            int boxno = GetBox(r);
            if (boxno == 0)
            {
                MessageBox.Show("请先选择盒子");
                return 0;
            }
            return boxno;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string mcvname = "test1";
            string mcvpath = m_sMcPath + mcvname;//mcdata/test1
            //mcStartSendMcv()
            int boxno = getCurBox();
            if (boxno == 0) return;
           
            progressBar1.Value = 0;
            if (!mc.mcStartSendMcv(boxno,mcvpath))
            {
                MessageBox.Show("上传失败");
            }
            lblUpload.Text = "正在上传:" + System.IO.Path.GetFileName(mcvpath);
            timer1.Enabled = true;
            m_upmode = 0;
            m_uploadBox = boxno;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //刷新进度
            if (m_uploadBox!=0)
            {

                int jd = 0;
                if (m_upmode == 0)
                {
                    jd = mc.mcGetUploadProgress(m_uploadBox);
                    lblUpload.Text = "正在下载..";
                }
                if (m_upmode == 1)
                {
                    jd = mc.mcGetProgressFTP(m_uploadBox);
                    int state = mc.mcGetFTPState(m_uploadBox);
                    //0-无 1-正下载 2-正常结束 3-连结中 4-连接异常/无法登录  5-停止取消 6-磁盘满  7-目录错误
                    string s1 = "无FTP下载";
                    switch (state)
                    {
                        case 1: s1 = "正下载"; break;
                        case 2: s1 = "已结束"; break;
                        case 3: s1 = "连结中"; break;
                        case 4: s1 = "FTP异常"; break;
                        case 5: s1 = "已取消"; break;
                        case 6: s1 = "磁盘满"; break;
                        case 7: s1 = "目录错误"; break;
                    }
                    lblUpload.Text = s1;
                }

                System.Diagnostics.Debug.WriteLine("jd="+jd);
                progressBar1.Value = jd * progressBar1.Maximum / 100;
                if (jd >= 100)
                {
                    m_uploadBox = 0;
                    lblUpload.Text = "已结束.";
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
	        //假定test1节目已经经保存到FtpRoot下
            string svrpath = "/test1";
	        //mcStartSendMcv()
            int boxno = getCurBox();
            if (boxno == 0) return;

	        mc.TMcFTP ftp=new mc.TMcFTP();
	        ftp.svrip = "192.168.0.8";
	        ftp.port = 21;
	        ftp.user = "xlq";
	        ftp.pwd = "8888";
            ftp.charset = "GBK";

	        //
            progressBar1.Value = 0;

	        if (!mc.mcStartFTP(boxno,svrpath,"",1,ref ftp))
	        {
		        MessageBox.Show("上传失败");
	        }
	        lblUpload.Text ="由FTP下载:"+svrpath;
            timer1.Enabled = true;
	        m_upmode=1;
	        m_uploadBox=boxno;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (timer2.Enabled)
            {
                timer2.Enabled = false;
                return;
            }
            int boxno = getCurBox();
            if (boxno == 0) return;
            timer2.Enabled = true;
        }

        static int m_tno = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
	        int boxno=getCurBox();
            if (boxno==0)
            {
                timer2.Enabled=false;
                return;
            }
            m_tno++;
            string txt = "动态文字 " + m_tno;
	        mc.mcSendSingleText(boxno,0,"文字框1",txt);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int boxno = getCurBox();
            if (boxno == 0) return;
            if (m_upmode == 0)
            {
                mc.mcStopUpload(boxno);
            }
            if (m_upmode == 1)
            {
                mc.mcStopFTP(boxno);
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (m_IsInitDisplay)
            {
                mc.mcDisplayShow(0,false);
                timerDisplay.Enabled = true;
            }

        }

        private void timerDisplay_Tick(object sender, EventArgs e)
        {
            mc.mcDisplayDraw();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timerDisplay.Enabled = false;
            mc.mcDisplayStop();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text;
            int boxno = mc.mcFindBoxByIP(ip);
            if (boxno > 0 && mc.mcBoxIsConnect(boxno))
            {
                MessageBox.Show("该盒子已连接");
                return;
            }
            mc.mcConnectBox(ip, 8100);
        }


    }
}
