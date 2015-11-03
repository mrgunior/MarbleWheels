using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarbleWheels.scripts
{
    class Semicolon : Instruction
    {
        Instruction A, B;
        bool isADone = false, isBDone = false;
        public Semicolon(Instruction A, Instruction B)
        {
            this.A = A;
            this.B = B;
        }

        public override InstructionResult Execute(float deltaTime)
        {
            if (!isADone)
            {
                var Ares = A.Execute(deltaTime);
                switch (Ares)
                {
                    case InstructionResult.Done:
                        isADone = true;
                        return InstructionResult.Running;

                    case InstructionResult.DoneAndCreateEnemyObject:
                        isADone = true;

                        return InstructionResult.RunningAndCreateEnemyObject;
                    default:
                        return Ares;
                }
            }
            else
            {
                if (!isBDone)
                {
                    var Bres = B.Execute(deltaTime);
                    switch (Bres)
                    {
                        case InstructionResult.Done:
                            isBDone = true;
                            break;

                        case InstructionResult.DoneAndCreateEnemyObject:
                            isBDone = true;
                            break;
                    }
                    return Bres;
                }
                else
                {
                    return InstructionResult.Done;
                }
            }
        }

        //new object
        public override Instruction Reset()
        {
            return new Semicolon(A.Reset(), B.Reset());
        }
    }
}