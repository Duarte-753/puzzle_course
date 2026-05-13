using System.Numerics;
using Godot;

namespace Game.Component;

public partial class BuildingComponent : Node2D
{
	[Export]
	public int BuildableRadius {get; private set;}

	public override void _Ready()
	{
		AddToGroup(nameof(BuildingComponent));
	}

	public Vector2I GetGridCellPosition()
	{
		var gridPosition = GlobalPosition / 64;
		gridPosition = gridPosition.Floor();
		//GD.Print(gridPosition);
		return new Vector2I((int)gridPosition.X, (int)gridPosition.Y);// converta a posição do mouse para coordenadas de grade dividindo pela largura/altura da célula e arredondando para baixo
	}

}
