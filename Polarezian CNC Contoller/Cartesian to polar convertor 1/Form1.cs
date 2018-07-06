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
using System.Timers;
using System.Diagnostics;
using System.Collections;
using System.Windows.Input;
using System.Threading;

namespace Cartesian_to_polar_convertor_1
{
    public partial class Form1 : Form
    {
        int size = -1; int linesCount = -1; int wordCount = -1;
        double oldTempr = -1.0, oldTempt = 0.0;
        int xFlag = 0; int yFlag = 0;
        StringBuilder outputFile = new StringBuilder();
        double tempx = 0.0; double tempy = 0.0; double oldTempx = 0.0; double oldTempy = 0.0; double tempr = 0.0; double tempt = 0.0;
        double newTempr, newTempt;
        StringBuilder replaceBuilder = new StringBuilder();
        StringBuilder numberBuilder = new StringBuilder();
        Boolean isLooping = false; bool sherif;
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        int somesortofflag = -1; string temprString, temptString;
        int xflag2 = 0, yflag2 = 0, indexJ = 0, indexReplace = 0;
        string[] outputFilelines; int Slider = 0;
        Thread countThread = null;
        double distance; StringBuilder Delta = new StringBuilder();
        double x = 0.0, y = 0.0, tan, newTan; double newX, newY; double newX1, newY1;
        string sBudrate, comPort;
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(SerialPort.GetPortNames());
            


                 
                  
                
               
           
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
  
