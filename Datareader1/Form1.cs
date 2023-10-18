using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;


namespace Datareader1
{
    public partial class Form1 : Form
    {
 
        SerialPort COMPort = new SerialPort();

        StreamWriter logFile;
        string fileName = "";
        string logData = "";

        public Form1()
        {
            InitializeComponent();
            if (Directory.Exists("data") == false)
            {
                Directory.CreateDirectory("data");
            }

            timer1.Interval = 15 * 60  * 1000;

            DateTime timeStamp = DateTime.Now;
            string dt = timeStamp.ToString("yyyy_MM_dd_HH_mm_ss");
            fileName ="data/"+ dt + ".csv";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] arrayComPortNames = null;
            arrayComPortNames = SerialPort.GetPortNames();
            cmbPort.DataSource = arrayComPortNames;

        }





        private void DataReceivedHandler(object sender,SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            logData = indata;

            DateTime timeStamp = DateTime.Now;
            string dt = timeStamp.ToString("dd / MM / yyyy HH: mm: ss.fff");
            string log_date = timeStamp.ToString("dd / MM / yyyy");
            string log_time = timeStamp.ToString("HH: mm: ss.fff");

            string dataLog = dt+" : " + indata;

            ConsoleUpdate(dataLog);
        }

        public void ConsoleUpdate(string dataLog)
        {
            
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action<string>(ConsoleUpdate), dataLog);
            }
            else
            {
                richTextBox1.AppendText(dataLog);
                writeCSV(logData);
            }

           
        }

        public void writeCSV(string text)
        {
            if (text.Length > 0)
            {
                if (text.Substring(0, 1) == "#")
                {
                    DateTime timeStamp = DateTime.Now;
                    string dt = timeStamp.ToString("dd / MM / yyyy HH: mm: ss.fff");
                    string log_date = timeStamp.ToString("dd / MM / yyyy");
                    string log_time = timeStamp.ToString("HH: mm: ss.fff");

                    string logText = log_date + "," + log_time + "," + text;


                    logFile = new StreamWriter(fileName, true);
                    logFile.Write(logText);
                    logFile.Close();
                }
            }
         
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime timeStamp = DateTime.Now;
            string dt = timeStamp.ToString("yyyy_MM_dd_HH_mm_ss");
            fileName ="data/"+ dt + ".csv";

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (COMPort.IsOpen == true)
                {
                    COMPort.Close();
                }

                COMPort.PortName = cmbPort.Text;
                COMPort.BaudRate = 9600;

                COMPort.Open();
                toolStripStatusLabel1.Text = COMPort.PortName + " connected.";
                COMPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                btnConnect.Enabled = false;
                btnStop.Enabled = true;
            }
            catch(Exception ex)
            {
                toolStripStatusLabel1.Text = "Com port connection error";
                MessageBox.Show("Com port connection error\n" + ex.Message.ToString());
                btnConnect.Enabled = true;
                btnStop.Enabled = false;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (COMPort.IsOpen == true)
                {
                    COMPort.Close();

                    toolStripStatusLabel1.Text = "Com port disconeected.";

                    btnConnect.Enabled = true;
                    btnStop.Enabled = false;
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void cmbPort_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbPort_Click(object sender, EventArgs e)
        {
            string[] arrayComPortNames = null;
            arrayComPortNames = SerialPort.GetPortNames();
            cmbPort.DataSource = arrayComPortNames;
        }
    }
}
