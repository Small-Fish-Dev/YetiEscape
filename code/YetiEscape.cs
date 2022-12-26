global using Sandbox;
global using System.Linq;

namespace YetiGame;

public partial class YetiEscape : GameManager // To create a game, we have make a new class which derives from the GameManager class
{

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

}
