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

		var victimPos = Target.Position.WithZ( 0f ).Normal;
		var wishAngle = Math.Atan2( victimPos.y, victimPos.x );

		Position = Rotation.FromYaw( MathX.RadianToDegree( (float)wishAngle ) ).Forward * 512f;
		

		// ANIMATION //

		var animationHelper = new CitizenAnimationHelper( this );

	}

}
