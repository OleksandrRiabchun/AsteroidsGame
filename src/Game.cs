using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OOP2.Exceptions;

namespace OOP2
{
    internal class Game
    {
        private const int MAX_SIZE_OF_DISPLAY = 1000;

        private BufferedGraphicsContext _context;
        private BufferedGraphics _buffer;
        private BaseObject[] _objs;
        private Bullet _bullet;
        private Asteroid[] _asteroids;
        private readonly Ship _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(10, 10));
        private readonly Timer _timer = new Timer();
        private readonly Random _rnd = new Random();
        private Medic[] _medic;

        /// <summary>
        /// Ширина игрового поля
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота игрового поля
        /// </summary>
        public int Height { get; set; }

        public Game(Form form)
        {
            var g = form.CreateGraphics();              // Графическое устройство для вывода графики
            _context = BufferedGraphicsManager.Current; // предоставляет доступ к главному буферу 

            if (form.Width <= MAX_SIZE_OF_DISPLAY
             && form.Width > 0
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
            _buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            Load();
            _timer.Interval = 100;
            _timer.Start();
            _timer.Tick += Timer_Tick;
            form.KeyDown += Form_KeyDown;
            Ship.MessageDie += Finish;
        }
        
        private void Form_KeyDown(object sender, KeyEventArgs e) // Обработка события нажатия клавиш
        {
            if (e.KeyCode == Keys.ControlKey)
                _bullet = new Bullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 4),
                new Point(4, 0), new Size(4, 1));
            if (e.KeyCode == Keys.Up)
                _ship.Up();
            if (e.KeyCode == Keys.Down)
                _ship.Down();
        }

        static public void Finish()//Завершение игры
        {
            _timer.Stop();
            Journal();
            _buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline),
            Brushes.White, 200, 100);
            _buffer.Render();
        }

        static void Journal() //ведение журнала в консоль и в файл
        {
            Message message = () =>
            {
                using (StreamWriter sw = new StreamWriter(@"C:\Users\Олександр\source\repos\OOP2\OOP2\data.txt", false))
                {
                    sw.WriteLine($"Energy: {_ship.Energy}, Points: {_ship.Points}, Date: { DateTime.Now}");
                }
                Console.WriteLine($"Energy: {_ship.Energy}, Points: {_ship.Points}, Date: { DateTime.Now}");
            };
            message();
        }

        public static void Draw()
        {
            // Вывод графики
            _buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            foreach (Asteroid a in _asteroids)
                if (a != null)
                    a.Draw();
            if (_bullet != null)
                _bullet.Draw();
            foreach (Medic m in _medic)
                if (m != null)
                    m.Draw();
            _ship.Draw();
            _buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
            _buffer.Graphics.DrawString("Points:" + _ship.Points, SystemFonts.DefaultFont, Brushes.White, 0, 20);
            _buffer.Render();
        }

        public static void Load() // Инициализация обьектов
        {
            _objs = new BaseObject[30];
            //bullet = new Bullet(new Point(0, 200), new Point(5, 0), new Size(4, 1));
            _asteroids = new Asteroid[10];
            _medic = new Medic[5];
            for (int i = 0; i < _objs.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _objs[i] = new Star(new Point(800, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(3, 3));
            }
            for (int i = 0; i < _asteroids.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _asteroids[i] = new Asteroid(new Point(800, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new
                Size(r, r));
            }
            for (int i = 0; i < _medic.Length; i++)
            {
                int r = rnd.Next(5, 20);
                _medic[i] = new Medic(new Point(1000, Game.rnd.Next(0, Game.Height)), new Point(-r / 5, r),
                    new Size(20, 20));
            }
        }

        public static void Update() // Меняем состояние обьектов
        {
            foreach (BaseObject obj in _objs)
                obj.Update();

            if (_bullet != null)
                _bullet.Update();
            for (int i = 0; i < _asteroids.Length; i++)
            {
                if (_asteroids[i] != null)
                {
                    _asteroids[i].Update();
                    if (_bullet != null && _bullet.Collision(_asteroids[i]))//пуля с астероидом
                    {
                        _ship.Point(1);
                        System.Media.SystemSounds.Hand.Play();
                        _asteroids[i] = null;
                        _bullet = null;
                        continue;
                    }
                    if (_ship.Collision(_asteroids[i]))//корабль с астероидом
                    {
                        _ship.EnergyLow(5);
                        System.Media.SystemSounds.Asterisk.Play();
                        if (_ship.Energy <= 0)
                            _ship.Die();
                    }
                }
            }
            for (int i = 0; i < _medic.Length; i++)
            {
                _medic[i].Update();
                if (_ship.Collision(_medic[i]))//корабль с аптечкой
                {
                    if (_ship.Energy <= 100)
                    {
                        _ship.EnergyHigh(5);
                        System.Media.SystemSounds.Asterisk.Play();
                    }
                }
            }
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
    }
}

