using System;
using Godot;

public partial class MovingState : CharacterState{
    [Export]
    public NodePath IdlePath{get;set;}

    private IdleState idleState;

    public override void _Ready()
    {
        base._Ready();
        this.idleState = GetNode<IdleState>(IdlePath);
    }
    public override void Execute()
    {
        try{
            base.Execute();
            CharacterBody.CharacterStrategy.HandleWalkingState(this.CharacterBody,this);
        }
        catch(Exception e){
            GD.PrintErr(e);
        }
    }
    public override void StartState()
    {
        base.StartState();
        AnimationPlayer.Play("Walking");
    }
}