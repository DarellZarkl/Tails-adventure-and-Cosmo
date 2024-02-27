using Godot;
using System;

public partial class Interactible : Node
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

public interface IInteractibleNode<T> where T: ITriggerable{
	ITriggerable trigger{get;set;}
}



public interface ITriggerable{
	public void ExecuteAction();
}
