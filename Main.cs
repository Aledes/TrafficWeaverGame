using Godot;
using System;

public class Main : Node
{
    [Export]
    public PackedScene Traffic;

    
    private Player player;
    private HUD hud;
    public int score = 0;
    private Random rand = new Random();

    private Timer trafficTimer;
    private Timer scoreTimer;
    private Timer startTimer;
    private Position2D startPosition;
    private PathFollow2D trafficSpawnLocation;
    private RigidBody2D trafficInstance;

    public override void _Ready()
    {
        // Timers
        trafficTimer = GetNode("TrafficTimer") as Timer;
        scoreTimer = GetNode("ScoreTimer") as Timer;
        player = GetNode("Player") as Player;
        startTimer = GetNode("StartTimer") as Timer;

        startPosition = GetNode("StartPosition") as Position2D;
        trafficSpawnLocation = GetNode("TrafficPath/TrafficSpawnLocation") as PathFollow2D;
        hud = GetNode("HUD") as HUD;
        player.Hide();
    }

    public void GameOver()
    {
        scoreTimer.Stop();
        trafficTimer.Stop();
        hud.ShowGameOver();
    }

    public void NewGame()
    {
        score = 0;
        player.Start(startPosition.Position);
        startTimer.Start();
        hud.UpdateScore(score);
        hud.ShowMessage("Get ready");
    }

    private float RandRand(float min, float max)
    {
        return (float) (rand.NextDouble()* (max - min) + min);
    }

    public void OnStartTimerTimeout()
    {
        trafficTimer.Start();
        scoreTimer.Start();
    }

    public void OnScoreTimerTimeout()
    {
        score += 1;
        hud.UpdateScore(score);
    }

    public void OnTrafficTimerTimeout()
    {
        // CHANGE
        // Spawn location choice
        trafficSpawnLocation.SetOffset(rand.Next());

        // Create traffic instance and add to scene
        trafficInstance = Traffic.Instance() as Traffic;
        AddChild(trafficInstance);

        // Set traffic's direction perp to path direction
        var direction = trafficSpawnLocation.Rotation + Mathf.Pi / 2;

        // Set traffic's position to  a random location
        trafficInstance.Position = trafficSpawnLocation.Position;

        // Add randomness to direction
        direction += RandRand(-Mathf.Pi/4, Mathf.Pi/4);
        trafficInstance.Rotation = direction;

        // Choose velocity
        trafficInstance.SetLinearVelocity(new Vector2(RandRand(150f, 250f), 0).Rotated(direction));

    }
}
