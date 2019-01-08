using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication1
{
    public partial class 
        Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public Bitmap sobel(int width, int height, Bitmap bmp, Bitmap outimg)
        {
            for (int y = 0; y <= height - 1; y++)
            {
                for (int x = 0; x <= width - 1; x++)
                {
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        outimg.SetPixel(x, y, Color.FromArgb(0,0,0));
                    }
                    else
                    {
                        int r1=Math.Abs((bmp.GetPixel(x + 1, y - 1).R + 2 * bmp.GetPixel(x + 1, y).R + bmp.GetPixel(x + 1, y + 1).R)-
                            (bmp.GetPixel(x - 1, y - 1).R + 2 * bmp.GetPixel(x - 1, y).R +bmp.GetPixel(x - 1, y + 1).R)) + 
                            Math.Abs((bmp.GetPixel(x - 1, y + 1).R + 2 * bmp.GetPixel(x, y + 1).R + bmp.GetPixel(x + 1, y + 1).R)-
                            (bmp.GetPixel(x - 1, y - 1).R+2* bmp.GetPixel(x, y - 1).R+ bmp.GetPixel(x + 1, y - 1).R));
                        if (r1 > 255) r1 = 255;
                        if (r1 < 0) r1 = 0;
                        outimg.SetPixel(x, y, Color.FromArgb(r1, r1, r1));
                    }
                }
            }
            return outimg;
        }
        public int[,] Calculate_rho_and_theta(int x , int y,int[,] arry)
        {
            //for (double i = 0.1; i <= 255.0; i = i + 0.1)
            //{
                for (double j = 0.0; j <= 360.0-1.0; j = j + 0.1)
                {
                    double rho = x*Math.Cos(j) +y* Math.Sin(j);
                    //Console.WriteLine("rho: "+rho +"theta: "+j+ "x*Math.Cos(j): " + x * Math.Cos(j) + "y* Math.Sin(j): " + y * Math.Sin(j));
                    //arry[,]= arry[,]+1
                    arry[(int)rho+362,(int)(j*10)] = arry[(int)rho + 362, (int)(j*10)]+1;

                }
            //}
            return arry;
        }
        public int[,] Init_Hough_Transform_Count_Array(int[,] count_array)
        {
            for (int i = 0; i <= count_array.GetLength(0) - 1; i++)
            {
                for (int j = 0; j <= count_array.GetLength(1) - 1; j++)
                {
                    count_array[i, j] = 0;
                }
            }
            return count_array;
        }
        int max = 0;
        float max_theta = 0;
        float max_rho = 0;
        int[] max_count_point = new int[] { 0,0};
        int rho_len = 0;
        public int[,] Hough_Transform(int width, int height, Bitmap sobel_img, Bitmap outimg)
        {
            
            if (height*1.42> width * 1.42) { rho_len = (int) (height*1.42); }
            else { rho_len = (int)(width * 1.42); }
            int[,] count_array = new int[rho_len*2, 360];
            Console.WriteLine("init count_array start");
            count_array = Init_Hough_Transform_Count_Array(count_array);
            Console.WriteLine("init count_array end");
            Console.WriteLine(width+" "+ width+" "+ count_array.GetLength(0)+" "+ count_array.GetLength(1));
            
            for (int y = 0; y <= height - 1; y++)
            {
                for (int x = 0; x <= width - 1; x++)
                {
                    if(sobel_img.GetPixel(x, y).R == 255)
                    {
                        //Console.WriteLine("in 255:");
                        //count_array = Calculate_rho_and_theta(x,y, count_array);
                        for (int j = 0; j <= 360-1; j = (j +1))
                        {
                            //Console.WriteLine("j= "+j);
                            float rho = (float)(x * Math.Cos(j * Math.PI / 180.0) + y * Math.Sin(j * Math.PI / 180.0));
                            //Console.WriteLine("rho: "+rho +"theta: "+j+ "x*Math.Cos(j): " + x * Math.Cos(j) + "y* Math.Sin(j): " + y * Math.Sin(j));
                            //arry[,]= arry[,]+1
                            count_array[(int)rho + rho_len, (int)(j )] = count_array[(int)rho + rho_len, (int)(j )] + 1;
                            if(max< count_array[(int)rho + rho_len, (int)(j)])
                            {
                                max = count_array[(int)rho + rho_len, (int)(j)];
                                max_theta = j;
                                max_rho = rho;
                                max_count_point[0] = x;
                                max_count_point[1] = y;
                            }
                        }
                    }
                }
            }
           // Array.Sort(count_array);
            //Array.Reverse(count_array);
            Console.WriteLine("Max: " + max + "\nmax_theta: " + max_theta + "\nmax_rho: " + max_rho);
            return count_array;
        }
        private void button1_Click(object sender, EventArgs e)
        { 
            string lsAppDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            Console.WriteLine("Path" + lsAppDir);
            try
            {
                Bitmap bm1 = new Bitmap(lsAppDir + "/test4.jpg");
                int w1 = bm1.Width;
                int h1 = bm1.Height;
                Console.WriteLine(w1 + "," + h1);
                for (int yy = 0; yy <= h1 - 1; yy++)
                {
                    for (int xx = 0; xx <= w1 - 1; xx++)
                    {
                        //Console.WriteLine(y+","+x);
                        Color c1 = bm1.GetPixel(xx, yy);
                        int r1 = c1.R;
                        int g1 = c1.G;
                        int b1 = c1.B;
                        int avg1 = (r1 + g1 + b1) / 3;
                        bm1.SetPixel(xx, yy, Color.FromArgb(avg1, avg1, avg1));
                    }
                    //Console.WriteLine("ttt"+y);
                }
                Console.WriteLine("PathPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrr");
                Bitmap sobel_img = sobel(w1, h1, bm1, new Bitmap(w1, h1));
                Console.WriteLine("sobel_img end");
                int[,] Hough_Transform_array= Hough_Transform(w1, h1, sobel_img, new Bitmap(w1, h1));
                Console.WriteLine("Hough_Transform end");
                Console.WriteLine("write start");
                FileStream fs = new FileStream(lsAppDir + "/Hough_Transform_Count_Array.txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                 //開始寫入
                for(int i=0;i<= Hough_Transform_array.GetLength(0) - 1; i++)
                {
                    for(int j=0;j<= Hough_Transform_array.GetLength(1) - 1; j++)
                    {
                        sw.Write(Hough_Transform_array[i,j]+" , ");
                    }
                    sw.Write("\n");
                }
                 //清空緩衝區
                sw.Flush();
                 //關閉流
                sw.Close();
                fs.Close();
                Console.WriteLine("write end");
                Bitmap Result = new Bitmap(w1, h1);
                Image ori_gray = (Image)bm1;
                ori_gray.Save(lsAppDir + "/ori_gray.png");
                Image sobel_image = (Image)sobel_img;
                sobel_image.Save(lsAppDir + "/sobel_image.png");
                System.Collections.ArrayList myAL = new System.Collections.ArrayList();
                for (int y = 0; y <= h1 - 1; y++)
                {
                    for (int x = 0; x <= w1 - 1; x++)
                    {
                        if (sobel_img.GetPixel(x, y).R == 255)
                        {
                            if ((int)max_rho == (int)(float)(x * Math.Cos(max_theta * Math.PI / 180.0) + y * Math.Sin(max_theta * Math.PI / 180.0)))
                            {
                                myAL.Add(x+","+y);
                                Console.WriteLine(x + "," + y);
                            }
                        }
                    }
                }
                for (int i = 0; i <= myAL.Count - 1; i++)
                {
                    int x_pixel = Int32.Parse(myAL[i].ToString().Split(',')[0]);
                    int y_pixel = Int32.Parse(myAL[i].ToString().Split(',')[1]);
                    sobel_img.SetPixel(x_pixel, y_pixel, Color.FromArgb(255, 0, 255));
                    //sobel_img.SetPixel(x_pixel+1, y_pixel, Color.FromArgb(255, 0, 255));
                    //sobel_img.SetPixel(x_pixel - 1, y_pixel, Color.FromArgb(255, 0, 255));
                    //sobel_img.SetPixel(x_pixel + 2, y_pixel, Color.FromArgb(255, 0, 255));
                    //sobel_img.SetPixel(x_pixel - 2, y_pixel, Color.FromArgb(255, 0, 255));
                    //Console.WriteLine(myAL[i]);
                }
                Image Result_image = (Image)sobel_img;
                Result_image.Save(lsAppDir + "/Result_image.png");
                int[,] Hough_Transform_line_array= new int[myAL.Count, 360];
                Console.WriteLine("max_count_point= " + max_count_point[0] + "," + max_count_point[1]);
                Console.WriteLine("max count: " + max);
                Console.WriteLine("max count: " + max_count_point);
                for (int j = 0; j <= 360 -1; j = (int)(j +1))
                {
                    //float rho = (float)(max_count_point[0] * Math.Cos(j * Math.PI / 180.0) + max_count_point[1] * Math.Sin(j * Math.PI / 180.0));
                    //Hough_Transform_line_array[0,(int)(j)] = (int)rho;
                    for (int i=0;i<= myAL.Count - 1; i++)
                    {
                        int x_pixel = Int32.Parse(myAL[i].ToString().Split(',')[0]);
                        int y_pixel = Int32.Parse(myAL[i].ToString().Split(',')[1]);
                        float rho = (float)(x_pixel * Math.Cos(j * Math.PI / 180.0) + y_pixel * Math.Sin(j * Math.PI / 180.0));
                        Hough_Transform_line_array[i, (int)(j)] = (int)rho;
                    }
                    //rho = (float)(54 * Math.Cos(j * Math.PI / 180.0) + 165 * Math.Sin(j * Math.PI / 180.0));
                    //Hough_Transform_line_array[1, (int)(j)] = (int)rho;
                }
                Console.WriteLine("cala Hough_Transform_line_array end");
                /*int[,] array = new int[,] {
                    {1,8,9,7,105,11,50,999,500,1},
                    {12,15,11,18,733,5,4,3,2,500} };
                */

                //標題 最大數值
                int count = myAL.Count;
                if (count > 150) { count = 150; }
                System.Windows.Forms.DataVisualization.Charting.Series[] series = new System.Windows.Forms.DataVisualization.Charting.Series[myAL.Count];
                //System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series("第一條線", rho_len);
                //System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series("第二條線", rho_len);
                //int count = 10;
                //設定線條顏色
                for (int i=0 ; i<= count-1; i++)
                {
                    series[i] = new System.Windows.Forms.DataVisualization.Charting.Series("第"+(i+1)+"條線", rho_len);
                    series[i].Color = Color.Blue;
                    series[i].Font = new System.Drawing.Font("新細明體", 14);
                    series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    //series[i].IsValueShownAsLabel = true;
                }
                //series1.Color = Color.Blue;
                //series2.Color = Color.Red;

                //設定字型
                //series1.Font = new System.Drawing.Font("新細明體", 14);
                //series2.Font = new System.Drawing.Font("標楷體", 12);

                //折線圖
                //series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                //series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                //將數值顯示在線上
                //series1.IsValueShownAsLabel = true;
                //series2.IsValueShownAsLabel = true;


                //將數值新增至序列
                /*for (int index = 0; index <= Hough_Transform_line_array.GetLength(1)-1; index++)
                {
                    series1.Points.AddXY(index, Hough_Transform_line_array[0,index]);
                    series2.Points.AddXY(index, Hough_Transform_line_array[1, index]);
                    
                }*/
                for (int i = 0; i < count - 1; i++)
                {
                    for (int index = 0; index <= Hough_Transform_line_array.GetLength(1) - 1; index++)
                    {
                        series[i].Points.AddXY(index, Hough_Transform_line_array[i, index]);
                    }
                }
                Console.WriteLine("test end");
                //將序列新增到圖上
                //this.chart1.Series.Add(series1);
                //this.chart1.Series.Add(series2);
                for(int i = 0; i <= count - 1; i++)
                {
                    this.chart1.Series.Add(series[i]);
                }
                //標題
                this.chart1.Titles.Add("rho-theta");
                ChartArea area = this.chart1.ChartAreas[0];
                area.AxisX.MajorGrid.LineWidth = 0;
                area.AxisX.Maximum = 360;
                area.AxisX.Minimum = 0;
                area.AxisX.Interval = 10;
                area.AxisY.MajorGrid.LineWidth = 0;
                //area.AxisY.Maximum = 360;
                //area.AxisY.Minimum = 0;
                area.AxisY.Interval = 10;
                Console.WriteLine("end"+Math.Sin(30 * Math.PI / 180.0));
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
