using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarbleWheels.scripts
{
    class CreateEnemyObject : Instruction
    {

        //create enemy object
        public override InstructionResult Execute(float deltaTime)
        {
            return InstructionResult.DoneAndCreateEnemyObject;
        }

        public override Instruction Reset()
        {
            return new CreateEnemyObject();
        }
    }
}