using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsSetlSityTest
{
    class ManagerSort
    {
        public long CountEven(int size)
        {
            var array = GetArray(size);

            var task = SortAsync(array);

            Task.WaitAll(task);

            return task.Result.Where(i => i % 2 == 0).Count();
        }

        async Task<int[]> SortAsync(int[] array)
        {
            await Task.Factory.StartNew(() => Sort.SortB(array));
            await Task.Factory.StartNew(() => Sort.SortS(array));
            return await Task.Factory.StartNew(() => Sort.SortI(array));
        }

        int[] GetArray(int size)
        {
            int[] array = new int[size];

            var random = new Random();

            for (int i = 0; i < size; i++)
            {
                array[i] = random.Next(0, 2047);
            }

            return array;
        }
    }
}
