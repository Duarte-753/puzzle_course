using Game.Manager;
using Godot;

namespace Game;

public partial class Main : Node
{
	// Declare member variables here. Examples:
	// Declaração de variáveis membro aqui. Exemplos:
	private GridManager gridManager;
	private Sprite2D cursor;
	private PackedScene buildingScene;
	private Button placeBuildingButton;

	private Vector2? hoveredGridCell;
	
	
	// Called when the node enters the scene tree for the first time.
	// Chamado quando o nó entra na árvore de cena pela primeira vez.
	public override void _Ready()
	{
		//initialize the sprite and building scene
		//inicialize o sprite e a cena do edifício
		gridManager = GetNode<GridManager>("GridManager");
		cursor = GetNode<Sprite2D>("Cursor");
		buildingScene = GD.Load<PackedScene>("res://scenes/building/Building.tscn");
		placeBuildingButton = GetNode<Button>("PlaceBuildingButton");

		cursor.Visible = false;

		// Connect the button's pressed signal to the handler method
		// Conecte o sinal de pressionado do botão ao método manipulador
		/*
		placeBuildingButton.Connect("pressed", new Callable(this, nameof(OnButtonPressed)));
		placeBuildingButton.Connect(Button.SignalName.Pressed, Callable.From(OnButtonPressed));
		*/
		placeBuildingButton.Pressed += OnButtonPressed;

		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// Chamado a cada frame. 'delta' é o tempo decorrido desde o frame anterior.
	public override void _Process(double delta)
	{
		var gridPosition = gridManager.GetMouseGridCellPosition();
		cursor.GlobalPosition = gridPosition * 64;
		if (cursor.Visible && (!hoveredGridCell.HasValue || hoveredGridCell.Value != gridPosition))		
		{
			hoveredGridCell = gridPosition;
			gridManager.HighlightValidTilesInRadius(hoveredGridCell.Value, 3);
		}
	}

    public override void _UnhandledInput(InputEvent evt)
    {
		if (hoveredGridCell.HasValue && cursor.Visible && evt.IsActionPressed("left_click") && gridManager.IsTilePositionValid(hoveredGridCell.Value))
		{
			//GD.Print("left_click Clicked");
			PlaceBuildingAtHoveredCellPosition();
			cursor.Visible = false;
		}

		if (evt.IsActionPressed("right_click"))
		{
			//GD.Print("right_click Clicked");
		}
    }


	// This method can be called when the player clicks to place a building
	// Este método pode ser chamado quando o jogador clicar para colocar um edifício
	private void PlaceBuildingAtHoveredCellPosition()
	{
		if (!hoveredGridCell.HasValue)
		{
			return;
		}

		var building = buildingScene.Instantiate<Node2D>();
		AddChild(building);

		building.GlobalPosition = hoveredGridCell.Value * 64;
		gridManager.MarkTileAsOccupied(hoveredGridCell.Value);

		hoveredGridCell = null;
		gridManager.ClearHighlightedTiles();
	}


	//=======================signails========================

	///Metjod to handle button press event
	/// Método para lidar com o evento de pressionar o botão
	private void OnButtonPressed()
	{
		//GD.Print("Place Building Button Pressed");
		cursor.Visible = true;
	}
}
