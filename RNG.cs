public static class RNG {
    static uint x, y, z, w;
    static uint c;

    public static List<uint> RNGStates = new();

    public static void Init() {
        c = 0;
        x = ARNGPlus(0, ref c);
        y = ARNGPlus(x, ref c);
        z = ARNGPlus(y, ref c);
        w = ARNGPlus(z, ref c);
        RNGStates.Add(w);
        Program.Log($"{x}, {y}, {z}, {w}", true);
    }

    public static uint ARNGPlus(uint input, ref uint c) {
        uint m = input ^ (input >> 30);
        c++;
        uint output = ((m * 0x6C078965) + c) & 0xFFFFFFFF;
        return output;
    }

    public static uint Xorshift() {
        uint t = x ^ ((x << 11) & 0xFFFFFFFF);
        uint newW = w ^ (w >> 19) ^ (t ^ (t >> 8));
        Program.Log($"Generated random number ({newW})", true);

        x = y;
        y = z;
        z = w;
        w = newW;

        RNGStates.Add(w);

        return w;
    }

    public static uint GetRandomNumber(int X) {
        uint w;
        if (X < RNGStates.Count) w = RNGStates[X];
        else w = Xorshift();
        return w;
    }
}