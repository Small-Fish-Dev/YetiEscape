﻿global using Sandbox;
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

	}
}