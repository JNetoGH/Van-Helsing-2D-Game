using UnityEngine;

public interface IFloorManager
{
    public bool IsFloorRunning { get; set; }
   
    public abstract void OnPlayerDead();
    public abstract void InitPhase();
    
}
