using SandboxEditor;
using System.Numerics;

namespace YetiGame;

partial class Yeti : AnimatedEntity
{

	public SwimmingPlayer Target { get; set; }

	public override void Spawn()
	{

		SetModel( "models/citizen/citizen.vmdl" );
		Scale = 1.5f;
		EnableDrawing = false;
		Position = Vector3.Forward * 512f;

	}

	[Event.Tick.Server]
	public void ComputeAI()
	{

		if ( Target == null ) return;

		EnableDrawing = true;

		// MOVEMENT //

		Rotation currentRotation = Rotation.LookAt( Position.WithZ( 0f ) ); // Yeti is on the shore, you are under water so there's a height difference, we just want the horizontal rotation
		Rotation wishRotation = Rotation.LookAt( Target.Position.WithZ( 0f ) );
		Rotation difference = Rotation.Difference( currentRotation, wishRotation );
		float distance = Math.Max( currentRotation.Distance( wishRotation ), 1f ); // We don't want to divide under 1
		Rotation normalDirection = difference / distance;

		currentRotation *= difference / distance * Time.Delta * 50f;

		Position = currentRotation.Forward * 512f;

		// ANIMATION //

		var animationHelper = new CitizenAnimationHelper( this );

	}

}
