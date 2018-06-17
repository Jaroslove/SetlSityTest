using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsSetlSityTest
{
    public class Tasks
    {
        public string TaskOne(int input)
        {
            return Convert.ToString(input, 2);
        }

        public string TaskTwo(int input)
        {
            return input.ToString("X");
        }

        public long TaskThree(int input)
        {
            var manager = new ManagerSort();
            return manager.CountEven(input);
        }

        public long TaskFour(int input)
        {
            if (input < 3)
                return 1;
            else
                return TaskFour(input - 1) + TaskFour(input - 2);
        }

        public string TaskSeven(string input)
        {
            List<char> chars = new List<char> { ';', ',', ':', '.', '?', '!' };
            string[] r = input.Split(' ');
            List<string> list = new List<string>();
            foreach (var item in r)
            {
                if (chars.Contains(item[item.Length - 1]))
                {
                    char last = item[item.Length - 1];
                    var subStr = item.Substring(0, item.Length - 1);
                    var s = subStr.ToArray().Reverse().ToArray();
                    list.Add(new string(s) + last.ToString() + " ");
                }
                else
                {
                    list.Add(new string(item.ToCharArray().Reverse().ToArray()));
                }
            }

            return String.Concat(list).Trim();
        }
    }
}
