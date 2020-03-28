using System.Drawing; 

namespace OOP2
{
    class Medic : BaseObject
    {
		//public int Helth { get; set; } = 5;	 

		public Medic(Point pos, Point dir, Size size) : base(pos, dir, size)
		{			 
		}

		public override void Draw()
		{
			Game._buffer.Graphics.DrawRectangle(new Pen(Color.Red, 2), pos.X, pos.Y, size.Width, size.Height);
		}

		public override void Update()
		{
			pos.X = pos.X + dir.X;
			if (pos.X < 0)
				pos.X = Game.Width + size.Width;			 
		}
	}
}
