using GameTriggers;
using Godot;
using System;

public partial class Interactible : Node, IInteractibleNode
{
	[Export]
	public NodePath TriggerPath{get;set;}
	private ITriggerable _trigger;
    public ITriggerable Trigger { get{
		if(_trigger==null){
			_trigger = this.GetNode<ITriggerable>(TriggerPath);
		}
		return _trigger;
	} }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

public interface IInteractibleNode{
	ITriggerable Trigger{get;}
}



