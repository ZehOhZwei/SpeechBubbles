using Godot;
using System;

public partial class DropArea : Area2D
{
	public int value = 0;
	public Node2D placedBubble;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (placedBubble != null)
		{
			GD.Print("Placed");
		}
	}

	public void SetPlacedBubble(Node2D node)
	{
		placedBubble = node;
	}
}
