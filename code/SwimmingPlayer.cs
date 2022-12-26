namespace YetiGame;

partial class SwimmingPlayer : AnimatedEntity
{

	public ClothingContainer Clothing = new();
	public float Speed = 200f;
	[ClientInput] public Vector3 InputDirection { get; set; }

	public override void Spawn()
	{

		SetModel( "models/citizen/citizen.vmdl" );

	}

	public override void Simulate( IClient cl )
	{

		// MOVEMENT //

		Vector3 direction = new Vector3( InputDirection.x, InputDirection.y, 0 ).Normal;
		Vector3 wishSpeed = direction * Speed;

		Velocity = Vector3.Lerp( Velocity, wishSpeed, Time.Delta * 10f );

		if ( Velocity.Length > 0 )
		{

			Rotation = Rotation.Lerp( Rotation, Rotation.FromYaw( Velocity.EulerAngles.yaw ), Time.Delta * 10f );

		}

		Position += Velocity * Time.Delta;

		// ANIMATION //

		var animationHelper = new CitizenAnimationHelper( this );
		animationHelper.WithVelocity( Velocity );
		animationHelper.IsSwimming = true;

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
