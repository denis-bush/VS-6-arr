using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArrayHandling
{
    public partial class Form1 : Form
    {
        private string filePath;
        private string sourceArray = "";
        private int[] arrayInt;
        private int select = 1;

        public Form1()
        {
            InitializeComponent();
        }

        #region Protection from idiots

        private void txtCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }


        private void txtMin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8 && e.KeyChar != 45)
                e.Handled = true;
            if (e.KeyChar == 45 && txtMin.Text.Contains('-'))
                e.Handled = true;
        }

        private void txtMin_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtMin.Text != "")
                if (txtMin.Text.Contains('-') && txtMin.Text.IndexOf('-') != 0)
                    txtMin.Text = txtMin.Text.Remove(txtMin.Text.IndexOf('-'));
        }

        private void txtMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8 && e.KeyChar != 45)
                e.Handled = true;
            if (e.KeyChar == 45 && txtMax.Text.Contains('-'))
                e.Handled = true;
        }

        private void txtMax_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtMax.Text != "")
                if (txtMax.Text.Contains('-') && txtMax.Text.IndexOf('-') != 0)
                    txtMax.Text = txtMax.Text.Remove(txtMax.Text.IndexOf('-'));
        }


        #endregion

        // Считывание массива из внешнего файла
        private void btnReadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (openFileDialog1.FileName.Contains(".txt"))
                    {
                        filePath = openFileDialog1.FileName;
                        txtInputFile.Text = filePath;
                        string[] fileStrings = File.ReadAllLines(filePath);
                        if (fileStrings.Length > 0)
                            sourceArray = fileStrings[fileStrings.Length-1];
                        try
                        {
                            string[] arrayStrings = sourceArray.Split(' ');
                        arrayInt = new int[arrayStrings.Length];
                        for (int i = 0; i < arrayStrings.Length; i++)
                        {
                                arrayInt[i] = Convert.ToInt32(arrayStrings[i]);
                        }
                            txtArray.Text = sourceArray;
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Некорректные входные данные");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Вы выбрали файл другого формата. Выберите файл формата txt");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        // Считывание параметров генерации массива
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            int res;
            if (Int32.TryParse(txtCount.Text, out res) 
                && Int32.TryParse(txtMin.Text, out res) 
                &&Int32.TryParse(txtMax.Text, out res))
            { 
                int count = Convert.ToInt32(txtCount.Text);
                int max = Convert.ToInt32(txtMax.Text);
                int min = Convert.ToInt32(txtMin.Text);
                if (max < min)
                {
                    int tmp = min;
                    min = max;
                    max = tmp;
                    txtMax.Text = max.ToString();
                    txtMin.Text = min.ToString();
                }
                Generate(count,min,max);
            }
            else
            {
                MessageBox.Show("Поля заполнены некорректно");
            }
        }

        // Генерация случайного числового массива по заданным параметрам
        private void Generate(int count,int min,int max)
        {
            Random rnd = new Random();
            sourceArray = "";
            int[] genArray = new int[count];
            for (int i = 0; i < count; i++)
            {
                genArray[i] = rnd.Next(min, max+1);
                sourceArray += genArray[i].ToString()+" ";
            }
            arrayInt = genArray;
            txtArray.Text = sourceArray;
        }

        // Выполнить выбранное действие с массивом
        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (txtArray.Text == "")
            {
                MessageBox.Show("Не задан исходный массив");
                return;
            }
            switch (@select)
            {
                case 1:
                    txtResult.Text = Sum(arrayInt);
                    break;
                case 2:
                    txtResult.Text = Average(arrayInt);
                    break;
                case 3:
                    txtResult.Text = MinValue(arrayInt);
                    break;
                case 4:
                    txtResult.Text = MaxValue(arrayInt);
                    break;
                case 5:
                    txtResult.Text = Even(arrayInt);
                    break;
                case 6:
                    txtResult.Text = Odd(arrayInt);
                    break;
                case 7:
                    txtResult.Text = Asc(arrayInt);
                    break;
                default:
                    txtResult.Text = Desc(arrayInt);
                    break;

            }
        }


        #region Select Operation
        // Переключение выполняемой операции
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            select = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            select = 2;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            select = 3;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            select = 4;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            select = 5;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            select = 6;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            select = 7;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            select = 8;
        }
        #endregion

        #region Operations

        // Сумма элементов массива
        private string Sum(int[] array)
        {
            int sum = 0;
            foreach (var n in array)
            {
                sum += n;
            }

            return "Сумма элементов:" + sum;
        }

        // Среднее значение всех элементов массива
        private string Average(int[] array)
        {
            double avr = 0;
            foreach (var n in array)
            {
                avr += n;
            }
            avr /= array.Length;
            return "Среднее значение:" + avr;
        }

        // Элемент с минимальным значением в массиве
        private string MinValue(int[] array)
        {
            int min = array[0];
            foreach (var n in array)
            {
                if (min > n)
                    min = n;
            }
            return "Минимальный элемент:" + min;
        }

        // Элемент с максимальным значением в массиве
        private string MaxValue(int[] array)
        {
            int max = array[0];
            foreach (var n in array)
            {
                if (max < n)
                    max = n;
            }
            return "Максимальный элемент:" + max;
        }

        // Чётные числа в массиве
        private string Even(int[] array)
        {
            string result = "";
            foreach (var n in array)
            {
                if (n % 2 == 0)
                    result += n + " ";
            }

            return "Четные элементы:" + result;
        }

        // Нечётные числа в массиве
        private string Odd(int[] array)
        {
            string result = "";
            foreach (var n in array)
            {
                if (n % 2 != 0)
                    result += n + " ";
            }

            return "Нечетные элементы:" + result;
        }

        // Элементы массива в строке, отсортированные по возрастанию
        private string Asc(int[] array)
        {
            string result = "";
            Array.Sort(array);
            foreach (var n in array)
            {
                result += n + " ";
            }
            return "Сортировка по возрастанию: " + result;
        }

        // Элементы массива в строке, отсортированные по убыванию
        private string Desc(int[] array)
        {
            string result = "";
            Array.Sort(array);
            Array.Reverse(array);
            foreach (var n in array)
            {
                result += n + " ";
            }
            return "Сортировка по убыванию: " + result;
        }



        #endregion
        // Записать результат обработки массива в файл
        private void btnWriteFile_Click(object sender, EventArgs e)
        {
            string savePath = "";
            if (txtResult.Text == "")
            {
                MessageBox.Show("У вас есть незаполненные поля");
                return;
            }
            OpenFileDialog openFileDialog2 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                savePath = openFileDialog2.FileName;
                txtOutFile.Text = savePath;
                using (StreamWriter sw = new StreamWriter(savePath, true))
                {
                    sw.WriteLine(txtResult.Text);
                    sw.Close();
                }
            }

            openFileDialog2.Reset();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
