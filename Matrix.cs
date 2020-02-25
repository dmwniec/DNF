using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Data;

namespace DNF
{
    class Matrix
    {
       

        public int[,] Value { get; }
        public Matrix(int n, int m) { Value = new int[n, m]; }
        public Matrix(int[,] a) { Value = a; }

       
      
        public static Matrix LoadFromFile(int n,int m, string path) //Wykorzystuję tu bibliotekę LINQ, parametr to wielkość macierzy
        {
            Matrix matrix = new Matrix(n, m);
            try
            {

                var lines = File.ReadAllLines(path); //Zczytywanie całego pliku
                for (int i = 0; i < n; ++i)
                {
                    var data = lines[i].Split(' ').Select(c => Convert.ToInt32(c)).ToList();
                    for (int j = 0; j < m; ++j) matrix.Value[i, j] = data[j];

                }
            }
            catch
            {
                matrix = null;
                
            }
  

                return matrix;
            
            
        }
    }
}
