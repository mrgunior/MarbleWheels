using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarbleWheels.scripts
{
    class For : Instruction
    {
        int start, end, i;
        //Func in short is parameterized delegate. In C#, a delegate instance points towards a method
        //delegate TResult Func <in T, out TResult> (T arg);
        //We can and should use delegate to avoid rewriting a fresh method every time we get by a new rule we want to apply
        Func<int, Instruction> getBody;
        Instruction body;
        public For(int start, int end, Func<int, Instruction> getBody)
        {
            this.start = start; //0
            this.end = end; //10
            this.getBody = getBody;

            //a delegate instance points towards a method
            this.i = start; //0
            this.body = getBody(i); //getBody(0);
        }

        public override InstructionResult Execute(float deltaTime)
        {
            if (i >= end)
            {
                return InstructionResult.Done;
            }

            else
            {
                switch (body.Execute(deltaTime))
                {

                    //0
                    case InstructionResult.Done:
                        i++;
                        body = getBody(i);
                        //returning back for the switch case
                        return InstructionResult.Running;

                    //1
                    case InstructionResult.DoneAndCreateEnemyObject:
                        i++;
                        body = getBody(i);
                        //returning back for the switch case
                        return InstructionResult.RunningAndCreateEnemyObject;

                    case InstructionResult.Running:
                        //returning back for the switch case
                        return InstructionResult.Running;

                    case InstructionResult.RunningAndCreateEnemyObject:
                        //returning back for the switch case
                        return InstructionResult.RunningAndCreateEnemyObject;
                }

                return InstructionResult.Done;
            }
        }

        //new object
        public override Instruction Reset()
        {
            return new For(start, end, getBody);
        }
    }
}