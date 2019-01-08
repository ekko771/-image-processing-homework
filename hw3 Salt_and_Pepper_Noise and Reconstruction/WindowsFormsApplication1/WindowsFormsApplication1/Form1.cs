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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public Bitmap Salt_and_Pepper_Noise(int width, int height, Bitmap bmp, Bitmap outimg)
        {
            int countt = 0;
            Random rNumber = new Random();
            for (int y = 0; y <= height - 1; y++)
            {
                for (int x = 0; x <= width - 1; x++)
                {
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        outimg.SetPixel(x, y, Color.FromArgb(bmp.GetPixel(x, y).R, bmp.GetPixel(x, y).R, bmp.GetPixel(x, y).R));
                    }
                    else
                    {
                        double Random_value = rNumber.NextDouble();
                        int Random_Color = bmp.GetPixel(x,y).R;
                        if (Random_value < 0.25) { Random_Color = 0; countt++; }
                        else if(Random_value<0.5 && Random_value >= 0.25) { Random_Color = 255; countt++; }
                        outimg.SetPixel(x, y, Color.FromArgb(Random_Color, Random_Color, Random_Color));
                    }
                }
            }
            Console.WriteLine("countt : " + countt);
            return outimg;
        }
        public Bitmap Adaptive_Median_Filter(int width, int height, Bitmap bmp, Bitmap outimg)
        {
            for (int y = 0; y <= height - 1; y++)
            {
                for (int x = 0; x <= width - 1; x++)
                {
                    //Console.WriteLine((height - 1) + "," + (width - 1)+ " ");
                    //Console.WriteLine(y+","+x+" ");
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        outimg.SetPixel(x, y, Color.FromArgb(bmp.GetPixel(x, y).R, bmp.GetPixel(x, y).R, bmp.GetPixel(x, y).R));
                    }
                    else
                    {
                        int window_size = 3;
                        int Smax = 7;
                    LevelA:
                        int[] array = new int[window_size * window_size];
                        int count = 0;
                        int init_ = -1;
                        int end_ = 1;
                        if (window_size == 5) {
                            if (y <= 2 || x <= 2 || y>= height - 2 || x>= width - 2)
                            {
                                outimg.SetPixel(x, y, Color.FromArgb(bmp.GetPixel(x, y).R, bmp.GetPixel(x, y).R, bmp.GetPixel(x, y).R));
                                continue;
                            }
                            init_ = -2; end_ = 2;
                        }
                        else if(window_size == 7) {
                            if (y <= 3 || x <= 3 || y >= height - 3 || x >= width - 3)
                            {
                                outimg.SetPixel(x, y, Color.FromArgb(bmp.GetPixel(x, y).R, bmp.GetPixel(x, y).R, bmp.GetPixel(x, y).R));
                                continue;
                            }
                            init_ = -3; end_ = 3;
                        }
                        //Console.WriteLine("window_size " + window_size + " init_: " + init_+ " end_ " + end_+ " ,x " + x+" y "+y);
                        //Console.WriteLine("length_array " + array.Length+ "window_size*window_size" + window_size * window_size);
                        for (int i = init_; i <= end_; i++)
                        {
                            for (int j = init_; j <= end_; j++)
                            {
                                array[count] = bmp.GetPixel(x - i, y - j).R;
                                count++;
                            }
                        }
                        
                        Array.Sort(array);

                        int Zmin = array[0];
                        int Zmax = array[array.Length-1];
                        int Zmed = array[(int)(array.Length/2)+1];
                        int Zxy = bmp.GetPixel(x,y).R;
                        if (Zmed > Zmin && Zmed < Zmax) goto LevelB;
                        else
                        {
                            window_size = window_size + 2;
                        }
                        if (window_size < Smax)
                        {
                            goto LevelA;
                        }
                        else { outimg.SetPixel(x, y, Color.FromArgb(bmp.GetPixel(x, y).R, bmp.GetPixel(x, y).R, bmp.GetPixel(x, y).R)); }
                    /*for (int i=0;i< array.Length; i++)
                    {
                        Console.WriteLine(array[i]);
                    }*/
                    LevelB:
                            if(Zxy-Zmin>0 && Zxy-Zmax<0) outimg.SetPixel(x, y, Color.FromArgb(bmp.GetPixel(x, y).R, bmp.GetPixel(x, y).R, bmp.GetPixel(x, y).R));
                            else outimg.SetPixel(x, y, Color.FromArgb( Zmed, Zmed, Zmed));
                    }
                    //Console.WriteLine("------------------------------------------------------");
                }
            }
            return outimg;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string lsAppDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            Console.WriteLine("Path" + lsAppDir);
            try
            {
                Bitmap bm1 = new Bitmap(lsAppDir + "/test3.jpg");
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
                Bitmap Salt_and_Pepper_bitmap = Salt_and_Pepper_Noise(w1, h1, bm1, new Bitmap(w1, h1));
                Image Salt_and_Pepper_Image = (Image)Salt_and_Pepper_bitmap;
                Salt_and_Pepper_Image.Save(lsAppDir + "/Salt_and_Pepper_Image.png");
                Bitmap Adaptive_Median_Filter_bitmap = Adaptive_Median_Filter(w1, h1, Salt_and_Pepper_bitmap, new Bitmap(w1, h1));
                Image Adaptive_Median_Filter_Image = (Image)Adaptive_Median_Filter_bitmap;
                Adaptive_Median_Filter_Image.Save(lsAppDir + "/Adaptive_Median_Filter_Image.png");
                Console.WriteLine("end");
                /*
                Console.WriteLine("PathPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrPathrr");
                Bitmap laplacian_img = laplacian(w1, h1, bm1, new Bitmap(w1, h1));
                Bitmap sobel_img = sobel(w1, h1, bm1, new Bitmap(w1, h1));
                float[,] normalizationArr = avgfilter(w1, h1, sobel_img, new Bitmap(w1, h1));
                Bitmap Result = new Bitmap(w1, h1);
                for (int i = 0; i < w1 - 1; i++)
                {
                    for (int j = 0; j < h1; j++)
                    {
                        int c1 = (int)((float)(laplacian_img.GetPixel(i, j).R * normalizationArr[i,j]) + (float)bm1.GetPixel(i, j).R);
                        if (c1 > 255) c1 = 255;
                        Result.SetPixel(i, j, Color.FromArgb(c1, c1, c1));
                    }
                }
                Image ori_gray = (Image)bm1;
                ori_gray.Save(lsAppDir + "/ori_gray.png");
                Image laplacian_image = (Image)laplacian_img;
                laplacian_image.Save(lsAppDir + "/laplacian_image.png");
                Image sobel_image = (Image)sobel_img;
                sobel_image.Save(lsAppDir + "/sobel_image.png");
                Image Result_image = (Image)Result;
                Result_image.Save(lsAppDir + "/Result_image.png");
                */
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
