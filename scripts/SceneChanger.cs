using Godot;
using System;

public class SceneChanger : Control
{
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      if(Input.IsActionJustPressed("Fire"))
        GetTree().ChangeScene("res://scenes/Level_01.tscn"); 
  }
}
