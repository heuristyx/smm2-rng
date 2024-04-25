public class Simulation {
    // Which frames should new clown cars be loaded?
    int[] loadOnFrames = new int[] {
        1
    };

    public int Length = 0;
    public int Frame = 0;
    public int X = 0;

    public List<ClownCar> ClownCars = new();

    public void Log(string text) {
        Program.Log($"F{Frame}: {text}");
    }

    /// Generate <count> random numbers and discard them
    public void RandDiscard(int count = 1) {
        X += count;
        for (int i = 0; i < count; i++) RNG.Xorshift();
    }

    /// Generate new blink timer (rand(170) + 10)
    public uint RandBlinkInterval() {
        uint val = 10 + (uint)((float)RNG.Xorshift()/UInt32.MaxValue * 170) >> 32;
        X++;
        Program.Log($"New blink interval generated: {val}", true);
        return val;
    }

    /// Generate stun direction after hitting spike (w_n % 2)
    public uint RandSpikeDirection() {
        uint val = RNG.Xorshift() % 2;
        X++;
        Program.Log($"New spike direction generated: {val}", true);
        return val;
    }

    public void Start() {
        RNG.Init();

        Program.Log("=== SIMULATION START ===");
        while (Frame < Length) FrameAdvance();
        Program.Log($"=== SIMULATION END ({Frame} frames) ===");
    }

    public void FrameAdvance() {
        Frame++;

        if (loadOnFrames.Contains(Frame)) LoadClownCar();

        // TO-DO: player RNG (just 1X per frame but before or after clown car update?)

        foreach (ClownCar cc in ClownCars) {
            cc.Update();
        }
    }

    private void LoadClownCar() {
        Log("Loading a new clown car");
        ClownCar cc = new ClownCar(this);
        ClownCars.Add(cc);
        cc.ID = ClownCars.Count;
        RandDiscard(3); // Clown car 3X
        cc.BlinkInterval = RandBlinkInterval();
    }
}