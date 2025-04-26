namespace ConsoleDemo;

public static class MyExtensions
{
    extension(IEnumerable<int> source) 
    {
        public IEnumerable<int> WhereGreaterThan(int threshold)
            => source.Where(x => x > threshold);

        public bool IsEmpty
            => !source.Any();
    }
}