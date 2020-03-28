using System.Drawing;

namespace AsteroidsGame
{
    internal class Bullet : BaseObject
    {
        public int Speed { get; }

        public bool IsAbroad => _pos.X > Game.Width;

        public Bullet(Point pos, Point dir, Size size, int speed = 10) 
            : base(pos, dir, size)
        {
            Speed = speed;
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawRectangle(Pens.OrangeRed, _pos.X, _pos.Y, _size.Width, _size.Height);
        }

        public override void Update() => _pos.X += Speed;
    }
}
