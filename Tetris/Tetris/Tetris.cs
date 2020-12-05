using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Tetris
{
	public partial class Tetris : Form
	{
		private Bitmap sprite;
		private Bitmap backBuffer;
		private Timer timer;
		public Graphics graphics;
		//Số thứ tự của frame, có 8 màu lầ 8 frame
		private int index;
		// dòng hiện tại của frame
		private int curFrameColumn;
		// cột hiện tại của frame
		//private int curFrameRow;
		const int M = 20, N = 10;
		int[,] field = new int[M, N];
		int[,] figures = new int[7, 4]
		{
			{ 1,3,5,7 }, // I
			{ 2,4,5,7 }, // Z
			{ 3,5,4,6 }, // S
			{ 3,5,4,7 }, // T
			{ 2,3,5,7 }, // L
			{ 3,5,7,6 }, // J
			{ 2,3,4,5 } // O} 
		};
		Point[] fall = new Point[4];

		int dx = 0;
		bool rotate = false;
		//int columnNum = 1;
		public Tetris()
		{
			InitializeComponent();
			graphics = this.CreateGraphics();
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			backBuffer = new Bitmap(this.ClientSize.Width,
			this.ClientSize.Height);
			sprite = new Bitmap("images/tiles.png");
			//this.DoubleBuffered = true;
			index = 0;
			Render();
			graphics.DrawImageUnscaled(backBuffer, 0, 0);
			// Khởi tạo một đồng hồ
			timer = new Timer();
			timer.Enabled = true;
			timer.Interval = 300;
			timer.Tick += new EventHandler(timer_Tick);
		}

		private void Render()
		{
			// Lấy đối tượng graphics để vẽ lên back buffer
			Graphics g = Graphics.FromImage(backBuffer);
			g.Clear(Color.White);
			// Xác dịnh số dòng, cột của một frame trên ảnh sprite
			curFrameColumn = index;// % 5;
			//Move
			for(int i=0;i<4;i++)
			{
				fall[i].X += dx;
			}	
			//Rotate
			if(rotate)
			{
				Point temp = fall[1];
				for(int i=0;i<4;i++)
				{
					int x = fall[i].Y - temp.Y;
					int y = fall[i].X - temp.X;
					fall[i].X = temp.X - x;
					fall[i].Y = temp.Y + y;
				}	
			}

			///// Logic game make me confused -.- try this game later i guess
			//Set lại ban đầu
			dx = 0;
			rotate = false;


			int n = 3;
			if(fall[0].X==0)
			for(int i=0;i<4;i++)
			{
				fall[i] = new Point(figures[n,i] % 2, figures[n,i] / 2);
			}


			//Vẽ lên buffer
			for(int i=0;i<4;i++)
			{
				g.DrawImage(sprite, fall[i].X*18, fall[i].Y*18, 
					new Rectangle(curFrameColumn * 18, 0, 18, 18), GraphicsUnit.Pixel);
			}	
			
			g.Dispose();
			// Tăng thứ tự frame để lấy frame tiếp theo
			//index++;
			//if (index >= 8)
			//	index = 0;
			//else
			//	index++;
		}

		private void Tetris_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up)
			{
				rotate = true;
			}
			else if (e.KeyCode == Keys.Right) dx = 1;
			else if (e.KeyCode == Keys.Left) dx = -1;
			this.Invalidate();
		}

		private void Tetris_Paint(object sender, PaintEventArgs e)
		{
			Render();
			graphics.DrawImageUnscaled(backBuffer, 0, 0);
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			for(int i=0;i<4;i++)
			{
				fall[i].Y += 1;
			}
			this.Invalidate();
		}
	}
}
