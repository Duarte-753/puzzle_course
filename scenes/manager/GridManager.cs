using System.Collections.Generic;
using Godot;

namespace Game.Manager;

public partial class GridManager : Node
{
	private HashSet<Vector2> occupiedCells = new(); 
	
	// This class is responsible for managing the grid, including highlighting valid tiles for building placement and checking if a tile is occupied. 
	// Esta classe é responsável por gerenciar a grade, incluindo destacar os tiles válidos para colocação de edifícios e verificar se um tile está ocupado.
	[Export]
	private TileMapLayer highlightTilemapLayer;

	// This is the base terrain tilemap layer, which can be used to check if a tile is occupied or not.
	// Esta é a camada de tilemap de terreno base, que pode ser usada para verificar
	[Export]
	private TileMapLayer baseTerrainTilemapLayer;

	// _Ready is called when the node enters the scene tree for the first time.
	// _Ready é chamado quando o nó entra na árvore de cena pela primeira vez.
	public override void _Ready()
	{
		
	}

	// This method checks if a tile is valid for building placement by checking the occupiedCells set and the base terrain tilemap layer.
	// Este método verifica se um tile é válido para colocação de edifícios verificando o conjunto occupiedCells e a camada de tilemap de terreno base.
	public bool IsTilePositionValid(Vector2 tilePosition)
	{
		return !occupiedCells.Contains(tilePosition);
	}

	// This method checks if a tile is occupied by checking the occupiedCells set and the base terrain tilemap layer.
	// Este método verifica se um tile está ocupado verificando o conjunto occupiedCells e a camada de tilemap de terreno base.
	public void MarkTileAsOccupied(Vector2 tilePosition)
	{
		occupiedCells.Add(tilePosition);
	}
	
	// This method highlights the valid tiles in a radius around the root cell
	// Esse método destaca os tiles válidos em um raio ao redor da célula raiz
	public void HighlightValidTilesInRadius(Vector2 rootCell, int radius)
	{
		ClearHighlightedTiles();
		
		for(var x = (int)rootCell.X - radius; x <= (int)rootCell.X + radius; x++)
		{
			for(var y = (int)rootCell.Y - radius; y <= (int)rootCell.Y + radius; y++)
			{
				if(!IsTilePositionValid(new Vector2(x, y))) continue;
				highlightTilemapLayer.SetCell(new Vector2I((int)x, (int)y), 0, Vector2I.Zero);
			}
		}
	}

	// This method Clears all highlighted tiles from the highlight tilemap layer
	// Este método limpa todos os tiles destacados da camada de tilemap de destaque
	public void ClearHighlightedTiles()
	{
		highlightTilemapLayer.Clear();
	}

	// Get the mouse position in grid coordinates (assuming each cell is 64x64 pixels)
	// Obtenha a posição do mouse em coordenadas de grade (supondo que cada célula seja de 64x64 pixels)
	public Vector2 GetMouseGridCellPosition()
	{
		var mousePosition = highlightTilemapLayer.GetGlobalMousePosition();
		var gridPosition = mousePosition / 64;
		gridPosition = gridPosition.Floor();
		//GD.Print(gridPosition);
		return gridPosition;
	}
}
