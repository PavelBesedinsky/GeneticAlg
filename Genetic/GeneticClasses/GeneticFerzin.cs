using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic.GeneticClasses
{
    class GeneticFerzin
    {
        int Max = 100;        // Максимальное число городов
        int K = 50;           // Число особей в популяции
        int Mnogo = 30;       // Длительность эволюции

        int[,] pop = null;
        int N;                             // Размер поля
        double[] dl = null;         // Длины маршрутов

        /// <summary>
        /// Конструктор GeneticFerzin
        /// </summary>
        /// <param name="fieldSize">Размер поля</param>
        /// <param name="countSpecimen">Размер популяции</param>
        public GeneticFerzin(int fieldSize, int countSpecimen)
        {
            N = fieldSize;
            Max = fieldSize;
            K = countSpecimen;
            pop = new int[K, fieldSize];    // Популяция
            dl = new double[K];
        }
        /// <summary>
        /// Определить количество ошибок
        /// </summary>
        /// <param name="field">Решение особи</param>
        /// <param name="specimen">Номер особи</param>
        /// <returns></returns>
        private int getError(int[,] field, int specimen)
        {
            int errors = 0;
            for (int j = 0; j < N; j++)
            {
                int k = -j;
                int ferz = field[specimen,j]; // положение ферзя на доске
                for (int i = 0; i < N; i++)
                {
                    if (i != j)
                    {
                        if (ferz == field[specimen, i]) // есть ли в строке еще ферзи
                        {
                            errors++;
                        }
                        if (ferz - k == field[specimen, i] || ferz + k == field[specimen, i])
                        {
                            errors++;
                        }

                    }
                    k++;
                }
            }
            return errors;
        }

        // Инициализация
        private void Inic()
        {
            int s1, jj;
            int[] mas = new int[Max];
            for (int i = 0; i < K; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    mas[j] = j;
                }
                for (int j = 0; j < N; j++)
                {
                    s1 = Rand.IntRand(0, N - j - 1);
                    pop[i, j] = mas[s1];
                    for (jj = (s1 + 1); jj < N; jj++)
                    {
                        mas[jj - 1] = mas[jj];
                    }
                }
            }
        }

        // Селекция
        private void Selec()
        {
            int nm;
            double s, x1, x2, y1, y2, min;
            for (int i = 0; i < K; i++)
            {              
                dl[i] = getError(pop, i);
            }
            // Сортировка
            for (int i = 0; i < K - 1; i++)
            {
                min = dl[i];
                nm = i;
                for (int j = i + 1; j < K; j++)
                {
                    if (dl[j] < min)
                    {
                        min = dl[j];
                        nm = j;
                    }
                }
                if (nm != i)
                {
                    dl[nm] = dl[i];
                    dl[i] = min;
                    int temp;
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
        private void Papa(int p, int r)
        {
            int s1, pos;
            int[] mas = new int[Max];
            for (int j = 0; j < N; j++)
            {
                mas[j] = j;
            }
            for (int j = 0; j < N / 2; j++)
            {
                s1 = Rand.IntRand(0, N - j - 1);
                pos = mas[s1];
                pop[r, pos] = pop[p, pos];
                for (int i = s1 + 1; i < N; i++)
                {
                    mas[i - 1] = mas[i];
                }
            }
        }

        // Наследование от мамы
        private void Mama(int m, int r)
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
        private void Mut(int r)
        {
            int c, p1, p2;
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
            int r, m, p;
            for (r = K / 2; r < K; r++)
            {
                p = Rand.IntRand(0, K / 2 - 1);
                do
                {
                    m = Rand.IntRand(0, K / 2 - 1);
                } while (m == p);
                Papa(p, r);
                Mama(m, r);
                Mut(r);
            }
        }

        public void Execute()
        {
            Inic();
            for (int i = 0; i < Mnogo; i++)
            {
                Selec();
                Scresh();
            }
            Selec();
            for (int i = 0; i < N; i++)
            {
                Console.Write(pop[0, i]);
                if (i + 1 != N) Console.Write(" - "); 
            }

            Console.WriteLine("\nSolution:\n");
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++)
                {
                    if (pop[0,x] == y)
                    {
                        Console.Write("o");
                    }
                    else
                    {
                        Console.Write("-");
                    }
                }
                Console.Write("\n");
            }
            Console.Write("Errors: " + getError(pop, 0) + "\n");
            Console.ReadKey();
        }
    }
}
