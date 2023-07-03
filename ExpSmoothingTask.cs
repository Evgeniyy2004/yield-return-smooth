using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace yield;

public static class ExpSmoothingTask
{
	public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
	{
		DataPoint point=null;
		foreach(var e in data) 
		{
			if(point == null)
			{
				point = new DataPoint(e);
				point=point.WithExpSmoothedY(point.OriginalY);
				yield return point;
            }

			else
			{
                var l = new DataPoint(e);
                l = l.WithExpSmoothedY(alpha * l.OriginalY + (1 - alpha) * point.ExpSmoothedY);
				point = l;
				yield return l;
            }
		}
	}
}
