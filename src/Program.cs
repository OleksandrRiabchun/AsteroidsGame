using System.Windows.Forms;

namespace OOP2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var form = new Form
            {
                Width = 1000,
                Height = 800
            };

            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);            
        }
    }
}
