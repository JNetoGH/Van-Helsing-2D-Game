using UnityEngine;

public interface IFloorManager
{
    public bool IsFloorRunning { get; set; }
    void RespawnPlayer();
}
