using UnityEngine;

namespace Interfaces
{
    public interface IPlayerObserver
    {
        public abstract void OnNotifyStart(PlayerController playerController);
        public abstract void OnNotifyUpdate(PlayerController playerController);
    }
}
