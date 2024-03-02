using GameTriggers;
using Godot;
using System;

public partial class Door : Triggerable
{
	[Export]
	public NodePath AnimationPlayerPath{get;set;}
	private AnimationPlayer _player;
	public AnimationPlayer AnimationPlayer{
		get{
			if(_player==null){
				_player = GetNode<AnimationPlayer>(AnimationPlayerPath);
			}
			return _player;
		}
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AnimationPlayer.Play("doorAnimation"); 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
