using Godot;
using System;

namespace Game;

public partial class Main : Node2D
{
	private Sprite2D sprite;
	private PackedScene buildingScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Cursor");
		buildingScene = GD.Load<PackedScene>("res://scenes/building/Building.tscn");
	}

    public override void _UnhandledInput(InputEvent evt)
    {
		if (evt.IsActionPressed("left_click"))
		{
			//GD.Print("left_click Clicked");
			PlaceBuildingAtMousePosition();
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
		sprite.Position = gridPosition * 64;
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
}
