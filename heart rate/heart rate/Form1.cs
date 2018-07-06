using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
namespace heart_rate
{
    public partial class Form1 : Form
    {
        int ECG = 0, HR = 0;
        double[] ECGBuffer = new double[2000];
        double[] HRBuffer = new double[2000];
        string comPort;
        long HRP = 0;
        int avgHR;
        int avgSPO;
        List<int> hrMoving = new List<int>();
        List<int> hrplotMoving = new List<int>();
        List<int> ecgplotMoving = new List<int>();
        List<int> spoMoving = new List<int>();
        int flag=0,flag1=0;
        int nhbp = 120, nlbp = 80;
        int hbp, lbp;
        int NMax = 0, NMin = 1000;
        double SMax = 0, SMin = 0;
        int EMax = 0, EMin = 1000;
        double ESMax = 0, ESMin = 0;
        Stopwatch HRTime = new Stopwatch();
        Stopwatch Reset = new Stopwatch();
        Stopwatch session = new Stopwatch();
        int count = 0; int sessiontime = 3;
        List<int> sp = new List<int>();
        int spo1;
        string filepath1 = "HR.txt";
        string filepath2 = "ECG.txt";
        string photo = Path.Combine(Environment.CurrentDirectory, "1.png");
        string toFile1 = "";
        string path1;
        string toFile2 = "";
        string path2;
    
        
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(SerialPort.GetPortNames());

            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.Maximum = 300;
            chart1.ChartAreas[0].AxisY.Interval = 10;
            Connection.ForeColor = Color.Red;
            Connection.Text = "Disconnected";
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comPort = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            serialPort1.PortName = comPort;
            serialPort1.BaudRate = 115200;
            serialPort1.Parity = Parity.None;
            serialPort1.DataBits = 8;
            serialPort1.Open();
            if (serialPort1.IsOpen)
            {

                button2.Enabled = false;
                button3.Enabled = true;
                Connection.ForeColor = Color.Green;
                Connection.Text = "Connected";
                time.Text = "3 Minutes";
                Reset.Start();
                session.Start();
                timer2.Enabled = true;
                timer1.Enabled = true;
                
                
            }
            
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                button2.Enabled = true;
                button3.Enabled = false;
                Connection.ForeColor = Color.Red;
                Connection.Text = "Disconnected";
                session.Stop();
                timer1.Enabled = false;
                Reset.Stop();
                sessiontime = 3;
                count = 0;
                timer2.Enabled = false;
                toFile1 = "";
                toFile2 = "";
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            UpdateGraph();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            sessiontime--;
            time.Text = sessiontime + " Minutes";
            count++;
            if (count == 3)
            {
                serialPort1.Close();
                button2.Enabled = true;
                button3.Enabled = false;
                Connection.ForeColor = Color.Red;
                Connection.Text = "Disconnected";
                session.Stop();
                timer1.Enabled = false;
                timer2.Enabled = false;
                sessiontime = 3;
                count = 0;
                Reset.Stop();
                path1 = Path.Combine(Environment.CurrentDirectory, filepath1);
                path2 = Path.Combine(Environment.CurrentDirectory, filepath2);
                    TextWriter tw1 = new StreamWriter(path1);
                    tw1.WriteLine(toFile1);
                    tw1.Close();

                    TextWriter tw2 = new StreamWriter(path2);
                    tw2.WriteLine(toFile2);
                    tw2.Close();
                toFile1 = "";
                toFile2 = "";
            }

        }

        private void scottPlotUC1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
           
            Process.Start(photo);

           
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click_1(object sender, EventArgs e)
        {

        }


        private void Form1_Load(object sender, EventArgs e)
        {

           
        }
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {

            if (serialPort1.IsOpen == true)
            {
                for (int i = 0; i < serialPort1.BytesToRead; i++)
                    sp.Add(serialPort1.ReadByte());
            }
        }

