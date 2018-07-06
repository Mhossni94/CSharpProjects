using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Runtime.InteropServices;
using Luxand;

namespace Car_Security_System
{
    public partial class Form2 : Form
    {
        // program states: whether we recognize faces, or user has clicked a face
        enum ProgramState { psRemember, psRecognize }
        ProgramState programState = ProgramState.psRecognize;
        String camera ;
        bool needClose = false;
        string[] cameraList1;
        string userName;
        String TrackerMemoryFile = "Andrew.dat";
       public int count;

        // WinAPI procedure to release HBITMAP handles returned by FSDKCam.GrabFrame
        [DllImport("gdi32.dll")]
        static extern bool DeleteObject(IntPtr hObject);
        public Form2(string cameraName, int cameraCount)
        {
            InitializeComponent();
            camera = cameraName;
            count = cameraCount;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
           
            this.Top = 535;
            this.Left = 0;
            Form1 f = new Form1();
            
            if (FSDK.FSDKE_OK != FSDK.ActivateLibrary("k6EA6/1PbBZRvLZsN8l/ElrP2jCEEJfy+7QHG/VE3Of+dyXk1Iu7NwNXviNYkiLJ+5WUnM2jFxIKFA68H8VMegLx4OossmhM2WgUSiTOI6dSemywHipK7GDsBzw+ikzrk0qoRzf8H/WoZZZU/lhIltVIfOrNbXeB19vr4YMuMNg="))
            {
                MessageBox.Show("Please run the License Key Wizard (Start - Luxand - FaceSDK - License Key Wizard)", "Error activating FaceSDK", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            FSDK.InitializeLibrary();
            FSDKCam.InitializeCapturing();
            FSDKCam.GetCameraList(out cameraList1, out count);
            


            if (0 == count)
            {
                MessageBox.Show("Please attach a camera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }




        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            needClose = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Camera intialization
            this.button1.Enabled = false;           
            FSDKCam.VideoFormatInfo[] formatList;
            FSDKCam.GetVideoFormatList(ref camera, out formatList, out count);
            // choose a video format
            int VideoFormat = 0;
            pictureBox1.Width = formatList[VideoFormat].Width;
            pictureBox1.Height = formatList[VideoFormat].Height;
            this.Width = formatList[VideoFormat].Width + 48;
            this.Height = formatList[VideoFormat].Height + 96;
            int cameraHandle1 = 0;

            int r1 = FSDKCam.OpenVideoCamera(ref camera, ref cameraHandle1);
            if (r1 != FSDK.FSDKE_OK)
            {
                MessageBox.Show("Error opening the first camera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Application.Exit();
            }
           
            // creating a Tracker
            int tracker1 = 0;
            if (FSDK.FSDKE_OK != FSDK.LoadTrackerMemoryFromFile(ref tracker1, TrackerMemoryFile)) // try to load saved tracker state
                FSDK.CreateTracker(ref tracker1); // if could not be loaded, create a new tracker
           
            int err = 0; // set realtime face detection parameters
            FSDK.SetTrackerMultipleParameters(tracker1, "HandleArbitraryRotations=false; DetermineFaceRotationAngle=false; InternalResizeWidth=100; FaceDetectionThreshold=5;", ref err);
            while (!needClose)
            {
                Int32 imageHandle1 = 0;
                if (FSDK.FSDKE_OK != FSDKCam.GrabFrame(cameraHandle1, ref imageHandle1)) // grab the current frame from the camera
                {

                    Application.DoEvents();
                    continue;
                }
                FSDK.CImage image1 = new FSDK.CImage(imageHandle1);
                long[] IDs;
                long faceCount = 0;
                FSDK.FeedFrame(tracker1, 0, image1.ImageHandle, ref faceCount, out IDs, sizeof(long) * 256);
                Array.Resize(ref IDs, (int)faceCount);

                // make UI controls accessible (to find if the user clicked on a face)
                Application.DoEvents();

                Image frameImage1 = image1.ToCLRImage();
                Graphics gr1 = Graphics.FromImage(frameImage1);
                for (int i = 0; i < IDs.Length; ++i)
                {
                    FSDK.TFacePosition facePosition1 = new FSDK.TFacePosition();
                    FSDK.TFacePosition facePosition2 = new FSDK.TFacePosition();
                    FSDK.GetTrackerFacePosition(tracker1, 0, IDs[i], ref facePosition1);
                    int left1 = facePosition1.xc - (int)(facePosition1.w * 0.6);
                    int top1 = facePosition1.yc - (int)(facePosition1.w * 0.5);
                    int w1 = (int)(facePosition1.w * 1.2);

                    String name;
                    int res1 = FSDK.GetAllNames(tracker1, IDs[i], out name, 65536); // maximum of 65536 characters
                    if (FSDK.FSDKE_OK == res1 && name.Length > 0)
                    { // draw name
                        StringFormat format = new StringFormat();
                        format.Alignment = StringAlignment.Center;
                     //   Form1.faceDetected1 = 1;
                        Form1.faceName1 = name;
                        if (Form1.faceDetected1 == 1)
                        {
                            Form1 f = new Form1();
                            //  f.detected();
                            if (Form1.serialPort1.IsOpen)
                            {


                                Form1.serialPort1.Write("F");
                            }
                            else
                            {
                                MessageBox.Show("Camera 1 can't send");
                            }
                            Form1.faceDetected1 = 0;
                        }
                            gr1.DrawString(name, new System.Drawing.Font("Arial", 16),
                            new System.Drawing.SolidBrush(System.Drawing.Color.LightGreen),
                            facePosition1.xc, top1 + w1 + 5, format);

                    }
                    

                    Pen pen = Pens.LightGreen;

                    if (ProgramState.psRemember == programState) // capture image
                    {
                        if (FSDK.FSDKE_OK == FSDK.LockID(tracker1, IDs[i]))
                        {
                            // get the user name
                            
                            userName = textBox1.Text;
                            if (userName == null || userName.Length <= 0)
                            {
                                String s = "";
                                FSDK.SetName(tracker1, IDs[i], "");
                                FSDK.PurgeID(tracker1, IDs[i]);
                            }
                            else
                            {
                                FSDK.SetName(tracker1, IDs[i], userName);
                            }
                            FSDK.UnlockID(tracker1, IDs[i]);
                            
                        }
                        
                    }

                    gr1.DrawRectangle(pen, left1, top1, w1, w1);
                  
                }
                programState = ProgramState.psRecognize;

                // display current frame
                pictureBox1.Image = frameImage1;
                GC.Collect(); // collect the garbage after the deletion
            }
            FSDK.SaveTrackerMemoryToFile(tracker1, TrackerMemoryFile);

            FSDK.FreeTracker(tracker1);


            FSDKCam.CloseVideoCamera(cameraHandle1);

            FSDKCam.FinalizeCapturing();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            programState = ProgramState.psRemember;
        }
    }
}
