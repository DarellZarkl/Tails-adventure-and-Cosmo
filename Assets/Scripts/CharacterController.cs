using CollisionsManagement;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CharacterController : CharacterBody2D, IOnCollisionEnter
{


	[Export]
	public NodePath CharacterStateMachinePath;
	[Export]
	public NodePath CharacterStrategyPath;

	public CharacterStateMachine characterStateMachine{get;set;}
	public ICharacterStrategy CharacterStrategy{get;set;}
	
	public Vector2 UpdateVelocity;

	public Vector2 SavedPosition{get;set;}

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _EnterTree()
	{
		base._EnterTree();
		Collisionhandler.Instance.AddCollisionBody(this,CollisionGroups.DeadZone);
		GD.Print("registered in handler");
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		Collisionhandler.Instance.RemoveCollisionBody(this,CollisionGroups.DeadZone);
	}
	public override void _Ready()
	{
		base._Ready();
		this.characterStateMachine = GetNode<CharacterStateMachine>(CharacterStateMachinePath);
		this.CharacterStrategy = GetNode<ICharacterStrategy>(CharacterStrategyPath);
		CharacterStrategy.InitializeStates(this);
		GD.Print("strategy : "+this.CharacterStrategy.ToString());
		GD.Print(characterStateMachine.GetType().Name);
		this.SavePosition();
	}
	public void SavePosition(){
		this.SavedPosition = this.GlobalPosition;
	}
	public void Reset(){
		this.CharacterStrategy.Die(this,this.SavedPosition);
	}
	public List<Node> GetChildStates(){
		return this.characterStateMachine.GetChildren().ToList();
	} 
	public override void _PhysicsProcess(double delta)
	{
		CharacterStrategy.Delta = delta;
		characterStateMachine.ExecuteStateLogic();
		Velocity = UpdateVelocity;
		MoveAndSlide();
	}

	public void OnEnterArea(Node2D area)
	{
		if(area==this){
	   GD.Print($"DEADZONE ENTERED : ",area.Name);
	   this.Reset();
		}
		else{
			GD.Print("wut?");
		}
	}

}


public interface ICharacterStrategy{
	void HandleIdleState(CharacterController characterController, CharacterState state);
	void HandleWalkingState(CharacterController characterController, CharacterState state);
	void HandleFlyingState(CharacterController characterController, CharacterState state);
	void HandleJumpingState(CharacterController characterController, CharacterState state);
	void HandleFallingState(CharacterController characterController, CharacterState state);
	double Delta{get;set;}
	void InitializeStates(CharacterController characterController);
	void HandleAnimationDirection(Sprite2D sprite);
	void Die(CharacterController characterController,Vector2 SavedPosition);
}
