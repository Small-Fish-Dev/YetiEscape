using SandboxEditor;

namespace YetiGame;

partial class Yeti : AnimatedEntity
{

	public SwimmingPlayer Target { get; set; }

	public override void Spawn()
	{

		SetModel( "models/citizen/citizen.vmdl" );
		Scale = 1.5f;
		EnableDrawing = false;

	}

	[Event.Tick.Server]
	public void ComputeAI()
	{

		if ( Target == null ) return;

		EnableDrawing = true;

		// MOVEMENT //

		Vector3 wishPosition = Target.Position.WithZ( 0 ).Normal * 512f; // Basically treats the player's position as a directional vector with the world's origin as the start
		Position = wishPosition;
		

		// ANIMATION //

		var animationHelper = new CitizenAnimationHelper( this );

	}

}
