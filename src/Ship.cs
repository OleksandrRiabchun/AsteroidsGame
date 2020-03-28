using System.Drawing;
 
namespace OOP2
{
    class Ship : BaseObject
    {
        public static event Message MessageDie;
        const int maxEnergy = 100;
        int energy = maxEnergy;
        int point = 0;
        public int Points
        {
            get { return point; }
        }
        
        public void Point(int a)
        {
            point += a;
        }
        public int Energy
        {
            get { return energy; }
        }
        public void EnergyLow(int n) // Уменьшение енергии
        {
             energy -= n; 
        }
        public void EnergyHigh(int n) // Добавление енергии
        {
            if ((energy + n) >= maxEnergy)
                energy = maxEnergy;
            else
                energy += n;
        }
        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        public override void Draw()
        {
            Game._buffer.Graphics.FillEllipse(Brushes.Wheat, pos.X, pos.Y, size.Width, size.Height);
        }
        public override void Update()
        {
        }
        public void Up()
        {
            if (pos.Y > 0) pos.Y = pos.Y - dir.Y;
        }
        public void Down()
        {
            if (pos.Y < Game.Height) pos.Y = pos.Y + dir.Y;
        }
        public void Die()
        {
            if (MessageDie != null)            
                MessageDie();            
        }
    }
}
