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

		if ( client.Pawn is SwimmingPlayer currentPawn ) // Useful way to check if it exists and to type match it, we can now use currentPawn
		{
			currentPawn.Delete();
			currentPawn.Yeti.Delete();
		}

		var pawn = new SwimmingPlayer();
		client.Pawn = pawn;

		pawn.Clothing.LoadFromClient( client );
		pawn.Clothing.DressEntity( pawn );

		pawn.Position = Entity.All.OfType<SpawnPoint>().FirstOrDefault().Position;
		pawn.Yeti = new Yeti { Target = pawn };

	}

}
