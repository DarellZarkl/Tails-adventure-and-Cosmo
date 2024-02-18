using System;
using System.Collections.Generic;
using Godot;

public partial class TailsCharacterStrategy : Node, ICharacterStrategy{

#region StatePath

#endregion

#region States
	public IdleState IdleState{get;set;}
	public MovingState MovingState{get;set;}
	public JumpState JumpingState{get;set;}
	public FlyingState FlyingState{get;set;}
	public FallingState FallingState{get;set;}
#endregion

	//export
	[Export(PropertyHint.None,"Coyote time")]
	public double CoyoteTime{get;set;}
	[Export(PropertyHint.None,"Max flying time")]
	public double TailsMaxFlyTime{get;set;}
	[Export(PropertyHint.None,"Number of jump")]
	public int MaxJumpNumber{get;set;} = 1;
	[Export(PropertyHint.None,"Number of Fly")]
	public int MaxFlyNumber{get;set;} = 3;
	[Export(PropertyHint.None,"Jump time limit")]
	public double jumptimeLimit{get;set;}
	[Export]
	public float Speed = 300.0f;
	[Export]
	public float JumpVelocity = 400.0f;	
	[Export]
	public float FlyVelocity = 75.0f;
	[Export(PropertyHint.Range,"0,1")]
	public float FlyingSpeedFactor = 0.35f;

	//private
	double currentCoyoteTimer;
	private double TailsCurrentFly{get;set;}
	private int CurrentFlyNumber{get;set;}
	private int jumpnumber{get;set;}
	private int CurrentJumpNumber{get;set;}
	private double currentjumpTime{get;set;}
	public bool FlipH {get;set;}

	//public properties
	public double Delta {get;set; }


	public override void _Ready()
	{
		//First solution : PackedScene
		//Second Solution : Dictionary on the characterController at Ready.
		//Two Way Request function
		base._Ready();
		//IdleState = GetNode<IdleState>(IdleStatePath);
		//MovingState = GetNode<MovingState>(MovingStatePath);
		//JumpingState = GetNode<JumpState>(JumpingStatePath);
		//FlyingState = GetNode<FlyingState>(FlyingStatePath);
		//FallingState = GetNode<FallingState>(FallingStatePath);
	}

