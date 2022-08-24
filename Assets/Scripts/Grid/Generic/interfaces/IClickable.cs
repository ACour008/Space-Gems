using System;

public interface IClickable
{
    public event EventHandler OnClicked;
    public bool CanClick { set; }
}
