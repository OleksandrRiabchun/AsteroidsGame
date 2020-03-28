using System.Drawing;

namespace AsteroidsGame
{
    internal class Asteroid : BaseObject
    {
        /// <summary>
        /// Сила урона
        /// </summary>
        public int Power { get; }

        public Asteroid(Point pos, Point dir, Size size, int power) 
            : base(pos, dir, size)
        {
            Power = power;
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.White, _pos.X, _pos.Y, _size.Width, _size.Height);
        }

        public override void Update()
        {
            _pos.X += _dir.X;
            if (_pos.X < 0)
                _pos.X = Game.Width + _size.Width;
        }
    }
}
