using Game.Manager;
using Godot;

namespace Game;

public partial class Main : Node
{
	private GridManager gridManager;// esta variável armazena a referência ao GridManager, que é um nó filho deste nó, ela é usada para verificar a validade dos tiles, marcar tiles como ocupados e destacar tiles válidos para colocação de edifícios
	private Sprite2D cursor;// esta variável armazena o sprite do cursor que é usado para mostrar onde o jogador está prestes a colocar um edifício, ela é ativada quando o jogador clica no botão de colocação de edifícios e desativada quando o jogador coloca um edifício ou clica com o botão direito para cancelar
	private PackedScene buildingScene; // esta variável armazena a cena do edifício que será instanciada quando o jogador clicar para colocar um edifício
	private Button placeBuildingButton;// este botão é usado para ativar o modo de colocação de edifícios, ele torna o cursor visível e permite que o jogador clique para colocar um edifício

	private Vector2I? hoveredGridCell;// esta variável armazena a posição do tile atualmente sob o cursor, ela é usada para evitar atualizações desnecessárias da camada de destaque quando o cursor se move dentro do mesmo tile
	
	
	// Chamado quando o nó entra na árvore de cena pela primeira vez.
	public override void _Ready()
	{
		//inicialize o sprite e a cena do edifício
		gridManager = GetNode<GridManager>("GridManager");
		cursor = GetNode<Sprite2D>("Cursor");
		buildingScene = GD.Load<PackedScene>("res://scenes/building/Building.tscn");
		placeBuildingButton = GetNode<Button>("PlaceBuildingButton");

		cursor.Visible = false;

		// Conecte o sinal de pressionado do botão ao método manipulador
		/* aqui outras formas de conectar o sinal, mas a mais simples é usando o operador +=
			placeBuildingButton.Connect("pressed", new Callable(this, nameof(OnButtonPressed)));
			placeBuildingButton.Connect(Button.SignalName.Pressed, Callable.From(OnButtonPressed));
		*/
		placeBuildingButton.Pressed += OnButtonPressed;

		
	}

	// Chamado a cada frame. 'delta' é o tempo decorrido desde o frame anterior.
	public override void _Process(double delta)
	{
		var gridPosition = gridManager.GetMouseGridCellPosition();
		cursor.GlobalPosition = gridPosition * 64;
		if (cursor.Visible && (!hoveredGridCell.HasValue || hoveredGridCell.Value != gridPosition))		
		{
			hoveredGridCell = gridPosition;
			gridManager.HigllightBuildableTiles();
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

	// Este método pode ser chamado quando o jogador clicar para colocar um edifício
	private void PlaceBuildingAtHoveredCellPosition()
	{
		if (!hoveredGridCell.HasValue)// se não houver uma célula atualmente sob o cursor
		{
			return;
		}

		var building = buildingScene.Instantiate<Node2D>(); // instancie a cena do edifício como um Node2D
		AddChild(building); // adicione o edifício como filho deste nó para que ele apareça na cena
		building.GlobalPosition = hoveredGridCell.Value * 64;
		gridManager.MarkTileAsOccupied(hoveredGridCell.Value);

		hoveredGridCell = null; // limpe a célula atualmente sobre o cursor para evitar que o destaque fique preso em um tile inválido
		gridManager.ClearHighlightedTiles();// limpe os tiles destacados para que o jogador possa ver claramente onde o edifício foi colocado e quais tiles ainda são válidos para construção
	}


	//=======================signails========================

	/// Método para lidar com o evento de pressionar o botão
	private void OnButtonPressed()
	{
		//GD.Print("Place Building Button Pressed");
		cursor.Visible = true;
	}
}
