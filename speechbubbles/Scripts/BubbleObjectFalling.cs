using Godot;
using System;

public partial class BubbleObjectFalling : RigidBody2D
{

    protected Vector2 positionOffset;

    private int timerLength = 1;

    protected bool dragging = false;
    protected bool draggable = false;
    protected bool isInsideDropable = false;
    protected bool dropped = false;
    protected Node2D bodyRef;
    protected Area2D area;
    private Timer timer;

    [Export]
    public int value = 1;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        timer.Timeout += () => OnTimeout();
        area = GetNode<Area2D>("Area2D");
        MouseEntered += OnMouseEntered;
        MouseExited += () => OnMouseExited();
        area.AreaEntered += (area) => OnBodyEntered(area);
        area.AreaExited += (area) => OnBodyExited();
    }


    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton)
        {
            if (draggable)
            {
                if (eventMouseButton.Pressed)
                {
                    timer.Start(timerLength);
                }
                positionOffset = Position - eventMouseButton.Position;
                dragging = eventMouseButton.Pressed;
            }
            if (isInsideDropable)
            {
                Position = bodyRef.Position;
                Freeze = true;
                dropped = true;
                Rotation = 0;
                bodyRef.Set("value", value);
                var dropArea = (DropArea)bodyRef;
                dropArea.SetPlacedBubble(this);
            }
            if (!isInsideDropable && !eventMouseButton.Pressed)
            {
                Freeze = false;
                dropped = false;
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (dragging)
        {
            Freeze = true;
            SetPosition(GetViewport().GetMousePosition() + positionOffset);
        }
        else if (!dropped)
        {
            Freeze = false; 
        }
        base._PhysicsProcess(delta);
    }
    private void OnTimeout()
    {
        GD.Print("Timeout");
        if(dragging)
        {
            dragging = false;
            Freeze = false;
        }
    }

    protected void OnMouseEntered()
    {
        GD.Print("MouseEntered");
        draggable = true;
        Scale = Scale * 1.05f;
    }

    protected void OnMouseExited()
    {
        GD.Print("MouseExited");
        draggable = false;
        dragging = false;
        ConstantForce = new Vector2(0, 0);
        Scale = (Scale / 105) * 100;
    }
    protected void OnBodyEntered(Node2D body)
    {
        isInsideDropable = true;
        bodyRef = body;
        GD.Print("Entered " + body.ToString());
    }

    protected void OnBodyExited()
    {
        isInsideDropable = false;
        bodyRef = null;
    }
}