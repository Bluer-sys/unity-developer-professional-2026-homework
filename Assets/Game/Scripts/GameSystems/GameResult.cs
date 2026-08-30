using System;
using Fusion;

namespace Game
{
    public class GameResult : NetworkBehaviour
    {
        public event Action OnWin;
        public event Action OnLose;
        
        [Networked, OnChangedRender(nameof(InvokeStateChanged))]
        public Result CurrentResult { get; private set; }

        public void Change(Result result)
        {
            if(CurrentResult != Result.None)
                return;
            
            CurrentResult = result;
        }
        
        private void InvokeStateChanged()
        {
            switch (CurrentResult)
            {
                case Result.Win:
                    OnWin?.Invoke();
                    break;
                
                case Result.Lose:
                    OnLose?.Invoke();
                    break;
            }
        }
        
        
        public enum Result : byte
        {
            None = 0,
            
            Win = 3,
            Lose = 4
        }
    }
}
