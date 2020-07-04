using Godot;
using System;

public class EnemyAgent : KinematicBody2D
{
    [Export]
    public float Speed = 100f;
    private Node2D Target = null;
    private float StartRot = 0f;

    private float Health = 100f;

    private PackedScene Explode = (PackedScene)GD.Load("res://scenes/Explosion.tscn");

    public override void _Ready()
    {
        StartRot = Rotation;
        
        RandomNumberGenerator NG = new RandomNumberGenerator();
        NG.Randomize();
        float RandomScale = NG.RandfRange(0.2f,0.6f);
        Scale = new Vector2(RandomScale,RandomScale);
        Modulate = new Color(NG.RandfRange(0.3f,1), NG.RandfRange(0.3f,1), NG.RandfRange(0.3f,1));
    }

    public override void _PhysicsProcess(float delta)
    {
        //If not target then don't move
        if(Target == null)return;

        Rotation = (Target.GlobalPosition - GlobalPosition).Angle() + StartRot;
        Rotation = Mathf.Wrap(Rotation, Mathf.Deg2Rad(-360),Mathf.Deg2Rad(360));

        MoveAndSlide(GlobalTransform.y.Normalized() * Speed);
    }

    public void SetTarget(Node2D N)
    {
        Target = N;
    }

    public void TakeDamage(float Amount)
    {
        Health -= Amount;
        Node2D N = Explode.Instance() as Node2D;
        AddChild(N);

        if(Health <= 0)
            QueueFree();
    }
}
