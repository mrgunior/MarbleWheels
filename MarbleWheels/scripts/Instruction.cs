using MarbleWheels.scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarbleWheels.scripts
{
    abstract class Instruction
    {
        public abstract InstructionResult Execute(float deltaTime);
        public abstract Instruction Reset();

        //Operator Overloading
        //Operator overloading permits user-defined operator implementations to
        //be specified for operations where one or both of the operands are of a user-defined class or struct type
        static public Instruction operator +(Instruction A, Instruction B)
        {
            return new Semicolon(A, B);
        }
    }
}