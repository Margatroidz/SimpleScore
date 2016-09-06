using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleScore.Model
{
    public class File
    {
        DirectoryInfo currentDirectoryInfo;
        FileInfo currentFileInfo;
        Random random;

        public File()
        {
            currentDirectoryInfo = null;
            currentFileInfo = null;
            random = new Random();
        }

        public byte[] Load(string path)
        {
            currentFileInfo = new FileInfo(path);
            currentDirectoryInfo = new DirectoryInfo(Path.GetDirectoryName(path));
            BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open));
            List<Byte> inputList = new List<Byte>();
            while (reader.BaseStream.Position < reader.BaseStream.Length)
                inputList.Add(reader.ReadByte());
            reader.Close();
            return inputList.ToArray();
        }

        public byte[] RandomLoad()
        {
            FileInfo[] files = currentDirectoryInfo.GetFiles("*.mid");
            return Load(files[random.Next(0, files.Count() - 1)].FullName);
        }

        public byte[] SequentialLoad(int offset)
        {
            FileInfo[] files = currentDirectoryInfo.GetFiles("*.mid");
            int position = 0;
            for (position = 0; position < files.Count(); position++)
            {
                if (files[position].Name == currentFileInfo.Name) break;
            }//shit teammate, can't do anything but messed up, doing 10 hr shit document
            //加上files.Count()是因為offset可以為負的(往前)，所以加上一個較大較大的整數
            return Load(files[(position + offset + files.Count()) % files.Count()].FullName);
        }

        public string CurrentFileName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(currentFileInfo.Name);
            }
        }
    }
}
