﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

public struct Point3D
{
    public double pos_x;
    public double pos_y;
    public double pos_z;
    public void setPosition(double x, double y, double z)
    {
        pos_x = x;
        pos_y = y;
        pos_z = z;
    }
    public void printPoint()
    {
        Console.WriteLine("({0}, {1}, {2})", pos_x, pos_y, pos_z);
    }
}

namespace PrinterTiltCompensation
{
    class PrinterTiltCompensation
    {

        static Matrix<double> TransfomationMatrix = Matrix<double>.Build.Dense(3, 3);

        public static Vector<double> CrossProduct(Vector<double> left, Vector<double> right)
        {
            if ((left.Count != 3 || right.Count != 3))
            {
                string message = "Vectors must have a length of 3.";
                throw new Exception(message);
            }
            Vector<double> result = Vector<double>.Build.Dense(3);
            result[0] = left[1] * right[2] - left[2] * right[1];
            result[1] = -left[0] * right[2] + left[2] * right[0];
            result[2] = left[0] * right[1] - left[1] * right[0];

            return result;
        }

        public static Matrix<double> CrossProductMatrix(Vector<double> vector)
        {
            if (vector.Count != 3)
            {
                string message = "Vector must have a length of 3.";
                throw new Exception(message);
            }
            Matrix<double> result = Matrix<double>.Build.Dense(3, 3);

            result[1, 0] = vector[2];
            result[0, 1] = -vector[2];
            result[2, 0] = -vector[1];
            result[0, 2] = vector[1];
            result[1, 2] = -vector[0];
            result[2, 1] = vector[0];

            return result;
        }

        public static Matrix<double> CreateTransformationMatrix(Matrix<double> rotation, Matrix<double> translation)
        {
            var M = Matrix<double>.Build;
            double[,] auxMatrixArray = { { 0, 0, 0, 1 }, };
            var auxMatrix = M.DenseOfArray(auxMatrixArray);

            Matrix<double>[,] AuxMatrix = { { rotation, translation.Transpose() } };
            Matrix<double> ConcatinatedMatrix = M.DenseOfMatrixArray(AuxMatrix);

            Matrix<double>[,] TransfomationMatrixArray = { { ConcatinatedMatrix }, { auxMatrix } };
            Matrix<double> TransfomationMatrix = M.DenseOfMatrixArray(TransfomationMatrixArray);

            return TransfomationMatrix;
        }


        public static Matrix<double> CalculateTranslationMatrix(Vector<double> normal, Point3D point)
        {
            var M = Matrix<double>.Build;
            double[,] translationVectorArray = { { 0, 0, 0 } };
            var translationMatrix = M.DenseOfArray(translationVectorArray);
            var z = normal[0] * point.pos_x + normal[1] * point.pos_y + normal[2] * point.pos_z;
            translationMatrix[0, 2] = z;
            return translationMatrix;
        }

        public static Matrix<double> CalculateRotationMatrix(Vector<double> rotationAxis, double angle)
        {
            var M = Matrix<double>.Build;
            var RotationMatrix = Math.Cos(angle) * M.DenseIdentity(3) + Math.Sin(angle) * CrossProductMatrix(rotationAxis) + (1 - Math.Cos(angle)) * rotationAxis.OuterProduct(rotationAxis);
            return RotationMatrix;

        }

        public static void PrepareTiltCompensation(Point3D point_1, Point3D point_2, Point3D point_3)
        {
            Console.WriteLine("Calculating tilt compensation");

            var M = Matrix<double>.Build;
            var V = Vector<double>.Build;

            double[] idealNormalArray = { 0, 0, 1 };
            var idealNormal = V.DenseOfArray(idealNormalArray);

            double[] coor_array_1 = { point_1.pos_x, point_1.pos_y, point_1.pos_z };
            double[] coor_array_2 = { point_2.pos_x, point_2.pos_y, point_2.pos_z };
            double[] coor_array_3 = { point_3.pos_x, point_3.pos_y, point_3.pos_z };

            var coor_1 = V.DenseOfArray(coor_array_1);
            var coor_2 = V.DenseOfArray(coor_array_2);
            var coor_3 = V.DenseOfArray(coor_array_3);

            var normal = CrossProduct(coor_1 - coor_2, coor_3 - coor_2);
            normal = normal / normal.L2Norm();

            var rotationAxis = CrossProduct(normal, idealNormal);
            rotationAxis = rotationAxis / rotationAxis.L2Norm();

            var angle = Math.Acos(normal.DotProduct(idealNormal));

            var RotationMatrix = CalculateRotationMatrix(rotationAxis, angle);
            var TranslationMatrix = CalculateTranslationMatrix(normal, point_1);
            PrinterTiltCompensation.TransfomationMatrix = CreateTransformationMatrix(RotationMatrix, TranslationMatrix);

        }

        public static Point3D ApplyTiltCompensation(Point3D point)
        {
            double[] coorArray = { point.pos_x, point.pos_y, point.pos_z, 1 };
            var coor = Vector<double>.Build.DenseOfArray(coorArray);
            var newVector = PrinterTiltCompensation.TransfomationMatrix * coor;

            Console.WriteLine(PrinterTiltCompensation.TransfomationMatrix);
            Console.WriteLine(coor);

            Point3D newPoint = new Point3D();
            newPoint.setPosition(newVector[0], newVector[1], newVector[2]);
            return newPoint;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Performing printer tilt compensation algorithms");
            Point3D pos_1 = new Point3D();
            pos_1.setPosition(0, -1, 0.5);

            Point3D pos_2 = new Point3D();
            pos_2.setPosition(-1, 0, 0.5);

            Point3D pos_3 = new Point3D();
            pos_3.setPosition(1, 0, 0.51);

            Point3D test_point = new Point3D();
            test_point.setPosition(0, 0, 1);

            PrepareTiltCompensation(pos_1, pos_2, pos_3);
            ApplyTiltCompensation(test_point);
        }
    }
}