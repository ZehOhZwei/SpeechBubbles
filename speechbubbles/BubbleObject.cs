using Godot;
using System;

public partial class BubbleObject : Node2D
{
	public Area2D area;

	private Vector2 positionOffset;

	private bool dragging = false;
	private bool draggable = false;

	public override void _Ready()
	{
		area = GetNode<Area2D>("Area2D");
		area.MouseExited += () => MouseExited();
		area.MouseEntered += () => MouseEntered();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			if (draggable)
			{
				positionOffset = Position - eventMouseButton.Position;
				dragging = eventMouseButton.Pressed;
			}
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		if (dragging)
		{
			SetPosition(GetViewport().GetMousePosition() + positionOffset);
		}
		base._PhysicsProcess(delta);
	}
	private void MouseEntered()
	{
		draggable = true;
		Scale = new Vector2(1.05f, 1.05f);

	}

	private void MouseExited()
	{
		draggable = false;
		Scale = new Vector2(1, 1);
	}
}
