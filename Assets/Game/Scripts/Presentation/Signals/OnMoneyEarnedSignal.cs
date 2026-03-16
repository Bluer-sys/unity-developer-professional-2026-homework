using UnityEngine;

namespace Game.Presentation.Signals
{
    public struct OnMoneyEarnedSignal
    {
        public Vector3 From;
        public int Range;

        public OnMoneyEarnedSignal(Vector3 from, int range)
        {
            From = from;
            Range = range;
        }
    }
}
