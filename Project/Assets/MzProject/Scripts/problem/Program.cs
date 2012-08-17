using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixProblem
{
    class Program
    {
//        public static void Main(string[] args)
		public static void Main()
        {
            int[,] matrixData = new int[,]
            {
                { 0, 0, 0, 2, 2 },
                { 1, 1, 7, 2, 2 },
                { 2, 2, 7, 2, 1 },
                { 2, 1, 7, 4, 4 },
                { 2, 7, 7, 4, 4 },
                { 4, 6, 6, 0, 4 },
                { 4, 4, 6, 4, 4 },
                { 4, 4, 6, 4, 4 }
            };

            Matrix matrix = new Matrix(matrixData);

//            Console.WriteLine("Matrix:");
			UnityEngine.Debug.Log("Matrix:");
		    matrix.Print();
		
		    int countElementsOfBiggestArea = matrix.FindCountElementOfBiggestArea();
//            Console.WriteLine("countElementsOfBiggestArea:" + countElementsOfBiggestArea);
			UnityEngine.Debug.Log("countElementsOfBiggestArea:" + countElementsOfBiggestArea);
        }
    }
}
