using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Gameplay
{
    public class ControlsPresenter : IControlsPresenter
    {
        private readonly ISaveManager _saveManager;

        public ControlsPresenter(ISaveManager saveManager)
        {
            _saveManager = saveManager;
        }
        
        public void Save(Action<bool, int> callback)
        {
            _saveManager.Save(callback).Forget();
        }

        public void Load(string version, Action<bool, int> callback)
        {
            if(int.TryParse(version, out int versionNumber))
                _saveManager.Load(versionNumber, callback).Forget();
            else
                Debug.LogError("Version is not a number");
        }
    }
}
