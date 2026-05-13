using System.Collections.Generic;
using Godot;

namespace Game.Manager;

public partial class GridManager : Node
{
	private HashSet<Vector2I> occupiedCells = new(); // este conjunto armazena as posições dos tiles ocupados para verificação rápida de validade
	
	[Export]
	private TileMapLayer highlightTilemapLayer; // esta camada de tilemap é usada para destacar os tiles válidos para colocação de edifícios, ela é limpa e atualizada a cada movimento do cursor

	[Export]
	private TileMapLayer baseTerrainTilemapLayer; // esta camada de tilemap representa o terreno base do jogo, ela é usada para verificar se um tile é construível ou não, através de custom data nos tiles

	// Este método verifica se um tile é válido para colocação de edifícios verificando o conjunto occupiedCells e a camada de tilemap de terreno base.
	public bool IsTilePositionValid(Vector2I tilePosition)
	{
		
		var customData = baseTerrainTilemapLayer.GetCellTileData(tilePosition);
		if (customData == null) return false; // Se não houver dados de tile, não é válido
		if (!(bool)customData.GetCustomData("buildable")) return false; // Se os dados do tile não tiverem a custom data "buildable" ou for falso, não é válido

		return !occupiedCells.Contains(tilePosition);
	}

	// Este método verifica se um tile está ocupado verificando o conjunto occupiedCells e a camada de tilemap de terreno base.
	public void MarkTileAsOccupied(Vector2I tilePosition)
	{
		occupiedCells.Add(tilePosition);
	}
	
	// Esse método destaca os tiles válidos em um raio ao redor da célula raiz
	public void HighlightValidTilesInRadius(Vector2I rootCell, int radius)
	{
		ClearHighlightedTiles();
		
		for(var x = (int)rootCell.X - radius; x <= (int)rootCell.X + radius; x++)
		{
			for(var y = (int)rootCell.Y - radius; y <= (int)rootCell.Y + radius; y++)
			{
				var tilePosition = new Vector2I(x, y);
				if(!IsTilePositionValid(tilePosition)) continue;
				highlightTilemapLayer.SetCell(tilePosition, 0, Vector2I.Zero);
			}
		}
	}

	// Este método limpa todos os tiles destacados da camada de tilemap de destaque
	public void ClearHighlightedTiles()
	{
		highlightTilemapLayer.Clear();
	}

	// Obtenha a posição do mouse em coordenadas de grade (supondo que cada célula seja de 64x64 pixels)
	public Vector2I GetMouseGridCellPosition()
	{
		var mousePosition = highlightTilemapLayer.GetGlobalMousePosition();
		var gridPosition = mousePosition / 64;
		gridPosition = gridPosition.Floor();
		//GD.Print(gridPosition);
		return new Vector2I((int)gridPosition.X, (int)gridPosition.Y);// converta a posição do mouse para coordenadas de grade dividindo pela largura/altura da célula e arredondando para baixo
	}
}
