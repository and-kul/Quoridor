using System;

namespace Quoridor
{
    public class Human : Player
    {
        public Human(string name) : base(name)
        {
        }

        public override Move CreateMove()
        {
            throw new NotImplementedException();
        }
    }
}