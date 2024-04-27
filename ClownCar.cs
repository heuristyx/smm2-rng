public class ClownCar {
    public enum ClownCarState {
        Unloaded,
        Idle,
        Blinking,
        Stunned
    }

    const int STUN_DURATION = 122;
    const int FRAMES_LOADED_LEFT = 1 + STUN_DURATION + 85; // or 84
    const int FRAMES_LOADED_RIGHT = 1 + STUN_DURATION + 72; // or 71

    public uint BlinkInterval;
    public uint StunDirection;

    ClownCarState State = ClownCarState.Idle;

    public int BlinkTimer = 1;
    public int StunTimer = 1;
    public int LifeTimer = 1;

    public int ID = 0;

    public bool HasUniqueDeloadTime = false; // First 3 clown cars in rhymes with toad

    Simulation Sim;

    public ClownCar(Simulation sim) {
        Sim = sim;
    }

    public void Update() {
        switch (State) {
            case ClownCarState.Unloaded:
                break;
            case ClownCarState.Blinking:
                if (BlinkTimer >= 5) {
                    BlinkTimer = 1;
                    State = ClownCarState.Idle;
                    // Blink interval RNG call
                    BlinkInterval = Sim.RandBlinkInterval(ID);
                    Sim.Log($"(CC {ID}) Opened eyes again -- new blink interval is {BlinkInterval}");
                }
                BlinkTimer++;
                break;
            case ClownCarState.Idle:
                // Stunned on 1st frame after loading?
                if (LifeTimer == 1) Stun();
                if (BlinkTimer >= BlinkInterval ) {
                    Sim.Log($"(CC {ID}) Starting blink");
                    BlinkTimer = 1;
                    State = ClownCarState.Blinking;
                    // Unknown RNG call
                    Sim.RandDiscard(reason: $"({ID}) Blink start");
                } else BlinkTimer++;
                break;
            case ClownCarState.Stunned:
                if (StunTimer >= STUN_DURATION) {
                    State = ClownCarState.Idle;
                    BlinkInterval = Sim.RandBlinkInterval(ID);
                    BlinkTimer = 1;
                    Sim.Log($"(CC {ID}) Exiting stun state -- new blink interval is {BlinkInterval}");
                }
                StunTimer++;
                break;
        }

        if (HasUniqueDeloadTime) {
            int despawn = -1;
            switch (ID) {
                case 1: // Should move right
                    despawn = 1;
                    break;
                case 2: // Should move right
                    despawn = 1;
                    break;
                case 3: // Should move left
                    despawn = 1;
                    break;
            }
        } else {
            bool hasLessLoadedTime = (ID - 1 + Sim.ExtraLoadFramePosition) % 3 == 0;
            if (StunDirection == 1 && LifeTimer >= FRAMES_LOADED_LEFT - (hasLessLoadedTime ? 1 : 0) && State != ClownCarState.Unloaded) {
                Sim.Log($"(CC {ID}) Unloaded (T-{BlinkInterval - BlinkTimer} until blink). I was {State}");
                State = ClownCarState.Unloaded;
            } else if (StunDirection == 0 && LifeTimer >= FRAMES_LOADED_RIGHT - (hasLessLoadedTime ? 1 : 0) && State != ClownCarState.Unloaded) {
                Sim.Log($"(CC {ID}) Unloaded (T-{BlinkInterval - BlinkTimer} until blink). I was {State}");
                State = ClownCarState.Unloaded;
            }
        }

        LifeTimer++;
    }

    public void Stun() {
        State = ClownCarState.Stunned;
        StunTimer = 1;

        StunDirection = Sim.RandSpikeDirection(ID);
        Sim.Directions[ID - 1] = (int)StunDirection;
        Sim.Log($"(CC {ID}) Hit spike -- moving {(StunDirection == 0 ? "right" : "left")}");

        // Cut simulation at the last clown car stun - everything after isn't relevant
        if (ID == Sim.Cars) Sim.Stop = true;
    }
}