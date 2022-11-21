using SandboxEditor;

namespace YetiGame;

[HammerEntity]
[EditorModel( "models/citizen/citizen.vmdl" )]
partial class Yeti : AnimatedEntity
{

	public override void Spawn()
	{

		SetModel( "models/citizen/citizen.vmdl" );
		Scale = 1.5f;

	}

	[Event.Tick]
	public void ComputeAI()
	{

		// MOVEMENT //
		

		// ANIMATION //

		var animationHelper = new CitizenAnimationHelper( this );

	}

}
