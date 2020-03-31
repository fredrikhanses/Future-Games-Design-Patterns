using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Tools;

namespace AI
{
    public class Dijkstra : IPathFinder
	{
		HashSet<Vector2Int> accessibles;
		Queue<Vector2Int> frontier = new Queue<Vector2Int>();
		LinkedList<Vector2Int> path = new LinkedList<Vector2Int>();
		Vector2Int? current = new Vector2Int?();
		Dictionary<Vector2Int, Vector2Int?> ancestors = new Dictionary<Vector2Int, Vector2Int?>();
		Vector2Int next = new Vector2Int();

		public Dijkstra(IEnumerable<Vector2Int> accessibles)
		{
			this.accessibles = new HashSet<Vector2Int>(accessibles);
		}

		public IEnumerable<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
		{
			Clear();
			if (start.Equals(goal))
			{
				path.AddLast(start);
				return path;
			}
			if(accessibles.Contains(start) && accessibles.Contains(goal))
			{
				ancestors.Add(start, null);
				frontier.Enqueue(start);
			}
			while (frontier.Any())
			{
				current = frontier.Dequeue();
				if (current.Equals(goal))
				{
					break;
				}
				CheckFrontier(goal);
			}
			if (ancestors.ContainsKey(goal))
			{
				while (current.HasValue)
				{
					path.AddLast(current.Value);
					current = ancestors[current.Value];
				}
				return path.Reverse();
			}
			else
			{
				return Enumerable.Empty<Vector2Int>();
			}
		}	
		
		void Clear()
		{
			frontier.Clear();
			path.Clear();
			ancestors.Clear();
		}

		void CheckFrontier(Vector2Int goal)
		{
			foreach (Vector2Int direction in DirectionTools.Dirs)
			{
				next = current.Value + direction;
				if(accessibles.Contains(next) && !ancestors.ContainsKey(next))
				{
					frontier.Enqueue(next);
					ancestors[next] = current;
				}
			}
		}
	}    
}
