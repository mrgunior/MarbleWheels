using MarbleWheels.scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarbleWheels.scripts
{
    abstract class Instruction
    {
        public abstract InstructionResult Execute(float dt);
        public abstract Instruction Reset();

        static public Instruction operator +(Instruction A, Instruction B)
        {
            return new Semicolon(A, B);
        }
    }
}