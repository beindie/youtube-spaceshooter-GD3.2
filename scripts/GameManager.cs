using Godot;
using System;

public class GameManager : Node2D
{
    private Player PlayerNode = null;
    private RandomNumberGenerator NG = new RandomNumberGenerator();

    private float TimeInLevel = 0f;

    private PackedScene EnemyObject = (PackedScene)GD.Load("res://scenes/EnemyPhantom.tscn");
    
    public override void _Ready()
    {
        //Connect Add and remove Signals
        GetTree().Connect("node_added", this, nameof(OnNodeAdded));
        GetTree().Connect("node_removed", this, nameof(OnNodeRemoved));

        //Find Player
        PlayerNode = FindNode("Player", true, false) as Player;

        //Make enemies find player
        Godot.Collections.Array NodesArray = GetTree().GetNodesInGroup("Enemy");
        
        foreach(Node N in NodesArray)
        {
            if(N is EnemyAgent EA && PlayerNode != null)
                EA.SetTarget(PlayerNode);
        }

        TimeInLevel = 0f;
    }

    public override void _Process(float delta)
    {
        TimeInLevel += delta;
    }

    public void OnNodeAdded(Node N)
    {
        if(N is EnemyAgent EA && PlayerNode != null)
        {
            EA.SetTarget(PlayerNode);
        }
    }

    public void OnNodeRemoved(Node N)
    {
        if(N is Player PN)
        {
            PlayerNode = null;
            //Clear Enenmies
            Godot.Collections.Array NodesArray = GetTree().GetNodesInGroup("Enemy");
            
            foreach(Node EN in NodesArray)
            {
                if(EN is EnemyAgent EA)
                    EA.SetTarget(null);
            }

            //Game Over
            Label GO = GetNode("GameOver") as Label;
            GO.Visible = true;
            GO.Text = "GAME OVER\nYOU SURVIVED FOR: " + Mathf.RoundToInt(TimeInLevel) + " SECONDS";
        }
    }

    public void SpawnEnemy()
    {
        NG.Randomize();
        Rect2 VisibleRect = GetViewport().GetVisibleRect();
        Vector2 ScreenCenter = new Vector2(VisibleRect.Size.x/2f, VisibleRect.Size.y/2f);
        Vector2 Offset = Vector2.Up;
        float Angle = NG.RandfRange(0f,360f);
        float Length = NG.RandfRange(500f,1500f);
        Offset = Offset.Rotated(Angle).Normalized() * Length;

        Node2D N = EnemyObject.Instance() as Node2D;
        N.Position = ScreenCenter + Offset;
        GetTree().Root.GetChild(0).AddChild(N);
    }
}
