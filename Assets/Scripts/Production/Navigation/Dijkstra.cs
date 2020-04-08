using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Tools;

namespace AI
{
    public class Dijkstra : IPathFinder
	{
		private HashSet<Vector2Int> m_Accessibles;
		private Queue<Vector2Int> m_Frontier = new Queue<Vector2Int>();
		private LinkedList<Vector2Int> m_Path = new LinkedList<Vector2Int>();
		private Vector2Int? m_Current = new Vector2Int?();
		private Dictionary<Vector2Int, Vector2Int?> m_Ancestors = new Dictionary<Vector2Int, Vector2Int?>();
		private Vector2Int m_Next = new Vector2Int();

		public Dijkstra(IEnumerable<Vector2Int> accessibles)
		{
			m_Accessibles = new HashSet<Vector2Int>(accessibles);
		}

		public IEnumerable<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
		{
			Clear();
			if (start.Equals(goal))
			{
				m_Path.AddLast(start);
				return m_Path;
			}
			if(m_Accessibles.Contains(start) && m_Accessibles.Contains(goal))
			{
				m_Ancestors.Add(start, null);
				m_Frontier.Enqueue(start);
			}
			while (m_Frontier.Any())
			{
				m_Current = m_Frontier.Dequeue();
				if (m_Current.Equals(goal))
				{
					break;
				}
				CheckFrontier();
			}
			if (m_Ancestors.ContainsKey(goal))
			{
				while (m_Current.HasValue)
				{
					m_Path.AddLast(m_Current.Value);
					m_Current = m_Ancestors[m_Current.Value];
				}
				return m_Path.Reverse();
			}
			else
			{
				return Enumerable.Empty<Vector2Int>();
			}
		}	
		
		void Clear()
		{
			m_Frontier.Clear();
			m_Path.Clear();
			m_Ancestors.Clear();
		}

		void CheckFrontier()
		{
			foreach (Vector2Int direction in DirectionTools.Dirs)
			{
				m_Next = m_Current.Value + direction;
				if(m_Accessibles.Contains(m_Next) && !m_Ancestors.ContainsKey(m_Next))
				{
					m_Frontier.Enqueue(m_Next);
					m_Ancestors[m_Next] = m_Current;
				}
			}
		}
	}    
}
