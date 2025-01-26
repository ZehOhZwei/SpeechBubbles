using System;
using Godot;
public partial class BubbleObjectFleeing : BubbleObject
{
    public int fleeingCounter = 4;
    private bool moving = false;
    private int movingCounter = 0;
    private Vector2 direction;
    private Vector2 target;

    private Action mouseEnteredFleeing;

    public override void _Ready()
    {
        base._Ready();
        area = GetNode<Area2D>("Area2D");

        area.MouseEntered += mouseEnteredFleeing = () => { MouseEnteredFleeing(); };
        GD.Print("Ready");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (dragging)
        {
            SetPosition(GetViewport().GetMousePosition() + positionOffset);
        }
        if (moving)
        {
            if (Position != target)
            {
                Position = Position.MoveToward(target, 10);
            }
            else
            {
                moving = false;
            }
        }
    }


    private void MouseEnteredFleeing()
    {
        var rand = new Random();
        if (fleeingCounter > 0 && !moving)
        {
            fleeingCounter--;
            var screenSize = GetViewportRect().Size;
            direction = new Vector2((float)rand.NextDouble() * 2 - 1, (float)rand.NextDouble() * 2 - 1);
            target = Position + (direction.Normalized() * 200);
            while (target.X < 20 || target.X > screenSize.X - 20 || target.Y < 20 || target.Y > screenSize.Y - 20)
            {
                direction = new Vector2((float)rand.NextDouble() * 2 - 1, (float)rand.NextDouble() * 2 - 1);
                target = Position + (direction.Normalized() * 200);
            }
            moving = true;
            GD.Print("Current Position: " + Position, "Direction: " + direction, "Target Position: " + target);
        }
        else if (fleeingCounter == 0 && !moving)
        {
            area.MouseEntered -= mouseEnteredFleeing;
            area.MouseEntered += () => OnMouseEntered();
            area.MouseExited += () => OnMouseExited();
            area.AreaEntered += (area) => OnAreaEntered(area);
            area.AreaExited += (area) => OnAreaExited();
        }
        GD.Print();
    }
}

