using System;
using System.Drawing;
 
namespace OOP2
{
    class Asteroid : BaseObject
    {
        public int Power { get; set; }
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 1;
        }
        public override void Draw()
        {
            Game._buffer.Graphics.FillEllipse(Brushes.White, pos.X, pos.Y, size.Width, size.Height);
        }

        public override void Update()
        {
            pos.X = pos.X + dir.X;
            if (pos.X < 0)
                pos.X = Game.Width + size.Width;
        }

        public void Collis()
        {
            var rnd = new Random();
            pos.X = 800;
            pos.Y = rnd.Next(100, 500);
        }
    }
}
