global using Sandbox;
global using System.Linq;

namespace YetiEscape;

public partial class YetiEscape : GameManager // To create a game, we have make a new class which derives from the GameManager class
{

	// We can use static variables from anywhere in the code with YetiEscape.Radius
	public static float Radius = 512f;	// How big the Lake is, the Yeti will follow the circumference of an imaginary circle around it and the player will escape when reaching this distance

	public YetiEscape() // This is the constructor, every class has one and it runs the code when created on both server and client.
	{

		if ( Game.IsClient )	// Run the code only if it's the client, we do this for most UI elements
			new Hud();			// Create a new Hud instance. Hud is a partial class that was created automatically by the compiler from our Hud.razor file and is our rootpanel

	}

	public override void ClientJoined( IClient client ) // Whenever a client joins, run this code and pass the client variable
	{

		base.ClientJoined( client );	// This is autocompleted, but not necessary. All it does it print in the console that a player has joined
		Reset( client );				// Call the Reset method on the client

	}

	public static void Reset( IClient client ) // By having this as as static method, we can use it anywhere through YetiEscape.Reset
	{

		client.Pawn?.Delete(); // If this client's pawn exists, meaning it has already been assigned one, it will delete it

		var pawn = new SwimmingPlayer();	// Create the player class
		client.Pawn = pawn;                 // Assign the player to the client's Pawn, this lets us use the Simulate method and pass the client's inputs to the class

		var clothing = new ClothingContainer(); // S&box comes with a way to put clothing on Citizens
		clothing.LoadFromClient( client );      // Load the client's clothing
		clothing.DressEntity( pawn );           // Apply the clothing, this handles everything for us, including disabling bodygroups and changing skin

		var spawnPoint = SpawnPoint.All.First();	// Find the first available SpawnPoint, this is the info_player_start we placed in Hammer
		pawn.Position = spawnPoint.Position;		// Set the player's position to the spawnpoint's position

	}

	public static void DisplayText( IClient client, string text )
	{

		if ( client.Pawn is not SwimmingPlayer player ) return;

		player.GameText = text;

		GameTask.RunInThreadAsync( async () =>
		{
			await GameTask.DelaySeconds( 1f );
			player.GameText = "";
		} );
	}

}
