using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Runtime.InteropServices;
using Luxand;
using System.Net;
using System.Net.Mail;

namespace Car_Security_System
{
    public partial class Form1 : Form
    {
        public string cameraOne, cameraTwo, comPort;
        string[] cameraList1;
        string[] cameraList2;
        public int show = 0;
        public int count = 0;
        private static int fD1 = 0;
        private static string fN1;
        private static int fD2 = 0;
        private static string fN2;
        public string D;
        public string pass ="";
        public int flag = 0;
        public int ac = 0;
      public static  SerialPort serialPort1 = new SerialPort();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort1.Close();
            Application.Exit();
        }

   
      
        public static int faceDetected1
        {
            get // this makes you to access value in form2
            {
                return fD1;
            }
            set // this makes you to change value in form2
            {
                fD1 = value;
            }
        }
        public static string faceName1
        {
            get // this makes you to access value in form2
            {
                return fN1;
            }
            set // this makes you to change value in form2
            {
                fN1 = value;
            }
        }
        public static int faceDetected2
        {
            get // this makes you to access value in form2
            {
                return fD2;
            }
            set // this makes you to change value in form2
            {
                fD2 = value;
               
            }
        }
        public static string faceName2
        {
            get // this makes you to access value in form2
            {
                return fN2;
            }
            set // this makes you to change value in form2
            {
                fN2 = value;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Top = 0;
            this.Left = 0;
            
            FSDKCam.GetCameraList(out cameraList1, out count);
            comboBox1.Items.AddRange(cameraList1);
            FSDKCam.GetCameraList(out cameraList2, out count);
            comboBox2.Items.AddRange(cameraList2);
            comboBox3.Items.AddRange(SerialPort.GetPortNames());
            if (FSDK.FSDKE_OK != FSDK.ActivateLibrary("k6EA6/1PbBZRvLZsN8l/ElrP2jCEEJfy+7QHG/VE3Of+dyXk1Iu7NwNXviNYkiLJ+5WUnM2jFxIKFA68H8VMegLx4OossmhM2WgUSiTOI6dSemywHipK7GDsBzw+ikzrk0qoRzf8H/WoZZZU/lhIltVIfOrNbXeB19vr4YMuMNg="))
            {
                MessageBox.Show("Please run the License Key Wizard (Start - Luxand - FaceSDK - License Key Wizard)", "Error activating FaceSDK", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            if (0 == count)
            {
                MessageBox.Show("Please attach a camera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            while(flag != 0 )
            {
                MessageBox.Show("okay");
            }
            
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
              cameraOne = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
              cameraTwo = this.comboBox2.GetItemText(this.comboBox2.SelectedItem);
              comPort = this.comboBox3.GetItemText(this.comboBox3.SelectedItem);
              serialPort1.PortName = comPort;
              serialPort1.BaudRate = 9600;
              serialPort1.Parity = Parity.None;
              serialPort1.StopBits = StopBits.One;
              serialPort1.DataBits = 8;
              serialPort1.Handshake = Handshake.None;
              serialPort1.RtsEnable = true;
              Form2 f2 = new Form2(cameraOne, count);
              Form3 f3 = new Form3(cameraTwo, count);
              serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
              serialPort1.Open();
              f2.Show();
              f3.Show();
              


        }
        private void label()
        {
            switch (D)
            {
                case "C":
                    label4.Text = "";
                    label2.Text = "";
                    label1.Text = "";
                    pass = "";
                    break;
                case "P":
                    label4.Text = "Enter Password";
                    break;
                case "*":
                    
                    label1.Text = pass;
                    break;
                case "O":
                    label4.Text = "Welcome";
                    break;
                case "R":
                    label4.Text = "Right Password";
                    break;
                case "U":
                    label4.Text = "Car Unlocked";
                    faceDetected2 = 1;
                    break;
                case "L":
                    label4.Text = "Car Locked";
                    break;
                case "W":
                    label4.Text = "Wrong Password";
                    break;
                case "T":
                    label1.Text = "Password Timeout";
                    break;
                case "S":
                    label4.Text = "User Locked Car";
                    
                    break;
                case "D":
                    label2.Text = "Driver Ultrasonic 180";
                    faceDetected1 = 1;
                    break;
                case "N":
                    label2.Text = "Passenger Ultrasonic 0";
                    faceDetected1 = 1;
                    break;
                case "B":
                    label2.Text = "Both Ultrasonics";
                    faceDetected1 = 1;
                    break;
                case "A":
                    label7.Text = "Welcome Andrew Car is Yours";
                    break;
                default:
                    break;
            }
        }
        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.label4.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
                label();
            }
            else
            {
                D = text;
                label();
            }
        }
        private void DataReceivedHandler(
                      object sender,
                      SerialDataReceivedEventArgs e)
        {
            
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            flag = 1;
            if (indata == "*")
                pass += "*";
            else if (indata == "C")
                pass = "";
            SetText(indata);
            
            

           

        }
    

    }
}
