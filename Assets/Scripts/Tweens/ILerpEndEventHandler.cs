using System;
public interface ILerpEndEventHandler
{
    public void RegisterOnCompleteEvent(EventHandler action);
    public void DeregisterOnCompleteEvent(EventHandler action);
}
