using Godot;

public partial class CharacterState : State{
    [Export]
    public NodePath CharacterBodyPath{get;set;}
    [Export]
    public NodePath animationPlayerPath{get;set;}
    [Export]
    public NodePath CharacterSpritePath{get;set;}

    public AnimationPlayer AnimationPlayer{get;set;}

    public CharacterController CharacterBody{get;set;}

    public StateMachine StateMachine{
        get{
            return CharacterBody.characterStateMachine;
        }
    }
    public ICharacterStrategy CharacterStrategy{
        get{
            return CharacterBody.CharacterStrategy;
        }
    }
    public override void _Ready()
    {
        base._Ready();
        CharacterBody = GetNode<CharacterController>(CharacterBodyPath);
        AnimationPlayer = GetNode<AnimationPlayer>(animationPlayerPath);
    }

}