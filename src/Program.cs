using System.Windows.Forms;

namespace AsteroidsGame
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

            try
            {
                Game.Init(form);

                form.Show();
                Game.Draw();
                Application.Run(form);  
            }
            catch (Exceptions.CustomException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
                Application.Exit();
            }
        }
    }
}
