using AsteroidsGame.Logging;
using System.IO;

namespace AsteroidsGame
{
    class FileLogger : ILogger
    {
        public void Write(string message)
        {
            using (StreamWriter sw = new StreamWriter(@"C:\Users\Олександр\Desktop\LOG.txt", true))
            {
                sw.WriteLine(message);
            }
        }
    }
}
