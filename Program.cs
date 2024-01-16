namespace Devoir2;

internal class Program
{
    private static void Main(string[] args)
    {
        var testRecursif = new TestRecursif(1, "Nicer");
        var test = new Test(2, 420, "Nice", testRecursif);

        Console.WriteLine(Serialization.Serialize(test));

        Console.WriteLine("\n\nDéserialisation:");
        Deserialization.LoadObject();
    }
}