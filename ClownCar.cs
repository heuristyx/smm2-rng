public class ClownCar {
    public enum ClownCarState {
        Unloaded,
        Idle,
        Blinking,
        Stunned
    }

    const int STUN_DURATION = 122;
    // TO-DO: unloading times
    // const int FRAMES_LOADED_LEFT = 250;
    // const int FRAMES_LOADED_RIGHT = 250;

    public uint BlinkInterval;
    public uint StunDirection;

    ClownCarState State = ClownCarState.Idle;

    public int BlinkTimer = 1;
    public int StunTimer = 1;
    public int LifeTimer = 1;

    public int ID = 0;

    Simulation Sim;

    public ClownCar(Simulation sim) {
        Sim = sim;
    }

    public void Update() {
        switch (State) {
            case ClownCarState.Unloaded:
                break;
            case ClownCarState.Blinking:
                // Unknown RNG call
                Sim.RandDiscard();
                if (BlinkTimer >= 5) {
                    // Blink interval RNG call
                    BlinkInterval = Sim.RandBlinkInterval();
                    BlinkTimer = 1;
                    Sim.Log($"(CC {ID}) Blink on frames {Sim.Frame - 4}-{Sim.Frame} -- new blink interval is {BlinkInterval}");
                    State = ClownCarState.Idle;
                }
                BlinkTimer++;
                break;
            case ClownCarState.Idle:
                // Stunned on 1st frame after loading?
                if (LifeTimer == 1) Stun();
                if (BlinkTimer >= BlinkInterval) {
                    BlinkTimer = 1;
                    State = ClownCarState.Blinking;
                } else BlinkTimer++;
                break;
            case ClownCarState.Stunned:
                if (StunTimer >= STUN_DURATION) {
                    State = ClownCarState.Idle;
                    BlinkInterval = Sim.RandBlinkInterval();
                    BlinkTimer = 1;
                    Sim.Log($"(CC {ID}) Exited stun state -- new blink interval is {BlinkInterval}");
                }
                StunTimer++;
                break;
        }

        // TO-DO: unloading times
        //if (StunDirection == 0 && LifeTimer >= FRAMES_LOADED_LEFT) {
        //    State = ClownCarState.Unloaded;
        //} else if (StunDirection == 0 && LifeTimer >= FRAMES_LOADED_RIGHT) {
        //    State = ClownCarState.Unloaded;
        //}

        LifeTimer++;
    }

    public void Stun() {
        State = ClownCarState.Stunned;
        StunTimer = 1;

        StunDirection = Sim.RandSpikeDirection();
        Sim.Log($"(CC {ID}) Hit spike -- moving {(StunDirection == 0 ? "right" : "left")}");
    }
}