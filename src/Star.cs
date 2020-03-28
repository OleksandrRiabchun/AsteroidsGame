using System.Drawing; 

namespace OOP2
{
    class Star : BaseObject
    {
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        public override void Draw()
        {
            Game._buffer.Graphics.DrawLine(Pens.White, pos.X, pos.Y, pos.X + size.Width, pos.Y + size.Height);
            Game._buffer.Graphics.DrawLine(Pens.White, pos.X + size.Width, pos.Y, pos.X, pos.Y + size.Height);
        }

        public override void Update()
        {
            pos.X = pos.X - dir.X;
            pos.Y = pos.Y + dir.Y;
            if (pos.X < 0)
                pos.X = Game.Width - size.Width;
            if (pos.X > Game.Width)
                dir.X = -dir.X;
            if (pos.Y < 0)
                dir.Y = -dir.Y;
            if (pos.Y > Game.Height)
                dir.Y = -dir.Y;
        }
    }
}
