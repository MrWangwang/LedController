using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Test_CSharp
{
    partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            // TODO: 在此处添加代码以启动服务。
            System.IO.File.AppendAllText("D:\\car_service.txt", "服务已启动..." + DateTime.Now.ToString());
            Program program = new Program();
            program.run();
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            System.IO.File.AppendAllText("D:\\car_service.txt", "服务已停止..." + DateTime.Now.ToString());
        }
    }
}
