using Xunit;
using MathNet.Numerics.LinearAlgebra;

using System.Diagnostics;
using System;

namespace PrinterTiltCompensation.Tests;


public class UnitTests
{
    [Theory]
    [InlineData(new double[] { 1, 0, 0 }, new double[] { 1, 1, 0 }, new double[] { 0, 0, 1 })]
    [InlineData(new double[] { 1, 0, 3 }, new double[] { 1, 2, 0 }, new double[] { -6, 3, 2 })]
    [InlineData(new double[] { 1, 0, 3 }, new double[] { 1, 0, 1 }, new double[] { 0, 2, 0 })]
    public void TestCrossProduct(double[] first_vector, double[] second_vector, double[] expected)
    {
        var V = Vector<double>.Build;
        var result = PrinterTiltCompensationClass.CrossProduct(V.Dense(first_vector), V.Dense(second_vector));
        var equal = result.Equals(V.Dense(expected));
        Debug.Assert(equal, "CrossProduct function received unexpected output");
    }

    [Theory]
    [InlineData(
        new double[] { 1, 1, 2 },
        new double[] { 0, -2, 1 },
        new double[] { 2, 0, -1 },
        new double[] { -1, 1, 0 }
        )
    ]
    public void TestCrossProductMatrix(double[] vector, double[] row1, double[] row2, double[] row3)
    {
        var V = Vector<double>.Build;
        var M = Matrix<double>.Build;
        var result = PrinterTiltCompensationClass.CrossProductMatrix(V.Dense(vector));
        var equal = result.Equals(M.DenseOfRowArrays(row1, row2, row3));
        Debug.Assert(equal, "CrossProductMatrix function received unexpected output");
    }

    [Theory]
    [InlineData(
        new double[] { 1, 0, 0 },
        0.5,
        new double[] { 1, 0, 0 },
        new double[] { 0, 0.877583, -0.479426 },
        new double[] { 0, 0.479426, 0.877583 }
        )
    ]
    public void TestCalculateRotationMatrix(double[] rotationAxis, double angle, double[] row1, double[] row2, double[] row3)
    {
        var V = Vector<double>.Build;
        var M = Matrix<double>.Build;
        var result = PrinterTiltCompensationClass.CalculateRotationMatrix(V.Dense(rotationAxis), angle);
        Console.WriteLine(result.ToString());
        Console.WriteLine(M.DenseOfRowArrays(row1, row2, row3).ToString());
        var equal = result.ToString() == M.DenseOfRowArrays(row1, row2, row3).ToString();
        Debug.Assert(equal, "CrossProductMatrix function received unexpected output");
    }

    [Theory]
    [InlineData(
        new double[] { 1, 0, 0.5 },
        new double[] { -1, 0, 0.5 },
        new double[] { -1, 1, 0.5 },
        new double[] { 1, 0, 0, 0 },
        new double[] { 0, 1, 0, 0 },
        new double[] { 0, 0, 1, 0.5 },
        new double[] { 0, 0, 0, 1 }
        )
    ]
    public void TestPrepareTiltCompensation(
        double[] point1,
        double[] point2,
        double[] point3,
        double[] row1,
        double[] row2,
        double[] row3,
        double[] row4
        )
    {
        var V = Vector<double>.Build;
        var M = Matrix<double>.Build;

        Point3D pos_1 = new Point3D();
        pos_1.setPosition(point1[0], point1[1], point1[2]);
        Point3D pos_2 = new Point3D();
        pos_2.setPosition(point2[0], point2[1], point2[2]);
        Point3D pos_3 = new Point3D();
        pos_3.setPosition(point3[0], point3[1], point3[2]);


        var result = PrinterTiltCompensationClass.PrepareTiltCompensation(pos_1, pos_2, pos_3);
        Console.WriteLine(result.ToString());
        Console.WriteLine(M.DenseOfRowArrays(row1, row2, row3, row4).ToString());
        var equal = result.ToString() == M.DenseOfRowArrays(row1, row2, row3, row4).ToString();
        Debug.Assert(equal, "CrossProductMatrix function received unexpected output");
    }

    [Theory]
    [InlineData(
        new double[] { 0, 0, 1 },
        new double[] { 1, 0, 0.5 },
        new double[] { -1, 0, 0.5 },
        new double[] { -1, 1, 0.5 },
        new double[] { 0, 0, 1.5 }
        )
    ]
    public void ApplyTiltCompensation(
        double[] pointToCompensate,
        double[] point1,
        double[] point2,
        double[] point3,
        double[] expected
        )
    {
        var V = Vector<double>.Build;
        var M = Matrix<double>.Build;

        Point3D pos_1 = new Point3D();
        pos_1.setPosition(point1[0], point1[1], point1[2]);
        Point3D pos_2 = new Point3D();
        pos_2.setPosition(point2[0], point2[1], point2[2]);
        Point3D pos_3 = new Point3D();
        pos_3.setPosition(point3[0], point3[1], point3[2]);

        Point3D point = new Point3D();
        point.setPosition(pointToCompensate[0], pointToCompensate[1], pointToCompensate[2]);

        PrinterTiltCompensationClass.PrepareTiltCompensation(pos_1, pos_2, pos_3);
        var result = PrinterTiltCompensationClass.ApplyTiltCompensation(point);

        var equal = result.ToArray().ToString() == expected.ToString();
        Debug.Assert(equal, "CrossProductMatrix function received unexpected output");
    }
}