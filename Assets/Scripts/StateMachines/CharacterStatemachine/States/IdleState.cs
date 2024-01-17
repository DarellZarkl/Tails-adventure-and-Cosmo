using Godot;

public partial class IdleState : CharacterState{

    [Export]
    public NodePath MovingPath{get;set;}

    private MovingState movingState;

    public override void _Ready()
    {
        base._Ready();
        movingState = GetNode<MovingState>(MovingPath);
    }
    public override void Execute()
    {
        base.Execute();
        CharacterBody.CharacterStrategy.HandleIdleState(CharacterBody,this);
    }
    public override void StartState()
    {
        base.StartState();
        AnimationPlayer.Play("Idle");
    }

}