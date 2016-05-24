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
        public delegate void LoadCompleteEventHandler();
        public event LoadCompleteEventHandler loadComplete;

        MidiParser parser;
        DirectoryInfo currentDirectoryInfo;
        FileInfo currentFileInfo;
        Random random;

        public File()
        {
            parser = new MidiParser();
            currentDirectoryInfo = null;
            currentFileInfo = null;
            random = new Random();
        }

        public void Load(string path, Score score)
        {
            currentFileInfo = new FileInfo(path);
            currentDirectoryInfo = new DirectoryInfo(Path.GetDirectoryName(path));
            score.Name = Path.GetFileNameWithoutExtension(currentFileInfo.Name);
            BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open));
            List<Byte> inputList = new List<Byte>();
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                byte j = reader.ReadByte();
                inputList.Add(j);
            }
            reader.Close();
            parser.Parse(inputList, score);
            NotifyLoadComplete();
        }

        public void RandomLoad(Score score)
        {
            FileInfo[] files = currentDirectoryInfo.GetFiles("*.mid");
            Load(files[random.Next(0, files.Count() - 1)].FullName, score);
        }

        public void SequentialLoad(Score score, int offset)
        {
            FileInfo[] files = currentDirectoryInfo.GetFiles("*.mid");
            for (int i = 0; i < files.Count(); i++)
            {
                if(files[i].Name == currentFileInfo.Name){
                    Load(files[(i + offset + files.Count()) % files.Count()].FullName, score);
                    break;
                }
            }
        }

        private void NotifyLoadComplete()
        {
            if (loadComplete != null)
            {
                loadComplete();
            }
        }
    }
}
