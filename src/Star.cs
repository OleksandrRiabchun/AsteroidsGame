using System.Drawing;

namespace AsteroidsGame
{
    internal class Star : BaseObject
    {
        public Star(Point pos, Point dir, Size size) 
            : base(pos, dir, size)
        {
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawLine(Pens.White, _pos.X, _pos.Y, _pos.X + _size.Width, _pos.Y + _size.Height);
            Game.Buffer.Graphics.DrawLine(Pens.White, _pos.X + _size.Width, _pos.Y, _pos.X, _pos.Y + _size.Height);
        }

        public override void Update()
        {
            _pos.X -= _dir.X;
            _pos.Y += _dir.Y;
            if (_pos.X < 0)
                _pos.X = Game.Width - _size.Width;
            if (_pos.X > Game.Width)
                _dir.X = -_dir.X;
            if (_pos.Y < 0)
                _dir.Y = -_dir.Y;
            if (_pos.Y > Game.Height)
                _dir.Y = -_dir.Y;
        }
    }
}
