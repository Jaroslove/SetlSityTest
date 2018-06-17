using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsSetlSityTest
{
    class Sort
    {

        public static int[] SortB(int[] array)
        {
            int length = array.Length;

            int temp = array[0];

            for (int i = 0; i < length; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    if (array[i] > array[j])
                    {
                        temp = array[i];

                        array[i] = array[j];

                        array[j] = temp;
                    }
                }
            }

            return array;
        }

        public static int[] SortS(int[] array)
        {
            int pos_min, temp;

            for (int i = 0; i < array.Length - 1; i++)
            {
                pos_min = i;

                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[j] < array[pos_min])
                    {
                        pos_min = j;
                    }
                }

                if (pos_min != i)
                {
                    temp = array[i];
                    array[i] = array[pos_min];
                    array[pos_min] = temp;
                }
            }

            return array;
        }

        public static int[] SortI(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (array[j - 1] > array[j])
                    {
                        int temp = array[j - 1];
                        array[j - 1] = array[j];
                        array[j] = temp;
                    }
                }
            }

            return array;
        }
    }
}
