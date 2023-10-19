using Godot;
using System;

public class Player : Area2D
{
    // Declare member variables here. Examples:
    [Signal]
    public delegate void Hit();
    [Export]
    public int Speed = 400; // How fast the player will move (pixels/sec).

    public Vector2 ScreenSize; // Size of the game window.
    [Export]
    public NodePath AnimatedSpritePath; // NodePath variable to store the path to the AnimatedSprite node.

    private AnimatedSprite _animatedSprite;

    [Export]
    public NodePath CollisionShape2DPath;

    private CollisionShape2D _collisionShape2D;
    
    // Called when the node enters the scene tree.
    public override void _EnterTree()
    {
        _animatedSprite = GetNode<AnimatedSprite>(AnimatedSpritePath);
        _collisionShape2D=GetNode<CollisionShape2D>(CollisionShape2DPath);
    }   
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size;
        Hide();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var velocity = Vector2.Zero; // The player's movement vector.

        if (Input.IsActionPressed("move_right"))
        {
            velocity.x += 1;
        }

        if (Input.IsActionPressed("move_left"))
        {
            velocity.x -= 1;
        }

        if (Input.IsActionPressed("move_down"))
        {
            velocity.y += 1;
        }

        if (Input.IsActionPressed("move_up"))
        {
            velocity.y -= 1;
        }

        

        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
            _animatedSprite.Play();
        }
        else
        {
            _animatedSprite.Stop();
        }

        Position += velocity * delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.x, 0, ScreenSize.x),
            y: Mathf.Clamp(Position.y, 0, ScreenSize.y)
        );

        if (velocity.x != 0)
        {
            _animatedSprite.Animation = _animatedSprite.Frames.GetAnimationNames()[0];
            _animatedSprite.FlipV = false;
            _animatedSprite.FlipH = velocity.x < 0;
        }
        else if (velocity.y != 0)
        {
            _animatedSprite.Animation = _animatedSprite.Frames.GetAnimationNames()[1];
            _animatedSprite.FlipH = false;
            _animatedSprite.FlipV = velocity.y > 0;
        }
    }
    public void OnPlayerBodyEntered(PhysicsBody2D body)
{
    Hide(); // Player disappears after being hit.
    EmitSignal(nameof(Hit));
    // Must be deferred as we can't change physics properties on a physics callback.
    _collisionShape2D.SetDeferred("disabled", true);
}
    public void Start(Vector2 pos)
{
    Position = pos;
    Show();
    _collisionShape2D.Disabled=false;
}
}
