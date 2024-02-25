using Godot;
using System;

public partial class DayNightCycle : CanvasModulate
{
	[Export]
	public double DayDuration{get;set;} = 20;
	[Export]
	public GradientTexture1D Gradient{get;set;}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DayNightSingleton.Instance.DayDuration = DayDuration;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var timeofDay = DayNightSingleton.Instance.TimeOfDay();
        this.Color = Gradient.Gradient.Sample((float)timeofDay);
        GD.Print($"Setting color at {DayNightSingleton.Instance.Time} : {this.Color.ToString()}");
	}
}