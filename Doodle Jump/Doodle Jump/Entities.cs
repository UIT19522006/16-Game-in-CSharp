using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
namespace Doodle_Jump
{
    enum Direction { Up, Down, Left, Right };
    class Entities
    {
        protected Image image;
        protected Point loc;
        protected Size size;
        protected int v;
        public Entities(Image img, Point loc, Size size, int v)
        {
            this.image = img;
            this.loc = loc;
            this.size = size;
            this.v = v;
        }
        public Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        public Image Image { get => image; set => image = value; }
        public Point Loc { get => loc; set => loc = value; }
        public Size Size { get => size; set => size = value; }
        public int V { get => v; set => v = value; }
        public Rectangle Rect { get => new Rectangle(loc, size); }
        virtual public void Draw(Graphics g)
        {
            g.DrawImage(ResizeImage(this.image, size.Width, size.Height),
                new Rectangle(this.loc, this.size),
                new Rectangle(0, 0, this.size.Width, this.size.Height),
                GraphicsUnit.Pixel);
        }
        virtual public void Move(Direction dir)
        {
            switch (dir)
            {
                case Direction.Down:
                    loc.Y += v;
                    break;
                case Direction.Up:
                    loc.Y -= v;
                    break;
                case Direction.Left:
                    loc.X -= v;
                    break;
                case Direction.Right:
                    loc.X += v;
                    break;
            }
        }
    }
}