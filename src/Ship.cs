using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Configuration;

namespace AsteroidsGame
{
    internal class Ship : BaseObject
    {
        public const int MIN_ENERGY = 1;
        public const int MAX_ENERGY = 100;

        private readonly IList<Bullet> _bullets = new List<Bullet>();
        public int CurrentScore { get; private set; }

        public int Energy { get; private set; } = MAX_ENERGY;

        public event Action Died;
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
             Energy -= n / 2; 
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

            foreach (var bullet in _bullets)
                bullet?.Draw();
        }

        public override void Update()
        {
            for (var i = 0; i < _bullets.Count; i++)
            {
                if (_bullets[i] == null || _bullets[i].IsAbroad)
                {
                    _bullets.RemoveAt(i);
                    continue;
                }

                _bullets[i].Update();
            }
        }

        public void Shot()
        {
            var bullet = new Bullet(
                new Point(Rect.X + 10, Rect.Y + 4),
                new Point(4, 0), new Size(4, 1));
            _bullets.Add(bullet);
        }

        public bool CheckGoal(Asteroid asteroid)
        {
            for (var i = 0; i < _bullets.Count; i++)
            {
                if (asteroid != null && _bullets[i] != null && 
                    _bullets[i].Collision(asteroid))
                {
                    _bullets.RemoveAt(i);
                    ScoreHigh(1);
                    RaiseLog("Сбит астероид", DateTime.Now);
                    return true;
                }
            }
            return false;
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
            LogAction?.Invoke(new LogMessage(this, "Корабль сбит", DateTime.Now));
        }
    }
}
