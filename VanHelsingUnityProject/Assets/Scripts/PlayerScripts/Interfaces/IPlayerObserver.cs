using UnityEngine;

namespace Interfaces
{
    public interface IPlayerObserver
    {
        public abstract void OnNotify(PlayerController playerController);
    }
}