using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;

namespace BabyMonitoring
{
    public partial class Form2 : Form
    {

        int b1c = 0, byte_counter = 0, discard_flag = 0, time_flag = 0, first_byte_flag = 0;double  counter = 0;
        char[] recieved_bytes = new char[7];
        Char temp;
        
        StringBuilder s = new StringBuilder();
        Stopwatch timer = new Stopwatch();
       
        // byte[] recieved_bytes = new byte[];
        public Form2(string name, string number , string portname)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            Form1 f = new Form1();
            label17.Text = name;       
            label11.Text = number;
            serialPort1.PortName = portname;
            
        }
        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
        }

        public void button1_Click(object sender, EventArgs e)
        {


            if (b1c % 2 == 0)
            {
                TransparencyKey = SystemColors.Control;
                chart1.Series["Tempreture"].Color = Color.Blue;
                chart1.ChartAreas[0].AxisY.Minimum = 30;
                chart1.ChartAreas[0].AxisY.Maximum = 40;
                button1.ForeColor = Color.Green;
                button1.Text = "On";
                TopMost = true;
                serialPort1.Open();
               // timer.Start();
            }
            else
            {
                TransparencyKey = Color.DarkSeaGreen;
                button1.ForeColor = Color.Red;
                button1.Text = "Off";
                TopMost = false;
                serialPort1.Close();
                timer.Reset();
              
            }
            if (serialPort1.IsOpen == true)
            {
                if (serialPort1.BytesToRead != 0)
                   
            }


            b1c++;
        }
                  
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
        }

        private void SET_Click(object sender, EventArgs e)
        {
          
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void byte_recieved(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

            MessageBox.Show("a7a howa fe a");
            /* int nbrDataRead = serialPort1.Read(recieved_bytes, 0, 15);
             */
            first:
            if (first_byte_flag == 0)
            {
                first_byte_flag = 1;
                byte_counter = 0;
                timer.Start();
                label18.Text = "";
            }
            if ((byte_counter < 6)&& (timer.ElapsedMilliseconds <250))
            {
                while (serialPort1.BytesToRead != 0)
                {
                    timer.Restart();
                    recieved_bytes[byte_counter] = Convert.ToChar(serialPort1.ReadByte());
                    // MessageBox.Show(recieved_bytes[byte_counter].ToString());
                    s.Append(recieved_bytes[byte_counter].ToString());
                    byte_counter++;
                    if(byte_counter == 6)
                    {
                        if (serialPort1.BytesToRead > 0)
                        {
                            serialPort1.DiscardInBuffer();
                            s.Clear();
                            first_byte_flag = 0;
                            timer.Reset();
                            goto first;
                        }
                    }
                }
            }
             if ((byte_counter == 6) && (timer.ElapsedMilliseconds < 250))
            {
               
                label16.Text = s[0].ToString() + s[1].ToString() + s[2].ToString() + "." + s[3].ToString() + " C";
                chart1.Series["Tempreture"].Points.AddXY(counter, Convert.ToDouble(s[0].ToString() + s[1].ToString() + s[2].ToString() + "." + s[3].ToString())) ;
                counter++;
                if (counter >= 100)
                    counter = 0;
                label12.Text = s[7].ToString() + s[8].ToString() + s[9].ToString() + s[10].ToString() + " C";
                label13.Text = s[12].ToString() + " Kg";
                if (s[5].ToString() == "1")
                {
                    label15.Text = "Opened";
                }
                else if (s[5].ToString() == "0")
                {
                    label15.Text = "Closed";
                }
                if (s[14].ToString() == "1")
                {
                    label14.Text = "Opened";
                }
                else if (s[14].ToString() == "0")
                {
                    label14.Text = "Closed";
                }                                                                                                           
                //MessageBox.Show(s.ToString());
                s.Clear();
                first_byte_flag = 0;
                timer.Reset();
            }
           
            if ((timer.ElapsedMilliseconds >= 250))
            {
                
                s.Clear();
                first_byte_flag = 0;
               

                label18.Text = "Error Time Limit Exceeded";
                timer.Reset();
                goto first;
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }


    }
}
