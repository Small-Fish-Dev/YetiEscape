namespace YetiGame;

public partial class SwimmingPlayer : AnimatedEntity
{

	public Yeti Yeti;
	public float Speed = 200f;
	[ClientInput] public Vector3 InputDirection { get; set; }

	public override void Spawn()
	{

		SetModel( "models/citizen/citizen.vmdl" );
		Yeti = new Yeti { Target = this };			// Spawn a Yeti and assign its target to this player

	}

	protected override void OnDestroy()
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
			Rotation = Rotation.Lerp( Rotation, Rotation.LookAt( Velocity ), Time.Delta * 10f ); // Rotate the player towards it's Velocity's direction, but smoothly

		// ANIMATION //

		var animationHelper = new CitizenAnimationHelper( this );				// CitizenAnimationHelper is useful when handling a Citizen's animations, it sets all the Animation Parameters for us
		animationHelper.WithVelocity( Velocity / Time.Delta );					// The running animation velocity is in UnitsPerSecond and not UnitsPerTick, so we divide by Time.Delta
		float distanceFromCenter = Position.Length;								// Since the center of the lake is 0, 0, 0 we can use the magnitude of our player's position as the distance from the center
		animationHelper.IsSwimming = distanceFromCenter < YetiEscape.Radius;	// If the player is still inside of the lake, then we use the swimming animations

		if ( Game.IsServer ) return;

		// CAMERA //

		Camera.Position = Vector3.Up * 1000f;
		Camera.Rotation = Rotation.FromPitch( 90f );

	}

	public override void BuildInput()
	{
		InputDirection = Input.AnalogMove;
	}

}
