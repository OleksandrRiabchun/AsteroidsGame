using System.Drawing; 

namespace OOP2
{
    delegate void Message();
    interface ICollision
    {
        bool Collision(ICollision obj);
        Rectangle Rect { get; }
    }

    abstract class BaseObject : ICollision
    {
        protected Point pos;
        protected Point dir;
        protected Size size;

        public BaseObject(Point pos, Point dir, Size size)
        {
            this.pos = pos;
            this.dir = dir;
            this.size = size;
        }
        abstract public void Draw();

        abstract public void Update();         

        public bool Collision(ICollision o)
        {
            if (o.Rect.IntersectsWith(this.Rect))
                return true;
            else return false;
        }
        public Rectangle Rect
        {
            get { return new Rectangle(pos, size); }
        }

    }
}
