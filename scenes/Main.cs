using Godot;
using System;

namespace Game;

public partial class Main : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	private Sprite2D cursor;
	private PackedScene buildingScene;
	private Button placeBuildingButton;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//initialize the sprite and building scene
		cursor = GetNode<Sprite2D>("Cursor");
		buildingScene = GD.Load<PackedScene>("res://scenes/building/Building.tscn");
		placeBuildingButton = GetNode<Button>("PlaceBuildingButton");

		cursor.Visible = false;

		// Connect the button's pressed signal to the handler method
		//or other using the Connect method
		/*
		placeBuildingButton.Connect("pressed", new Callable(this, nameof(OnButtonPressed)));
		placeBuildingButton.Connect(Button.SignalName.Pressed, Callable.From(OnButtonPressed));
		*/
		placeBuildingButton.Pressed += OnButtonPressed;

		
	}

    public override void _UnhandledInput(InputEvent evt)
    {
		if ( cursor.Visible && evt.IsActionPressed("left_click"))
		{
			//GD.Print("left_click Clicked");
			PlaceBuildingAtMousePosition();
			cursor.Visible = false;
		}

		if (evt.IsActionPressed("right_click"))
		{
			//GD.Print("right_click Clicked");
		}
    }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var gridPosition = GetMouseGridCellPosition();
		cursor.Position = gridPosition * 64;
	}


	// Get the mouse position in grid coordinates (assuming each cell is 64x64 pixels)
	private Vector2 GetMouseGridCellPosition()
	{
		var mousePosition = GetGlobalMousePosition();
		var gridPosition = mousePosition / 64;
		gridPosition = gridPosition.Floor();
		//GD.Print(gridPosition);
		return gridPosition;
	}

	// This method can be called when the player clicks to place a building
	private void PlaceBuildingAtMousePosition()
	{
		var building = buildingScene.Instantiate<Node2D>();
		AddChild(building);

		var gridPosition = GetMouseGridCellPosition();
		building.Position = gridPosition * 64;
	}

	//=======================signails========================

	///Metjod to handle button press event
	private void OnButtonPressed()
	{
		//GD.Print("Place Building Button Pressed");
		cursor.Visible = true;
	}
}
