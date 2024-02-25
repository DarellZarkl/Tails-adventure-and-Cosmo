using Godot;
using System;

public partial class DayNightCycleUpdater :Node
{

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		DayNightSingleton.Instance.Time+=delta;
	}
}
