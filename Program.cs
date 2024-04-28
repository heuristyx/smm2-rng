public static class Program {
    public static bool Debug = false;

    public static void Main() {
        Console.Write("Debug? (y/n): ");
        string debug = Console.ReadLine();
        Debug = (debug == "y");

        RNG.Init();

        // Rhymes with toad last 25
        //int[] toMatch = new int[] {
        //    1, 1, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1
        //};

        List<int[]> patterns = new();

        List<Tuple<int, int>> matches = new();

        /// Change this to check initial X
        for(int X = 1; X < 2; X++) {
            /// Change 15 to number of clown cars
            Simulation sim = new Simulation(X, 15) {
                ExtraLoadFramePosition = 2,
                //OnlyLogResult = true
            };
            int[] result = sim.Start();
            //bool isMatch = result.SequenceEqual(toMatch);
            //Log($"{X}X, {2} ELFP {(isMatch ? "matched!" : "did not match")}");
            //patterns.Add(result);
            //if(isMatch) matches.Add(new Tuple<int,int>(X, 0));
        }

        //using (StreamWriter sw = new StreamWriter(File.Create("patterns.txt"))) {
        //    for (int i = 0; i < patterns.Count; i+=3) {
        //        string s1 = patterns[i].Select((n) => n.ToString()).Aggregate((n1, n2) => n1 + n2);
        //        sw.Write(s1 + "\n");
        //        //string s2 = patterns[i+1].Select((n) => n.ToString()).Aggregate((n1, n2) => n1 + n2);
        //        //string s3 = patterns[i+2].Select((n) => n.ToString()).Aggregate((n1, n2) => n1 + n2);
        //        //sw.Write(s1 + " | " + s2 + " | " + s3 + "\n");
        //    }
        //}

        //Log("==========");
        //Log("Successful matches:");
        //foreach (var match in matches) Log($"{match.Item1}X, {match.Item2} ELFP");

        Console.Read();
    }

    public static void Log(string text, bool debug = false) {
        if (debug && !Debug) return;
        Console.WriteLine($"{(debug ? "[DEBUG]\t" : "[LOG]\t")}{text}");
    }
}