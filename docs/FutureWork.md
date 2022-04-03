# Future Work

* The trajectory of the printer end effector is defined by the GCODE, if the compensation of the trayectories are done in the fly, this may require to use too much processing power for an embedded system. It could be useful to parse the GCODE file before printing and created a compensated version of the GCODE file before printing to avoid having to do on the fly calculations.
* The code could be optimized by changing some data types to avoid having to do type translation inside the classes methods, increasing the algorithms performance.
* In order to compensate for surface roughness, Once the tilt compensation is measured, the printer could do a sweep in the surface to seek for imperfections creatin a surface map with the irregularities and take them into consideration once the printing starts. In case of deformation in the bed surface, the printer could sample certain points in the surface, create a grid and interpolate between the points. This could be added to the compensation algorithm by assing this irregularities once the tilt compensation transformation is done.  
* Increase the number of cases in the unit tests to consider different bed's inclinations. Consider corner cases.