        private void button2_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {


                serialPort1.WriteLine("E");

            }
            /* int SerialsCounter = 0; int wordsCount = 0;
             string[] lines = textBox4.Text.Split('\n'); 
       SerialsCounter = lines.Length;
             MessageBox.Show(SerialsCounter.ToString());
             for (int i = 0; i < SerialsCounter; i++)
             {
                 wordsCount = lines[i].Length;
                 for (int j = 0; j < wordsCount; j++)
                 {
                 }

                 }*/
           
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox4.Clear();
            
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    string text = System.IO.File.ReadAllText(file);
                    size = text.Length;
                    string[] line = text.Split('\n');
                    linesCount = line.Length;
                    wordCount = line[0].Length;
                    for (int i = 0; i < linesCount; i++)
                    {
                        wordCount = line[i].Length;
                        outputFile.Append("#");
                        somesortofflag = -1;
                        for (int j = 0; j < wordCount; j++)
                        {
                            // index++;
                            //Console.WriteLine(index);
                            //indexJ = j;
                            if (line[i][j] == 'X')
                            {

                                if (xFlag == 1)
                                {
                                    tempx = double.Parse(numberBuilder.ToString(), System.Globalization.CultureInfo.InvariantCulture);

                                }
                                else if (yFlag == 1)
                                {
                                    tempy = double.Parse(numberBuilder.ToString(), System.Globalization.CultureInfo.InvariantCulture);

                                }
                                yFlag = 0;
                                xFlag = 1;
                                xflag2 = 1;
                                outputFile.Append(line[i][j]);


                                outputFile.Append("R");



                                numberBuilder.Clear();
                            }
                            else if (line[i][j] == 'Y')
                            {
                                if (xFlag == 1)
                                {
                                    tempx = double.Parse(numberBuilder.ToString(), System.Globalization.CultureInfo.InvariantCulture);

                                }
                                else if (yFlag == 1)
                                {
                                    tempy = double.Parse(numberBuilder.ToString(), System.Globalization.CultureInfo.InvariantCulture);

                                }
                                yFlag = 1;
                                xFlag = 0;
                                yflag2 = 1;
                                outputFile.Append(line[i][j]);

                                indexJ++;

                                outputFile.Append("@");

                                //indexJ++;
                                numberBuilder.Clear();
                            }
                            else if (((line[i][j] != 'X') && (line[i][j] != 'Y') && (line[i][j] > 64)))
                            {

                                if (xFlag == 1)
                                {
                                    tempx = double.Parse(numberBuilder.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                                }
                                else if (yFlag == 1)
                                {
                                    tempy = double.Parse(numberBuilder.ToString(), System.Globalization.CultureInfo.InvariantCulture);

                                }
                                yFlag = 0;

                                xFlag = 0;

                                //  MessageBox.Show("X" + tempx.ToString() + "Y" + tempy.ToString());
                                outputFile.Append(line[i][j]);

                                indexJ++;
                                numberBuilder.Clear();
                            }
                            else
                            {
                                if (xFlag == 1 || yFlag == 1)
                                {
                                    numberBuilder.Append(line[i][j]);

                                }
                                else
                                {
                                    if (line[i][j] != '\r')
                                        outputFile.Append(line[i][j]);



                                }
                            }
                            if ((wordCount - 1) == j)
                            {
                                if (xFlag == 1)
                                {
                                    tempx = double.Parse(numberBuilder.ToString(), System.Globalization.CultureInfo.InvariantCulture);

                                }
                                else if (yFlag == 1)
                                {
                                    tempy = double.Parse(numberBuilder.ToString(), System.Globalization.CultureInfo.InvariantCulture);

                                }


                                yFlag = 0;
                                xFlag = 0;
                                //  MessageBox.Show("X" + tempx.ToString() + "Y" + tempy.ToString());
                                numberBuilder.Clear();
                                // outputFile.Append("#");
                            }
                        }

                        if (xflag2 == 1 && yflag2 == 0)
                        {
                            outputFile.Append("Y@");
                            outputFile.AppendLine();


                        }
                        else if (yflag2 == 1 && xflag2 == 0)
                        {
                            outputFile.Append("XR");
                            outputFile.AppendLine();

                        }
                        else
                        {
                            outputFile.AppendLine();
                        }
                        xflag2 = 0;
                        yflag2 = 0;
                        tan = tempy / tempx;
                        if (tempy == 0)
                            tan = 0;
                        x = Math.Pow(tempx, 2);
                        y = Math.Pow(tempy, 2);
                        //    Console.WriteLine(tempx.ToString());
                        //    Console.WriteLine(tempy.ToString());


                        tempr = Math.Sqrt((x + y));
                        tempt = Math.Atan(tan) * (180 / Math.PI);
                        if (tempx < 0 && tempy > 0)
                            tempt = Math.Abs(tempt) + 90;
                        else if (tempx < 0 && tempy < 0)
                            tempt = Math.Abs(tempt) + 180;
                        else if (tempx > 0 && tempy < 0)
                            tempt = Math.Abs(tempt) + 270;

                        outputFile.Replace("R", tempr.ToString());
                        temprString = tempr.ToString();



                        outputFile.Replace("@", tempt.ToString());
                        //oldTempr = -1;
                        if (oldTempr == tempr && tempt != oldTempt)
                        {
                            oldTempt = tempt;
                            //  MessageBox.Show(tempr.ToString());
                            //  replaceBuilder.AppendLine();
                            replaceBuilder.Append("X");
                            newX = (tempx + oldTempx) / 2;
                            newY = (tempy + oldTempy) / 2;
                            newTan = newY / newX;
                            if (newY == 0)
                                newTan = 0;
                            newX1 = Math.Pow(newX, 2);
                            newY1 = Math.Pow(newY, 2);
                            newTempr = Math.Sqrt((newX1 + newY1));

                            newTempt = Math.Atan(newTan) * (180 / Math.PI);
                            if (newX < 0 && newY >= 0)
                                newTempt = Math.Abs(newTempt) + 180;
                            else if (newX < 0 && newY < 0)
                                newTempt = Math.Abs(newTempt) + 180;
                            else if (newX >= 0 && newY < 0)
                                newTempt = Math.Abs(newTempt) + 180;
                            //outputFilelines = outputFile.ToString().Split('\n');
                            tempt = newTempt;
                            replaceBuilder.Append(newTempr.ToString());
                            replaceBuilder.Append("Y");
                            replaceBuilder.Append(newTempt);
                            replaceBuilder.AppendLine();
                            //indexReplace = (outputFilelines[(outputFilelines.Length)-1].Length)-2;
                            outputFile.Replace("#", replaceBuilder.ToString());
                            replaceBuilder.Clear();
                            somesortofflag = 0;
                            oldTempx = tempx;
                            oldTempy = tempy;

                            // Console.WriteLine(outputFile[indexReplace-2]);

                        }
                        else
                            outputFile.Replace("#", "");
                        indexJ = 0;
                        if (tempr != 0 && somesortofflag != 0)
                        {
                            oldTempx = tempx;
                            oldTempy = tempy;
                            oldTempr = tempr;
                            oldTempt = tempt;
                        }
                    }

                }

                catch (IOException)
                {
                }
            }
            // outputFile.Replace( @"^\s+$[\r\n]*", "");
            Console.WriteLine(outputFile.ToString()); // <-- Shows file size in debugging mode.
                                                      //Console.WriteLine(Math.Atan(1.0).ToString()); // <-- For debugging use.
            
