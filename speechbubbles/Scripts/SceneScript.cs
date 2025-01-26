using Godot;
using System;

public partial class SceneScript : Node2D
{
	public Timer timer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
        timer.Timeout += () => OnTimeout();
	}

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseButton eventMouseButton)
		{
			if (timer.IsStopped())
			{
				timer.Start(10);
			}
		}
    }

	private void OnTimeout() 
	{
		GD.Print("Scene Timeout");
	}
}
