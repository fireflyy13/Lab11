﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Windows.Forms;

namespace Lab11
{
    public partial class Form1 : Form
    {
        private int[] initialArray; 
        private int[] arrayToSort;   

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public static int[] Chaotic(int length)
        {
            Random random = new Random();
            int[] a = new int[length];
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = random.Next(1, 100000);
            }
            return a;
        }

        public static int[] PartiallySorted(int length)
        {
            Random random = new Random();
            int[] a = new int[length];
            a[0] = random.Next(1, 300);
            for (int i = 1; i < a.Length; i++)
            {
                a[i] = a[i - 1] + random.Next(1, 100000);
            }

            int[] b = Chaotic(length / 2);
            for (int i = 0; i < (length / 2); i++)
            {
                a[i] = b[i];
            }
            return a;
        }

        public static int[] Duplicates(int length)
        {
            Random random = new Random();
            int[] a = new int[length];
            for (int i = 1; i < a.Length; i++)
            {
                a[i] = random.Next(1, 100000);
                if (a[i] % 2 == 0)
                {
                    a[i] = a[i - 1];
                }
            }
            return a;
        }

        public static void Swap(int[] a, int i, int j)
        {
            int temp = a[i];
            a[i] = a[j];
            a[j] = temp;
        }

        public static void SortByMerge(int[] a, int left, int right)
        {
            if (left < right) // Перевіряємо, чи кількість елементів у масиві не є рівною 0 або 1,
                              // в протилежному випадку об'єднуємо підмасиви, сортуючи
            {
                int middle = left + (right - left) / 2;
                SortByMerge(a, left, middle);
                SortByMerge(a, middle + 1, right);
                Merge(a, left, middle, right);
            }
        }

        public static void Merge(int[] a, int left, int middle, int right)
        {
            int length1 = middle - left + 1; // Довжина лівого підмасиву
            int length2 = right - middle; // Довжина правого підмасиву
            int[] left_a = new int[length1]; // Лівий підмасив
            int[] right_a = new int[length2]; // Правий підмасив

            int i, j;

            for (i = 0; i < length1; i++)
            {
                left_a[i] = a[left + i]; // Копіюємо знаечння в тимч. лівий підмасив
            }

            for (i = 0; i < length2; i++)
            {
                right_a[i] = a[middle + 1 + i]; // Копіюємо значення в тимч. правий підмасив
            }

            int k = left;
            i = 0;
            j = 0;
            while (i < length1 && j < length2) // Працює доти, доки в обох підмасивах залишаються елементи
            {
                if (left_a[i] <= right_a[j])
                {
                    a[k] = left_a[i];  // Зливаємо відсортовані підмасиви в один масив
                    i++;
                }
                else
                {
                    a[k] = right_a[j];
                    j++;
                }
                k++;
            }

            while (i < length1) // Оброблюємо залишкові елементи
            {
                a[k] = left_a[i];
                i++;
                k++;
            }
            while (j < length2)
            {
                a[k] = right_a[j];
                j++;
                k++;
            }
        }


        public static int Partition(int[] a, int low, int high)
        {
            int pivot = a[high];
            int i = low - 1;
            for (int j = low; j <= high - 1; j++)
            {
                if (a[j] < pivot)
                {
                    i++;
                    Swap(a, i, j);
                }
            }
            Swap(a, i + 1, high);
            return i + 1;
        }

        public void QuickSort(int[] a, int low, int high)
        {
            if (low < high)
            {
                int pivot = Partition(a, low, high);
                QuickSort(a, low, pivot - 1);
                QuickSort(a, pivot + 1, high);
            }
        }

        public void SortWithHeap(int[] a)
        {
            for (int i = a.Length / 2 - 1; i >= 0; i--)
            {
                Heapify(a, a.Length, i);
            }

            for (int i = a.Length - 1; i >= 0; i--)
            {
                int temp = a[0];
                a[0] = a[i];
                a[i] = temp;
                Heapify(a, i, 0);
            }
        }

        public void Heapify(int[] a, int length, int i)
        {
            int greatest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < length && a[left] > a[greatest])
            {
                greatest = left;
            }

            if (right < length && a[right] > a[greatest])
            {
                greatest = right;
            }