            textBox4.Text = outputFile.ToString();
            chart1.Series["Series1"].Points.AddXY(45 + 90, 28.2842712474619);
            chart1.Series["Series1"].Points.AddXY(18.434948822922 + 90, 63.2455532033676);
            chart1.Series["Series1"].Points.AddXY(45 + 90, 84.8528137423857);
            chart1.Series["Series1"].Points.AddXY(71.565051177078 + 90, 63.2455532033676);
            chart1.Series["Series1"].Points.AddXY(45 + 90, 28.2842712474619);
        }

     

      

        private void Up_MouseDown(object sender, MouseEventArgs e)
        {
            /* isLooping = true;
             loop();*/

            isLooping = true;
            countThread = new Thread(() =>
            {
                while (isLooping)
                {
                    if (serialPort1.IsOpen == true)
                    {


                        Byte b = 0b0100_0010;
                        for (int i = 0; i < Slider; i++)
                            serialPort1.WriteLine(">");

                    }
                    //Thread.Sleep(100);
                }
            });
            countThread.Start();
           
                //Thread.Sleep(100);
            
        }

        private void Up_MouseUp(object sender, MouseEventArgs e)
        {
            isLooping = false;
            countThread.Join();
            //   isLooping = false;
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
                serialPort1.Close();
                button2.Enabled = true;
                connectToolStripMenuItem.Enabled = true;
                disconnectToolStripMenuItem.Enabled = false;
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int zint;
            zint = int.Parse(Zbox.Text);
            if (serialPort1.IsOpen == true)
            {
                if (zint < 0)
                {
                    zint *= -1;
                    zint *= 200;
                    for (int i = 0; i < zint; i++)
                        serialPort1.WriteLine("<");
                }
               else if (zint > 0)
                {
                    zint *= 200;
                    for (int i = 0; i < zint; i++)
                        serialPort1.WriteLine(">");
                }
            }
        }


        private void button4_MouseUp(object sender, MouseEventArgs e) // z down out
        {
            isLooping = false;
            countThread.Join();
        }

        private void button4_MouseDown(object sender, MouseEventArgs e) // Z down pressed
        {

            isLooping = true;
            countThread = new Thread(() =>
            {
                while (isLooping)
                {
                    if (serialPort1.IsOpen == true)
                    {


                        Byte b = 0b0100_0011;
                        for (int i = 0; i < Slider; i++)
                            serialPort1.WriteLine("<");

                    }
                    //Thread.Sleep(100);
                }
            });
            countThread.Start();

            //Thread.Sleep(100);

        }

        private void button7_MouseDown(object sender, MouseEventArgs e) // theta left pressed
        {

            isLooping = true;
            countThread = new Thread(() =>
            {
                while (isLooping)
                {
                    if (serialPort1.IsOpen == true)
                    {


                        Byte b = 0b0100_1100;
                        for (int i = 0; i < Slider; i++)
                            serialPort1.WriteLine("S");

                    }
                    //Thread.Sleep(100);
                }
            });
            countThread.Start();

            //Thread.Sleep(100);

        }

        private void button8_MouseDown(object sender, MouseEventArgs e) // theta right pressed
        {

            isLooping = true;
            countThread = new Thread(() =>
            {
                while (isLooping)
                {
                    if (serialPort1.IsOpen == true)
                    {


                        Byte b = 0b0100_1000;
                        for (int i = 0; i < Slider; i++)
                            serialPort1.WriteLine("W");

                    }
                    //Thread.Sleep(100);
                }
            });
            countThread.Start();

            //Thread.Sleep(100);

        }

        private void button10_MouseDown(object sender, MouseEventArgs e) // left R pressed
        {

            isLooping = true;
            countThread = new Thread(() =>
            {
                while (isLooping)
                {
                    if (serialPort1.IsOpen == true)
                    {


                        Byte b = 0b0111_0000;
                        for (int i = 0; i < Slider; i++)
                            serialPort1.WriteLine("A");

                    }
                    //Thread.Sleep(100);
                }
            });
            countThread.Start();

            //Thread.Sleep(100);

        }

        private void button11_MouseDown(object sender, MouseEventArgs e) // right R pressed
        {

            isLooping = true;
            countThread = new Thread(() =>
            {
                while (isLooping)
                {
                    if (serialPort1.IsOpen == true)
                    {


                        Byte b = 0b0110_0000;
                        for (int i = 0; i < Slider; i++)
                            serialPort1.WriteLine("D");

                    }
                    //Thread.Sleep(100);
                }
            });
            countThread.Start();

            //Thread.Sleep(100);

        }

        private void button7_MouseUp(object sender, MouseEventArgs e)
        {
            isLooping = false;
            countThread.Join();
        }

        private void button8_MouseUp(object sender, MouseEventArgs e)
        {
            isLooping = false;
            countThread.Join();
        }

        private void conecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
                MessageBox.Show("Machine is connected");
            else
                MessageBox.Show("Machine is disconnected");
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comPort = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            serialPort1.PortName = comPort;
            serialPort1.BaudRate = 38400;
            serialPort1.Parity = Parity.None;
            serialPort1.DataBits = 8;
            serialPort1.Open();
            if (serialPort1.IsOpen)
            {
                button2.Enabled = false;
                connectToolStripMenuItem.Enabled = false;
                disconnectToolStripMenuItem.Enabled = true;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("C");
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            Slider = trackBar1.Value;
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            int tint;
            tint = int.Parse(Tbox.Text);
            if (serialPort1.IsOpen == true)
            {
                if (tint < 0)
                {
                    tint *= -1;
                    tint *= 400;
                    for (int i = 0; i < tint; i++)
                        serialPort1.WriteLine("W");
                }
               else if (tint > 0)
                {
                    tint *= 400;
                    for (int i = 0; i < tint; i++)
                        serialPort1.WriteLine("S");
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
             int rint;
            rint = int.Parse(Rbox.Text);
            if (serialPort1.IsOpen == true)
            {
                if (rint < 0)
                {
                    rint *= -1;
                    rint *= 200;
                    for (int i = 0; i < rint; i++)
                        serialPort1.WriteLine("A");
                }
               else  if (rint > 0)
                {
                    rint *= 200;
                    for (int i = 0; i < rint; i++)
                        serialPort1.WriteLine("D");
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(SerialPort.GetPortNames());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button10_MouseUp(object sender, MouseEventArgs e)
        {
            isLooping = false;
            countThread.Join();
        }

        private void button11_MouseUp(object sender, MouseEventArgs e)
        {
            isLooping = false;
            countThread.Join();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("       Polarezian Version 0.01 \n \n       Made By Mohamed Hosny ");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
           
        }

            
            
        private void Up_Click(object sender, EventArgs e)
        {

        /*    isLooping = true;
            countThread = new Thread(() =>
            {
                while (isLooping)
                {
                    if (serialPort1.IsOpen == true)
                    {


                        Byte b = 0b0100_0010;
                        for (int i = 0; i < Slider; i++)
                            serialPort1.WriteLine(",");

                    }
                    //Thread.Sleep(100);
                }
            });
            countThread.Start();

            //Thread.Sleep(100);
            */
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            comPort = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            serialPort1.PortName = comPort;
            serialPort1.BaudRate = 38400;
            serialPort1.Parity = Parity.None;
            serialPort1.DataBits = 8;
            serialPort1.Open();
            if(serialPort1.IsOpen)
            {
                button2.Enabled = false;
                connectToolStripMenuItem.Enabled = false;
                disconnectToolStripMenuItem.Enabled = true;
            }
          
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (radioButton1.Checked)
            {
                while (keyData == Keys.Left)
                {
                    if (serialPort1.IsOpen == true)
                    {


                       // Byte b = 0b0111_0000;
                        for (int i = 0; i < Slider; i++)
                            serialPort1.WriteLine("A");

                    }
                    //Thread.Sleep(100);
                    return true;
                }
                while (keyData == Keys.Right)
                {
                    if (serialPort1.IsOpen == true)
                    {


                        //Byte b = 0b0110_0000;
                        for(int i=0; i<Slider;i++)
                        serialPort1.WriteLine("D");

                    }
                    //Thread.Sleep(100);
                    return true;
                }
                while (keyData == Keys.Up)
                {
                    if (serialPort1.IsOpen == true)
                    {


                       // Byte b = 0b0100_1100;
                        for (int i = 0; i < Slider; i++)
                            serialPort1.WriteLine("W");

                    }
                    //Thread.Sleep(100);

                    return true;
                }
                while (keyData == Keys.Down)
                {
                    if (serialPort1.IsOpen == true)
                    {


                        //Byte b = 0b0100_1000;
                            for (int i = 0; i < Slider; i++)
                            serialPort1.WriteLine("S");

                    }
                    //Thread.Sleep(100); return true;
                }
                while (keyData == Keys.PageUp)
                {
                    if (serialPort1.IsOpen == true)
                    {


                        //Byte b = 0b0100_0010;
                        for (int i = 0; i < Slider; i++)
                            serialPort1.WriteLine(">");

                    }
                    //Thread.Sleep(100); return true;
                }
                while (keyData == Keys.PageDown)
                {
                    if (serialPort1.IsOpen == true)
                    {


                       // Byte b = 0b0100_0011;
                        for (int i = 0; i < Slider; i++)
                            serialPort1.WriteLine("<");

                    }
                    //Thread.Sleep(100);

                    return true;
                }
                if(keyData == Keys.Enter)
                {
                    if (serialPort1.IsOpen == true)
                    {


                        serialPort1.WriteLine("E");

                    }
                    //Thread.Sleep(100);

                    return true;
                }
                if (keyData == Keys.Space)
                {
                    if (serialPort1.IsOpen == true)
                    {

                        
                            serialPort1.WriteLine("C");

                    }
                    //Thread.Sleep(100);

                    return true;
                }
                return base.ProcessCmdKey(ref msg, keyData);
            }
            else if (radioButton2.Checked)
                return false;
            else
                return false;
        }
        
    }
}
