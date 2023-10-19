using Godot;
using System;

public class Main : Node
{
    // Declare member variables here. 
    [Export]
    public NodePath MobTimerPath, ScoreTimerPath, StartTimerPath, PlayerPath, StartPositionPath, MobSpawnLocationPath; 

    private Timer _mobTimer, _scoreTimer, _startTimer;
    private Player _player;
    private Position2D _startPosition; 
    private PathFollow2D _mobSpawnLocation;
#pragma warning disable 649
    // We assign this in the editor, so we don't need the warning about not being assigned.

    [Export]
    public PackedScene MobScene;
#pragma warning restore 649

    public int Score;
    // Called when the node enters the scene tree.
    public override void _EnterTree()
    {
        _mobTimer = GetNode<Timer>(MobTimerPath);
        _scoreTimer = GetNode<Timer>(ScoreTimerPath);
        _startTimer=GetNode<Timer>(StartTimerPath);
        _player=GetNode<Player>(PlayerPath);
        _startPosition=GetNode<Position2D>(StartPositionPath);
        _mobSpawnLocation=GetNode<PathFollow2D>(MobSpawnLocationPath);
    }  
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Randomize();
        NewGame();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void GameOver()
    {
        _mobTimer.Stop();
        _scoreTimer.Stop();
    }

    public void NewGame()
    {
        Score = 0;
        _player.Start(_startPosition.Position);
    _startTimer.Start();
    }

    #region ////////////////////-------------------- Timeout Functions --------------------////////////////////
    public void OnScoreTimerTimeout()
    {
        Score++;
    }
    public void OnStartTimerTimeout(){
        _mobTimer.Start();
        _scoreTimer.Start();
    }
    
    public void OnMobTimerTimeout()
    {
        // Create a new instance of the Mob scene.
        Mob mob = (Mob)MobScene.Instance();

        // Choose a random location on Path2D.
        _mobSpawnLocation.Offset = GD.Randi();

        // Set the mob's direction perpendicular to the path direction.
        float direction = _mobSpawnLocation.Rotation + Mathf.Pi / 2;

        // Set the mob's position to a random location.
        mob.Position = _mobSpawnLocation.Position;

        // Add some randomness to the direction.
        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mob.Rotation = direction;

        // Choose the velocity.
        var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
        mob.LinearVelocity = velocity.Rotated(direction);

        // Spawn the mob by adding it to the Main scene.
        AddChild(mob);
    }
    #endregion
}


