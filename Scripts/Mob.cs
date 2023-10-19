using Godot;
using System;

public class Mob : RigidBody2D
{
    // Declare member variables here. Examples:
    [Export]
    public NodePath AnimatedSpritePath; // NodePath variable to store the path to the AnimatedSprite node.

    private AnimatedSprite _animatedSprite;
    // Called when the node enters the scene tree.
    public override void _EnterTree()
    {
        _animatedSprite = GetNode<AnimatedSprite>(AnimatedSpritePath);
    }  

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _animatedSprite.Playing = true;
        string[] mobTypes = _animatedSprite.Frames.GetAnimationNames();
        _animatedSprite.Animation = mobTypes[GD.Randi() % mobTypes.Length];
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    public void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }
}
