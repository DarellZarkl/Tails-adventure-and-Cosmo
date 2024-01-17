using Godot;
using System;

public partial class Camera2D : Godot.Camera2D
{
	[Export]
	public NodePath CharacterPath {get;set;}
	[Export]
	public float pixelLerpFactor{get;set;}
	
	CharacterBody2D _player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print(CharacterPath.ToString());
		_player = GetNode<CharacterBody2D>(CharacterPath);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		PixelPerfectCameraFollow();
	}

	private void PixelPerfectCameraFollow(){
		var target = _player.GlobalPosition;
		var target_pos_x = (int)(Mathf.Lerp(this.GlobalPosition.X,target.X,0.2f));
		var target_pos_y = (int)(Mathf.Lerp(this.GlobalPosition.Y,target.Y,0.2f));
		this.GlobalPosition = new Vector2(target_pos_x,target_pos_y);
	}
}
