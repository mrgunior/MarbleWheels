using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarbleWheels.scripts
{
    class Wait : Instruction
    {
        float timeToWait;
        //Func in short is parameterized delegate. In C#, a delegate instance points towards a method
        //delegate TResult Func <in T, out TResult> (T arg);
        //We can and should use delegate to avoid rewriting a fresh method every time we get by a new rule we want to apply
        Func<float> getTimeToWait;

        public Wait(Func<float> getTimeToWait)
        {
            //a delegate instance points towards a method
            this.timeToWait = getTimeToWait();
            //
            this.getTimeToWait = getTimeToWait;
        }

        public override InstructionResult Execute(float deltaTime)
        {
            timeToWait -= deltaTime;
            if (timeToWait <= 0.0f)
                return InstructionResult.Done;
            else
                return InstructionResult.Running;
        }

        //new object
        public override Instruction Reset()
        {
            return new Wait(getTimeToWait);
        }
    }
}