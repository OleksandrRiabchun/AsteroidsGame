using System;
using System.Drawing;

namespace AsteroidsGame
{
    internal class Ship : BaseObject
    {
        public const int MIN_ENERGY = 1;
        public const int MAX_ENERGY = 100;

        public int CurrentScore { get; private set; }

        public int Energy { get; private set; } = MAX_ENERGY;

        public static event Action Died;
        public event Action<LogMessage> LogAction;

        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        /// <summary>
        /// Уменьшение енергии
        /// </summary>
        /// <param name="n"></param>
        public void EnergyLow(int n)
        {
             Energy -= n; 
             RaiseLog("Нанесен урон кораблю", DateTime.Now);
        }

        /// <summary>
        /// Добавление енергии
        /// </summary>
        /// <param name="n"></param>
        public void EnergyHigh(int n)
        {
            if (Energy + n >= MAX_ENERGY)
                Energy = MAX_ENERGY;
            else
            {
                Energy += n;
                RaiseLog("Получил енергию", DateTime.Now);
            }
        }

        public void ScoreHigh(int a) => CurrentScore += a;

        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.Wheat, _pos.X, _pos.Y, _size.Width, _size.Height);
        }

        public override void Update()
        {
        }

        public void Up()
        {
            if (_pos.Y > 0) _pos.Y -= _dir.Y;
        }

        public void Down()
        {
            if (_pos.Y < Game.Height) _pos.Y += _dir.Y;
        }

        public void RaiseLog(string message, DateTime timestamp)
        {
            LogAction?.Invoke(new LogMessage(this, message, timestamp));
        }

        public void RaiseDie()
        {
            Died?.Invoke();
            LogAction?.Invoke(new LogMessage(this, "Корабель сбит", DateTime.Now));
        }
    }
}
