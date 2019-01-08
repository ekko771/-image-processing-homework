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
        public Bitmap laplacian(int width, int height, Bitmap bmp, Bitmap outimg)
        {
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
                        int r1 = (-1 * bmp.GetPixel(x - 1, y - 1).R + -1 * bmp.GetPixel(x - 1, y).R + -1 * bmp.GetPixel(x - 1, y + 1).R +
                                  -1 * bmp.GetPixel(x, y - 1).R + 8 * bmp.GetPixel(x, y).R + -1 * bmp.GetPixel(x, y + 1).R +
                                  -1 * bmp.GetPixel(x + 1, y - 1).R + -1 * bmp.GetPixel(x + 1, y).R + -1 * bmp.GetPixel(x + 1, y + 1).R) ;
                        if (r1 > 255) r1 = 255;
                        if (r1 < 0) r1 = 0;
                        outimg.SetPixel(x, y, Color.FromArgb(r1, r1, r1));
                    }
                }
            }
            return outimg;
        }
        public float[,] avgfilter(int width,int  height,Bitmap bmp, Bitmap outimg) {
            //var myArr = new int[width] [height];
            var myArr = new float[width, height];
            for (int y = 0; y <= height - 1; y++)
            {
                for (int x = 0; x <= width - 1; x++)
                {
                    if (x==0||x== width - 1|| y == 0 || y == height - 1) {
                        outimg.SetPixel(x, y, Color.FromArgb(bmp.GetPixel(x, y).R, bmp.GetPixel(x, y).R, bmp.GetPixel(x, y).R));
                        myArr[x,y] = bmp.GetPixel(x, y).R;
                    }
                    else { 
                        int r1 = (bmp.GetPixel(x-1, y-1).R + bmp.GetPixel(x - 1, y).R + bmp.GetPixel(x - 1, y + 1).R +
                                  bmp.GetPixel(x , y - 1).R + bmp.GetPixel(x , y ).R + bmp.GetPixel(x, y + 1).R +
                                  bmp.GetPixel(x + 1, y - 1).R + bmp.GetPixel(x + 1, y).R + bmp.GetPixel(x + 1, y + 1).R) / 9;
                        outimg.SetPixel(x, y, Color.FromArgb(r1, r1, r1));
                        myArr[x,y] = r1;
                    }
                }
            }
            float max = myArr.Cast<float>().Max();
            float min = myArr.Cast<float>().Min();
            Console.WriteLine("max:" + max);
            Console.WriteLine("min:" + min);
            float[,] normalizationArr = new float[width,height];
            for (int x = 0; x <= width - 1; x++)//normalization [0,1]
            {
                for (int y = 0; y <= height - 1; y++)
                {
                    normalizationArr[x, y] = (float)(myArr[x, y] / max);
                    //Console.WriteLine("myArr[x, y]" + normalizationArr[x, y]);
                }
            }
            return normalizationArr;
        }
        public Bitmap sobel(int width, int height, Bitmap bmp, Bitmap outimg)
        {
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
        private void button1_Click(object sender, EventArgs e)
        {
            string lsAppDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            Console.WriteLine("Path" + lsAppDir);
            try
            {
                Bitmap bm1 = new Bitmap(lsAppDir + "/lena.jpg");
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
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
