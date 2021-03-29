using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Prometric.MathApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var mathApp = new MathApp();

            var circle = new MathApp.Circle(5);
            Console.WriteLine(circle.Name);
            Console.WriteLine($"- Area: {circle.Area}");
            Console.WriteLine($"- Perimeter: {circle.Perimeter}");
            Console.WriteLine();

            var triangle1 = new MathApp.Triangle(5, 4);
            Console.WriteLine(triangle1.Name);
            Console.WriteLine($"- Area: {triangle1.Area}");
            Console.WriteLine($"- Perimeter: {triangle1.Perimeter}");
            Console.WriteLine();

            var triangle2 = new MathApp.Triangle(5, 5, 5);
            Console.WriteLine(triangle2.Name);
            Console.WriteLine($"- Area: {triangle2.Area}");
            Console.WriteLine($"- Perimeter: {triangle2.Perimeter}");
            Console.WriteLine();

            var triangle3 = new MathApp.Triangle(5, 5, 1);
            Console.WriteLine(triangle3.Name);
            Console.WriteLine($"- Area: {triangle3.Area}");
            Console.WriteLine($"- Perimeter: {triangle3.Perimeter}");
            Console.WriteLine();

            var square = new MathApp.Square(5, 5);
            Console.WriteLine(square.Name);
            Console.WriteLine($"- Area: {square.Area}");
            Console.WriteLine($"- Perimeter: {square.Perimeter}");
            Console.WriteLine();

            var rect = new MathApp.Rectangle(10, 10);
            Console.WriteLine(rect.Name);
            Console.WriteLine($"- Area: {rect.Area}");
            Console.WriteLine($"- Perimeter: {rect.Perimeter}");
            Console.WriteLine();

            Console.WriteLine($"Shapes serialized to JSON format:\n{mathApp.SerializeToJson}\n");
            mathApp.OrderShapesBy(MathApp.Type.Perimeter, MathApp.OrderBy.Desc);
            Console.WriteLine($"Shapes ordering desc by Perimeter and serialized to JSON format:\n{mathApp.SerializeToJson}\n");
            Console.WriteLine($"Number of Shapes in memory: {mathApp.NumberOfShapes}");

            Console.ReadLine();
        }
    }

    public class MathApp
    {
        private static IList<Shape> _shapes;

        public enum Type { Area, Perimeter }

        public enum OrderBy { Asc, Desc }

        public string SerializeToJson => JsonSerializer.Serialize(_shapes);

        public int NumberOfShapes => _shapes.Count();

        public MathApp()
        {
            _shapes = new List<Shape>();
        }

        public void OrderShapesBy(Type type = Type.Area, OrderBy orderBy = OrderBy.Asc)
        {
            IEnumerable<Shape> result = type switch
            {
                Type.Area => _shapes.OrderBy(x => x.Area),
                Type.Perimeter => _shapes.OrderBy(x => x.Perimeter),
                _ => null
            };

            if (orderBy == OrderBy.Desc)
            {
                _shapes = result.Reverse().ToList();
            }

            _shapes = result.ToList();
        }

        public abstract class Shape
        {
            public string Name { get; protected set; }
            public virtual double Area { get; set; }
            public virtual double Perimeter { get; set; }

            protected Shape(string name)
            {
                Name = name;
            }
        }

        public class Circle : Shape
        {
            private const double Pi = 3.14;

            private int Radius { get; }

            public Circle(int radius) : base("Circle")
            {
                Radius = radius;
                _shapes.Add(this);
            }

            public override double Area => Pi * Radius * Radius;

            public override double Perimeter => 2 * Pi * Radius;
        }

        public class Triangle : Shape
        {
            private double Side1 { get; }
            private double Side2 { get; }
            private double Side3 { get; }

            public Triangle(double side1, double side2, double side3 = -1)
                : base("Triangle")
            {
                Side1 = side1;
                Side2 = side2;
                Side3 = (side3 == -1) ? 
                    Math.Sqrt(Math.Pow(side1, 2) + Math.Pow(side2, 2)) : // hypotenuse
                    side3;

                if (side1 == side2 && side2 == side3)
                {
                    Name = "Equilateral";
                }
                else if (side1 == side2 || side1 == side3 || side2 == side3)
                {
                    Name = "Isosceles";
                }

                _shapes.Add(this);
            }

            public override double Area =>
                Name switch
                {
                    "Equilateral" => Math.Sqrt(3) / 4 * Side1 * Side2,
                    "Isosceles" => Math.Sqrt(SemiPerimeter * (SemiPerimeter - Side1) * (SemiPerimeter - Side2) * (SemiPerimeter - Side3)),
                    _ => (Side1 * Side2) / 2
                };

            public override double Perimeter => Side1 + Side2 + Side3;

            private double SemiPerimeter => Perimeter / 2;
        }

        public class Square : Shape
        {
            public int Width { get; }
            public int Length { get; }

            public Square(int width, int length, string name = "Square")
                : base(name)
            {
                Width = width;
                Length = length;
                _shapes.Add(this);
            }

            public override double Area => Width * Length;

            public override double Perimeter => (Width + Length) * 2;
        }

        public class Rectangle : Square
        {
            public Rectangle(int width, int length)
                : base(width, length, "Rectangle")
            {
            }
        }
    }
}