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

		// MOVEMENT //
		

		// ANIMATION //

		var animationHelper = new CitizenAnimationHelper( this );

	}

}
