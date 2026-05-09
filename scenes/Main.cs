using Godot;
using System;

public partial class Main : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var testNumber = 42;
		var testArray = new string[] { "Hello", "World" };
		TestMethod();
		GD.Print("Hello World from C#!");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void TestMethod()
	{
		GD.Print("This is a test method.");
	}
}
