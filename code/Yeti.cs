﻿namespace YetiGame;

partial class Yeti : AnimatedEntity
{

	public SwimmingPlayer Target;	// This is who the Yeti will follow, we'll set it once a player joins
	public float SpeedRatio = 4f;	// How faster the Yeti is compared to the player, anything above PI will make it fast enough to catch up anywhere
	public float Radius = 512f;	// How big the Lake is, the Yeti will follow the circumference of an imaginary circle around it

	public override void Spawn() // AnimatedEntity has a default method called Spawn(), which runs on the Server once the Yeti has been created, we can override it to run our code
	{

		SetModel( "models/citizen/citizen.vmdl" );	// Default citizen model
		Scale = 1.5f;								// Make the Yeti bigger!
		Position = Vector3.Forward * Radius;			// Set the starting position somewhere around the lake
		EnableDrawing = false;						// We'll "Dress" our citizen as a Yeti, so hide the model, this is usually handled by the clothing asset but we're not using that

		var kongCostume = new AnimatedEntity( "models/kong/kong.vmdl" );	// Create the costume
		kongCostume.SetParent( this, true );								// Set the costume's parent and merge the bones, so it follows all movements of the parent

	}

	// When we're dealing with a value such as UnitsPerSecond (Velocity) we multiply it by Time.Delta (Time passed since the last tick) when calculating every tick

	[Event.Tick.Server] // Call this method every tick on the server, we can't set the position of an entity on the client if we want other players to see the change
	public void ComputeAI()
	{

		if ( Target == null ) return; // Do not run any code below if the Yeti doesn't have a target to follow

		// MOVEMENT //

		// Since the center of the lake is 0 0 0, we can treat the entity's positions as directional vectors
		Rotation currentRotation = Rotation.LookAt( Position );				// Rotation from the center to the Yeti's position
		Rotation wishRotation = Rotation.LookAt( Target.Position );			// Rotation from the center to the Target's position

		float distance = currentRotation.Distance( wishRotation );							// How many degrees are between the Yeti and the Target's rotation
		float deltaAngle = MathX.RadianToDegree( Target.Speed / Radius ) * Time.Delta;		// Ratio between the Target's max speed and the lake's radius
		float yetiSpeed = deltaAngle * SpeedRatio;											// Yeti's angular velocity around the lake
		float angleStep = yetiSpeed / distance;												// Divide by distance so the interpolation moves at a fixed rate

		Rotation newRotation = Rotation.Slerp( currentRotation, wishRotation, angleStep );	// Spherical interpolation between the two rotations
		Vector3 newPosition = newRotation.Forward * Radius;		// Position forms a circle around the rotation with the lake's radius
		Velocity = newPosition - Position;						// Calculate the velocity to also use in the Animation

		if ( !Velocity.IsNearlyZero() ) // Don't run this code if the Yeti is basically standing still
		{

			Rotation = Rotation.Lerp( Rotation, Rotation.LookAt( Velocity ), Time.Delta * 10f ); // Rotate the Yeti towards it's Velocity's direction, but smoothly

		}

		Position += Velocity; // Update the Position using the Yeti's velocity

		// ANIMATION //

		var animationHelper = new CitizenAnimationHelper( this );	// If you're using a Citizen as the model, this handles all the animation parameters for us
		animationHelper.WithVelocity( Velocity / Time.Delta );		// Set the running speed to the current velocity, since the value requires a UnitsPerSecond value we divide instead

	}

}
