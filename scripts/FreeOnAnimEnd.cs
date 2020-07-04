using Godot;
using System;

public class FreeOnAnimEnd : AnimatedSprite
{
    public void OnAnimComplete(string AnimName)
    {
        QueueFree();
    }
}
