using Xunit;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;

using System.Diagnostics;
using System;

namespace PrinterTiltCompensation.Tests;

public class UnitTests
{
    [Fact]
    public void TestCrossProduct()
    {
        var V = Vector<double>.Build;


        var tupleList = new (Vector<double>, Vector<double>, Vector<double>)[]
        {
            (V.Dense(new double [3] {1, 0, 0}), V.Dense(new double [3] {1, 1, 0}), V.Dense(new double [3] {0, 0, 1})),
            (V.Dense(new double [3] {1, 0, 3}), V.Dense(new double [3] {1, 2, 0}), V.Dense(new double [3] {-6, 3, 2})),
            (V.Dense(new double [3] {1, 0, 3}), V.Dense(new double [3] {1, 0, 1}), V.Dense(new double [3] {0, 2, 0})),
        };

        foreach ((Vector<double>, Vector<double>, Vector<double>) test in tupleList)
        {
            var result = PrinterTiltCompensationClass.CrossProduct(test.Item1, test.Item2);
            var equal = result.Equals(test.Item3);
            Debug.Assert(equal, "CrossProduct function received unexpected output");
        }

    }
}