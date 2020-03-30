using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Tools;

namespace AI
{
    public class Dijkstra : IPathFinder
	{
		HashSet<Vector2Int> accessibles = new HashSet<Vector2Int>();
		Queue<Vector2Int> frontier = new Queue<Vector2Int>();
		LinkedList<Vector2Int> path = new LinkedList<Vector2Int>();
		Vector2Int current = new Vector2Int();
		Dictionary<Vector2Int, Vector2Int> ancestors = new Dictionary<Vector2Int, Vector2Int>();
		Vector2Int next = new Vector2Int();

		public Dijkstra(List<Vector2Int> accessibles)
		{
			this.accessibles.Clear();
			foreach (Vector2Int vector2Int in accessibles)
			{
				this.accessibles.Add(vector2Int);
			}
		}

		public IEnumerable<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
		{
			Clear();
			if (start.Equals(goal))
			{
				path.AddLast(start);
				return path;
			}
			frontier.Enqueue(start);
			while (frontier.Any())
			{
				current = frontier.Dequeue();
				if (current.Equals(goal))
				{
					break;
				}
				CheckFrontier(goal);
			}
			if (current.Equals(goal))
			{
				while (!current.Equals(start))
				{
					path.AddLast(current);
					current = ancestors[current];
				}
				path.AddLast(start);
				return path.Reverse();
			}
			else
			{
				return path;
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
				next = current + direction;
				if((accessibles.Contains(next) && !ancestors.ContainsKey(next)) || (next.Equals(goal) && !ancestors.ContainsKey(next)))
				{
					frontier.Enqueue(next);
					ancestors.Add(next, current);
				}
			}
		}
	}    
}
