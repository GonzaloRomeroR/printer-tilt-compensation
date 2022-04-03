# Problem
The beds of the printer get a tilt during shipping 

	* Unable to stick to the platform
	* Users need to use levelling screws
	* This is hard to use

# Solution
	* Create a method bed tilt compensation, no user maintenance

# Requirements
	* Needs to be in C# or C++
	* Needs to implement two methods:
		* Void PrepareTiltCompensation
			* Prepares the tilt compensation with three measuring points on the bed surface.
			* If completely levelled -> all Z will be zero
			* The measurement will be done before each print to set up the compensation method
			* Receives three Point3D, with the coordinates of the 3 measurement points
			* Returns Void
		* Point3D ApplyTiltCompensation(Point3D)
			* Used to change all the coordinates that the printer moves to
			* Receives a Point3D apply the compensation calculated by the PrepareTiltCompensation and returns a new Point3D to move the printer
		* Use summaries and input description.
		* Unit tests.
		* Diagram of the implementation.
		* Optimize code.
		* Belt tilt compensation is assuming that the bed surface is flat.
