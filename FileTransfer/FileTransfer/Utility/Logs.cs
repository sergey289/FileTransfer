using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer.Utility
{
    public class Logs
    {
        public static Logs logDir;
        private string dirPath = null;

        private Logs(string dirPath)
        {
            this.dirPath = dirPath;
        }

        public static Logs createSingletonLog(string dirPath)
        {
            if (logDir==null)
            {
                logDir = new Logs(dirPath);
            }
            return logDir;
        }
        public void write(string fileName, string tempContent)
        {
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            string dateTime = DateTime.Now.ToString("dd/MM/yyyy h:mm:ss");
            string content = dateTime +" :: "+ tempContent+ "\n";
            string fullPath = dirPath + "\\" + fileName;
            File.AppendAllText(fullPath, content);
      
        }
        public void write(string fileName , string subDir, string tempContent)
        {
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            string nowDir = dirPath + "\\" + subDir;
            if (!Directory.Exists(nowDir)) Directory.CreateDirectory(nowDir);

            string dateTime = DateTime.Now.ToString("dd/MM/yyyy h:mm:ss");
            string content = dateTime + " :: " + tempContent + "\n";
            string fullPath = nowDir + "\\" + fileName;
            File.AppendAllText(fullPath, content);

        }
       

    }
}
