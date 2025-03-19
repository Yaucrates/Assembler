# General Notes

Instruction **ARE** NOT case sensitive.

Instructions are 4 bytes.

## Pass 1

The job of pass 1 is to encode the labels to the program counter.

Labels:
- **ARE** case sensitive.
- record the memory location and ??line number??
- always end in a colon

Pseudoinstruction will complicate pass 1 since it adds instructions

Comments start with a pound

All leading and trailing whitespace must be trimmed

## Pass 2

Idc it was super simple

## Stpush Pseudo-Instruction

Expands to multiple push instructions

Null terminator should be added by default

Three escapes are supported: \\\\, \\", \\n

Also, the push instruction pushes in little-endian, therefore, your instruction must swap them to be in the right order.

**stprint**:
- should print it exactly
- prints stack top to bottom, aka, LAST THING PUSHED IS THE FIRST THING READ