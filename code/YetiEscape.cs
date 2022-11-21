global using Sandbox;
global using Sandbox.UI.Construct;
global using System;
global using System.IO;
global using System.Linq;
global using System.Threading.Tasks;

namespace YetiGame;

public partial class YetiEscape : Sandbox.Game
{
	public YetiEscape()
	{
	}

	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );

		var pawn = new SwimmingPlayer();
		client.Pawn = pawn;

		// Get all of the spawnpoints
		var spawnpoints = Entity.All.OfType<SpawnPoint>();

		// chose a random one
		var randomSpawnPoint = spawnpoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

		// if it exists, place the pawn there
		if ( randomSpawnPoint != null )
		{
			var tx = randomSpawnPoint.Transform;
			tx.Position = tx.Position + Vector3.Up * 50.0f; // raise it up
			pawn.Transform = tx;
		}
	}
}
