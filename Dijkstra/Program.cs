using System;
using System.Collections.Generic;
using System.IO;

namespace Dijkstra
{
    class Program
    {
        const int INF = 1000000000;
        static List<List<Pair<int, int>>> g = new List<List<Pair<int, int>>>();
        static int n;
        static int s;
        static List<int> d = new List<int>();
        static int[] p;
        static bool[] u;
        static string errorMsg = "\nНекорректный ввод!\nПожалуйста, убедитесь в корректности введённых данных и попробуйте снова." +
                "\nВ первой строке укажите n - количество вершин графа\n" +
                "\nВо второй строке укажите s - начальный маршрутизатор (исток), s не превышает n\n" +
                "\nВ последующих n строках укажите матрицу смежности графа (n x n), представляющей собой сеть," +
                "где номера маршрутизаторов - вершины, связи - рёбра, с указанием стоимости пути между" +
                "каждым маршрутизатором, если таковой путь имеется.\n";



        static void Initialize()
        {
            for (int i = 0; i < n; i++)
            {
                d.Add(INF);
            }
            u = new bool[n];
            p = new int[n];
        }



        static void BuildG()
        {
            Console.Write("Количество маршрутизаторов в сети: ");
            n = Convert.ToInt32(Console.ReadLine());
            while (true)
            {
                Console.Write("Начальный маршрутизатор (исток): ");
                s = Convert.ToInt32(Console.ReadLine()) - 1;
                if (s + 1 > n)
                    Console.WriteLine(errorMsg);
                else
                    break;
            }
            Console.WriteLine();

            Initialize();

            while (true)
            {
                Console.WriteLine("Матрица смежности графа, где за вершины приняты маршрутизаторы," +
                    "\nза рёбра - связи между ними." +
                    "\nВ случае отсутствия связи между маршрутизаторами укажите 0):");
                try
                {
                    for (int i = 0; i < n; i++)
                    {
                        g.Add(new List<Pair<int, int>>());
                        string[] temp_str = Console.ReadLine().Split(' ');
                        for (int j = 0; j < n; j++)
                        {
                            int to = Convert.ToInt32(temp_str[j]);
                            if (to != 0)
                                g[i].Add(new Pair<int, int>(j, to));
                        }
                    }
                    break;
                }
                catch
                {
                    Console.WriteLine(errorMsg);
                }
            }
        }



        static void BuildG(string path)
        {
            string text = ReadFromFile(path);
            string[] lines = text.Split('\n');

            try
            {
                n = Convert.ToInt32(lines[0]);
                s = Convert.ToInt32(lines[1]);

                if (s + 1 > n)
                    throw new Exception("s не может превосходить n");

                Initialize();

                for (int i = 0; i < n; i++)
                {
                    g.Add(new List<Pair<int, int>>());
                    string[] temp_str = lines[2 + i].Split(' ');
                    for (int j = 0; j < n; j++)
                    {
                        int to = Convert.ToInt32(temp_str[j]);
                        if (to != 0)
                            g[i].Add(new Pair<int, int>(j, to));
                    }
                }
            }
            catch
            {
                Console.WriteLine(errorMsg);
            }

        }



        static void Dijkstra()
        {
            d[s] = 0;
            for (int i = 0; i < n; i++)
            {
                int v = -1;
                for (int j = 0; j < n; j++)
                {
                    if (!u[j] && (v == -1 || d[j] < d[v]))
                        v = j;
                }
                if (d[v] == INF)
                    break;
                u[v] = true;

                for (int j = 0; j < g[v].Count; j++)
                {
                    int to = g[v][j].First, len = g[v][j].Second;
                    if (d[v] + len < d[to])
                    {
                        d[to] = d[v] + len;
                        p[to] = v;
                    }
                }
            }
        }



        static void Output()
        {
            bool isEmpty = true;
            for (int i = 0; i < n; i++)
            {
                if (i != s && d[i] != INF)
                {
                    Console.WriteLine("Наименьшая стоимости пути до маршрутизатора {0} равна {1}", i + 1, d[i]);
                    RestorePath(i + 1);
                    isEmpty = false;
                }
            }
            if(isEmpty)
            {
                Console.WriteLine("Связи с другими маршрутизаторами отсутствуют!");
            }
        }



        static string ReadFromFile(string path)
        {
            string text;

            using (StreamReader reader = new StreamReader(path))
            {
                text = reader.ReadToEnd();
            }
            return text;
        }



        static void Choice()
        {
            Console.WriteLine("Желаете ввести граф в консоль или считать из файла?" +
                "\n1 - консоль" +
                "\n2 - считать из файла");
            int choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            if (choice == 1)
                BuildG();
            else BuildG(Environment.CurrentDirectory.ToString() + "\\graph.txt");

        }



        static void RestorePath(int finish)
        {
            List<int> destination = new List<int>();
            for (int v = finish - 1; v != s; v = p[v])
            {
                destination.Add(v + 1);
            }
            destination.Add(s + 1);
            destination.Reverse();

            if (d[finish - 1] != INF)
            {
                Console.Write("Восстановленный путь: ");
                for (int i = 0; i < destination.Count; i++)
                {
                    if (i != destination.Count - 1)
                        Console.Write($"{destination[i]} --> ");
                    else
                        Console.Write($"{destination[i]}");
                }
                Console.WriteLine();
            }
        }



        static void Main(string[] args)
        {
            Choice();
            Dijkstra();
            Output();
            Console.ReadLine();
        }
    }
}
