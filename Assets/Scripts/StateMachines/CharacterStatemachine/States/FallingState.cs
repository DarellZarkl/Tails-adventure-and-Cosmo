using Godot;

public partial class FallingState : CharacterState{

    public override void Execute()
    {
        base.Execute();
        this.CharacterBody.CharacterStrategy.HandleFallingState(this.CharacterBody,this);
    }
}