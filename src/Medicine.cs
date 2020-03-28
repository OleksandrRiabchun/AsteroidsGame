using System.Drawing;

namespace AsteroidsGame
{
    internal class Medicine : BaseObject
    {
        public int Health { get; private set; } = 5;

		public Medicine(Point pos, Point dir, Size size) : base(pos, dir, size)
		{
		}

		public override void Draw()
		{
			Game.Buffer.Graphics.DrawRectangle(new Pen(Color.Red, 2), _pos.X, _pos.Y, _size.Width, _size.Height);
		}

		public override void Update()
		{
			_pos.X += _dir.X;
			if (_pos.X < 0)
				_pos.X = Game.Width + _size.Width;			 
		}
	}
}
