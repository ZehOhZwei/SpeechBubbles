using Godot;
using System;

public partial class BubbleObjectPetrified : BubbleObject
{
    public int clickCounter = 20;
    public Sprite2D sprite;

    public override void _Ready()
    {
        area = GetNode<Area2D>("Area2D");
        sfxClick = GetNode<AudioStreamPlayer>("sfxClick");
        sprite = GetNode<Sprite2D>("Sprite2D");
        area.MouseExited += () => OnMouseExited();
        area.MouseEntered += () => OnMouseEntered();
        area.AreaEntered += (area) => OnAreaEntered(area);
        area.AreaExited += (area) => OnAreaExited();
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton)
        {
            if (clickCounter > 0 && draggable)
            {

                if (clickCounter == 15)
                {
                    var img = (Texture2D)GD.Load("res://Bilder/SpeechBubbleBilder/Liebe2.png");
                    sprite.SetTexture(img);
                }
                if(clickCounter == 10)
                {
                    var img = (Texture2D)GD.Load("res://Bilder/SpeechBubbleBilder/Liebe3.png");
                    sprite.SetTexture(img);
                }                
                if (clickCounter == 5)
                {
                    var img = (Texture2D)GD.Load("res://Bilder/SpeechBubbleBilder/Liebe4.png");
                    sprite.SetTexture(img);
                }

                clickCounter--;
            }
            else
            {
                if(clickCounter == 0)
                {
                    var img = (Texture2D)GD.Load("res://Bilder/SpeechBubbleBilder/Liebe5.png");
                    sprite.SetTexture(img);
                }
                if (draggable)
                {
                    positionOffset = Position - eventMouseButton.Position;
                    dragging = eventMouseButton.Pressed;

                }
                if (isInsideDropable)
                {
                    sfxClick.Play(); // Soundeffekt beim Ancklicken? der Speechbubble
                    Position = bodyRef.Position;
                }
            }
        }
    }
}
