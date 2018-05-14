using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwoD00m.cWorld;

namespace TwoD00m
{
    public static class Loader
    {
        public static GameKit<TObj> LoadKit<TObj>(string filePath) where TObj : new()
        {
            if (File.Exists(filePath))
            {
                GameKit<TObj> kit = new GameKit<TObj>();
                string[] allStr = File.ReadAllLines(filePath).Where(str => !string.IsNullOrWhiteSpace(str)).ToArray();
                List<string> unitInfo = new List<string>();
                bool startBlockInformation = false;
                foreach (var str in allStr)
                {
                    if (str.Trim().StartsWith("}"))
                    {
                        startBlockInformation = false;
                        kit.Add((TObj)Activator.CreateInstance(typeof(TObj), unitInfo));
                        unitInfo.Clear();
                    }
                    if (startBlockInformation)
                    {
                        unitInfo.Add(str.Trim());
                    }
                    if (str.Trim().StartsWith("{"))
                        startBlockInformation = true;
                }
                return kit;
            }
            Console.WriteLine("Файл не найден: {0}!", filePath);
            return null;
        }

        public static World LoadWorld(string filePath)
        {
            if (File.Exists(filePath))
            {
                string[] allStr = File.ReadAllLines(filePath).Where(str => !string.IsNullOrWhiteSpace(str)).ToArray();
                string[] splitStr = allStr[0].Split(new char[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries);
                World world = new World(splitStr);
                return world;
            }
            return null;
        }

        public static string GetStringParameter(List<string> str, string key)
        {
            string line = str.First(s => s.StartsWith(key));
            line = line.Replace(key + ":", "");
            return line.Replace("\"", "").Trim();
        }
        public static bool GetBooleanParameter(List<string> str, string key)
        {
            string line = str.First(s => s.StartsWith(key));
            line = line.Replace(key + ":", "");
            if (line.Trim().Equals("true"))
                return true;
            else
                return false;
        }
        
        public static Direction GetModelDirection(string str)
        {
            return Direction.Code(Convert.ToInt32(str));
        }
    }

    public class GameKit<TObj>
    {
        private List<TObj> kit = new List<TObj>();

        public void Add(TObj model)
        {
            kit.Add(model);
        }

        public TObj GetObject(int index)
        {
            if (index < kit.Count)
                return kit[index];
            else
                return default(TObj);
        }

        public List<TObj> GetKit()
        {
            return kit;
        }
    }

}
