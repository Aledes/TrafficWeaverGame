using Godot;
using System;

public class HUD : CanvasLayer
{
    [Signal]
    public delegate void StartGame();

    private Timer messageTimer;
    private Label messageLabel;
    private Button startButton;
    private Label scoreLabel;

    public override void _Ready()
    {
        messageTimer = GetNode("MessageTimer") as Timer;
        messageLabel = GetNode("MessageLabel") as Label;
        startButton = GetNode("StartButton") as Button;
        scoreLabel = GetNode("ScoreLabel") as Label;
    }

    public void ShowMessage(string text)
    {
        messageLabel.Text = text;
        messageLabel.Show();
        messageTimer.Start();
    }

    async public void ShowGameOver()
    {
        ShowMessage("GG");
        await ToSignal(messageTimer, "timeout");
        messageLabel.Text = "Weave!";
        messageLabel.Show();
        startButton.Show();
    }

    public void UpdateScore(int score)
    {
        scoreLabel.Text = score.ToString();
    }
    
    public void OnMessageTimerTimeout()
    {
        messageLabel.Hide();
    }

    public void OnStartButtonPressed()
    {
        startButton.Hide();
        EmitSignal("StartGame");
    }
}
