using System;
using GalaSoft.MvvmLight;

namespace Lab_5_Rasterization.Model
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public static class AlgorithmsNames
    {
        public const String StepByStep = "Step by Step";
        public const String DDA = "DDA";
        public const String BresenhamLine = "Bresenham_Line";
        public const String BresenhamCircle = "Bresenham_Circle";
    }
}