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