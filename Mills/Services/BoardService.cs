using Mills.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Mills.Services
{
    public class BoardService
    {
        public Tuple<List<PointModel>, List<List<PointModel>>> CreateInitialBoard()
        {
            // Object pool pattern
            var a1 = new PointModel() { X = "a", Y = 1, Bounds = new Rect() { X = -25, Y = 475, Width = 50, Height = 50 } };
            var a4 = new PointModel() { X = "a", Y = 4, Bounds = new Rect() { X = -25, Y = 225, Width = 50, Height = 50 } };
            var a7 = new PointModel() { X = "a", Y = 7, Bounds = new Rect() { X = -25, Y = -25, Width = 50, Height = 50 } };

            var b2 = new PointModel() { X = "b", Y = 2, Bounds = new Rect() { X = 50, Y = 400, Width = 50, Height = 50 } };
            var b4 = new PointModel() { X = "b", Y = 4, Bounds = new Rect() { X = 50, Y = 225, Width = 50, Height = 50 } };
            var b6 = new PointModel() { X = "b", Y = 6, Bounds = new Rect() { X = 50, Y = 50, Width = 50, Height = 50 } };

            var c3 = new PointModel() { X = "c", Y = 3, Bounds = new Rect() { X = 125, Y = 325, Width = 50, Height = 50 } };
            var c4 = new PointModel() { X = "c", Y = 4, Bounds = new Rect() { X = 125, Y = 225, Width = 50, Height = 50 } };
            var c5 = new PointModel() { X = "c", Y = 5, Bounds = new Rect() { X = 125, Y = 125, Width = 50, Height = 50 } };

            var d1 = new PointModel() { X = "d", Y = 1, Bounds = new Rect() { X = 225, Y = 475, Width = 50, Height = 50 } };
            var d2 = new PointModel() { X = "d", Y = 2, Bounds = new Rect() { X = 225, Y = 400, Width = 50, Height = 50 } };
            var d3 = new PointModel() { X = "d", Y = 3, Bounds = new Rect() { X = 225, Y = 325, Width = 50, Height = 50 } };
            var d5 = new PointModel() { X = "d", Y = 5, Bounds = new Rect() { X = 225, Y = 125, Width = 50, Height = 50 } };
            var d6 = new PointModel() { X = "d", Y = 6, Bounds = new Rect() { X = 225, Y = 50, Width = 50, Height = 50 } };
            var d7 = new PointModel() { X = "d", Y = 7, Bounds = new Rect() { X = 225, Y = -25, Width = 50, Height = 50 } };

            var e3 = new PointModel() { X = "e", Y = 3, Bounds = new Rect() { X = 325, Y = 325, Width = 50, Height = 50 } };
            var e4 = new PointModel() { X = "e", Y = 4, Bounds = new Rect() { X = 325, Y = 225, Width = 50, Height = 50 } };
            var e5 = new PointModel() { X = "e", Y = 5, Bounds = new Rect() { X = 325, Y = 125, Width = 50, Height = 50 } };

            var f2 = new PointModel() { X = "f", Y = 2, Bounds = new Rect() { X = 400, Y = 400, Width = 50, Height = 50 } };
            var f4 = new PointModel() { X = "f", Y = 4, Bounds = new Rect() { X = 400, Y = 225, Width = 50, Height = 50 } };
            var f6 = new PointModel() { X = "f", Y = 6, Bounds = new Rect() { X = 400, Y = 50, Width = 50, Height = 50 } };

            var g1 = new PointModel() { X = "g", Y = 1, Bounds = new Rect() { X = 475, Y = 475, Width = 50, Height = 50 } };
            var g4 = new PointModel() { X = "g", Y = 4, Bounds = new Rect() { X = 475, Y = 225, Width = 50, Height = 50 } };
            var g7 = new PointModel() { X = "g", Y = 7, Bounds = new Rect() { X = 475, Y = -25, Width = 50, Height = 50 } };

            var points = new List<PointModel> { a1, a4, a7, b2, b4, b6, c3, c4, c5, d1, d2, d3, d5, d6, d7, e3, e4, e5, f2, f4, f6, g1, g4, g7 };

            var mills = new List<List<PointModel>>()
            {
                // outer mills
                new List<PointModel>() { a1, d1, g1},
                new List<PointModel>() { a1, a4, a7},
                new List<PointModel>() { a7, d7, g7},
                new List<PointModel>() { g1, g4, g7},

                // middle mills
                new List<PointModel>() { b2, d2, f2},
                new List<PointModel>() { b2, b4, b6},
                new List<PointModel>() { b6, d6, f6},
                new List<PointModel>() { f2, f4, f6},

                // inner mills
                new List<PointModel>() { c3, c4, c5},
                new List<PointModel>() { c5, d5, e5},
                new List<PointModel>() { e3, e4, e5},
                new List<PointModel>() { c3, d3, e3},

                // paralel mills
                new List<PointModel>() { d1, d2, d3},
                new List<PointModel>() { a4, b4, c4},
                new List<PointModel>() { e4, f4, g4},
                new List<PointModel>() { d5, d6, d7}
            };

            return new Tuple<List<PointModel>, List<List<PointModel>>>(points, mills);
        }
    }
}
