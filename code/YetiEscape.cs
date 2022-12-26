global using Sandbox;
global using Sandbox.UI.Construct;
global using System;
global using System.IO;
global using System.Linq;
global using System.Threading.Tasks;

namespace YetiGame;

public partial class YetiEscape : GameManager
{

	public YetiEscape()
	{
	}

	public override void ClientJoined( IClient client )
	{

		base.ClientJoined( client );

		Reset( client );

	}

	public static void Reset( IClient client )
	{

		client.Pawn?.Delete(); // If this client's pawn exists, meaning it has already been assigned one, it will delete it

		var pawn = new SwimmingPlayer();
		client.Pawn = pawn;

		pawn.Clothing.LoadFromClient( client );
		pawn.Clothing.DressEntity( pawn );

		pawn.Position = Entity.All.OfType<SpawnPoint>().FirstOrDefault().Position;

	}

}
