using Godot;
using System;



public partial class BubbleObject : Node2D
{
	public Area2D area;
	public AudioStreamPlayer sfxClick; 

	protected Vector2 positionOffset;

	protected bool dragging = false;
	protected bool draggable = false;
	protected bool isInsideDropable = false;
	protected Node2D bodyRef;

    [Export]
    public int value = 1;


    public override void _Ready()
	{
		area = GetNode<Area2D>("Area2D");
		sfxClick = GetNode<AudioStreamPlayer>("sfxClick");
		area.MouseExited += () => OnMouseExited();
		area.MouseEntered += () => OnMouseEntered();
		area.AreaEntered += (area) => OnAreaEntered(area);
		area.AreaExited += (area) => OnAreaExited();
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
			if (isInsideDropable)
			{
                bodyRef.Set("value", value);
                var dropArea = (DropArea)bodyRef;
                dropArea.SetPlacedBubble(this);
                sfxClick.Play(); // Soundeffekt beim Ancklicken? der Speechbubble
				if(bodyRef is not null)
				{
					Position = bodyRef.Position;
				}
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

	protected void OnMouseEntered()
	{
		draggable = true;
		Scale = Scale * 1.05f;
		
	}

    protected void OnMouseExited()
	{
		draggable = false;
		Scale = (Scale / 105) * 100 ;
	}

    protected void OnAreaEntered(Area2D area)
	{
		isInsideDropable = true;
		bodyRef = area;
		GD.Print("Entered " + area.ToString());
	}

    protected void OnAreaExited()
	{
		isInsideDropable = false;
		bodyRef = null;
	}
}
