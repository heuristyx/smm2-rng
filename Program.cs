public static class Program {
    public static bool Debug = false;

    public static void Main() {
        Console.Write("Debug? (y/n): ");
        string debug = Console.ReadLine();
        Debug = (debug == "y");

        Console.Write("Simulation length (frames): ");
        string simLength = Console.ReadLine();
        Simulation sim = new Simulation() {
            Length = int.Parse(simLength) // to-do: validate this...
        };

        sim.Start();

        Console.Read();
    }

    public static void Log(string text, bool debug = false) {
        if (debug && !Debug) return;
        Console.WriteLine($"{(debug ? "[DEBUG]\t" : "[LOG]\t")}{text}");
    }
}