using UnityEngine;

namespace Game.Presentation.Signals
{
    public readonly struct OnMoneyEarnedSignal
    {
        public readonly Vector3 From;
        public readonly int Range;

        public OnMoneyEarnedSignal(Vector3 from, int range)
        {
            From = from;
            Range = range;
        }
    }
}
