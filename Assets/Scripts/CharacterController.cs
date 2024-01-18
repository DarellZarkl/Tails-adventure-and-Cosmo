using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CharacterController : CharacterBody2D
{
	[Export]
	public float Speed = 300.0f;
	[Export]
	public float JumpVelocity = -400.0f;	
	[Export]
	public float FlyVelocity = -75.0f;

	[Export]
	public NodePath CharacterStateMachinePath;
	[Export]
	public NodePath CharacterStrategyPath;

	public CharacterStateMachine characterStateMachine{get;set;}
	public ICharacterStrategy CharacterStrategy{get;set;}
	
	public Vector2 UpdateVelocity;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        base._Ready();
		this.characterStateMachine = GetNode<CharacterStateMachine>(CharacterStateMachinePath);
		this.CharacterStrategy = GetNode<ICharacterStrategy>(CharacterStrategyPath);
		CharacterStrategy.InitializeStates(this);
		GD.Print("strategy : "+this.CharacterStrategy.ToString());
		GD.Print(characterStateMachine.GetType().Name);
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
}


public interface ICharacterStrategy{
	void HandleIdleState(CharacterController characterController, CharacterState state);
	void HandleWalkingState(CharacterController characterController, CharacterState state);
	void HandleFlyingState(CharacterController characterController, CharacterState state);
	void HandleJumpingState(CharacterController characterController, CharacterState state);
	void HandleFallingState(CharacterController characterController, CharacterState state);
	double Delta{get;set;}
	void InitializeStates(CharacterController characterController);
	void Jump(CharacterController currentController);
	void Grab();
	void Fly(CharacterController currentController);
	void Moving(CharacterController currentController);
	void Falling(CharacterController currentController);
	void HandleAnimationDirection(Sprite2D sprite);
}
