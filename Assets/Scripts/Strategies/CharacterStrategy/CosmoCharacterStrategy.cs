using System;
using System.Collections.Generic;
using Godot;

public partial class CosmoCharacterStrategy : Node, ICharacterStrategy{
    public void HandleIdleState(CharacterController characterController, CharacterState state){

    }
	public void HandleWalkingState(CharacterController characterController, CharacterState state){

    }
	public void HandleFlyingState(CharacterController characterController, CharacterState state){

    }
	public void HandleJumpingState(CharacterController characterController, CharacterState state){

    }
	public void HandleFallingState(CharacterController characterController, CharacterState state){

    }
	public double Delta{get;set;}
	public void InitializeStates(CharacterController characterController){

    }
	public void HandleAnimationDirection(Sprite2D sprite){

    }
}