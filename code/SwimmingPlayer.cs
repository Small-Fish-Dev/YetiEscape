﻿namespace YetiGame;

partial class SwimmingPlayer : AnimatedEntity
{

	public override void Spawn()
	{

		SetModel( "models/citizen/citizen.vmdl" );

	}

	public override void Simulate( Client cl )
	{

		Vector3 direction = new Vector3( Input.Forward, Input.Left, 0 ).Normal;

		Velocity = Vector3.Lerp( Velocity, direction, Time.Delta * 50f );
		Rotation = Rotation.Lerp( Rotation, Rotation.FromYaw( direction.EulerAngles.yaw ), Time.Delta * 50f );

		Position += direction * 100f * Time.Delta;

	}

	public override void PostCameraSetup( ref CameraSetup setup )
	{

		setup.Position = Vector3.Up * 800f;
		setup.Rotation = Rotation.FromPitch( 90f );

	}

}