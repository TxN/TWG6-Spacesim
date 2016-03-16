Easy Bezier

Introduction
Easy Bezier is an extension for Unity3D that makes it easy to create and use bezier spline paths.

Usage
Create a path

This extension adds a new menuitem under "GameObject-Create Other" called "BezierPath". This item is also reachable through the popup menu above the hierarchy tab. After clicking on this menuitem, a new bezierpath will be added to you scene. After selecting the new BezierPath object, you can assign colors to it in the inspector. These colors are only for the visualization in the scene view. In the scene view you will see a small cube signifying the first point. After you select it, you can drag its handle and the point itself around by using the position handles. It will also show 2 buttons: "+" and "-". "+" adds a new point after the current selected point and "-" removes the current selected point.

By dragging around the points and handles, you can create the curve you want. For more precise control over the points, you can also edit them in the inspector.
Create a script

To usage of the path is quite simple. You can query the path about a position on the path in two ways. The first way is to use GetPositionByT. This provides an interpolated position based on t where 0>=t<=points. This means that it will return the first point when t=0 and the last point when t = last point number. At t=1 it will return the second point and at values in between, the interpolated position. The second way to use the path is with the function GetPositionByDistance. This returns the interpolated position based on the distance from the first point. The distance calculations are approximate but should be precise enough for real world usage.

Example:
 transfrom.position = path.GetPositionByT(0.5f); // get the position halfway between the first and second point

 transfrom.position = path.GetPositionByDistance(10); // get the position 10 units along the line

Author:
    Sander Homan - http://homans.nhlrebel.com

Copyright:
    Sander Homan 2012 

