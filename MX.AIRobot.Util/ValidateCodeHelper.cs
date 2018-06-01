using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MX.AIRobot.Util
{
    public class ValidateCodeHelper
    {
        public string AllChar = "1,2,3,4,5,6,7,8,9,0,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,X,Y,Z";
        public Color DrawColor = Color.BlueViolet;
        public bool FontTextRenderingHint = false;
        public int ImageHeight = 0x17;
        private byte TrueValidateCodeCount = 4;
        protected string ValidateCode = "";
        public string ValidateCodeFont = "Arial";
        public float ValidateCodeSize = 13f;

        private void CreateImageBmp(out Bitmap ImageFrame)
        {
            char[] chArray = this.ValidateCode.ToCharArray(0, this.ValidateCodeCount);
            int width = (int)(((this.TrueValidateCodeCount * this.ValidateCodeSize) * 1.3) + 4.0);
            ImageFrame = new Bitmap(width, this.ImageHeight);
            Graphics graphics = Graphics.FromImage(ImageFrame);
            graphics.Clear(Color.White);
            Font font = new Font(this.ValidateCodeFont, this.ValidateCodeSize, FontStyle.Bold);
            Brush brush = new SolidBrush(this.DrawColor);
            int maxValue = (int)Math.Max((float)((this.ImageHeight - this.ValidateCodeSize) - 3f), (float)2f);
            Random random = new Random();
            for (int i = 0; i < this.TrueValidateCodeCount; i++)
            {
                int[] numArray = new int[] { (((int)(i * this.ValidateCodeSize)) + random.Next(1)) + 3, random.Next(maxValue) };
                Point point = new Point(numArray[0], numArray[1]);
                if (this.FontTextRenderingHint)
                {
                    graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                }
                else
                {
                    graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                }
                graphics.DrawString(chArray[i].ToString(), font, brush, (PointF)point);
            }
            graphics.Dispose();
        }

        private void CreateImageGif()
        {
            AnimatedGifEncoder encoder = new AnimatedGifEncoder();
            MemoryStream stream = new MemoryStream();
            encoder.Start();
            encoder.SetDelay(5);
            encoder.SetRepeat(0);
            for (int i = 0; i < 10; i++)
            {
                Bitmap bitmap;
                this.CreateImageBmp(out bitmap);
                this.DisposeImageBmp(ref bitmap);
                bitmap.Save(stream, ImageFormat.Png);
                encoder.AddFrame(Image.FromStream(stream));
                stream = new MemoryStream();
            }
            encoder.OutPut(ref stream);
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "image/Gif";
            HttpContext.Current.Response.BinaryWrite(stream.ToArray());
            stream.Close();
            stream.Dispose();
        }

        private void CreateValidate()
        {
            this.ValidateCode = "";
            string[] strArray = this.AllChar.Split(new char[] { ',' });
            int index = -1;
            Random random = new Random();
            for (int i = 0; i < this.ValidateCodeCount; i++)
            {
                if (index != -1)
                {
                    random = new Random((i * index) * ((int)DateTime.Now.Ticks));
                }
                //int num3 = random.Next(0x23);
                int num3 = random.Next(AllChar.Replace(",", "").Length);
                if (index == num3)
                {
                    this.CreateValidate();
                }
                index = num3;
                this.ValidateCode = this.ValidateCode + strArray[index];
            }
            if (this.ValidateCode.Length > this.TrueValidateCodeCount)
            {
                this.ValidateCode = this.ValidateCode.Remove(this.TrueValidateCodeCount);
            }
        }

        private void DisposeImageBmp(ref Bitmap ImageFrame)
        {
            Graphics graphics = Graphics.FromImage(ImageFrame);
            Pen pen = new Pen(this.DrawColor, 1f);
            Random random = new Random();
            Point[] pointArray = new Point[2];
            for (int i = 0; i < 15; i++)
            {
                pointArray[0] = new Point(random.Next(ImageFrame.Width), random.Next(ImageFrame.Height));
                pointArray[1] = new Point(random.Next(ImageFrame.Width), random.Next(ImageFrame.Height));
                graphics.DrawLine(pen, pointArray[0], pointArray[1]);
            }
            graphics.Dispose();
        }

        public void OutPutValidate()
        {
            this.CreateValidate();
            this.CreateImageGif();
            HttpContext.Current.Session[SessionKeys.VerifyCode] = this.ValidateCode;
        }

        public byte ValidateCodeCount
        {
            get
            {
                return this.TrueValidateCodeCount;
            }
            set
            {
                if (value > 4)
                {
                    this.TrueValidateCodeCount = value;
                }
            }
        }
    }

    public class AnimatedGifEncoder
    {
        protected int colorDepth;
        protected byte[] colorTab;
        protected int delay = 0;
        protected int dispose = -1;
        protected bool firstFrame = true;
        protected int height;
        protected Image image;
        protected byte[] indexedPixels;
        protected MemoryStream Memory;
        protected int palSize = 7;
        protected byte[] pixels;
        protected int repeat = -1;
        protected int sample = 10;
        protected bool sizeSet = false;
        protected bool started = false;
        protected int transIndex;
        protected Color transparent = Color.Empty;
        protected bool[] usedEntry = new bool[0x100];
        protected int width;

        public bool AddFrame(Image im)
        {
            if (!((im != null) && this.started))
            {
                return false;
            }
            bool flag3 = true;
            try
            {
                if (!this.sizeSet)
                {
                    this.SetSize(im.Width, im.Height);
                }
                this.image = im;
                this.GetImagePixels();
                this.AnalyzePixels();
                if (this.firstFrame)
                {
                    this.WriteLSD();
                    this.WritePalette();
                    if (this.repeat >= 0)
                    {
                        this.WriteNetscapeExt();
                    }
                }
                this.WriteGraphicCtrlExt();
                this.WriteImageDesc();
                if (!this.firstFrame)
                {
                    this.WritePalette();
                }
                this.WritePixels();
                this.firstFrame = false;
            }
            catch (IOException)
            {
                flag3 = false;
            }
            return flag3;
        }

        protected void AnalyzePixels()
        {
            int length = this.pixels.Length;
            int num2 = length / 3;
            this.indexedPixels = new byte[num2];
            NeuQuant quant = new NeuQuant(this.pixels, length, this.sample);
            this.colorTab = quant.Process();
            int num3 = 0;
            for (int i = 0; i < num2; i++)
            {
                int index = quant.Map(this.pixels[num3++] & 0xff, this.pixels[num3++] & 0xff, this.pixels[num3++] & 0xff);
                this.usedEntry[index] = true;
                this.indexedPixels[i] = (byte)index;
            }
            this.pixels = null;
            this.colorDepth = 8;
            this.palSize = 7;
            if (this.transparent != Color.Empty)
            {
                this.transIndex = this.FindClosest(this.transparent);
            }
        }

        protected int FindClosest(Color c)
        {
            if (this.colorTab == null)
            {
                return -1;
            }
            int r = c.R;
            int g = c.G;
            int b = c.B;
            int num5 = 0;
            int num6 = 0x1000000;
            int length = this.colorTab.Length;
            for (int i = 0; i < length; i++)
            {
                int num9 = r - (this.colorTab[i++] & 0xff);
                int num10 = g - (this.colorTab[i++] & 0xff);
                int num11 = b - (this.colorTab[i] & 0xff);
                int num12 = ((num9 * num9) + (num10 * num10)) + (num11 * num11);
                int index = i / 3;
                if (this.usedEntry[index] && (num12 < num6))
                {
                    num6 = num12;
                    num5 = index;
                }
            }
            return num5;
        }

        protected void GetImagePixels()
        {
            int width = this.image.Width;
            int height = this.image.Height;
            if ((width != this.width) || (height != this.height))
            {
                Image image = new Bitmap(this.width, this.height);
                Graphics graphics = Graphics.FromImage(image);
                graphics.DrawImage(this.image, 0, 0);
                this.image = image;
                graphics.Dispose();
            }
            this.pixels = new byte[(3 * this.image.Width) * this.image.Height];
            int index = 0;
            Bitmap bitmap = new Bitmap(this.image);
            for (int i = 0; i < this.image.Height; i++)
            {
                for (int j = 0; j < this.image.Width; j++)
                {
                    Color pixel = bitmap.GetPixel(j, i);
                    this.pixels[index] = pixel.R;
                    index++;
                    this.pixels[index] = pixel.G;
                    index++;
                    this.pixels[index] = pixel.B;
                    index++;
                }
            }
        }

        public void OutPut(ref MemoryStream MemoryResult)
        {
            this.started = false;
            this.Memory.WriteByte(0x3b);
            this.Memory.Flush();
            MemoryResult = this.Memory;
            this.Memory.Close();
            this.Memory.Dispose();
            this.transIndex = 0;
            this.Memory = null;
            this.image = null;
            this.pixels = null;
            this.indexedPixels = null;
            this.colorTab = null;
            this.firstFrame = true;
        }

        public void SetDelay(int ms)
        {
            this.delay = (int)Math.Round((double)(((float)ms) / 10f));
        }

        public void SetDispose(int code)
        {
            if (code >= 0)
            {
                this.dispose = code;
            }
        }

        public void SetFrameRate(float fps)
        {
            if (!(fps == 0f))
            {
                this.delay = (int)Math.Round((double)(100f / fps));
            }
        }

        public void SetQuality(int quality)
        {
            if (quality < 1)
            {
                quality = 1;
            }
            this.sample = quality;
        }

        public void SetRepeat(int iter)
        {
            if (iter >= 0)
            {
                this.repeat = iter;
            }
        }

        public void SetSize(int w, int h)
        {
            if (!this.started || this.firstFrame)
            {
                this.width = w;
                this.height = h;
                if (this.width < 1)
                {
                    this.width = 320;
                }
                if (this.height < 1)
                {
                    this.height = 240;
                }
                this.sizeSet = true;
            }
        }

        public void SetTransparent(Color c)
        {
            this.transparent = c;
        }

        public void Start()
        {
            this.Memory = new MemoryStream();
            this.WriteString("GIF89a");
            this.started = true;
        }

        protected void WriteGraphicCtrlExt()
        {
            int num;
            int num2;
            this.Memory.WriteByte(0x21);
            this.Memory.WriteByte(0xf9);
            this.Memory.WriteByte(4);
            if (this.transparent == Color.Empty)
            {
                num = 0;
                num2 = 0;
            }
            else
            {
                num = 1;
                num2 = 2;
            }
            if (this.dispose >= 0)
            {
                num2 = this.dispose & 7;
            }
            num2 = num2 << 2;
            this.Memory.WriteByte(Convert.ToByte((int)(num2 | num)));
            this.WriteShort(this.delay);
            this.Memory.WriteByte(Convert.ToByte(this.transIndex));
            this.Memory.WriteByte(0);
        }

        protected void WriteImageDesc()
        {
            this.Memory.WriteByte(0x2c);
            this.WriteShort(0);
            this.WriteShort(0);
            this.WriteShort(this.width);
            this.WriteShort(this.height);
            if (this.firstFrame)
            {
                this.Memory.WriteByte(0);
            }
            else
            {
                this.Memory.WriteByte(Convert.ToByte((int)(0x80 | this.palSize)));
            }
        }

        protected void WriteLSD()
        {
            this.WriteShort(this.width);
            this.WriteShort(this.height);
            this.Memory.WriteByte(Convert.ToByte((int)(240 | this.palSize)));
            this.Memory.WriteByte(0);
            this.Memory.WriteByte(0);
        }

        protected void WriteNetscapeExt()
        {
            this.Memory.WriteByte(0x21);
            this.Memory.WriteByte(0xff);
            this.Memory.WriteByte(11);
            this.WriteString("NETSCAPE2.0");
            this.Memory.WriteByte(3);
            this.Memory.WriteByte(1);
            this.WriteShort(this.repeat);
            this.Memory.WriteByte(0);
        }

        protected void WritePalette()
        {
            this.Memory.Write(this.colorTab, 0, this.colorTab.Length);
            int num = 0x300 - this.colorTab.Length;
            for (int i = 0; i < num; i++)
            {
                this.Memory.WriteByte(0);
            }
        }

        protected void WritePixels()
        {
            new LZWEncoder(this.width, this.height, this.indexedPixels, this.colorDepth).Encode(this.Memory);
        }

        protected void WriteShort(int value)
        {
            this.Memory.WriteByte(Convert.ToByte((int)(value & 0xff)));
            this.Memory.WriteByte(Convert.ToByte((int)((value >> 8) & 0xff)));
        }

        protected void WriteString(string s)
        {
            char[] chArray = s.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                this.Memory.WriteByte((byte)chArray[i]);
            }
        }
    }

    public class GifDecoder
    {
        protected int[] act;
        protected int bgColor;
        protected int bgIndex;
        protected Bitmap bitmap;
        protected byte[] block = new byte[0x100];
        protected int blockSize = 0;
        protected int delay = 0;
        protected int dispose = 0;
        protected int frameCount;
        protected ArrayList frames;
        protected int[] gct;
        protected bool gctFlag;
        protected int gctSize;
        protected int height;
        protected int ih;
        protected Image image;
        protected Stream inStream;
        protected bool interlace;
        protected int iw;
        protected int ix;
        protected int iy;
        protected int lastBgColor;
        protected int lastDispose = 0;
        protected Image lastImage;
        protected Rectangle lastRect;
        protected int[] lct;
        protected bool lctFlag;
        protected int lctSize;
        protected int loopCount = 1;
        protected static readonly int MaxStackSize = 0x1000;
        protected int pixelAspect;
        protected byte[] pixels;
        protected byte[] pixelStack;
        protected short[] prefix;
        protected int status;
        public static readonly int STATUS_FORMAT_ERROR = 1;
        public static readonly int STATUS_OK = 0;
        public static readonly int STATUS_OPEN_ERROR = 2;
        protected byte[] suffix;
        protected int transIndex;
        protected bool transparency = false;
        protected int width;

        protected void DecodeImageData()
        {
            int num11;
            int num12;
            int num13;
            int num14;
            int num15;
            int num16;
            int num = -1;
            int num2 = this.iw * this.ih;
            if ((this.pixels == null) || (this.pixels.Length < num2))
            {
                this.pixels = new byte[num2];
            }
            if (this.prefix == null)
            {
                this.prefix = new short[MaxStackSize];
            }
            if (this.suffix == null)
            {
                this.suffix = new byte[MaxStackSize];
            }
            if (this.pixelStack == null)
            {
                this.pixelStack = new byte[MaxStackSize + 1];
            }
            int num3 = this.Read();
            int num4 = ((int)1) << num3;
            int num5 = num4 + 1;
            int index = num4 + 2;
            int num7 = num;
            int num8 = num3 + 1;
            int num9 = (((int)1) << num8) - 1;
            int num10 = 0;
            while (num10 < num4)
            {
                this.prefix[num10] = 0;
                this.suffix[num10] = (byte)num10;
                num10++;
            }
            int num17 = num16 = num15 = num14 = num13 = num12 = num11 = 0;
            int num18 = 0;
            while (num18 < num2)
            {
                if (num13 == 0)
                {
                    if (num16 < num8)
                    {
                        if (num15 == 0)
                        {
                            num15 = this.ReadBlock();
                            if (num15 <= 0)
                            {
                                break;
                            }
                            num11 = 0;
                        }
                        num17 += (this.block[num11] & 0xff) << num16;
                        num16 += 8;
                        num11++;
                        num15--;
                        continue;
                    }
                    num10 = num17 & num9;
                    num17 = num17 >> num8;
                    num16 -= num8;
                    if ((num10 > index) || (num10 == num5))
                    {
                        break;
                    }
                    if (num10 == num4)
                    {
                        num8 = num3 + 1;
                        num9 = (((int)1) << num8) - 1;
                        index = num4 + 2;
                        num7 = num;
                        continue;
                    }
                    if (num7 == num)
                    {
                        this.pixelStack[num13++] = this.suffix[num10];
                        num7 = num10;
                        num14 = num10;
                        continue;
                    }
                    int num19 = num10;
                    if (num10 == index)
                    {
                        this.pixelStack[num13++] = (byte)num14;
                        num10 = num7;
                    }
                    while (num10 > num4)
                    {
                        this.pixelStack[num13++] = this.suffix[num10];
                        num10 = this.prefix[num10];
                    }
                    num14 = this.suffix[num10] & 0xff;
                    if (index >= MaxStackSize)
                    {
                        break;
                    }
                    this.pixelStack[num13++] = (byte)num14;
                    this.prefix[index] = (short)num7;
                    this.suffix[index] = (byte)num14;
                    index++;
                    if (((index & num9) == 0) && (index < MaxStackSize))
                    {
                        num8++;
                        num9 += index;
                    }
                    num7 = num19;
                }
                num13--;
                this.pixels[num12++] = this.pixelStack[num13];
                num18++;
            }
            for (num18 = num12; num18 < num2; num18++)
            {
                this.pixels[num18] = 0;
            }
        }

        protected bool Error()
        {
            return (this.status != STATUS_OK);
        }

        public int GetDelay(int n)
        {
            this.delay = -1;
            if ((n >= 0) && (n < this.frameCount))
            {
                this.delay = ((GifFrame)this.frames[n]).delay;
            }
            return this.delay;
        }

        public Image GetFrame(int n)
        {
            Image image = null;
            if ((n >= 0) && (n < this.frameCount))
            {
                image = ((GifFrame)this.frames[n]).image;
            }
            return image;
        }

        public int GetFrameCount()
        {
            return this.frameCount;
        }

        public Size GetFrameSize()
        {
            return new Size(this.width, this.height);
        }

        public Image GetImage()
        {
            return this.GetFrame(0);
        }

        public int GetLoopCount()
        {
            return this.loopCount;
        }

        private int[] GetPixels(Bitmap bitmap)
        {
            int[] numArray = new int[(3 * this.image.Width) * this.image.Height];
            int index = 0;
            for (int i = 0; i < this.image.Height; i++)
            {
                for (int j = 0; j < this.image.Width; j++)
                {
                    Color pixel = bitmap.GetPixel(j, i);
                    numArray[index] = pixel.R;
                    index++;
                    numArray[index] = pixel.G;
                    index++;
                    numArray[index] = pixel.B;
                    index++;
                }
            }
            return numArray;
        }

        protected void Init()
        {
            this.status = STATUS_OK;
            this.frameCount = 0;
            this.frames = new ArrayList();
            this.gct = null;
            this.lct = null;
        }

        protected int Read()
        {
            int num = 0;
            try
            {
                num = this.inStream.ReadByte();
            }
            catch (IOException)
            {
                this.status = STATUS_FORMAT_ERROR;
            }
            return num;
        }

        public int Read(Stream inStream)
        {
            this.Init();
            if (inStream != null)
            {
                this.inStream = inStream;
                this.ReadHeader();
                if (!this.Error())
                {
                    this.ReadContents();
                    if (this.frameCount < 0)
                    {
                        this.status = STATUS_FORMAT_ERROR;
                    }
                }
                inStream.Close();
            }
            else
            {
                this.status = STATUS_OPEN_ERROR;
            }
            return this.status;
        }

        public int Read(string name)
        {
            this.status = STATUS_OK;
            try
            {
                name = name.Trim().ToLower();
                this.status = this.Read(new FileInfo(name).OpenRead());
            }
            catch (IOException)
            {
                this.status = STATUS_OPEN_ERROR;
            }
            return this.status;
        }

        protected int ReadBlock()
        {
            this.blockSize = this.Read();
            int offset = 0;
            if (this.blockSize <= 0)
            {
                return offset;
            }
            try
            {
                int num2 = 0;
                while (offset < this.blockSize)
                {
                    num2 = this.inStream.Read(this.block, offset, this.blockSize - offset);
                    if (num2 == -1)
                    {
                        goto Label_007A;
                    }
                    offset += num2;
                }
            }
            catch (IOException)
            {
            }
        Label_007A:
            if (offset < this.blockSize)
            {
                this.status = STATUS_FORMAT_ERROR;
            }
            return offset;
        }

        protected int[] ReadColorTable(int ncolors)
        {
            int num = 3 * ncolors;
            int[] numArray = null;
            byte[] buffer = new byte[num];
            int num2 = 0;
            try
            {
                num2 = this.inStream.Read(buffer, 0, buffer.Length);
            }
            catch (IOException)
            {
            }
            if (num2 < num)
            {
                this.status = STATUS_FORMAT_ERROR;
                return numArray;
            }
            numArray = new int[0x100];
            int num3 = 0;
            int num4 = 0;
            while (num3 < ncolors)
            {
                int num5 = buffer[num4++] & 0xff;
                int num6 = buffer[num4++] & 0xff;
                int num7 = buffer[num4++] & 0xff;
                numArray[num3++] = (((unchecked((int)0xff000000L)) | (num5 << 0x10)) | (num6 << 8)) | num7;
            }
            return numArray;
        }

        protected void ReadContents()
        {
            bool flag = false;
            while (!flag && !this.Error())
            {
                switch (this.Read())
                {
                    case 0x2c:
                        this.ReadImage();
                        break;

                    case 0x3b:
                        flag = true;
                        break;

                    case 0:
                        break;

                    case 0x21:
                        switch (this.Read())
                        {
                            case 0xf9:
                                this.ReadGraphicControlExt();
                                break;

                            case 0xff:
                                {
                                    this.ReadBlock();
                                    string str = "";
                                    for (int i = 0; i < 11; i++)
                                    {
                                        str = str + ((char)this.block[i]);
                                    }
                                    if (str.Equals("NETSCAPE2.0"))
                                    {
                                        this.ReadNetscapeExt();
                                    }
                                    else
                                    {
                                        this.Skip();
                                    }
                                    break;
                                }
                        }
                        this.Skip();
                        break;

                    default:
                        this.status = STATUS_FORMAT_ERROR;
                        break;
                }
            }
        }

        protected void ReadGraphicControlExt()
        {
            this.Read();
            int num = this.Read();
            this.dispose = (num & 0x1c) >> 2;
            if (this.dispose == 0)
            {
                this.dispose = 1;
            }
            this.transparency = (num & 1) != 0;
            this.delay = this.ReadShort() * 10;
            this.transIndex = this.Read();
            this.Read();
        }

        protected void ReadHeader()
        {
            string str = "";
            for (int i = 0; i < 6; i++)
            {
                str = str + ((char)this.Read());
            }
            if (!str.StartsWith("GIF"))
            {
                this.status = STATUS_FORMAT_ERROR;
            }
            else
            {
                this.ReadLSD();
                if (!(!this.gctFlag || this.Error()))
                {
                    this.gct = this.ReadColorTable(this.gctSize);
                    this.bgColor = this.gct[this.bgIndex];
                }
            }
        }

        protected void ReadImage()
        {
            this.ix = this.ReadShort();
            this.iy = this.ReadShort();
            this.iw = this.ReadShort();
            this.ih = this.ReadShort();
            int num = this.Read();
            this.lctFlag = (num & 0x80) != 0;
            this.interlace = (num & 0x40) != 0;
            this.lctSize = ((int)2) << (num & 7);
            if (this.lctFlag)
            {
                this.lct = this.ReadColorTable(this.lctSize);
                this.act = this.lct;
            }
            else
            {
                this.act = this.gct;
                if (this.bgIndex == this.transIndex)
                {
                    this.bgColor = 0;
                }
            }
            int num2 = 0;
            if (this.transparency)
            {
                num2 = this.act[this.transIndex];
                this.act[this.transIndex] = 0;
            }
            if (this.act == null)
            {
                this.status = STATUS_FORMAT_ERROR;
            }
            if (!this.Error())
            {
                this.DecodeImageData();
                this.Skip();
                if (!this.Error())
                {
                    this.frameCount++;
                    this.bitmap = new Bitmap(this.width, this.height);
                    this.image = this.bitmap;
                    this.SetPixels();
                    this.frames.Add(new GifFrame(this.bitmap, this.delay));
                    if (this.transparency)
                    {
                        this.act[this.transIndex] = num2;
                    }
                    this.ResetFrame();
                }
            }
        }

        protected void ReadLSD()
        {
            this.width = this.ReadShort();
            this.height = this.ReadShort();
            int num = this.Read();
            this.gctFlag = (num & 0x80) != 0;
            this.gctSize = ((int)2) << (num & 7);
            this.bgIndex = this.Read();
            this.pixelAspect = this.Read();
        }

        protected void ReadNetscapeExt()
        {
            do
            {
                this.ReadBlock();
                if (this.block[0] == 1)
                {
                    int num = this.block[1] & 0xff;
                    int num2 = this.block[2] & 0xff;
                    this.loopCount = (num2 << 8) | num;
                }
            }
            while ((this.blockSize > 0) && !this.Error());
        }

        protected int ReadShort()
        {
            return (this.Read() | (this.Read() << 8));
        }

        protected void ResetFrame()
        {
            this.lastDispose = this.dispose;
            this.lastRect = new Rectangle(this.ix, this.iy, this.iw, this.ih);
            this.lastImage = this.image;
            this.lastBgColor = this.bgColor;
            this.lct = null;
        }

        protected void SetPixels()
        {
            int[] pixels = this.GetPixels(this.bitmap);
            if (this.lastDispose > 0)
            {
                if (this.lastDispose == 3)
                {
                    int num = this.frameCount - 2;
                    if (num > 0)
                    {
                        this.lastImage = this.GetFrame(num - 1);
                    }
                    else
                    {
                        this.lastImage = null;
                    }
                }
                if (this.lastImage != null)
                {
                    Array.Copy(this.GetPixels(new Bitmap(this.lastImage)), 0, pixels, 0, this.width * this.height);
                    if (this.lastDispose == 2)
                    {
                        Graphics graphics = Graphics.FromImage(this.image);
                        Color empty = Color.Empty;
                        if (this.transparency)
                        {
                            empty = Color.FromArgb(0, 0, 0, 0);
                        }
                        else
                        {
                            empty = Color.FromArgb(this.lastBgColor);
                        }
                        Brush brush = new SolidBrush(empty);
                        graphics.FillRectangle(brush, this.lastRect);
                        brush.Dispose();
                        graphics.Dispose();
                    }
                }
            }
            int num2 = 1;
            int num3 = 8;
            int num4 = 0;
            for (int i = 0; i < this.ih; i++)
            {
                int num6 = i;
                if (this.interlace)
                {
                    if (num4 >= this.ih)
                    {
                        num2++;
                        switch (num2)
                        {
                            case 2:
                                num4 = 4;
                                break;

                            case 3:
                                num4 = 2;
                                num3 = 4;
                                break;

                            case 4:
                                num4 = 1;
                                num3 = 2;
                                break;
                        }
                    }
                    num6 = num4;
                    num4 += num3;
                }
                num6 += this.iy;
                if (num6 < this.height)
                {
                    int num8 = num6 * this.width;
                    int index = num8 + this.ix;
                    int num10 = index + this.iw;
                    if ((num8 + this.width) < num10)
                    {
                        num10 = num8 + this.width;
                    }
                    int num11 = i * this.iw;
                    while (index < num10)
                    {
                        int num12 = this.pixels[num11++] & 0xff;
                        int num13 = this.act[num12];
                        if (num13 != 0)
                        {
                            pixels[index] = num13;
                        }
                        index++;
                    }
                }
            }
            this.SetPixels(pixels);
        }

        private void SetPixels(int[] pixels)
        {
            int num = 0;
            for (int i = 0; i < this.image.Height; i++)
            {
                for (int j = 0; j < this.image.Width; j++)
                {
                    Color color = Color.FromArgb(pixels[num++]);
                    this.bitmap.SetPixel(j, i, color);
                }
            }
        }

        protected void Skip()
        {
            do
            {
                this.ReadBlock();
            }
            while ((this.blockSize > 0) && !this.Error());
        }

        public class GifFrame
        {
            public int delay;
            public Image image;

            public GifFrame(Image im, int del)
            {
                this.image = im;
                this.delay = del;
            }
        }
    }

    public class LZWEncoder
    {
        private int a_count;
        private byte[] accum = new byte[0x100];
        private static readonly int BITS = 12;
        private bool clear_flg = false;
        private int ClearCode;
        private int[] codetab = new int[HSIZE];
        private int cur_accum = 0;
        private int cur_bits = 0;
        private int curPixel;
        private static readonly int EOF = -1;
        private int EOFCode;
        private int free_ent = 0;
        private int g_init_bits;
        private int hsize = HSIZE;
        private static readonly int HSIZE = 0x138b;
        private int[] htab = new int[HSIZE];
        private int imgH;
        private int imgW;
        private int initCodeSize;
        private int[] masks = new int[] { 
            0, 1, 3, 7, 15, 0x1f, 0x3f, 0x7f, 0xff, 0x1ff, 0x3ff, 0x7ff, 0xfff, 0x1fff, 0x3fff, 0x7fff, 
            0xffff
         };
        private int maxbits = BITS;
        private int maxcode;
        private int maxmaxcode = (((int)1) << BITS);
        private int n_bits;
        private byte[] pixAry;
        private int remaining;

        public LZWEncoder(int width, int height, byte[] pixels, int color_depth)
        {
            this.imgW = width;
            this.imgH = height;
            this.pixAry = pixels;
            this.initCodeSize = Math.Max(2, color_depth);
        }

        private void Add(byte c, Stream outs)
        {
            this.accum[this.a_count++] = c;
            if (this.a_count >= 0xfe)
            {
                this.Flush(outs);
            }
        }

        private void ClearTable(Stream outs)
        {
            this.ResetCodeTable(this.hsize);
            this.free_ent = this.ClearCode + 2;
            this.clear_flg = true;
            this.Output(this.ClearCode, outs);
        }

        private void Compress(int init_bits, Stream outs)
        {
            int num5;
            this.g_init_bits = init_bits;
            this.clear_flg = false;
            this.n_bits = this.g_init_bits;
            this.maxcode = this.MaxCode(this.n_bits);
            this.ClearCode = ((int)1) << (init_bits - 1);
            this.EOFCode = this.ClearCode + 1;
            this.free_ent = this.ClearCode + 2;
            this.a_count = 0;
            int code = this.NextPixel();
            int num2 = 0;
            int hsize = this.hsize;
            while (hsize < 0x10000)
            {
                num2++;
                hsize *= 2;
            }
            num2 = 8 - num2;
            int num4 = this.hsize;
            this.ResetCodeTable(num4);
            this.Output(this.ClearCode, outs);
            while ((num5 = this.NextPixel()) != EOF)
            {
                hsize = (num5 << this.maxbits) + code;
                int index = (num5 << num2) ^ code;
                if (this.htab[index] == hsize)
                {
                    code = this.codetab[index];
                }
                else
                {
                    if (this.htab[index] >= 0)
                    {
                        int num7 = num4 - index;
                        if (index == 0)
                        {
                            num7 = 1;
                        }
                        do
                        {
                            index -= num7;
                            if (index < 0)
                            {
                                index += num4;
                            }
                            if (this.htab[index] == hsize)
                            {
                                code = this.codetab[index];
                                continue;
                            }
                        }
                        while (this.htab[index] >= 0);
                    }
                    this.Output(code, outs);
                    code = num5;
                    if (this.free_ent < this.maxmaxcode)
                    {
                        this.codetab[index] = this.free_ent++;
                        this.htab[index] = hsize;
                    }
                    else
                    {
                        this.ClearTable(outs);
                    }
                }
            }
            this.Output(code, outs);
            this.Output(this.EOFCode, outs);
        }

        public void Encode(Stream os)
        {
            os.WriteByte(Convert.ToByte(this.initCodeSize));
            this.remaining = this.imgW * this.imgH;
            this.curPixel = 0;
            this.Compress(this.initCodeSize + 1, os);
            os.WriteByte(0);
        }

        private void Flush(Stream outs)
        {
            if (this.a_count > 0)
            {
                outs.WriteByte(Convert.ToByte(this.a_count));
                outs.Write(this.accum, 0, this.a_count);
                this.a_count = 0;
            }
        }

        private int MaxCode(int n_bits)
        {
            return ((((int)1) << n_bits) - 1);
        }

        private int NextPixel()
        {
            if (this.remaining == 0)
            {
                return EOF;
            }
            this.remaining--;
            int num2 = this.curPixel + 1;
            if (num2 < this.pixAry.GetUpperBound(0))
            {
                byte num4 = this.pixAry[this.curPixel++];
                return (num4 & 0xff);
            }
            return 0xff;
        }

        private void Output(int code, Stream outs)
        {
            this.cur_accum &= this.masks[this.cur_bits];
            if (this.cur_bits > 0)
            {
                this.cur_accum |= code << this.cur_bits;
            }
            else
            {
                this.cur_accum = code;
            }
            this.cur_bits += this.n_bits;
            while (this.cur_bits >= 8)
            {
                this.Add((byte)(this.cur_accum & 0xff), outs);
                this.cur_accum = this.cur_accum >> 8;
                this.cur_bits -= 8;
            }
            if ((this.free_ent > this.maxcode) || this.clear_flg)
            {
                if (this.clear_flg)
                {
                    this.maxcode = this.MaxCode(this.n_bits = this.g_init_bits);
                    this.clear_flg = false;
                }
                else
                {
                    this.n_bits++;
                    if (this.n_bits == this.maxbits)
                    {
                        this.maxcode = this.maxmaxcode;
                    }
                    else
                    {
                        this.maxcode = this.MaxCode(this.n_bits);
                    }
                }
            }
            if (code == this.EOFCode)
            {
                while (this.cur_bits > 0)
                {
                    this.Add((byte)(this.cur_accum & 0xff), outs);
                    this.cur_accum = this.cur_accum >> 8;
                    this.cur_bits -= 8;
                }
                this.Flush(outs);
            }
        }

        private void ResetCodeTable(int hsize)
        {
            for (int i = 0; i < hsize; i++)
            {
                this.htab[i] = -1;
            }
        }
    }

    public class NeuQuant
    {
        protected static readonly int alphabiasshift = 10;
        protected int alphadec;
        protected static readonly int alpharadbias = (((int)1) << alpharadbshift);
        protected static readonly int alpharadbshift = (alphabiasshift + radbiasshift);
        protected static readonly int beta = (intbias >> betashift);
        protected static readonly int betagamma = (intbias << (gammashift - betashift));
        protected static readonly int betashift = 10;
        protected int[] bias = new int[netsize];
        protected int[] freq = new int[netsize];
        protected static readonly int gamma = (((int)1) << gammashift);
        protected static readonly int gammashift = 10;
        protected static readonly int initalpha = (((int)1) << alphabiasshift);
        protected static readonly int initrad = (netsize >> 3);
        protected static readonly int initradius = (initrad * radiusbias);
        protected static readonly int intbias = (((int)1) << intbiasshift);
        protected static readonly int intbiasshift = 0x10;
        protected int lengthcount;
        protected static readonly int maxnetpos = (netsize - 1);
        protected static readonly int minpicturebytes = (3 * prime4);
        protected static readonly int ncycles = 100;
        protected static readonly int netbiasshift = 4;
        protected int[] netindex = new int[0x100];
        protected static readonly int netsize = 0x100;
        protected int[][] network;
        protected static readonly int prime1 = 0x1f3;
        protected static readonly int prime2 = 0x1eb;
        protected static readonly int prime3 = 0x1e7;
        protected static readonly int prime4 = 0x1f7;
        protected static readonly int radbias = (((int)1) << radbiasshift);
        protected static readonly int radbiasshift = 8;
        protected static readonly int radiusbias = (((int)1) << radiusbiasshift);
        protected static readonly int radiusbiasshift = 6;
        protected static readonly int radiusdec = 30;
        protected int[] radpower = new int[initrad];
        protected int samplefac;
        protected byte[] thepicture;

        public NeuQuant(byte[] thepic, int len, int sample)
        {
            this.thepicture = thepic;
            this.lengthcount = len;
            this.samplefac = sample;
            this.network = new int[netsize][];
            for (int i = 0; i < netsize; i++)
            {
                int num2;
                this.network[i] = new int[4];
                int[] numArray = this.network[i];
                numArray[2] = num2 = (i << (netbiasshift + 8)) / netsize;
                numArray[0] = numArray[1] = num2;
                this.freq[i] = intbias / netsize;
                this.bias[i] = 0;
            }
        }

        protected void Alterneigh(int rad, int i, int b, int g, int r)
        {
            int num = i - rad;
            if (num < -1)
            {
                num = -1;
            }
            int netsize = i + rad;
            if (netsize > NeuQuant.netsize)
            {
                netsize = NeuQuant.netsize;
            }
            int num3 = i + 1;
            int num4 = i - 1;
            int num5 = 1;
            while ((num3 < netsize) || (num4 > num))
            {
                int[] numArray;
                Exception exception;
                int num6 = this.radpower[num5++];
                if (num3 < netsize)
                {
                    numArray = this.network[num3++];
                    try
                    {
                        numArray[0] -= (num6 * (numArray[0] - b)) / alpharadbias;
                        numArray[1] -= (num6 * (numArray[1] - g)) / alpharadbias;
                        numArray[2] -= (num6 * (numArray[2] - r)) / alpharadbias;
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                    }
                }
                if (num4 > num)
                {
                    numArray = this.network[num4--];
                    try
                    {
                        numArray[0] -= (num6 * (numArray[0] - b)) / alpharadbias;
                        numArray[1] -= (num6 * (numArray[1] - g)) / alpharadbias;
                        numArray[2] -= (num6 * (numArray[2] - r)) / alpharadbias;
                    }
                    catch (Exception exception2)
                    {
                        exception = exception2;
                    }
                }
            }
        }

        protected void Altersingle(int alpha, int i, int b, int g, int r)
        {
            int[] numArray = this.network[i];
            numArray[0] -= (alpha * (numArray[0] - b)) / initalpha;
            numArray[1] -= (alpha * (numArray[1] - g)) / initalpha;
            numArray[2] -= (alpha * (numArray[2] - r)) / initalpha;
        }

        public byte[] ColorMap()
        {
            int num;
            byte[] buffer = new byte[3 * netsize];
            int[] numArray = new int[netsize];
            for (num = 0; num < netsize; num++)
            {
                numArray[this.network[num][3]] = num;
            }
            int num2 = 0;
            for (num = 0; num < netsize; num++)
            {
                int index = numArray[num];
                buffer[num2++] = (byte)this.network[index][0];
                buffer[num2++] = (byte)this.network[index][1];
                buffer[num2++] = (byte)this.network[index][2];
            }
            return buffer;
        }

        protected int Contest(int b, int g, int r)
        {
            int num = 0x7fffffff;
            int num2 = num;
            int index = -1;
            int num4 = index;
            for (int i = 0; i < netsize; i++)
            {
                int[] numArray = this.network[i];
                int num6 = numArray[0] - b;
                if (num6 < 0)
                {
                    num6 = -num6;
                }
                int num7 = numArray[1] - g;
                if (num7 < 0)
                {
                    num7 = -num7;
                }
                num6 += num7;
                num7 = numArray[2] - r;
                if (num7 < 0)
                {
                    num7 = -num7;
                }
                num6 += num7;
                if (num6 < num)
                {
                    num = num6;
                    index = i;
                }
                int num8 = num6 - (this.bias[i] >> (intbiasshift - netbiasshift));
                if (num8 < num2)
                {
                    num2 = num8;
                    num4 = i;
                }
                int num9 = this.freq[i] >> betashift;
                this.freq[i] -= num9;
                this.bias[i] += num9 << gammashift;
            }
            this.freq[index] += beta;
            this.bias[index] -= betagamma;
            return num4;
        }

        public void Inxbuild()
        {
            int num6;
            int index = 0;
            int num2 = 0;
            for (int i = 0; i < netsize; i++)
            {
                int[] numArray2;
                int[] numArray = this.network[i];
                int num4 = i;
                int num5 = numArray[1];
                num6 = i + 1;
                while (num6 < netsize)
                {
                    numArray2 = this.network[num6];
                    if (numArray2[1] < num5)
                    {
                        num4 = num6;
                        num5 = numArray2[1];
                    }
                    num6++;
                }
                numArray2 = this.network[num4];
                if (i != num4)
                {
                    num6 = numArray2[0];
                    numArray2[0] = numArray[0];
                    numArray[0] = num6;
                    num6 = numArray2[1];
                    numArray2[1] = numArray[1];
                    numArray[1] = num6;
                    num6 = numArray2[2];
                    numArray2[2] = numArray[2];
                    numArray[2] = num6;
                    num6 = numArray2[3];
                    numArray2[3] = numArray[3];
                    numArray[3] = num6;
                }
                if (num5 != index)
                {
                    this.netindex[index] = (num2 + i) >> 1;
                    num6 = index + 1;
                    while (num6 < num5)
                    {
                        this.netindex[num6] = i;
                        num6++;
                    }
                    index = num5;
                    num2 = i;
                }
            }
            this.netindex[index] = (num2 + maxnetpos) >> 1;
            for (num6 = index + 1; num6 < 0x100; num6++)
            {
                this.netindex[num6] = maxnetpos;
            }
        }

        public void Learn()
        {
            int num8;
            int num9;
            if (this.lengthcount < minpicturebytes)
            {
                this.samplefac = 1;
            }
            this.alphadec = 30 + ((this.samplefac - 1) / 3);
            byte[] thepicture = this.thepicture;
            int index = 0;
            int lengthcount = this.lengthcount;
            int num3 = this.lengthcount / (3 * this.samplefac);
            int num4 = num3 / ncycles;
            int initalpha = NeuQuant.initalpha;
            int initradius = NeuQuant.initradius;
            int rad = initradius >> radiusbiasshift;
            if (rad <= 1)
            {
                rad = 0;
            }
            for (num8 = 0; num8 < rad; num8++)
            {
                this.radpower[num8] = initalpha * ((((rad * rad) - (num8 * num8)) * radbias) / (rad * rad));
            }
            if (this.lengthcount < minpicturebytes)
            {
                num9 = 3;
            }
            else if ((this.lengthcount % prime1) != 0)
            {
                num9 = 3 * prime1;
            }
            else if ((this.lengthcount % prime2) != 0)
            {
                num9 = 3 * prime2;
            }
            else if ((this.lengthcount % prime3) != 0)
            {
                num9 = 3 * prime3;
            }
            else
            {
                num9 = 3 * prime4;
            }
            num8 = 0;
            while (num8 < num3)
            {
                int b = (thepicture[index] & 0xff) << netbiasshift;
                int g = (thepicture[index + 1] & 0xff) << netbiasshift;
                int r = (thepicture[index + 2] & 0xff) << netbiasshift;
                int i = this.Contest(b, g, r);
                this.Altersingle(initalpha, i, b, g, r);
                if (rad != 0)
                {
                    this.Alterneigh(rad, i, b, g, r);
                }
                index += num9;
                if (index >= lengthcount)
                {
                    index -= this.lengthcount;
                }
                num8++;
                if (num4 == 0)
                {
                    num4 = 1;
                }
                if ((num8 % num4) == 0)
                {
                    initalpha -= initalpha / this.alphadec;
                    initradius -= initradius / radiusdec;
                    rad = initradius >> radiusbiasshift;
                    if (rad <= 1)
                    {
                        rad = 0;
                    }
                    for (i = 0; i < rad; i++)
                    {
                        this.radpower[i] = initalpha * ((((rad * rad) - (i * i)) * radbias) / (rad * rad));
                    }
                }
            }
        }

        public int Map(int b, int g, int r)
        {
            int num = 0x3e8;
            int num2 = -1;
            int index = this.netindex[g];
            int num4 = index - 1;
            while ((index < netsize) || (num4 >= 0))
            {
                int[] numArray;
                int num5;
                int num6;
                if (index < netsize)
                {
                    numArray = this.network[index];
                    num5 = numArray[1] - g;
                    if (num5 >= num)
                    {
                        index = netsize;
                    }
                    else
                    {
                        index++;
                        if (num5 < 0)
                        {
                            num5 = -num5;
                        }
                        num6 = numArray[0] - b;
                        if (num6 < 0)
                        {
                            num6 = -num6;
                        }
                        num5 += num6;
                        if (num5 < num)
                        {
                            num6 = numArray[2] - r;
                            if (num6 < 0)
                            {
                                num6 = -num6;
                            }
                            num5 += num6;
                            if (num5 < num)
                            {
                                num = num5;
                                num2 = numArray[3];
                            }
                        }
                    }
                }
                if (num4 >= 0)
                {
                    numArray = this.network[num4];
                    num5 = g - numArray[1];
                    if (num5 >= num)
                    {
                        num4 = -1;
                    }
                    else
                    {
                        num4--;
                        if (num5 < 0)
                        {
                            num5 = -num5;
                        }
                        num6 = numArray[0] - b;
                        if (num6 < 0)
                        {
                            num6 = -num6;
                        }
                        num5 += num6;
                        if (num5 < num)
                        {
                            num6 = numArray[2] - r;
                            if (num6 < 0)
                            {
                                num6 = -num6;
                            }
                            num5 += num6;
                            if (num5 < num)
                            {
                                num = num5;
                                num2 = numArray[3];
                            }
                        }
                    }
                }
            }
            return num2;
        }

        public byte[] Process()
        {
            this.Learn();
            this.Unbiasnet();
            this.Inxbuild();
            return this.ColorMap();
        }

        public void Unbiasnet()
        {
            for (int i = 0; i < netsize; i++)
            {
                this.network[i][0] = this.network[i][0] >> netbiasshift;
                this.network[i][1] = this.network[i][1] >> netbiasshift;
                this.network[i][2] = this.network[i][2] >> netbiasshift;
                this.network[i][3] = i;
            }
        }
    }
}
