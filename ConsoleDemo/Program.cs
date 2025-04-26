using ConsoleDemo;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var list = new List<int> { 1, 2, 3, 4, 5 };
var large = list.WhereGreaterThan(8);

if (large.IsEmpty)
{
    Console.WriteLine("No large numbers");
}
else
{
    Console.WriteLine("Found large numbers");
}