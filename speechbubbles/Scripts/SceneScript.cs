using Godot;
using System;

public partial class SceneScript : Node2D
{
	public Timer timer;
	public DropArea dropArea;

	[Export]
	public int nextSceneNumber = 2;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		dropArea = GetNode<DropArea>("Node2D");
		dropArea.BubbleDropped += () => OnBubbleDropped();
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
		GetTree().ReloadCurrentScene();
		GD.Print("Scene Timeout");
	}

	private void OnBubbleDropped()
	{
		GetTree().ChangeSceneToFile("Scene" + nextSceneNumber + ".tscn");
	}
}
