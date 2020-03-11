using CrossCutting;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SystemMonitorWindowsClientSimulator
{
    public partial class Form1 : Form
    {
        SystemMonitor monitorAll = new SystemMonitor();
        string dateAndTime;
        string filespec;
        CancellationTokenSource cts = new CancellationTokenSource();
        string path = Application.StartupPath + @"\Dumps";
    
        public Form1()
        {
            InitializeComponent();
         
        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            monitorAll.ProcessDumps();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

           
        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dateAndTime = DateTime.Now.ToString("MMM-dd-yyyy-hh-mm-ss");
            filespec = path + @"\" + dateAndTime + ".dmp";
            File.Create(filespec).Close();
            using (StreamWriter writetext = new StreamWriter(filespec))
            {
                writetext.WriteLine("Normal dump");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dateAndTime = DateTime.Now.ToString("MMM-dd-yyyy-hh-mm-ss");
            filespec = path + @"\" + dateAndTime + ".d£mp";
            File.Create(filespec).Close();
            using (StreamWriter writetext = new StreamWriter(filespec))
            {
                writetext.WriteLine("Corrupt dump");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dateAndTime = DateTime.Now.ToString("MMM-dd-yyyy-hh-mm-ss");
            filespec = path + @"\" + dateAndTime + ".dmp";
            File.Create(filespec).Close();
            using (StreamWriter writetext = new StreamWriter(filespec))
            {
                writetext.WriteLine("Exception thrown the Crash File Logger");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dateAndTime = DateTime.Now.ToString("MMM-dd-yyyy-hh-mm-ss");
            filespec = path + @"\" + dateAndTime + ".d£mp";
            File.Create(filespec).Close();
            using (StreamWriter writetext = new StreamWriter(filespec))
            {
                writetext.WriteLine("Exception thrown the Corrupt File Logger");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (cts != null)
            {
                cts.Cancel();
            }
            Environment.Exit(0);
        }
    }
}
