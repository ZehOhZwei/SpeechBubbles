using Godot;
using System;

public partial class BubbleObjectSlingshot : RigidBody2D
{
    private Vector2 pivotPosition;
    private Vector2 positionOffset;
    private Vector2 startingPosition;

    private bool dragging;
    private bool draggable = false;
    private bool dropped;

    private Area2D area;

    [Export]
    public int value = 1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Freeze = true;
        startingPosition = Position;

        area = GetNode<Area2D>("Area2D");

        area.MouseEntered += () => OnMouseEntered();
        area.MouseExited += () => OnMouseExited();
        area.AreaEntered += (area) => OnAreaEntered(area);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton)
        {
            if (eventMouseButton.Pressed && Position == startingPosition && draggable)
            {
                pivotPosition = Position;
                positionOffset = Position - eventMouseButton.Position;
                dragging = true;
                Freeze = true;
            }
            if (!eventMouseButton.Pressed && dragging)
            {
                Freeze = false;
                dragging = false;
                draggable = false;
                ApplyImpulse((pivotPosition - Position) * 11);

            }
            if (dropped)
            {
                dropped = false;
                Position = startingPosition;
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (dragging)
        {
            Freeze = true;
            if ((GetViewport().GetMousePosition() + positionOffset - pivotPosition).Length() < 100)
            {
                GD.Print("Dragging in Bounds");
                SetPosition(GetViewport().GetMousePosition() + positionOffset);
            }
            else
            {
                GD.Print("Dragging out of Bounds");
                SetPosition(((GetViewport().GetMousePosition() + positionOffset - pivotPosition).Normalized() * 101 + pivotPosition));
            }
            Rotation = (GetViewport().GetMousePosition() + positionOffset - pivotPosition).Angle();
        }
        var screenSize = GetViewportRect().Size;
        if (Position.X < 0 || Position.Y < 0 || Position.X > screenSize.X || Position.Y > screenSize.Y)
        {
            GD.Print("Reset");
            Position = startingPosition;
            Freeze = true;
        }
        if (dropped)
        {
            if (!Freeze)
            {
                Rotation = 0;
                Freeze = true;
            }
        }
        base._PhysicsProcess(delta);
    }
    private void OnMouseEntered()
    {
        Scale *= 1.05f;
        draggable = true;
    }

    private void OnMouseExited()
    {
        Scale = (Scale / 105) * 100;
        draggable = false;
    }

    private void OnAreaEntered(Area2D area)
    {
        dropped = true;
        Position = area.Position;
    }

}
