using Godot;
using System;

namespace GameTriggers
{
	

public partial class Triggerable : Node, ITriggerable
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void ExecuteAction()
    {
        throw new System.NotImplementedException();
    }
}
public interface ITriggerable{
	public void ExecuteAction();
}
}