	public void InitializeStates(CharacterController characterController){
		List<Node> nodes = characterController.GetChildStates();
		foreach(Node node in nodes){
			GD.Print("Loading state : "+node?.GetType()?.Name);
			if(node is IdleState){
				IdleState = node as IdleState;
				GD.Print("loaded as : IdleState");
			}
			else if(node is MovingState){
				MovingState = node as MovingState;
				GD.Print("loaded as : MovingState");
			}
			else if(node is JumpState){
				JumpingState = node as JumpState;
				GD.Print("loaded as : JumpState");
			}
			else if(node is FlyingState){
				FlyingState = node as FlyingState;
				GD.Print("loaded as : FlyingState");
			}
			else if(node is FallingState){
				FallingState = node as FallingState;
				GD.Print("loaded as : FallingState");
			}
		}
	}
	public void Jump(CharacterController currentController ){
			CheckAxisFlip();
			if(InputFrameData.JustPressedJump){
				if(!currentController.IsOnFloor() && currentCoyoteTimer<CoyoteTime && CurrentJumpNumber<MaxJumpNumber){
					CurrentJumpNumber+=1;
					currentjumpTime+=Delta;
					currentController.UpdateVelocity.Y = -JumpVelocity;
				}
				else if(currentController.IsOnFloor()){
					CurrentJumpNumber+=1;
					currentjumpTime+=Delta;
					currentController.UpdateVelocity.Y = -JumpVelocity;
				}
			}
			else if(!InputFrameData.JustPressedJump
			&& InputFrameData.PressingJump
			&& !currentController.IsOnFloor() 
			&& jumptimeLimit>=currentjumpTime){
				currentjumpTime+=Delta;
				currentController.UpdateVelocity.Y = -JumpVelocity;
			}
	}
	public void Fly(CharacterController currentController){
		
		if(InputFrameData.JustPressedJump
		&& (!currentController.IsOnFloor() || currentController.characterStateMachine.CurrentState==FallingState)
		&& TailsCurrentFly<=TailsMaxFlyTime 
		&& CurrentFlyNumber<MaxFlyNumber){
			TailsCurrentFly+= Delta;
			CurrentFlyNumber++;
			currentController.UpdateVelocity.Y = -FlyVelocity;
			
		}
		else if(currentController.characterStateMachine.CurrentState == FlyingState
		&& !InputFrameData.JustPressedJump
		&& InputFrameData.PressingJump 
		&& (!currentController.IsOnFloor() || currentController.characterStateMachine.CurrentState==FallingState)
		&& TailsCurrentFly<=TailsMaxFlyTime
		&& CurrentFlyNumber<MaxFlyNumber){
			TailsCurrentFly+= Delta;
			currentController.UpdateVelocity.Y = -FlyVelocity;
			GD.Print($"jump number(fly) :{jumpnumber}");
		}
	}
	public void Grab(){}
	public void CheckAxisFlip(){
		var axisval = InputFrameData.AxisX; 
		if(axisval==-1){
			FlipH = true;
		}
		else if(axisval==1){
			FlipH=false;
		}
	}
	public void Moving(CharacterController currentController){
		CheckAxisFlip();
		 Vector2 direction = InputFrameData.Direction;
			if (direction.X != 0)
			{
				float factor = currentController.characterStateMachine.CurrentState==this.FlyingState?FlyingSpeedFactor:1f;
				currentController.UpdateVelocity.X = direction.X * Speed * factor;
			}
			else
			{
				currentController.UpdateVelocity.X = Mathf.MoveToward(currentController.UpdateVelocity.X, 0, Speed);
				//GD.Print("Updating state to : "+idleState?.GetType()?.Name);
			}
			if(!currentController.IsOnFloor()){
				currentCoyoteTimer+=Delta;
			}
	}

	public void Die(CharacterController characterController,Vector2 SavedPosition){
		characterController.GlobalPosition = SavedPosition;
		this.ResetOnGroundtouch();
		this.HardReset(characterController);
	}

	public void Falling(CharacterController currentController)
	{
		CheckAxisFlip();
		if(currentController.characterStateMachine.CurrentState==this.MovingState 
		&& !currentController.IsOnFloor() 
		&&currentCoyoteTimer>=CoyoteTime){
			currentController.UpdateVelocity.Y += currentController.gravity * (float)Delta;
		}
		else if(currentController.characterStateMachine.CurrentState!=this.MovingState && !currentController.IsOnFloor()){
			currentController.UpdateVelocity.Y += currentController.gravity * (float)Delta;
		}
		if(!currentController.IsOnFloor()&&currentCoyoteTimer<CoyoteTime){
			currentCoyoteTimer+=Delta;
		}
	}

	public void HandleAnimationDirection(Sprite2D sprite){
		sprite.FlipH = FlipH;
	}

	public void HandleIdleState(CharacterController characterController, CharacterState state)
	{
		this.Moving(characterController);
		this.Falling(characterController);
		this.Jump(characterController);
		this.HandleAnimationDirection(state.CharacterSprite);
		this.HandleIdleStateTransistions(characterController,state);
	}

	public void HandleWalkingState(CharacterController characterController, CharacterState state)
	{
		this.Moving(characterController);
		this.Jump(characterController);
		this.Falling(characterController);
		this.HandleAnimationDirection(state.CharacterSprite);
		this.HandleMovingStateTransitions(characterController,state);
	}

