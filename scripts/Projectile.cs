using Godot;
using System;

public class Projectile : Area2D
{
    [Export]
    public float DamageAmount = 100f;

    [Export]
    public float Speed = 200f;

    public override void _PhysicsProcess(float delta)
    {
        Position += GlobalTransform.y * Speed * delta;
    }

    public void BodyEntered(Node N)
    {
        if(N is EnemyAgent EA)
        {
            EA.TakeDamage(DamageAmount);
            QueueFree();
        }
    }

    public void Destroy()
    {
        QueueFree();
    }
}
