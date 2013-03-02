using System;

namespace SkipList
{
    class Program
    {
        static void Main()
        {
            var skipList = new SkipList<int>();
            var ran = new Random();
            int j;
            for (int i = 0; i < 15; i++)
            {
                j = ran.Next(0, 100);
                skipList.Add(j);
                Console.WriteLine(skipList.Contains(j).ToString() + j);
            }
            Console.WriteLine(skipList.ToString());
            Console.ReadLine();
        }
    }
}
