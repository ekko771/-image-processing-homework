using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public Bitmap bilinear(Bitmap myBmp,int out_Height,int out_Width,double ratio, Bitmap outbitmap)
        {
            int iy, iyy, ix, ixx;
            int[,,] color_element = new int[2, 2, 3]; ;
            //double doubleval = 1.0/ ratio;
            for (int i = 0; i < out_Height; i++)
            {
                //double doubleval = ((double)myBmp.Height / (double)out_Height);
                //Console.WriteLine("doubleval:" + doubleval);
                /*
                iy = (int)((double)i * doubleval);
                if (iy >= myBmp.Height)
                {
                    iy--;
                    //continue;
                }
                //doubleval = ((double)myBmp.Height / (double)out_Height);
                //iyy = (int)((double)(i+1) * doubleval);
                iyy = iy + 1;
                if (iyy >= myBmp.Height)
                {
                    iyy--;
                    //continue;
                }
                */
                for (int j = 0; j < out_Width; j++)
                {
                    /*
                   ix = (int)((double)j * doubleval);
                   if (ix >= myBmp.Width)
                   {
                       ix--;
                       //continue;
                   }
                   ixx = ix + 1;
                   if (ixx >= myBmp.Width)
                   {
                       ixx--;
                       //continue;
                   }*/
                    //iy = (int) (i*(1/ ratio));
                    //iy = (int)i;
                    double ixd=(double)(((double)j + 52.637) / 0.9543);
                    double iyd=(double)(((double)i + 74.3656) - (0.02164 * (double)ixd));
                    ix = (int)ixd;
                    iy = (int)iyd;
                    double distance_a = (double)ixd-ix;
                    double distance_b = (double)iyd-iy;
                    //iy = (int)( (((double)i + 108.646) - (0.14981 * (double)j))/0.93785 );
                    //Console.WriteLine(iy);
                    if (iy < 0)
                    {
                        //iy++;
                        continue;
                    }
                    if (iy >= myBmp.Height)
                    {
                        //iy= myBmp.Height-1;
                       // iy = iy - 1;
                        continue;
                    }
                    iyy = iy + 1;
                    if (iyy < 0)
                    {
                        //iyy++;
                        continue;
                    }
                    if (iyy >= myBmp.Height)
                    {
                        //iyy = myBmp.Height - 1;
                        //iyy = iyy - 1;
                        continue;
                    }
                    //ix= (int)(j * (1 / ratio));
                    //ix = (int)(((double)j + 257.022) / 0.982);
                    //ix = (int)(   (((double)j + 181.869) + 0.115*(double)iy)/ 0.94945);
                    //ix = (int)(j + 101.99);
                    if (ix < 0)
                    {
                        //ix++;
                        continue;
                    }
                    if (ix >= myBmp.Width)
                    {
                        //ix= myBmp.Width-1;
                        //ix = ix - 1;
                        continue;
                    }
                    ixx = ix + 1;
                    if (ixx < 0)
                    {
                        //ixx++;
                        continue;
                    }
                    if (ixx >= myBmp.Width)
                    {
                        //ixx= myBmp.Width - 1;
                        //ixx = ixx - 1;
                        continue;
                    }
                    //Console.WriteLine(i + " " + iy + " " + iyy + " ");
                    //Console.WriteLine(j + " " + ix + " " + ixx + " " + myBmp.Width);
                    //Console.WriteLine("-----------------------------------------------");
                    //ix =ix-1;
                    //ixx=ixx-1;
                    //Console.WriteLine(j+" "+ix + " " + ixx + " ");
                    Color color1 = myBmp.GetPixel(ix, iy);
                    color_element[0, 0, 0] = color1.R;
                    color_element[0, 0, 1] = color1.G;
                    color_element[0, 0, 2] = color1.B;
                    Color color2 = myBmp.GetPixel(ixx, iy);
                    color_element[0, 1, 0] = color2.R;
                    color_element[0, 1, 1] = color2.G;
                    color_element[0, 1, 2] = color2.B;
                    Color color3 = myBmp.GetPixel(ix, iyy);
                    color_element[1, 0, 0] = color3.R;
                    color_element[1, 0, 1] = color3.G;
                    color_element[1, 0, 2] = color3.B;
                    Color color4 = myBmp.GetPixel(ixx, iyy);
                    color_element[1, 1, 0] = color4.R;
                    color_element[1, 1, 1] = color4.G;
                    color_element[1, 1, 2] = color4.B;
                    //double distance_a_teamp=(iy * ratio);
                    //double distance_b_teamp = (ix * ratio);
                    // double distance_a_teamp = iy ;
                    //double distance_b_teamp = ix-101.99;
                    //int distance_a = (int)(i - iy);
                    //int distance_b = (int)(j - (ix-101.99));
                    //double distance_a = (double)Math.Abs(((double)i - (0.02164*(double)ix + 1.02777*(double)iy - 74.3656)));
                    //double distance_b = (double)Math.Abs(((double)j - (0.9543 * (double)ix - 52.637)));
                    //Console.WriteLine(" 輸出點i "+iy + " 原始點i " + i + " 輸出點j " + ix + " 原始點j " + j + " distance_a " + distance_a + " distance_b " + distance_b);
                    //Console.WriteLine("j " + j + " iy " + iy + " i " +i+ " ix "+ix);
                    //Console.WriteLine("distance_a " + distance_a + " distance_b " + distance_b + " ");
                    int finalR = (int)((double)(1 - distance_a) * (double)(1 - distance_b) * color_element[0, 0, 0] + (double)distance_a * (double)(1 - distance_b) * color_element[1, 0, 0] + (double)(1 - distance_a) * (double)distance_b * color_element[0, 1, 0] + (double)distance_a * (double)distance_b * color_element[1, 1, 0]);
                    int finalG = (int)((double)(1 - distance_a) * (double)(1 - distance_b) * color_element[0, 0, 1] + (double)distance_a * (double)(1 - distance_b) * color_element[1, 0, 1] + (double)(1 - distance_a) * (double)distance_b * color_element[0, 1, 1] + (double)distance_a * (double)distance_b * color_element[1, 1, 1]);
                    int finalB = (int)((double)(1 - distance_a) * (double)(1 - distance_b) * color_element[0, 0, 2] + (double)distance_a * (double)(1 - distance_b) * color_element[1, 0, 2] + (double)(1 - distance_a) * (double)distance_b * color_element[0, 1, 2] + (double)distance_a * (double)distance_b * color_element[1, 1, 2]);
                    //Console.WriteLine(i+" "+j+" "+finalR+" "+finalG+" "+ finalB);
                    //Console.WriteLine(i + " " + distance_a + " " + j + " " + distance_b + " "+" "+ (ix * ratio));
                    //out_rgbData[j, i, 0] = (Byte)finalR;
                    //out_rgbData[j, i, 1] = (Byte)finalG;
                    //out_rgbData[j, i, 2] = (Byte)finalB;

                    //rgbData[j, i, 0] = color.R;
                    //rgbData[j, i, 1] = color.G;
                    //rgbData[j, i, 2] = color.B;
                    //Console.WriteLine(i+" "+j+" : "+out_rgbData[0, 0, 0] + " " + out_rgbData[j, i, 1] + " " + out_rgbData[j, i, 2]);
                    //Console.WriteLine(i + " " + j + " : " + color_element[0, 0, 0] + " " + color_element[0, 0, 1] + " " + color_element[0, 0, 2]);
                    if (finalR > 255)
                        finalR = 255;
                    if (finalG > 255)
                        finalG = 255;
                    if (finalB > 255)
                        finalB = 255;
                    if (finalR < 0)
                        finalR = 0;
                    if (finalG < 0)
                        finalG = 0;
                    if (finalB < 0)
                        finalB = 0;
                    Color redColor = Color.FromArgb(finalR, finalG, finalB);
                    outbitmap.SetPixel(j, i, redColor);
                }
            }
            return outbitmap;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("start");
            int scalePercent = 100;
            //Bitmap myBmp = new Bitmap(@"C:\Users\USER\Desktop\888\WindowsFormsApplication2\a2.png");
            Bitmap left = new Bitmap(@"C:\Users\USER\Desktop\s1.jpg");
            Bitmap myBmp = new Bitmap(@"C:\Users\USER\Desktop\s2.jpg");
            int Height = myBmp.Height;
            int Width = myBmp.Width;
            int[,,] rgbData = new int[Width, Height, 3];
            int out_Width = Width * scalePercent / 100;
            int out_Height = Height * scalePercent / 100;
            Byte[,,] out_rgbData = new Byte[out_Width, out_Height, 3];
            double ratio = (double)scalePercent / 100;
            Bitmap outbitmap = new Bitmap(out_Width, out_Height);
            outbitmap = bilinear(myBmp, out_Height, out_Width, ratio, outbitmap);
            Image image1 = (Image)outbitmap;
            image1.Save(@"C:\Users\USER\Desktop\Result0.png");
            Console.WriteLine("ratio:" + ratio);
            Console.WriteLine("Height:" + Height);
            Console.WriteLine("Width:" + Width);
            Console.WriteLine("out_Height:" + out_Height);
            Console.WriteLine("out_Width:" + out_Width);
            Console.WriteLine("end");
            Bitmap right = new Bitmap(@"C:\Users\USER\Desktop\Result0.png");
            Bitmap tout = new Bitmap(out_Width*2, out_Height);
            for(int i=0;i< out_Height; i++)
            {
                int jj = 0;
                for (int j=0;j< out_Width*2; j++)
                {
                    //Color redColor = Color.FromArgb(finalR, finalG, finalB);
                    
                    /*if (j <= 845){
                    tout.SetPixel(j, i, left.GetPixel(j,i));
                    }*/
                    if (j <= 541){
                    tout.SetPixel(j, i, left.GetPixel(j,i));
                    }
                    else if (380 + jj < out_Width)//308
                    {
                        /* if (i<13)
                         {
                             Color redColor = Color.FromArgb(0, 0, 0);
                             tout.SetPixel(j, i, redColor);
                         }*/
                        if (i >= 113)
                        {
                            tout.SetPixel(j, i, right.GetPixel(380 + jj, i - 113));

                        }
                        //tout.SetPixel(j, i, right.GetPixel(308 + jj, i ));
                        jj++;
                    }
                    /*else if (308+jj< out_Width)//308
                    {
                        // if (i<13)
                         //{
                           //  Color redColor = Color.FromArgb(0, 0, 0);
                             //tout.SetPixel(j, i, redColor);
                         //}
                        if (i >= 13) { 
                            tout.SetPixel(j, i, right.GetPixel(308+jj,i-13));
                            
                        }
                        //tout.SetPixel(j, i, right.GetPixel(308 + jj, i ));
                        jj++;
                    }*/
                }
            }
            Image image2 = (Image)tout;
            image2.Save(@"C:\Users\USER\Desktop\Result1.png");
        }
        public static Image BufferToImage(byte[] Buffer)
        {
            if (Buffer == null || Buffer.Length == 0) { return null; }
            byte[] data = null;
            Image oImage = null;
            Bitmap oBitmap = null;
            //建立副本
            data = (byte[])Buffer.Clone();
            try
            {
                MemoryStream oMemoryStream = new MemoryStream(Buffer);
                //設定資料流位置
                oMemoryStream.Position = 0;
                oImage = System.Drawing.Image.FromStream(oMemoryStream);
                //建立副本
                oBitmap = new Bitmap(oImage);
            }
            catch
            {
                throw;
            }
            //return oImage;
            return oBitmap;
        }
    }
}
