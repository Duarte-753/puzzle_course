using Godot;
using System;
using System.Collections.Generic;

namespace Game;

public partial class Main : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	private Sprite2D cursor;
	private PackedScene buildingScene;
	private Button placeBuildingButton;
	private TileMapLayer highlightTilemapLayer;

	private Vector2? hoveredGridCell;
	private HashSet<Vector2> occupiedCells = new(); 
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//initialize the sprite and building scene
		cursor = GetNode<Sprite2D>("Cursor");
		buildingScene = GD.Load<PackedScene>("res://scenes/building/Building.tscn");
		placeBuildingButton = GetNode<Button>("PlaceBuildingButton");
		highlightTilemapLayer = GetNode<TileMapLayer>("HighLightTileMapLayer");

		cursor.Visible = false;

		// Connect the button's pressed signal to the handler method
		//or other using the Connect method
		/*
		placeBuildingButton.Connect("pressed", new Callable(this, nameof(OnButtonPressed)));
		placeBuildingButton.Connect(Button.SignalName.Pressed, Callable.From(OnButtonPressed));
		*/
		placeBuildingButton.Pressed += OnButtonPressed;

		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var gridPosition = GetMouseGridCellPosition();
		cursor.Position = gridPosition * 64;
		if (cursor.Visible && (!hoveredGridCell.HasValue || hoveredGridCell.Value != gridPosition))		
		{
			hoveredGridCell = gridPosition;
			UpdateHighlightTilemaplayer();
		}
	}

    public override void _UnhandledInput(InputEvent evt)
    {
		if ( cursor.Visible && evt.IsActionPressed("left_click") && !occupiedCells.Contains(GetMouseGridCellPosition()) )
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

	// Get the mouse position in grid coordinates (assuming each cell is 64x64 pixels)
	private Vector2 GetMouseGridCellPosition()
	{
		var mousePosition = highlightTilemapLayer.GetGlobalMousePosition();
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
		occupiedCells.Add(gridPosition);

		hoveredGridCell = null;
		UpdateHighlightTilemaplayer();
	}

	private void UpdateHighlightTilemaplayer()
	{
		highlightTilemapLayer.Clear();
		
		if (!hoveredGridCell.HasValue)
		{
			return;
		}
		
		for(var x = hoveredGridCell.Value.X - 3; x <= hoveredGridCell.Value.X + 3; x++)
		{
			for(var y = hoveredGridCell.Value.Y - 3; y <= hoveredGridCell.Value.Y + 3; y++)
			{
				highlightTilemapLayer.SetCell(new Vector2I((int)x, (int)y), 0, Vector2I.Zero);
			}
		}
	}

	//=======================signails========================

	///Metjod to handle button press event
	private void OnButtonPressed()
	{
		//GD.Print("Place Building Button Pressed");
		cursor.Visible = true;
	}
}
