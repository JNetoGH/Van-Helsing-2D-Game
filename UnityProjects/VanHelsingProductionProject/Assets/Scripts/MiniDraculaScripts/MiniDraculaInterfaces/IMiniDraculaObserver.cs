namespace MiniDraculaScripts.Interfaces
{
    public interface IMiniDraculaObserver
    {
        public abstract void OnNotifyStart(MiniDraculaController miniDraculaController);
        public abstract void OnNotifyUpdate(MiniDraculaController miniDraculaController);
    }
}