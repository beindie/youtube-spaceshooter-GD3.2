using Godot;
using System;

public class Weapon : Node2D
{
    [Export]
    public int AmmoRemaining = -1;
    
    private PackedScene Ammo = (PackedScene)GD.Load("res://scenes/AmmoStandard.tscn");

    public override void _Ready()
    {
        Godot.Collections.Array NodeArray = GetTree().GetNodesInGroup("Player");

        foreach(Node N in NodeArray)
            N.Connect("WeaponFired", this, nameof(OnWeaponFired));
    }

    public void OnWeaponFired()
    {
        Node2D NewAmmo = Ammo.Instance() as Node2D;
        NewAmmo.Position = GlobalPosition;
        NewAmmo.Rotation = GlobalRotation;
        
        GetNode("AmmoRoot").AddChild(NewAmmo);
    }
}
