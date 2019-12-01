using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic.GeneticClasses
{
    class GeneticCommon
    {
        const int Max = 100;        // Максимальное число городов
        const int K = 50;           // Число особей в популяции
        const int Mnogo = 30;       // Длительность эволюции

        City[] G = new City[Max];            // Города
        short[,] pop = new short[K, Max];    // Популяция
        short N;                             // Число городов
        double[] dl = new double[K];         // Длины маршрутов

        // Считывание городов из файла
        private short ReadCities(string FileName)
        {
            N = 0;
            try
            {
                StreamReader sr = new StreamReader(FileName, Encoding.Default);
                if (!string.IsNullOrEmpty(FileName))
                {
                    string line = "";
                    string[] temp = new string[3];
                    while (true)
                    {
                        line = sr.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        temp = line.Split(' ');
                        G[N].Name = temp[0];
                        G[N].X = Convert.ToDouble(temp[1]);
                        G[N].Y = Convert.ToDouble(temp[2]);
                        N++;
                    }
                }
                sr.Close();
            }
            catch
            {
                Console.WriteLine("Не удалось считать данные");
            }
            return N;
        }

        // Инициализация
        private void Inic()
        {
            short s1, jj;
            short[] mas = new short[Max];
            for (int i = 0; i < K; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    mas[j] = (short)j;
                }
                for (int j = 0; j < N; j++)
                {
                    s1 = (short)Rand.IntRand(0, N - j - 1);
                    pop[i, j] = mas[s1];
                    for (jj = (short)(s1 + 1); jj < N; jj++)
                    {
                        mas[jj - 1] = mas[jj];
                    }
                }
            }
        }

        // Селекция
        private void Selec()
        {
            short nm;
            double s, x1, x2, y1, y2, min;
            for (int i = 0; i < K; i++)
            {
                s = 0;
                for (int j = 1; j < N; j++)
                {
                    x1 = G[pop[i, j]].X;
                    x2 = G[pop[i, j + 1]].X;
                    y1 = G[pop[i, j]].Y;
                    y2 = G[pop[i, j + 1]].Y;
                    s += Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
                }
                dl[i] = s;
            }
            // Сортировка
            for (int i = 0; i < K - 1; i++)
            {
                min = dl[i];
                nm = (short)i;
                for (int j = i + 1; j < K; j++)
                {
                    if (dl[j] < min)
                    {
                        min = dl[j];
                        nm = (short)j;
                    }
                }
                if (nm != i)
                {
                    dl[nm] = dl[i];
                    dl[i] = min;
                    short temp;
                    for (int t = 0; t < Max; t++)
                    {
                        temp = pop[nm, t];
                        pop[nm, t] = pop[i, t];
                        pop[i, t] = temp;
                    }
                }
            }
            // Подготовка к скрещиванию
            for (int i = K / 2; i < K; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    pop[i, j] = 0;
                }
            }
        }

        // Наследование от папы
        private void Papa(short p, short r)
        {
            short s1, pos;
            short[] mas = new short[Max];
            for (int j = 0; j < N; j++)
            {
                mas[j] = (short)j;
            }
            for (int j = 0; j < N / 2; j++)
            {
                s1 = (short)Rand.IntRand(0, N - j - 1);
                pos = mas[s1];
                pop[r, pos] = pop[p, pos];
                for (int i = s1 + 1; i < N; i++)
                {
                    mas[i - 1] = mas[i];
                }
            }
        }

        // Наследование от мамы
        private void Mama(short m, short r)
        {
            bool est; // Флаг наличия
            for (int i = 0; i < N; i++)
            {
                est = false;
                for (int j = 0; j < N; j++)
                {
                    if (pop[r, j] == pop[m, i])
                    {
                        est = true;
                    }
                }
                if (!est)
                {
                    int j = -1;
                    do
                    {
                        j++;
                    } while (pop[r, j] != 0);
                    pop[r, j] = pop[m, i];
                }
            }
        }

        // Мутация
        private void Mut(short r)
        {
            short c, p1, p2;
            p1 = (short)Rand.IntRand(0, N - 1);
            do
            {
                p2 = (short)Rand.IntRand(0, N - 1);
            } while (p1 == p2);
            c = pop[r, p1];
            pop[r, p1] = pop[r, p2];
            pop[r, p2] = c;
        }

        // Скрещивание
        private void Scresh()
        {
            short r, m, p;
            for (r = K / 2; r < K; r++)
            {
                p = (short)Rand.IntRand(0, K / 2 - 1);
                do
                {
                    m = (short)Rand.IntRand(0, K / 2 - 1);
                } while (m == p);
                Papa(p, r);
                Mama(m, r);
                Mut(r);
            }
        }

        public void Execute()
        {
            for (int i = 0; i < Max; i++)
            {
                G[i] = new City();
            }
            string fName = "FileExample.txt";
            N = ReadCities(fName);
            Inic();
            for (int i = 0; i < Mnogo; i++)
            {
                Selec();
                Scresh();
            }
            Selec();
            for (int i = 0; i < N - 1; i++)
            {
                Console.Write(pop[0, i] + "-");
            }
            Console.WriteLine(pop[0, N - 1]);
            for (int i = 0; i < N; i++)
            {
                Console.WriteLine(G[pop[0, i]].Name);
            }
            Console.WriteLine(Environment.NewLine + "Расстояние: " + dl[0]);
            Console.ReadKey();
        }
    }
}
