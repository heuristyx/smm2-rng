public static class Program {
    public static bool Debug = false;

    public static void Main() {
        Console.Write("Debug? (y/n): ");
        string debug = Console.ReadLine();
        Debug = (debug == "y");

        RNG.Init();

        // Rhymes with toad -- this won't match because of the winged mushroom adding 3X at some point
        int[] toMatch = new int[] {
            0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 0
        };

        List<int[]> patterns = new();

        List<Tuple<int, int>> matches = new();

        /// Change this to sweep through initial X
        for(int X = 870; X < 890; X++) {
            int elfp = 2;
            /// Second argument is number of clown cars
            Simulation sim = new Simulation(X, 29) {
                ExtraLoadFramePosition = elfp,
                OnlyLogResult = true,
                ExportToCSV = false
            };
            int[] result = sim.Start();
            bool isMatch = result.SequenceEqual(toMatch);
            patterns.Add(result);
            if(isMatch) matches.Add(new Tuple<int,int>(X, elfp));
        }

        // patterns.txt contains all clown car directions at each X swept through
        using (StreamWriter sw = new StreamWriter(File.Create("patterns.txt"))) {
            for(int i = 0; i < patterns.Count; i++) {
                string s1 = patterns[i].Select((n) => n.ToString()).Aggregate((n1,n2) => n1 + n2);
                sw.Write(s1 + "\n");
                //string s2 = patterns[i+1].Select((n) => n.ToString()).Aggregate((n1, n2) => n1 + n2);
                //string s3 = patterns[i+2].Select((n) => n.ToString()).Aggregate((n1, n2) => n1 + n2);
                //sw.Write(s1 + " | " + s2 + " | " + s3 + "\n");
            }
        }

        Log("==========");
        Log("Successful matches:");
        foreach(var match in matches) Log($"{match.Item1}X, {match.Item2} ELFP");

        Console.Read();
    }

    public static void Log(string text, bool debug = false) {
        if (debug && !Debug) return;
        Console.WriteLine($"{(debug ? "[DEBUG]\t" : "[LOG]\t")}{text}");
    }
}