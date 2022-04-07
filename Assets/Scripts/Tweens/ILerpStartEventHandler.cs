using System;

public interface ILerpStartEventHandler
{
    public void RegisterOnStartEvent(EventHandler action);
    public void DeregisterOnStartEvent(EventHandler action);
}
