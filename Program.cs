public static class Program {
    public static bool Debug = false;

    public static void Main() {
        Console.Write("Debug? (y/n): ");
        string debug = Console.ReadLine();
        Debug = (debug == "y");

        int[] toMatch = new int[] {
            1, 1, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1
        };

        List<int[]> patterns = new();

        //int[] toMatch = new int[] {
        //    0, 1, 1, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0
        //};

        List<Tuple<int, int>> matches = new();

        for(int X = 2500; X < 5000; X++) {
            for(int elfp = 0; elfp < 3; elfp++) {
                Simulation sim = new Simulation(X, 25) {
                    ExtraLoadFramePosition = elfp,
                    OnlyLogResult = true
                };
                int[] result = sim.Start();
                bool isMatch = result.SequenceEqual(toMatch);
                Log($"{X}X, {elfp} ELFP {(isMatch ? "matched!" : "did not match")}");
                patterns.Add(result);
                if(isMatch) matches.Add(new Tuple<int,int>(X,elfp));
            }
        }

        using (StreamWriter sw = new StreamWriter(File.Create("patterns.txt"))) {
            for (int i = 0; i < patterns.Count; i+=3) {
                string s1 = patterns[i].Select((n) => n.ToString()).Aggregate((n1, n2) => n1 + n2);
                string s2 = patterns[i+1].Select((n) => n.ToString()).Aggregate((n1, n2) => n1 + n2);
                string s3 = patterns[i+2].Select((n) => n.ToString()).Aggregate((n1, n2) => n1 + n2);
                sw.Write(s1 + " | " + s2 + " | " + s3 + "\n");
            }
        }

        //Simulation sim = new Simulation(26, 26) {
        //    ExtraLoadFramePosition = 2
        //};
        //int[] result = sim.Start();

        Log("==========");
        Log("Successful matches:");
        foreach (var match in matches) Log($"{match.Item1}X, {match.Item2} ELFP");

        //sim = new Simulation(0) {
        //    Cars = 10,
        //    ExtraLoadFramePosition = 1
        //};
        //sim.Start();
        //sim = new Simulation(0) {
        //    Cars = 10,
        //    ExtraLoadFramePosition = 2
        //};
        //sim.Start();

        //for (int i = 0; i < 100; i++) {
        //    for (int j = 0; j < 3; j++) {
        //        Simulation sim = new Simulation(i) {
        //            OnlyLogResult = true,
        //            Cars = 10,
        //            ExtraLoadFramePosition = j
        //        };
        //        sim.Start();
        //    }
        //}

        Console.Read();
    }

    public static void Log(string text, bool debug = false) {
        if (debug && !Debug) return;
        Console.WriteLine($"{(debug ? "[DEBUG]\t" : "[LOG]\t")}{text}");
    }
}