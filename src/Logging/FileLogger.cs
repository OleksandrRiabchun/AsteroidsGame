using AsteroidsGame.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
