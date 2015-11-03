using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarbleWheels.scripts
{
    class Repeat : Instruction
    {
        Instruction body;
        public Repeat(Instruction body)
        {
            this.body = body;
        }

        public override InstructionResult Execute(float deltaTime)
        {
            switch (body.Execute(deltaTime))
            {
                case InstructionResult.Done:
                    body = body.Reset();
                    return InstructionResult.Running;

                case InstructionResult.DoneAndCreateEnemyObject:
                    body = body.Reset();
                    return InstructionResult.RunningAndCreateEnemyObject;

                case InstructionResult.Running:
                    return InstructionResult.Running;

                case InstructionResult.RunningAndCreateEnemyObject:
                    return InstructionResult.RunningAndCreateEnemyObject;
            }

            return InstructionResult.Running;
        }

        //new object
        public override Instruction Reset()
        {
            return new Repeat(body.Reset());
        }
    }
}