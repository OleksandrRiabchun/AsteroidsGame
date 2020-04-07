using System.Drawing;

namespace AsteroidsGame
{
    internal interface ICollision
    {
        bool Collision(ICollision obj);
        Rectangle Rect { get; }
    }

    internal abstract class BaseObject : ICollision
    {
        protected Point _pos;
        protected Point _dir;
        protected Size _size;

        protected BaseObject(Point pos, Point dir, Size size)
        {
            _pos = pos;
            _dir = dir;
            _size = size;
        }

        public abstract void Draw();

        public abstract void Update();

        public bool Collision(ICollision o) => o.Rect.IntersectsWith(this.Rect);

        public Rectangle Rect => new Rectangle(_pos, _size);
    }
}
