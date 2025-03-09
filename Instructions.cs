public interface IInstruction {
    int Encode();
}

// Miscellaneous Instructions
public class Exit : IInstruction {
    private readonly int _code;
    public Exit(int code = 0) {
        _code = code & 0xFF;
    }
    public int Encode() {
        return (0x00 << 24) | _code;
    }
}

public class Swap : IInstruction {
    private readonly int _from;
    private readonly int _to;
    public Swap(int from = 4, int to = 0) {
        _from = (from >> 2) & 0xFFF; // Should be a multiple of 4
        _to = (_to >> 2) & 0xFFF; // Should be a multiple of 4
    }
    public int Encode() {
        return (0x01 << 24) | (_from << 12) | _to;
    }
}

public class Nop : IInstruction {
    public Nop() {}
    public int Encode() {
        return 0x02 << 24;
    }
}

public class Input : IInstruction {
    public Input() {}
    public int Encode() {
        return 0x04 << 24;
    }
}

public class StInput : IInstruction {
    private readonly int _unsignedMaxChars;
    public StInput(int unsignedMaxChars = 0x00FFFFFF)
    {
        _unsignedMaxChars = unsignedMaxChars & 0x00FFFFFF; // Ensure only 24 bits are used.
    }
    public int Encode() {
        return (0x05 << 24) | _unsignedMaxChars;
    }
}

public class Debug : IInstruction {
    private readonly int _value;
    public Debug(int value)
    {
        _value = value & 0x00FFFFFF; // Ensure only 24 bits are used.
    }
    public int Encode() {
        return (0x0F << 24) | _value;
    }
}

// Pop Instructions
public class Pop : IInstruction {
    private readonly int _offset;
    public Pop(int offset)
    {
        _offset = offset & 0x0FFFFFFC; // Should be a multiple of 4
    }
    public int Encode() {
        return (0x0F << 24) | (_offset << 2);
    }
}

public class Dup : IInstruction {
    private readonly int _offset;
    public Dup(int offset) {
        _offset = offset & ~3;
    }
    public int Encode() {
        return (0b1100 << 28) | _offset;
    }
}