        public void UpdateGraph()
        {
            
            
            timer1.Enabled = false;
            if (sp.Count > 5)

            {
                for (int i = 0; i < sp.Count; i++)
                {
                    if (i + 5 < sp.Count)
                    {
                        
                        if (sp[i] == '$' && sp[i + 5] == '$')
                        {
                            
                            ECG = (((sp[(i + 1)]) * 256) + (sp[(i + 2)])) / 10; 
                            HR = (((sp[(i + 3)]) * 256) + (sp[(i + 4)])) / 10;
                            if (HR > 260 || HR < 40)
                            {

                            }
                            else
                            {
                                //Plotting HR
                                hrplotMoving.Add((int)HR);

                                if (hrplotMoving.Count == 10)
                                {

                                    HR = (int)hrplotMoving.Average();
                                    hrplotMoving.RemoveAt(0);
                                }
                                else
                                    HR = (int)hrplotMoving.Average();
                                toFile1 += HR+" ";
                                chart1.Series["Heart Rate"].Points.AddY(HR);
                                if (chart1.Series["Heart Rate"].Points.Count() > 1500)
                                {
                                    chart1.Series["Heart Rate"].Points.RemoveAt(0);
                                }

                                if (HR > NMax)
                                    NMax = HR;
                                 if (HR < NMin)
                                    NMin = HR;
                                if (SMax - SMin < 6)
                                {
                                    avgHR = 0;
                                    hrMoving.Clear();
                                    spoMoving.Clear();
                                    HeartRate.Text = avgHR.ToString();
                                    SPO.Text = "----------";
                                    BP.Text = "---/--";
                                    disease.Text = "-----";
                                    flag1 = 0;
                                    HRTime.Stop();
                                }
                                else
                                {
                                    if (HR >= SMax)
                                    {
                                        if (flag1 == 0)
                                        {
                                            HRTime.Start();
                                            flag1 = 1;
                                        }
                                        else if (flag == 1)
                                        {

                                            if (HRTime.ElapsedMilliseconds != 0)
                                                HRP = 60000 / HRTime.ElapsedMilliseconds;
                                            hrMoving.Add((int)HRP);

                                            if (hrMoving.Count == 10)
                                            {

                                                avgHR = (int)hrMoving.Average();
                                                hrMoving.RemoveAt(0);
                                               
                                            }
                                            else {
                                                avgHR = (int)hrMoving.Average();
                                            }
                                            if (avgHR > 75)
                                            {
                                                hbp = 120 + (int)(((nhbp - avgHR) * 0.25));
                                                lbp = 80 + (int)(((lbp - avgHR) * 0.25) * 0.1);
                                                BP.Text = hbp + "/" + lbp;
                                            }
                                            else
                                            {
                                                hbp = 120 - (int)(((nhbp - avgHR) * 0.25));
                                                lbp = 80 - (int)(((lbp - avgHR) * 0.25) * 0.1);
                                                BP.Text = hbp + "/" + lbp;
                                            }
                                            HeartRate.Text = avgHR.ToString();
                                            spo1 = (int) (avgHR * 1.4);
                                            
                                            if (spo1 > 98)
                                                spo1 = 98;
                                            spoMoving.Add(spo1);
                                            if (spoMoving.Count == 20)
                                            {

                                                avgSPO = (int)spoMoving.Average();
                                                spoMoving.RemoveAt(0);
                                            }
                                            else
                                            avgSPO = (int)spoMoving.Average();
                                            SPO.Text = avgSPO.ToString() + " %";
                                            if (avgHR <60 )
                                            {
                               
                                                disease.Text = "Low Heart Rate";
                                            }
                                            else if (avgHR > 59 && avgHR < 81)
                                            {
                                              
                                                disease.Text = "Normal";
                                            }
                                            else if (avgHR > 80)
                                            {
                                               
                                                disease.Text = "Hight Heart Rate";
                                            }
                                         

                                            HRTime.Restart();
                                        }
                                        flag = 0;

                                    }

                                    else if (HR <= SMin)
                                    {
                                        flag = 1;

                                    }
                                }
                                if (Reset.ElapsedMilliseconds > 5000)
                                {
                                    SMax = ((NMax - NMin) * 0.8) + NMin;
                                    SMin = ((NMax - NMin) * 0.2) + NMin;
                                    NMin = 1000;
                                    NMax = 0;
                                    ESMax = ((EMax - EMin) * 0.8) + EMin;
                                    ESMin = ((EMax - EMin) * 0.2) + EMin;
                                    EMin = 1000;
                                    EMax = 0;
                                    Reset.Restart();
                                }
                            }
                            //Plotting ECG
                            ecgplotMoving.Add((int)ECG);
                            if (HR > EMax)
                                EMax = HR;
                            else if (HR < EMin)
                                EMin = HR;
                            if (ESMax - ESMin < 6)
                                QRS.Text = "-----";
                            else
                                QRS.Text = "0.9 Sec";
                                if (ecgplotMoving.Count == 20)
                            {

                                ECG = (int)ecgplotMoving.Average();
                                ecgplotMoving.RemoveAt(0);
                            }
                            else
                                ECG = (int)ecgplotMoving.Average();
                            toFile2 += ECG + " ";
                           chart1.Series["ECG"].Points.AddY(ECG);
                            if (chart1.Series["ECG"].Points.Count() > 1500)
                            {
                                chart1.Series["ECG"].Points.RemoveAt(0);
                            }
                          
                        }


                    }
                }
            }
            sp.Clear();
            Application.DoEvents();
           
            timer1.Enabled = true;
        }
        

            private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(SerialPort.GetPortNames());
        }

    }
}
