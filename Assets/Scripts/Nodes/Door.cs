using CollisionsManagement;
using GameTriggers;
using Godot;
using System;

public partial class Door : Triggerable,IOnCollisionEnter
{
	[Export]
	public NodePath AnimationPlayerPath{get;set;}
	[Export]
	public string CollisionGroupName{get;set;} ="Door001";
	private AnimationPlayer _player;
	private bool triggered = false;
	public AnimationPlayer AnimationPlayer{
		get{
			if(_player==null){
				_player = GetNode<AnimationPlayer>(AnimationPlayerPath);
			}
			return _player;
		}
	}
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		//AnimationPlayer.Play("doorAnimation"); 
	}
    public override void _EnterTree()
    {
		//AnimationPlayer.Play("Closed");
		GD.Print($"Current anim : {AnimationPlayer.CurrentAnimation}");
       	Collisionhandler.Instance.AddCollisionBody(this,CollisionGroupName);
		GD.Print($"Door connected to {CollisionGroupName}");
    }

	public override void ExecuteAction(){
		if(!triggered){
			GD.Print($"{CollisionGroupName} triggered !");
			AnimationPlayer.Play("doorAnimation");
			triggered = true;
		}
	}
	public void reverse(){
		if(triggered){
			GD.Print($"{CollisionGroupName} reverse !");
			AnimationPlayer.PlayBackwards();
			triggered = false;
		}
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    public void OnEnterArea(Node2D area)
    {
       this.ExecuteAction();
    }

    public void OnExitArea(Node2D area)
    {
        this.reverse();
    }
}
