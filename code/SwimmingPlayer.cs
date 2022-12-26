namespace YetiEscape;

public partial class SwimmingPlayer : AnimatedEntity
{

	public Yeti Yeti;			// Each player needs a Yeti assigned so they can be hunted, we don't create it now though
	public float Speed = 200f;	// How fast the player is moving, UnitsPerSecond so 200f means they will move at 200 units each second
	[ClientInput] public Vector3 InputDirection { get; set; }	// [ClientInput] attribute gives the client authority over this property and network it, the server will replicate it instead

	// The [Net] attribute lets use network properties from the server to the client, since we are handling gameplay on the server but our HUD is on the client
	[Net] public string GameText { get; set; } = "";		// The HUD will follow this variable and display whatever we set it to on the screen

	public override void Spawn() // AnimatedEntity has a default method called Spawn(), which runs on the Server once the player has been created, we can override it to run our code
	{

		SetModel( "models/citizen/citizen.vmdl" );  // Default citizen model
		Yeti = new Yeti { Target = this };			// Spawn a Yeti and assign its target to this player

	}

	protected override void OnDestroy() // AnimatedEntity has a default method called OnDestroy(), which runs once the player has been deleted, we can override it to run our code
	{

		if ( Game.IsServer )	// Do nothing if this is being run on the client, the server should always handle these entities
			Yeti.Delete();      // Delete the yeti too if the player is gone, useful when players disconnect or we are resetting the game

	}

	public override void Simulate( IClient client ) // This is called once every tick, both for client and server. Only entities that have been assigned to a Client's pawn have this
	{

		// MOVEMENT //

		Vector3 direction = new Vector3( InputDirection.x, InputDirection.y, 0 ).Normal; // Which direction the player is moving to, we normalize it or else the player moves faster diagonally
		Vector3 wishVelocity = direction * Speed * Time.Delta;		// Since the direction is normalised, meaning the lenght is always 1, we multiply by the speed

		Velocity = Vector3.Lerp( Velocity, wishVelocity, Time.Delta * 10f );    // Smooth the velocity from the current velocity to the new velocity so it doesn't snap
		Position += Velocity;	// Update the Position using the player's velocity

		if ( !Velocity.IsNearlyZero( 0.1f ) ) // Don't run this code if the Yeti is basically standing still
			Rotation = Rotation.Lerp( Rotation, Rotation.LookAt( Velocity, Vector3.Up ), Time.Delta * 10f ); // Rotate the player towards it's Velocity's direction, but smoothly

		// ANIMATION //

		var animationHelper = new CitizenAnimationHelper( this );				// CitizenAnimationHelper is useful when handling a Citizen's animations, it sets all the Animation Parameters for us
		animationHelper.WithVelocity( Velocity / Time.Delta );					// The running animation velocity is in UnitsPerSecond and not UnitsPerTick, so we divide by Time.Delta
		float distanceFromCenter = Position.Length;								// Since the center of the lake is 0, 0, 0 we can use the magnitude of our player's position as the distance from the center
		animationHelper.IsSwimming = distanceFromCenter < YetiEscape.Radius;	// If the player is still inside of the lake, then we use the swimming animations

		if ( Game.IsServer ) return;	// The code below won't run on the server

		// CAMERA //

		Camera.Position = Vector3.Up * 1000f;			// Place the game's camera in the sky
		Camera.Rotation = Rotation.FromPitch( 90f );    // Rotate the game's camera downwards

	}

	public override void BuildInput()	// This is called once every frame for the client for us to access and modify inputs. Only entities that have been assigned to a Client's pawn have this
	{
		InputDirection = Input.AnalogMove;	// Our InputDirection is set to Input.AnalogMove, which is the resulting direction from WASD or a controller's stick
	}

}
