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
	public int MaxJumpNumber{get;set;} = 5;
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
                if(!currentController.IsOnFloor() && currentCoyoteTimer<CoyoteTime && CurrentJumpNumber<1){
                    CurrentJumpNumber+=1;
                    currentjumpTime+=Delta;
                    currentController.UpdateVelocity.Y = -JumpVelocity;
                    currentController.characterStateMachine.UpdateState(JumpingState);
                }
                else if(currentController.IsOnFloor()){
                    CurrentJumpNumber+=1;
                    currentjumpTime+=Delta;
                    currentController.UpdateVelocity.Y = -JumpVelocity;
                    if(currentController.characterStateMachine.CurrentState!=JumpingState)
                        currentController.characterStateMachine.UpdateState(JumpingState);
                }
            }
            else if(!InputFrameData.JustPressedJump
            && InputFrameData.PressingJump
            && !currentController.IsOnFloor() 
            && jumptimeLimit>=currentjumpTime){
                currentjumpTime+=Delta;
                currentController.UpdateVelocity.Y = -JumpVelocity;
                if(currentController.characterStateMachine.CurrentState!=JumpingState)
                        currentController.characterStateMachine.UpdateState(JumpingState);
            }
            if((InputFrameData.ReleaseJump || currentjumpTime>=jumptimeLimit) && !currentController.IsOnFloor()){
                currentController.characterStateMachine.UpdateState(FallingState);
            }
	}
    public void Fly(CharacterController currentController){
        if(InputFrameData.JustPressedJump
        && !currentController.IsOnFloor() 
        && CurrentJumpNumber>=1 
        && TailsCurrentFly<=TailsMaxFlyTime 
        && CurrentJumpNumber<=MaxJumpNumber){
            TailsCurrentFly+= Delta;
            CurrentJumpNumber+=1;
            currentController.UpdateVelocity.Y = -FlyVelocity;
            if(currentController.characterStateMachine.CurrentState==JumpingState 
            || currentController.characterStateMachine.CurrentState==FallingState
            || currentController.characterStateMachine.CurrentState == MovingState)
                currentController.characterStateMachine.UpdateState(FlyingState);
        }
        else if(!InputFrameData.JustPressedJump
        && InputFrameData.PressingJump 
        && !currentController.IsOnFloor() 
        && CurrentJumpNumber>=1  
        && TailsCurrentFly<=TailsMaxFlyTime  
        && CurrentJumpNumber<=MaxJumpNumber){
            TailsCurrentFly+= Delta;
            currentController.UpdateVelocity.Y = -FlyVelocity;
            GD.Print($"jump number(fly) :{jumpnumber}");
            if(currentController.characterStateMachine.CurrentState==JumpingState 
            || currentController.characterStateMachine.CurrentState==FallingState
            || currentController.characterStateMachine.CurrentState == MovingState)
                currentController.characterStateMachine.UpdateState(FlyingState);
        }
        else if(InputFrameData.ReleaseJump || TailsCurrentFly>TailsMaxFlyTime){
            if(currentController.characterStateMachine.CurrentState==FlyingState)
                currentController.characterStateMachine.UpdateState(FallingState);
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
                 if(currentController.characterStateMachine.CurrentState!=MovingState 
                 && currentController.characterStateMachine.CurrentState!=JumpingState 
                 && currentController.characterStateMachine.CurrentState!=FallingState
                 && currentController.characterStateMachine.CurrentState!=FlyingState)
                    currentController.characterStateMachine.UpdateState(MovingState);
		    }
		    else
		    {
		    	currentController.UpdateVelocity.X = Mathf.MoveToward(currentController.UpdateVelocity.X, 0, Speed);
                //GD.Print("Updating state to : "+idleState?.GetType()?.Name);
                if(currentController.characterStateMachine.CurrentState==MovingState)
                    currentController.characterStateMachine.UpdateState(IdleState);
		    }
            if(currentCoyoteTimer>CoyoteTime 
            && !currentController.IsOnFloor() 
            && currentController.characterStateMachine.CurrentState==MovingState){
                currentController.characterStateMachine.UpdateState(FallingState);
            }
            if(!currentController.IsOnFloor()){
                currentCoyoteTimer+=Delta;
            }
    }

    public void Falling(CharacterController currentController)
    {
        CheckAxisFlip();
        if(currentController.characterStateMachine.CurrentState==this.MovingState && !currentController.IsOnFloor() &&currentCoyoteTimer>=CoyoteTime){
			currentController.UpdateVelocity.Y += currentController.gravity * (float)Delta;
        }
        else if(currentController.characterStateMachine.CurrentState!=this.MovingState && !currentController.IsOnFloor()){
			currentController.UpdateVelocity.Y += currentController.gravity * (float)Delta;
        }
        if(!currentController.IsOnFloor()&&currentCoyoteTimer<CoyoteTime){
            currentCoyoteTimer+=Delta;
        }
        if(currentController.IsOnFloor() && currentController.characterStateMachine.CurrentState == FallingState){
            currentjumpTime =0;
            currentCoyoteTimer=0;
            TailsCurrentFly=0;
            CurrentJumpNumber=0;
            GD.Print("reset!");
            if(currentController.characterStateMachine.CurrentState!=IdleState 
            && currentController.characterStateMachine.CurrentState!=MovingState
            && currentController.characterStateMachine.CurrentState!=JumpingState)
                currentController.characterStateMachine.UpdateState(IdleState);
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
    }

    public void HandleWalkingState(CharacterController characterController, CharacterState state)
    {
        this.Moving(characterController);
        this.Jump(characterController);
        this.Falling(characterController);
        this.HandleAnimationDirection(state.CharacterSprite);
    }

    public void HandleFlyingState(CharacterController characterController, CharacterState state)
    {
        this.Fly(characterController);
        this.Moving(characterController);
        this.HandleAnimationDirection(state.CharacterSprite);
    }

    public void HandleJumpingState(CharacterController characterController, CharacterState state)
    {
        this.Jump(characterController);
        this.Moving(characterController);
        this.Falling(characterController);
        this.HandleAnimationDirection(state.CharacterSprite);
    }

    public void HandleFallingState(CharacterController characterController, CharacterState state)
    {
        this.Falling(characterController);
        this.Moving(characterController);
        this.Fly(characterController);
        this.HandleAnimationDirection(state.CharacterSprite);
    }

    public void HandleIdleStateTransistions(CharacterController currentController, CharacterState state){
        Vector2 direction = InputFrameData.Direction;
		    if (direction.X != 0)
		    {
                if(state!=MovingState 
                && state!=JumpingState 
                && state!=FallingState
                && state!=FlyingState
                && (currentController.IsOnFloor() || currentCoyoteTimer>CoyoteTime)){
                    currentController.characterStateMachine.UpdateState(MovingState);
                }
                if(currentCoyoteTimer>CoyoteTime 
                && !currentController.IsOnFloor() 
                && currentController.characterStateMachine.CurrentState==MovingState){
                    currentController.characterStateMachine.UpdateState(FallingState);
                }
		    }
            
    }
    public void HandleMovingStateTransition(CharacterController currentController, CharacterState state){

    }

}
