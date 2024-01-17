using Godot;

public partial class FlyingState : CharacterState{

    public override void Execute()
    {
        base.Execute();
        this.CharacterBody.CharacterStrategy.HandleFlyingState(this.CharacterBody,this);
    }
    public override void StartState()
    {
        base.StartState();
        AnimationPlayer.Play("Flying");
    }
}