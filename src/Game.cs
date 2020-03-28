using System;
using System.Drawing;
using System.Windows.Forms;
using AsteroidsGame.Exceptions;
using AsteroidsGame.Logging;

namespace AsteroidsGame
{
    internal static class Game
    {
        private const int MAX_SIZE_OF_DISPLAY = 1000;

        private static BufferedGraphicsContext _context;
        private static BaseObject[] _backgroundObjects;

        private static Asteroid[] _asteroids;
        private static Medicine[] _medics;
        private static Ship _ship;

        private static readonly Timer _timer = new Timer();
        private static readonly Random _rnd = new Random();

        private static readonly ILogger[] _loggers = { new ConsoleLogger() };

        /// <summary>
        /// Ширина игрового поля
        /// </summary>
        public static int Width { get; private set; }

        /// <summary>
        /// Высота игрового поля
        /// </summary>
        public static int Height { get; private set; }

        public static BufferedGraphics Buffer { get; private set; }

        /// <summary>
        /// Инициализация игры
        /// </summary>
        /// <exception cref="CustomException">При вводе недопустимного размера формы</exception>
        /// <param name="form"></param>
        public static void Init(Form form)
        {
            var g = form.CreateGraphics();              // Графическое устройство для вывода графики
            _context = BufferedGraphicsManager.Current; // предоставляет доступ к главному буферу 

            if (form.Width <= MAX_SIZE_OF_DISPLAY && form.Width > 0
                                                 && form.Height <= MAX_SIZE_OF_DISPLAY && form.Height > 0)
            {
                Width = form.Width;
                Height = form.Height;
            }
            else
            {
                throw new CustomException("Значение аргумента выходит за допустимый диапазон значений");
            }

            // Связываем буфер в памяти с графическим объектом. Для того, чтобы рисовать в буфере 
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(10, 10));

            Load();

            _timer.Interval = 100;
            _timer.Start();
            _timer.Tick += RegularUpdateView;
            form.KeyDown += OnKeyDown;
            _ship.Died += Finish;
            _ship.LogAction += OnLog;

            OnLog(new LogMessage(typeof(Game), "Началась новая игра", DateTime.Now));
        }

        private static void OnLog(LogMessage log)
        {
            foreach (var logger in _loggers)
                logger.Write($"{log.Timestamp}\t{log.Message} - {log.Sender}");
        }

        /// <summary>
        /// Инициализация обьектов
        /// </summary>
        private static void Load()
        {
            _backgroundObjects = new BaseObject[30];
            _asteroids = new Asteroid[10];
            _medics = new Medicine[5];
            const int MIN_OBJ_SIZE = 5;

            for (var i = 0; i < _backgroundObjects.Length; i++)
            {
                var dir = _rnd.Next(MIN_OBJ_SIZE, 50);
                _backgroundObjects[i] = new Star(new Point(800, _rnd.Next(0, Height)), new Point(-dir, dir), new Size(3, 3));
            }

            for (var i = 0; i < _asteroids.Length; i++)
            {
                var size = _rnd.Next(MIN_OBJ_SIZE, 50);
                _asteroids[i] = new Asteroid(new Point(800, _rnd.Next(0, Height)), new Point(-size / 5, size), new
                                                 Size(size, size), size);
            }

            for (var i = 0; i < _medics.Length; i++)
            {
                var dir = _rnd.Next(MIN_OBJ_SIZE, 20);
                _medics[i] = new Medicine(new Point(1000, _rnd.Next(0, Height)), new Point(-dir / 5, dir),
                                          new Size(width: 20, height: 20));
            }
        }

        public static void Draw()
        {
            // Вывод графики
            Buffer.Graphics.Clear(Color.Black);
            foreach (var obj in _backgroundObjects)
                obj.Draw();

            foreach (var a in _asteroids)
                a?.Draw();

            foreach (var m in _medics)
                m?.Draw();

            _ship.Draw();
            Buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
            Buffer.Graphics.DrawString("Score:" + _ship.CurrentScore, SystemFonts.DefaultFont, Brushes.White, 0, 20);
            Buffer.Render();
        }

        /// <summary>
        /// Меняем состояние обьектов
        /// </summary>
        private static void Update()
        {
            foreach (var obj in _backgroundObjects)
                obj.Update();

            _ship.Update();

            for (var i = 0; i < _asteroids.Length; i++)
            {
                _asteroids[i]?.Update();

                if (_ship.CheckGoal(_asteroids[i]))
                {
                    System.Media.SystemSounds.Hand.Play();
                    _asteroids[i] = null;
                }

                if (_asteroids[i] == null) continue;

                if (_ship.Collision(_asteroids[i])) //корабль с астероидом
                {
                    _ship.EnergyLow(_asteroids[i].Power);
                    System.Media.SystemSounds.Asterisk.Play();
                    if (_ship.Energy < Ship.MIN_ENERGY)
                        _ship.RaiseDie();
                }
            }

            foreach (var medic in _medics)
            {
                medic.Update();
                if (_ship.Collision(medic)) //корабль с аптечкой
                {
                    if (_ship.Energy <= Ship.MAX_ENERGY)
                    {
                        _ship.EnergyHigh(medic.Health);
                        System.Media.SystemSounds.Asterisk.Play();
                    }
                }
            }
        }

        /// <summary>
        /// Обработка события нажатия клавиш
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    _ship.Shot();
                    break;
                case Keys.Up:
                    _ship.Up();
                    break;
                case Keys.Down:
                    _ship.Down();
                    break;
            }
        }

        /// <summary>
        /// Завершение игры
        /// </summary>
        private static void Finish()
        {
            OnLog(new LogMessage(typeof(Game), "Игра закончилась", DateTime.Now));
            _timer.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline),
                                        Brushes.White, 200, 100);
            Buffer.Render();
        }

        private static void RegularUpdateView(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
    }
}