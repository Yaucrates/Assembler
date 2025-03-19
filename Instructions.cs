// Miscellaneous Instructions
public class Exit : IInstruction {
    private readonly int _code;
    public Exit(int code = 0) {
        _code = code & 0xFF;
    }
    public int Encode() {
        return _code;
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
        return (0b0001 << 24) | (_from << 12) | _to;
    }
}

public class Nop : IInstruction {
    public int Encode() {
        return 0b0010 << 24;
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
        return (0b0101 << 24) | _unsignedMaxChars;
    }
}

public class Debug : IInstruction {
    private readonly int _value;
    public Debug(int value = 0)
    {
        _value = value & 0x00FFFFFF; // Ensure only 24 bits are used.
    }
    public int Encode() {
        return (0b1111 << 24) | _value;
    }
}

// Pop Instructions
public class Pop : IInstruction {
    private readonly int _offset;
    public Pop(int offset = 4)
    {
        _offset = offset & 0x0FFFFFFC; // Should be a multiple of 4
    }
    public int Encode() {
        return (0b0001 << 28) | _offset;
    }
}

// Binary Arithmetic Instructions
public class Add : IInstruction {
    public Add() {}
    public int Encode() {
        return 0x2 << 28;
    }
}

public class Sub : IInstruction {
    public Sub() {}
    public int Encode() {
        return (0x2 << 28) | (0b0001 << 24);
    }
}

public class Mul : IInstruction {
    public Mul() {}
    public int Encode() {
        return (0x2 << 28) | (0b0010 << 24);
    }
}

public class Div: IInstruction {
    public Div() {}
    public int Encode() {
        return (0x2 << 28) | (0b0011 << 24);
    }
}

public class Rem: IInstruction {
    public Rem() {}
    public int Encode() {
        return (0x2 << 28) | (0b0100 << 24);
    }
}

public class And: IInstruction {
    public And() {}
    public int Encode() {
        return (0x2 << 28) | (0b0101 << 24);
    }
}

public class Or : IInstruction {
    public Or() {}
    public int Encode() {
        return (0x2 << 28) | (0b0110 << 24);
    }
}

public class Xor: IInstruction {
    public Xor() {}
    public int Encode() {
        return (0x2 << 28) | (0b0111 << 24);
    }
}

public class Lsl : IInstruction {
    public Lsl() {}
    public int Encode() {
        return (0x2 << 28) | (0b1000 << 24);
    }
}

public class Lsr: IInstruction {
    public Lsr() {}
    public int Encode() {
        return (0x2 << 28) | (0b1001 << 24);
    }
}

public class Asr : IInstruction {
    public Asr() {}
    public int Encode() {
        return (0x2 << 28) | (0b1011 << 24);
    }
}

// Unary Instructions
public class Neg : IInstruction {
    public Neg() {}
    public int Encode() {
        return 0x3 << 28;
    }
}

public class Not : IInstruction {
    public Not() {}
    public int Encode() {
        return (0b0011 << 28) | (0b0001 << 24);
    }
}

// String Print Instructions
public class Stprint : IInstruction {
    private readonly int _offset;
    public Stprint(int offset = 0) {
        _offset = offset & 0x0FFFFFFF;
    }
    public int Encode() {
        return (0b0100 << 28) | _offset;
    }
}

// Call Instructions
// public class Call : IInstruction {
//     private readonly int _offset;
//     public Call(int offset) {
//         _offset = offset & ~3;
//     }
//     public int Encode() {
//         return (0x5 << 28) | _offset;
//     }
// }

// Return Instructions
// public class Ret : IInstruction {
//     private readonly int _offset;
//     public Ret(int offset = 0) {
//         _offset = offset & ~3;
//     }
//     public int Encode() {
//         return (0x6 << 28) | _offset;
//     }
// }

// Unconditional Goto Instructions
public class Goto : IInstruction {
    private readonly int _offset;
    public Goto(int offset) {
        _offset = offset & 0x0FFFFFFF;
    }
    public int Encode() {
        return (0b0111 << 28) | _offset;
    }
}

// If Instructions
// public class If : IInstruction {
//     private readonly int _offset;
//     private readonly int _condition;
//     public If(int condition, int offset) {
//         _condition = condition & 0x7;
//         _offset = offset & 0x01FFFFFF;
//     }
//     public int Encode() {
//         return (0x8 << 28) | (_condition << 25) | _offset;
//     }
// }

// Unary If Instructions
public class UnaryIf : IInstruction {
    private readonly int _offset;
    private readonly int _condition;
    public UnaryIf(int condition, int offset) {
        _condition = condition & 0b011;
        _offset = offset & 0x01FFFFFF;
    }
    public int Encode() {
        return (0b1001 << 28) | (_condition << 25) | _offset;
    }
}

// Dup Instructions
// public class Dup : IInstruction {
//     private readonly int _offset;
//     public Dup(int offset) {
//         _offset = offset & ~3;
//     }
//     public int Encode() {
//         return (0xc << 28) | _offset;
//     }
// }

// Print Instructions
public class Print : IInstruction {
    private readonly int _offset;
    private readonly int _fmt;
    public Print(int fmt, int offset = 0) {
        _offset = offset & 0x0FFFFFFC;
        _fmt = fmt & 0b11;
    }
    public int Encode() {
        return (0b1101 << 28) | _offset | _fmt;
    }
}

// Dump Instructions
public class Dump : IInstruction {
    public Dump() {}
    public int Encode() {
        return 0b1110 << 28;
    }
}

// Push Instructions
public class Push : IInstruction {
    private readonly int _val;
    public Push(int val = 0) {
        _val = val & 0x0FFFFFFF;
    }
    public int Encode() {
        return (0b1111 << 28) | _val;
    }
}
