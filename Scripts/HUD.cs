using Godot;
using System;

public class HUD : CanvasLayer
{
    [Signal]
    public delegate void StartGame();

    [Export]
    public NodePath MessagePath,MessageTimerPath,StartButtonPath, ScorePath;
    private Label _message, _score;
    private Timer _messageTimer;
    private Button _startButton;

    public override void _EnterTree()
    {
        _message=GetNode<Label>(MessagePath);
        _score=GetNode<Label>(ScorePath);
        _messageTimer=GetNode<Timer>(MessageTimerPath);
        _startButton=GetNode<Button>(StartButtonPath);
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    public void ShowMessage(string text)
    {
        _message.Text = text;
        _message.Show();
        _messageTimer.Start();
    }
    async public void ShowGameOver()
    {
        ShowMessage("Game Over");

        await ToSignal(_messageTimer, "timeout");
        _message.Text = "Dodge the\nCreeps!";
        _message.Show();

        await ToSignal(GetTree().CreateTimer(1), "timeout");
        _startButton.Show();
    }
    public void UpdateScore(int score)
    {
        _score.Text = score.ToString();
    }

    public void OnStartButtonPressed()
    {
        _startButton.Hide();
        EmitSignal("StartGame");
    }

    public void OnMessageTimerTimeout()
    {
        _message.Hide();
    }
}
