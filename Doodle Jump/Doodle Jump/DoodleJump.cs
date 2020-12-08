using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Doodle_Jump
{
	public partial class DoodleJump : Form
	{
		Random r = new Random();
		List<Point> plat= new List<Point>();
		Bitmap backbuffer = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
		Image imagebackground = Image.FromFile("background.png");
		Image imagePlat = Image.FromFile("platform.png");
		Image imageDood = Image.FromFile("doodle.png");
		Entities background;
		List<Entities> Plat = new List<Entities>();
		Entities Dood;
		public DoodleJump()
		{
			InitializeComponent();
			this.DoubleBuffered = true;
			Init();
			timer1.Enabled = true;
		}
		private void Init()
		{
			for (int i = 0; i < 10; i++)
			{
				Point p = new Point(r.Next(0, this.Size.Width), r.Next(0, this.Size.Height));
				plat.Add(p);
				Entities tempplat = new Entities(imagePlat,
					new Point(plat[plat.Count - 1].X, plat[plat.Count - 1].Y),
					new Size(imagePlat.Size.Width, imagePlat.Size.Height), 0);
				Plat.Add(tempplat);
			}
			background = new Entities(imagebackground, new Point(0, 0),
				new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height), 0);
			Dood = new Entities(imageDood, new Point(0, 0),
				new Size(imageDood.Width, imageDood.Height), 0);
		}
		void Draw(Graphics g)
		{
			background.Draw(g);
			for (int i = 0; i < Plat.Count; i++)
			{
				Plat[i].Draw(g);
				Plat[i].Loc = plat[i];
			}
			Dood.Draw(g);
		}
		private void DoodleJump_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Draw(g);
		}

		int x = 100, y = 100, h = 200;
		int dx = 0, dy = 0;
		bool goRight = false;
		bool goLeft = false;
		private void DoodleJump_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.A)
			{
				goLeft = false;
			}
			else if (e.KeyCode == Keys.D)
			{
				goRight = false;
			}
		}

		private void DoodleJump_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode==Keys.A)
			{
				goLeft = true;
			}
			else if (e.KeyCode == Keys.D)
			{
				goRight = true;
			}
		}

		private void MoveDood()
		{	
			dy += 2;
			y += dy;
			if (y > this.Height - Dood.Size.Height)
			{
				dy -= 20;
			}
			if(goLeft==true)
			{
				x -= 10;
			}
			if (goRight == true)
			{
				x += 10;
			}
			//Move Background
			if (y < h)
			{
				for (int i = 0; i < 10; i++)
				{
					int X = plat[i].X, Y;
					y = h;
					Y = plat[i].Y - dy;

					if (plat[i].Y > this.Height)
					{
						Y = 0;
						X = r.Next(0, this.Width);
					}

					plat[i] = new Point(X, Y);
				}
			}	
			// Nhảy lên các thanh gạch
			for (int i = 0; i < 10; i++)
				if ((x + 50 > plat[i].X) && (x + 20 < plat[i].X + 68)
				 && (y + 70 > plat[i].Y) && (y + 70 < plat[i].Y + 14) && (dy > 0)) dy = -30;

			Dood.Loc = new Point(x, y);
			this.Invalidate();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			MoveDood();
		}
	}
}
