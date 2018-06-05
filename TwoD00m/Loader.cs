using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwoD00m.PlayerItems.Arsenal;
using System.Reflection;
using TwoD00m.cWorld;
using Microsoft.Xna.Framework;

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
                        kit.Add(unitInfo);
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
                string[] blocksInfo = allStr[0].Split(new char[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries);
                World world = new World();
                foreach (var blockInfo in blocksInfo)
                {
                    string[] info = blockInfo.Replace("{", "").Replace("}", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    Block block = new Block(info);
                    Point position = new Point(Convert.ToInt32(info[0]), Convert.ToInt32(info[1]));
                    world.AddBlock(position, block);
                }
                return world;
            }
            return null;
        }

        public static string GetStringParameter(this List<string> str, string key)
        {
            string line = str.First(s => s.StartsWith(key));
            string[] splitLine = line.Split( new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            return splitLine[1].Replace("\"", "").Trim();
        }

        public static bool ConteinsKey(this List<string> str, string key)
        {
            try
            {
                string line = str.First(s => s.StartsWith(key));
                return true;
            } catch (Exception)
            {
                return false;
            }
        }
        public static float GetFloatParameter(this List<string> str, string key)
        {
            string line = str.First(s => s.StartsWith(key));
            string[] splitLine = line.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                return float.Parse(splitLine[1].Trim());
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static Point GetPointParameter(this List<string> str, string key)
        {
            string line = str.First(s => s.StartsWith(key));
            string[] splitLine = line.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            string[] pointParameters = splitLine[1].Trim().Split(new char[] { ',' });
            try
            {
                return new Point(int.Parse(pointParameters[0]), int.Parse(pointParameters[1]));
            }
            catch
            {
                return new Point();
            }
        }
        public static bool GetBooleanParameter(this List<string> str, string key)
        {
            string line = str.First(s => s.StartsWith(key));
            string[] splitLine = line.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (splitLine[1].Trim().Equals("true"))
                return true;
            else
                return false;
        }
    }

    public class GameKit<TObj>
    {
        private List<TObj> kit = new List<TObj>();
        private Dictionary<string, ConstructorInfo> constructorsOfObj = new Dictionary<string, ConstructorInfo>();

        public GameKit()
        {
            SetHeirs();
        }

        public List<TObj> ToList()
        {
            return kit;
        }
        public void Add(List<string> unitInfo)
        {
            if (unitInfo.ConteinsKey("Type"))
            {
                string type = Loader.GetStringParameter(unitInfo, "Type");
                if (constructorsOfObj.ContainsKey(type))
                {
                    kit.Add((TObj)constructorsOfObj[type].Invoke(new object[] { unitInfo }));
                }
            } else {
                kit.Add((TObj)typeof(TObj).GetConstructor(new Type[] { typeof(List<string>) }).Invoke(new object[] { unitInfo }));
            }
        }

        public TObj GetObject(int index)
        {
            if (index < kit.Count)
                return kit[index];
            else
                return default(TObj);
        }
        public TObj GetObject(string id)
        {
            TObj[] obj = kit.Where(model => model.ToString() == id).ToArray();
            if (obj.Length > 0)
                return obj[0];
            else
                return default(TObj);
        }

        private void SetHeirs()
        {
            Type baseClass = typeof(TObj);
            List<Type> subClasses = Assembly.GetExecutingAssembly().GetExportedTypes().Where(i => i.IsSubclassOf(baseClass)).ToList();

            if (subClasses.Count != 0)
            {
                foreach (var subClass in subClasses)
                {
                    ConstructorInfo subClassConstructor = subClass.GetConstructor(new Type[] { typeof(List<string>) });
                    constructorsOfObj.Add(subClass.Name, subClassConstructor);
                }
            }
        }
    }

}
