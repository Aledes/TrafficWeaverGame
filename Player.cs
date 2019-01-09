using Godot;
using System;

public class Player : Area2D
{
    [Signal]
    public delegate void Hit();

    [Export]
    public int speed = 0;
    private Vector2 _screenSize;
    private Vector2 _velocity;
    private CollisionShape2D collisionShape2D;

    public override void _Ready()
    {
        collisionShape2D = GetNode("CollisionShape2D") as CollisionShape2D;
        _screenSize = GetViewport().GetSize();
    }

   public override void _Process(float delta)
   {
       	_velocity = new Vector2(); // Player's movement vector
		if (Input.IsActionPressed("ui_right") || Input.IsKeyPressed((int)KeyList.D)) {
			_velocity.x += 1;
		}
		if (Input.IsActionPressed("ui_left") || Input.IsKeyPressed((int)KeyList.A)) {
			_velocity.x -= 1;
		}
		if (Input.IsActionPressed("ui_down") || Input.IsKeyPressed((int)KeyList.S)) {
			_velocity.y += 1;
		}
		if (Input.IsActionPressed("ui_up") || Input.IsKeyPressed((int)KeyList.W)) {
			_velocity.y -= 1;
		}
		
		var animatedSprite = GetNode("AnimatedSprite") as AnimatedSprite;
		if (_velocity.Length() > 0) {
			_velocity = _velocity.Normalized() * speed;
			animatedSprite.Play();
		} else {
			animatedSprite.Stop();
		}
		
		this.Position += _velocity * delta;
		var posx = Mathf.Clamp(this.Position.x, 0, _screenSize.x);
		var posy = Mathf.Clamp(this.Position.y, 0, _screenSize.y);
		this.SetPosition(new Vector2(posx, posy));
		
		if (_velocity.x > 0) {
			animatedSprite.Animation = "right";
		}
		if (_velocity.x < 0) {
			animatedSprite.Animation = "left";
		}
		else if (_velocity.x == 0) {
			animatedSprite.Animation = "forward";
		}
   }

   public void OnPlayerBodyEntered(Godot.Object body)
   {
       Hide();
       EmitSignal("Hit");
       collisionShape2D.Disabled = true;
   }

   public void Start(Vector2 pos)
   {
       Position = pos;

       Show();
       collisionShape2D.Disabled = false;
   }
}