            if (greatest != i)
            {
                int temp = a[i];
                a[i] = a[greatest];
                a[greatest] = temp;
                Heapify(a, length, greatest);
            }
        }
        public void Chaotic_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            int length;
            if (!int.TryParse(textBox2.Text.Trim(), out length))
            {
                MessageBox.Show("Будь ласка, введіть коректну довжину масиву!", "Помилка", MessageBoxButtons.OK);
                return;
            }

            initialArray = Chaotic(length);
            arrayToSort = (int[])initialArray.Clone(); 
            PrintArray(textBox1, initialArray, "Початковий масив:");
        }

        public void PartiallySorted_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            int length;
            if (!int.TryParse(textBox2.Text.Trim(), out length))
            {
                MessageBox.Show("Будь ласка, введіть коректну довжину масиву!", "Помилка", MessageBoxButtons.OK);
                return;
            }

            initialArray = PartiallySorted(length);
            arrayToSort = (int[])initialArray.Clone(); 
            PrintArray(textBox1, initialArray, "Початковий масив:");
        }

        public void Duplicates_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            int length;
            if (!int.TryParse(textBox2.Text.Trim(), out length))
            {
                MessageBox.Show("Будь ласка, введіть коректну довжину масиву!", "Помилка", MessageBoxButtons.OK);
                return;
            }

            initialArray = Duplicates(length);
            arrayToSort = (int[])initialArray.Clone(); 
            PrintArray(textBox1, initialArray, "Початковий масив:");
        }

        private void Quick_Click(object sender, EventArgs e)
        {
            if (initialArray == null)
            {
                MessageBox.Show("Спочатку згенеруйте масив!", "Помилка", MessageBoxButtons.OK);
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            TimeSpan timespan;

            arrayToSort = (int[])initialArray.Clone(); 
            textBox1.Clear();
            PrintArray(textBox1, initialArray, "Початковий масив:"); 
            textBox1.AppendText(Environment.NewLine);

            stopwatch.Start();
            QuickSort(arrayToSort, 0, arrayToSort.Length - 1); 
            stopwatch.Stop();

            PrintArray(textBox1, arrayToSort, "Відсортований масив (Quick Sort):");
            timespan = stopwatch.Elapsed;
            label5.Text = $"Час виконання Quick Sort в мс: " + (double)timespan.Ticks / 10000;
        }

        public void Merge_Click(object sender, EventArgs e)
        {
            if (initialArray == null)
            {
                MessageBox.Show("Спочатку згенеруйте масив!", "Помилка", MessageBoxButtons.OK);
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            TimeSpan timespan;

            arrayToSort = (int[])initialArray.Clone(); 
            textBox1.Clear(); 
            PrintArray(textBox1, initialArray, "Початковий масив:"); 
            textBox1.AppendText(Environment.NewLine); 

            stopwatch.Start();
            SortByMerge(arrayToSort, 0, arrayToSort.Length - 1); 
            stopwatch.Stop();

            PrintArray(textBox1, arrayToSort, "Відсортований масив (MergeSort):");
            timespan = stopwatch.Elapsed;
            label3.Text = $"Час виконання Merge Sort в мс: " + (double)timespan.Ticks / 10000;
        }

        public void Heap_Click(object sender, EventArgs e)
        {
            if (initialArray == null)
            {
                MessageBox.Show("Будь ласка, згенеруйте масив перед сортуванням!", "Помилка", MessageBoxButtons.OK);
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            TimeSpan timespan;

            arrayToSort = (int[])initialArray.Clone();
            textBox1.Clear();
            PrintArray(textBox1, initialArray, "Початковий масив:");
            textBox1.AppendText(Environment.NewLine);

            stopwatch.Start();
            SortWithHeap(arrayToSort);
            stopwatch.Stop();

            PrintArray(textBox1, arrayToSort, "Відсортований масив (HeapSort):");
            timespan = stopwatch.Elapsed;
            label2.Text = $"Час виконання Heap Sort в мс: " + (double)timespan.Ticks / 10000;
        }

        static void PrintArray(System.Windows.Forms.TextBox textBox1, int[] array, string label)
        {
            textBox1.AppendText(label + " " + string.Join(" ", array) + Environment.NewLine);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

