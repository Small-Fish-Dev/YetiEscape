namespace YetiGame;

partial class SwimmingPlayer : AnimatedEntity
{

	public override void Spawn()
	{

		SetModel( "models/citizen/citizen.vmdl" );

	}

	public override void Simulate( Client cl )
	{

		// MOVEMENT //

		Vector3 direction = new Vector3( Input.Forward, Input.Left, 0 ).Normal;
		Vector3 wishSpeed = direction * 100f;
		
		Velocity = Vector3.Lerp( Velocity, wishSpeed, Time.Delta * 10f );

		if ( Velocity.Length > 0 )
		{

			Rotation = Rotation.Lerp( Rotation, Rotation.FromYaw( Velocity.EulerAngles.yaw ), Time.Delta * 10f );

		}

		Position += Velocity * Time.Delta;

		// ANIMATION //

		var animationHelper = new CitizenAnimationHelper( this );
		animationHelper.WithVelocity( wishSpeed );
		animationHelper.IsSwimming = true;

	}

	public override void PostCameraSetup( ref CameraSetup setup )
	{

		setup.Position = Vector3.Up * 800f;
		setup.Rotation = Rotation.FromPitch( 90f );

	}

}
