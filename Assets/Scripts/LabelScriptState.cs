using Godot;
using System;

public partial class LabelScriptState : Label
{
	[Export]
	public NodePath CharacterStateMachinePath{get;set;}

	private CharacterStateMachine characterStateMachine{get;set;}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Text = "Default";
		characterStateMachine = GetNode<CharacterStateMachine>(CharacterStateMachinePath);
		characterStateMachine.StateChangedSignal += OnStateChanged;
	}

	private void OnStateChanged(State state){
		GD.Print("OnchageState : "+state?.GetType()?.Name);
		if(state!=null)
			this.Text = state.GetType().Name;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
