using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export]
    public float Speed = 2f;

    [Export]
    public float Damping = 6f;

    private Vector2 Velocity = Vector2.Zero;
    private Vector2 CurrentVelocity = Vector2.Zero;
    private float StartRot = 0f;

    private PackedScene Explode = (PackedScene)GD.Load("res://scenes/Explosion.tscn");

    [Signal]
    public delegate void WeaponFired();

    public override void _Ready()
    {
          StartRot = Rotation;
    }

    public override void _Process(float delta)
    {
        Velocity = new Vector2(-Input.GetActionStrength("Left") + Input.GetActionStrength("Right"),
                                        Input.GetActionStrength("Down") + -Input.GetActionStrength("Up"));
        Velocity = Velocity.Normalized() * Speed;

        Vector2 CursorPos = GetLocalMousePosition();
        Rotation += CursorPos.Angle() + StartRot;
        Rotation = Mathf.Wrap(Rotation, Mathf.Deg2Rad(-360),Mathf.Deg2Rad(360));

        if(Input.IsActionJustPressed("Fire"))
            EmitSignal("WeaponFired");
    }

    public override void _PhysicsProcess(float delta)
    {
        CurrentVelocity = MoveAndSlide(CurrentVelocity.LinearInterpolate(Velocity, Damping * delta));

        if(GetSlideCount() > 0)
        {
            Node2D N = Explode.Instance() as Node2D;
            N.Position = GlobalPosition;
            GetTree().Root.GetChild(0).AddChild(N);
            QueueFree();
            return;
        }

        //Clamp Position
        Rect2 Rct = GetViewport().GetVisibleRect();
        Position = new Vector2(Mathf.Clamp(Position.x, Rct.Position.x, Rct.Size.x),
                               Mathf.Clamp(Position.y, Rct.Position.y, Rct.Size.y));
    }
}