	public void HandleFlyingState(CharacterController characterController, CharacterState state)
	{
		this.Fly(characterController);
		this.Moving(characterController);
		this.HandleAnimationDirection(state.CharacterSprite);
		this.HandleFlyingStateTransitions(characterController,state);
	}

	public void HandleJumpingState(CharacterController characterController, CharacterState state)
	{
		this.Jump(characterController);
		this.Moving(characterController);
		this.Falling(characterController);
		this.HandleAnimationDirection(state.CharacterSprite);
		this.HandleJumpingStateTransitions(characterController,state);
	}

	public void HandleFallingState(CharacterController characterController, CharacterState state)
	{
		this.Falling(characterController);
		this.Moving(characterController);
		this.Fly(characterController);
		this.HandleAnimationDirection(state.CharacterSprite);
		this.HandleFallingStateTransitions(characterController,state);
	}


	public void ResetOnGroundtouch(){
		currentjumpTime =0;
		currentCoyoteTimer=0;
		TailsCurrentFly=0;
		CurrentJumpNumber=0;
		CurrentFlyNumber=0;
	}
	public void HardReset(CharacterController characterController){
		this.ResetOnGroundtouch();
		characterController.UpdateVelocity = Vector2.Zero;
	}

	public void HandleIdleStateTransistions(CharacterController currentController, CharacterState state){
		Vector2 direction = InputFrameData.Direction;
			if(InputFrameData.JustPressedJump){
				currentController.characterStateMachine.UpdateState(JumpingState);
			}
			else if (direction.X != 0)
			{
				if(currentController.IsOnFloor() || currentCoyoteTimer<=CoyoteTime){
					currentController.characterStateMachine.UpdateState(MovingState);
				}
				else if(direction.Y!=0 && !InputFrameData.PressingJump && currentCoyoteTimer>CoyoteTime){
					currentController.characterStateMachine.UpdateState(MovingState);
				}
			}       
	}
	public void HandleMovingStateTransitions(CharacterController currentController, CharacterState state){
		Vector2 direction = InputFrameData.Direction;
		if(InputFrameData.JustPressedJump){
			currentController.characterStateMachine.UpdateState(JumpingState);
		}
		if(direction.X==0 && currentController.IsOnFloor()){
			currentController.characterStateMachine.UpdateState(IdleState);
		}
		else if(direction.X!=0 && currentController.IsOnFloor()){}
		else if(!currentController.IsOnFloor() && currentCoyoteTimer>CoyoteTime){
			currentController.characterStateMachine.UpdateState(FallingState);
		}
	}
	public void HandleFallingStateTransitions(CharacterController currentController, CharacterState state){
		 Vector2 direction = InputFrameData.Direction;
		 if(currentController.IsOnFloor()&& direction.X==0){
			this.ResetOnGroundtouch();
			GD.Print("reset!");
			currentController.characterStateMachine.UpdateState(IdleState);
		}
		else if(currentController.IsOnFloor()&& direction.X !=0){
			this.ResetOnGroundtouch();
			currentController.characterStateMachine.UpdateState(MovingState);
		}
		else if(InputFrameData.JustPressedJump
		&& TailsCurrentFly<=TailsMaxFlyTime 
		&& CurrentFlyNumber<MaxFlyNumber){
			currentController.characterStateMachine.UpdateState(FlyingState);
		}
	}
	public void HandleJumpingStateTransitions(CharacterController currentController, CharacterState state){
			if((InputFrameData.ReleaseJump || currentjumpTime>=jumptimeLimit) && !currentController.IsOnFloor()){
				currentController.characterStateMachine.UpdateState(FallingState);
			}
	}
	
	public void HandleFlyingStateTransitions(CharacterController currentController, CharacterState state){
		if(InputFrameData.ReleaseJump || TailsCurrentFly>TailsMaxFlyTime){
			if(currentController.characterStateMachine.CurrentState==FlyingState)
				currentController.characterStateMachine.UpdateState(FallingState);
		}
	}
}
