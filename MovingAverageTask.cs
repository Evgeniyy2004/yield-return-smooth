using System.Collections.Generic;

namespace yield;

public static class MovingAverageTask
{
	public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
	{
		var average=new Queue<DataPoint>();
		List<DataPoint> points = new List<DataPoint>();
		//var c=data.GetEnumerator();
		//c.MoveNext();
		double? smakprev = null;
		foreach (var item in data) 
		{
			points.Add(item);
		}

		for(int i=0;i<windowWidth-1 && i<points.Count;i++) 
		{
			double sum = 0;
			for(int j=0;j<=i;j++) 
			{
				sum += points[j].OriginalY;
			}

			sum /= (i+1);
			points[i]=(new DataPoint(points[i].X, points[i].OriginalY));
			points[i] = points[i].WithAvgSmoothedY(sum);
			yield return points[i];
        }

		for(int i=windowWidth-1;i<points.Count; i++)
		{
			if(smakprev == null)
			{
				smakprev = CountSmak(points,windowWidth,i);
				points[i] = points[i].WithAvgSmoothedY(smakprev.Value);
				yield return points[i];
			}
			
			else
			{
				smakprev = smakprev.Value + (points[i].OriginalY - points[i-windowWidth].OriginalY)/windowWidth;
                points[i] = points[i].WithAvgSmoothedY(smakprev.Value);
                yield return points[i];
            }
		}
	}

	public static double CountSmak(List<DataPoint> list,int width,int end) 
	{
		double sum = 0;
		for(int i=end-width+1;i<=end;i++)
		{
			sum+= list[i].OriginalY;
		}
		return sum /= width;
	}
}