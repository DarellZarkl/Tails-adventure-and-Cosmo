using Godot;
using System;

public class DayNightSingleton {
	public double DayDuration {get;set;}
    private static DayNightSingleton _instance;
	public static DayNightSingleton Instance{
		get{
			if(_instance == null){
				_instance = new DayNightSingleton();
			}	
			return _instance;
		}
	}
	private DayNightSingleton(){}
	public double Time{get;set;} = 0.0f;
	public double TimeOfDay(){
		var current = (Math.Sin((Time*1/DayDuration)-0.5d*Math.PI)+1.0d)/2.0d;
		return current;
	}
}
