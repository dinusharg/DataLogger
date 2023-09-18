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
        SerialPort mySerialPort;
        StreamWriter logFile;
        string fileName = "";

        public Form1()
        {
            InitializeComponent();
            DateTime timeStamp = DateTime.Now;
            string dt = timeStamp.ToString("yyyy_MM_dd_HH_mm_ss");
            fileName = dt + ".csv";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mySerialPort = new SerialPort("COM8", 9600);
            


        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                mySerialPort.Open();

            }
            catch (IOException ex)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string line = mySerialPort.ReadLine();
            richTextBox1.AppendText(line);
        }

        private void DataReceivedHandler(object sender,SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            //Console.WriteLine("Data Received:");
            //Console.Write(indata);
            ConsoleUpdate(indata);
        }

        public void ConsoleUpdate(string text)
        {
            if(richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action<string>(ConsoleUpdate), text);
            }
            else
            {
                richTextBox1.AppendText(text);
                writeCSV(text);
            }

           
        }

        public void writeCSV(string text)
        {
            DateTime timeStamp = DateTime.Now;
            //string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string dt = timeStamp.ToString("dd / MM / yyyy HH: mm: ss.fff");
            logFile = new StreamWriter(fileName, true);
            logFile.Write(dt + ","+ text);
            logFile.Close();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime timeStamp = DateTime.Now;
            string dt = timeStamp.ToString("yyyy_MM_dd_HH_mm_ss");
            fileName = dt + ".csv";

        }
    }
}
