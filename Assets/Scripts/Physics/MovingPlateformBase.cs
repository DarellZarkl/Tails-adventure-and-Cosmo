using Godot;
using System;
using System.IO;

public partial class MovingPlateformBase : Path2D
{

[Export]
public bool Loop{get;set;}
[Export]
public float speed{get;set;} = 2;
[Export]
public float SpeedScale {get;set;} = 1;
[Export]
public NodePath PathFollow2DPath{get;set;}
[Export]
public NodePath AnimationPlayerPath{get;set;}
[Export]
public bool Freeze{get;set;} = false;


public PathFollow2D PathFollow2D{get;set;}
public AnimationPlayer AnimationPlayer{get;set;}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PathFollow2D = GetNode<PathFollow2D>(PathFollow2DPath);
		AnimationPlayer = GetNode<AnimationPlayer>(AnimationPlayerPath);
		if(!Loop){
			this.AnimationPlayer.Play("Moving_Path");
			this.AnimationPlayer.SpeedScale = SpeedScale;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(!Freeze){
			this.PathFollow2D.Progress += speed;
		}		
	}
}
