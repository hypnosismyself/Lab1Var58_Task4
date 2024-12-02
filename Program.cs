using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab1Var58_Task4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //  задаем длину массивов
            int len_a = 5;
            int len_b = 7;

            //  выделяем память под массивы
            double[] a = new double[len_a];
            double[] b = new double[len_b];

            //  заполняем вызовом рандомными числами
            FillRand(a);
            FillRand(b);

            //  вывод массива а и b в консоль и в бинарник
            string ouput_binary_name = "mass_binary";
            Console.WriteLine("Массив А:");
            ReturnToConsole(a);
            Console.WriteLine("Массив B:");
            ReturnToConsole(b);
            OuputArraysToBinaryFile(a, b, ouput_binary_name);

            //  вывод массивов из бинарников
            Console.WriteLine("Вычитка из бинарника:");
            GetArraysFromBinaryFile(a, b, ouput_binary_name);

            //  запись массивов в текстовый файл
            string ouput_text_massive_name = "mass_text";
            OutputArraysToTextFile(a, b, ouput_text_massive_name);

            //  вывод кол-ва меньше С в массиве а
            Console.WriteLine(CountElemsLessArg(a));

            //  сумма целых частей элементов массива, расположенных после последнего отрицательного элемента
            Console.WriteLine(SumDecimalAfterLastNegative(a));

            //  перезаписываем массивы согласно условию:
            /*  
             *  преобразовать массив таким образом, чтобы сначала располагались все элементы,
             *  отличающиеся от максимального не более чем на 20%, а потом - все остальные.  */
            a = ProcessArray(a);
            b = ProcessArray(b);
            
            //  выводим преобразованные массивы
            Console.WriteLine("Массив А:");
            ReturnToConsole(a);
            Console.WriteLine("Массив B:");
            ReturnToConsole(b);

            //  вывод результатов в текстовый файл
            string result_file_name = "results";
            WriteResults(result_file_name, a, b);
        }

        static void WriteResults(string file_name, double[] arrayA, double[] arrayB)
        {
            //  запись результатов в файл

            //  param:
            //  double[] arrayA - массив а
            //  double[] arrayB - массив b
            //  string file_name - имя файла-источника

            string file_path = $"..\\..\\{file_name}.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(file_path, false))
                {
                    //  вывод кол-ва элементов меньше указанного для массива а
                    sw.WriteLine($"Для массива а:\n{CountElemsLessArg(arrayA)}\n");

                    //  вывод суммы целых частей с конца до первого отрицательного элемента для массива b
                    sw.WriteLine($"Для массива b:{SumDecimalAfterLastNegative(arrayB)}\n");

                    //  запись преобразованного масива а
                    sw.WriteLine($"Преобразованный массив a: ");
                    foreach (double value in arrayA)
                        sw.Write($"{value}\t");

                    sw.WriteLine();

                    //  запись преобразованного масива b
                    sw.WriteLine($"\nПреобразованный массив b: ");
                    foreach (double value in arrayB)
                        sw.Write($"{value}\t");
                }

                //  открываем файл
                Process.Start(file_path);
            }
            //  ловим выкидыши
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static double[] ProcessArray(double[] array)
        {
            //  преобразует массив по условию

            //  param:
            //  double[] array - массив, для которого ищем кол-во
            //  return:
            //  double[] res - новый массив

            //  максимальный элемент
            double max = array.Max();

            //  временные списки для фильтрации элементов
            List<double> list_head = new List<double>();
            List<double> list_tail = new List<double>();

            //  добавляем максимальный элемент в начало списка
            list_head.Add(max);

            //  получаем индекс максимального элемента, чтобы скипнуть иетрацию
            //  с максимальным числом
            int index_max = Array.IndexOf(array, array.Max());

            for (int i = 0; i < array.Length; i++)
            {
                //  находим процент отличия для текущего элемента
                double dif = (array[i] / max) * 100;

                //  скип максимального элемента
                if (i == index_max)
                    continue;

                //  элементы, где отличие < 20% добавляем в первый список
                //  иначе во второй
                if (dif >= 80)
                    list_head.Add(array[i]);
                else
                    list_tail.Add(array[i]);
            }

            //  объединяем первый и второй списки, преобразуя в массив
            double[] res = list_head.Union(list_tail).ToArray();

            return res;
        }

        static string SumDecimalAfterLastNegative(double[] array)
        {
            //  сумма целых частей элементов массива, расположенных после последнего отрицательного элемента

            //  param:
            //  double[] array - массив, для которого ищем кол-во
            //  return:
            //  string - ответ

            double res = 0;

            //  программная длина массива
            int i = array.Length - 1;

            //  пока элемент массива не отрицательный
            //  добавляем элемент к общей сумме
            //  массив перебираем с конца
            while (array[i] > 0)
            {
                //  округляем в меньшую сторону
                res += Math.Floor(array[i]);
                i--;
            }

            return $"\nСумма целых частей элементов массива, расположенных после последнего отрицательного элемента - {res}";
        }

        static string CountElemsLessArg(double[] array)
        {
            //  считает числа, меньше чем arg

            //  param:
            //  double[] array - массив, для которого ищем кол-во
            //  return:
            //  string - ответ

            int res = 0;
            double arg = 4;

            //  сравниваем C со всеми элементами массива
            foreach (double value in array)
            {
                if (value < arg)
                    res++;
            }

            return $"Количество элементов меньших чем {arg} - {res}";
        }

        static void FillRand(double[] array)
        {
            //  заполняет массив рандомными числами

            //  param:
            //  double[] array - заполняемый массив

            //  конфигуратор рандома, так как обычный рандом заполняет
            //  числами в диапазоне [0; 1)
            double min = -10;
            double max = 10;

            Random rand = new Random();

            for (int i = 0; i < array.Length; i++)
            {
                //  рандомно генерируем вещественные числа
                double rand_double = min + rand.NextDouble() * (max - min);
                
                //  окургляем числа до 2 знаков и добавляем в массив
                array[i] = Math.Round(rand_double, 2, MidpointRounding.ToEven);
            }

            //  задержкой процесса сбрасываем сид
            Thread.Sleep(100);
        }

        static void ReturnToConsole(double[] array)
        {
            //  выводит массив в консоль

            //  param:
            //  double[] array - входной массив

            for (int i = 0; i < array.Length; i++)
                Console.Write($"{array[i]}\t");
            Console.WriteLine("\n");
        }

        static void OuputArraysToBinaryFile(double[] arrayA, double[] arrayB, string file_name)
        {
            //  записывает массивы a и b в бинарный файл

            //  param:
            //  double[] arrayA - массив а
            //  double[] arrayB - массив b
            //  string file_name - имя файла-источника

            //  путь до файла
            string file_path = $"..\\..\\{file_name}.dat";

            try
            {
                //  перезаписываем бинарник
                using (BinaryWriter writer = new BinaryWriter(File.Open(file_path, FileMode.OpenOrCreate)))
                {
                    //  запись масива а
                    foreach (double value in arrayA)
                        writer.Write(value);

                    //  запись массива b
                    foreach (double value in arrayB)
                        writer.Write(value);

                    //  открываем файл
                    Process.Start(file_path);
                }
            }
            //  ловим выкидыши
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void OutputArraysToTextFile(double[] arrayA, double[] arrayB, string file_name)
        {
            //  запись результатов в текстовый файл

            //  param:
            //  double[] arrayA - массив а
            //  double[] arrayB - массив b
            //  string file_name - имя файла-выхода

            string file_path = $"..\\..\\{file_name}.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(file_path, false))
                {
                    //  запись масива а
                    sw.WriteLine($"Массив А: ");
                    foreach (double value in arrayA)
                        sw.Write($"{value}\t");

                    //  запись масива b
                    sw.WriteLine($"\nМассив B: ");
                    foreach (double value in arrayB)
                        sw.Write($"{value}\t");
                }

                //  открываем файл
                Process.Start(file_path);
            }
            //  ловим выкидыши
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void GetArraysFromBinaryFile(double[] arrayA, double[] arrayB, string file_name)
        {
            //  выводит массивы из бинарного файла

            //  param:
            //  double[] arrayA - массив а
            //  double[] arrayB - массив b
            //  string file_name - имя файла-выхода

            //  путь до файла
            string file_path = $"..\\..\\{file_name}.dat";

            //  общая длина двух массивов для сплита после вычитки
            int sum_array_len = arrayA.Length + arrayB.Length;

            //  массив всех чисел
            double[] res = new double[sum_array_len];

            try
            {
                using (BinaryReader br = new BinaryReader(File.Open(file_path, FileMode.OpenOrCreate)))
                {
                    //  вычитываем все из бинарника
                    for (int i = 0; i < sum_array_len; i++)
                        res[i] = br.ReadDouble();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //  вывод массива a
            Console.WriteLine("Массив А: ");
            for (int i = 0; i < arrayA.Length; i++)
                Console.Write($"{res[i]}\t");

            //  вывод массива B
            Console.WriteLine("\nМассив B: ");
            for (int i = arrayA.Length; i < sum_array_len; i++)
                Console.Write($"{res[i]}\t");
        }
    }
}
