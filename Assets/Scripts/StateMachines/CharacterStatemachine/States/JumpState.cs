using Godot;

public partial class JumpState : CharacterState{
    public override void Execute()
    {
        base.Execute();
        this.CharacterBody.CharacterStrategy.HandleJumpingState(this.CharacterBody,this);
    }
     public override void StartState()
    {
        base.StartState();
        AnimationPlayer.Play("Jumping");
    }
}