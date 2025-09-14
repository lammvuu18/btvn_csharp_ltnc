using System;

namespace MatrixApp
{
    class Matrix
    {
        public int Rows { get; }
        public int Cols { get; }
        public int[,] Data { get; }

        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Data = new int[rows, cols];
        }

        // Nhập ma trận
        public void Input()
        {
            Console.WriteLine($"Nhập ma trận {Rows}x{Cols}:");
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Console.Write($"Phần tử [{i},{j}]: ");
                    Data[i, j] = int.Parse(Console.ReadLine());
                }
            }
        }

        // Hiển thị ma trận
        public void Display()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Console.Write(Data[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        // Cộng hai ma trận
        public Matrix Add(Matrix other)
        {
            if (Rows != other.Rows || Cols != other.Cols)
                throw new Exception("Hai ma trận không cùng kích thước!");

            Matrix result = new Matrix(Rows, Cols);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    result.Data[i, j] = Data[i, j] + other.Data[i, j];

            return result;
        }

        // Nhân hai ma trận
        public Matrix Multiply(Matrix other)
        {
            if (Cols != other.Rows)
                throw new Exception("Không thể nhân: số cột A phải bằng số dòng B");

            Matrix result = new Matrix(Rows, other.Cols);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < other.Cols; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < Cols; k++)
                        sum += Data[i, k] * other.Data[k, j];
                    result.Data[i, j] = sum;
                }
            return result;
        }

        // Chuyển vị ma trận
        public Matrix Transpose()
        {
            Matrix result = new Matrix(Cols, Rows);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    result.Data[j, i] = Data[i, j];
            return result;
        }

        // Tìm giá trị lớn nhất
        public int GetMax()
        {
            int max = Data[0, 0];
            foreach (int val in Data) if (val > max) max = val;
            return max;
        }

        // Tìm giá trị nhỏ nhất
        public int GetMin()
        {
            int min = Data[0, 0];
            foreach (int val in Data) if (val < min) min = val;
            return min;
        }

        // Định thức (dùng đệ quy, chỉ áp dụng cho ma trận vuông)
        public int Determinant()
        {
            if (Rows != Cols)
                throw new Exception("Không thể tính định thức: ma trận không vuông");

            return CalculateDeterminant(Data, Rows);
        }

        private int CalculateDeterminant(int[,] matrix, int n)
        {
            if (n == 1) return matrix[0, 0];
            if (n == 2) return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

            int det = 0;
            for (int col = 0; col < n; col++)
            {
                int[,] minor = new int[n - 1, n - 1];
                for (int i = 1; i < n; i++)
                {
                    int minorCol = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (j == col) continue;
                        minor[i - 1, minorCol] = matrix[i, j];
                        minorCol++;
                    }
                }
                det += (col % 2 == 0 ? 1 : -1) * matrix[0, col] * CalculateDeterminant(minor, n - 1);
            }
            return det;
        }

        // Kiểm tra đối xứng
        public bool IsSymmetric()
        {
            if (Rows != Cols) return false;
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    if (Data[i, j] != Data[j, i]) return false;
            return true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Nhập ma trận A
            Console.Write("Nhập số dòng của A: ");
            int rowsA = int.Parse(Console.ReadLine());
            Console.Write("Nhập số cột của A: ");
            int colsA = int.Parse(Console.ReadLine());

            Matrix A = new Matrix(rowsA, colsA);
            A.Input();

            Console.WriteLine("Ma trận A:");
            A.Display();

            // Nhập ma trận B cùng kích thước
            Console.WriteLine("\nNhập ma trận B:");
            Matrix B = new Matrix(rowsA, colsA);
            B.Input();

            Console.WriteLine("Ma trận B:");
            B.Display();

            // Cộng A + B
            Console.WriteLine("\nA + B:");
            Matrix C = A.Add(B);
            C.Display();

            // Nhân A × B (nếu có thể)
            try
            {
                Console.WriteLine("\nA × B:");
                Matrix D = A.Multiply(B);
                D.Display();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Chuyển vị
            Console.WriteLine("\nChuyển vị của A:");
            Matrix At = A.Transpose();
            At.Display();

            // Min/Max
            Console.WriteLine($"\nMax của A: {A.GetMax()}");
            Console.WriteLine($"Min của A: {A.GetMin()}");

            // Định thức (nếu vuông)
            if (A.Rows == A.Cols)
            {
                Console.WriteLine($"\nĐịnh thức A = {A.Determinant()}");
                Console.WriteLine($"A {(A.IsSymmetric() ? "là" : "không là")} ma trận đối xứng");
            }
        }
    }
}
