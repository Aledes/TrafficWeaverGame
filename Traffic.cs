using Godot;
using System;

public class Traffic : RigidBody2D
{
    [Export]
    public int minSpeed = 150;

    [Export]
    public int maxSpeed = 250;

    private String[] _trafficTypes = {"fast", "average", "slow"};

    private Sprite sprite;

    public override void _Ready()
    {
        sprite = GetNode("Sprite") as Sprite;
        var randomTraffic = new Random();
    }

    public void OnVisibilityScreenExited()
    {
        QueueFree();
    }
}
