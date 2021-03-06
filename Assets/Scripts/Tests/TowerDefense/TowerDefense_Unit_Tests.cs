﻿using System;
using System.Collections.Generic;
using System.Linq;
using AI;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class TowerDefense_Unit_Tests
    {
        private byte[,] m_Map_0, m_Map_1, m_Map_2, m_Map_3, m_Map_4;
        private List<Vector2Int> m_Accessibles = new List<Vector2Int>();
        private MapReader m_MapReader = new MapReader();

        [SetUp]
        public void Setup()
        {
            m_Map_0 = new byte[ , ]
            {
                { 0, 0, 0, 0, 0 },
                { 0, 1, 1, 1, 1 },
                { 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 0 },
                { 0, 0, 0, 0, 0 }
            }; 
            
            m_Map_1 = new byte[ , ]
            {
                { 1, 0, 0},
                { 1, 0, 1},
                { 0, 0, 1},
            };

            m_Map_2 = new byte[,]
            {
                { 0, 0, 0, 0, 0 },
                { 0, 1, 0, 1, 0 },
                { 0, 0, 0, 0, 0 },
                { 1, 0, 1, 1, 0 },
                { 0, 0, 0, 0, 0 }
            };

            m_Map_3 = new byte[,]
            {
                { 0, 0, 0, 0, 0 },
                { 0, 1, 0, 1, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 1, 0, 1, 0 },
                { 0, 0, 0, 0, 0 }
            };

            m_Map_4 = new byte[,]
{
                { 0, 0, 0},
                { 1, 0, 0},
                { 0, 0, 0},
};
        }
        
        [Test]
        [TestCase(/*MapID*/ 1, /*xStart*/ 0, /*yStart*/ 0, /*xGoal*/ 2, /*yGoal*/ 2, /*Result*/ 5)]
        [TestCase(/*MapID*/ 1, /*xStart*/ 2, /*yStart*/ 2, /*xGoal*/ 0, /*yGoal*/ 0, /*Result*/ 5)]
        [TestCase(/*MapID*/ 0, /*xStart*/ 0, /*yStart*/ 0, /*xGoal*/ 4, /*yGoal*/ 4, /*Result*/ 17)]
        [TestCase(/*MapID*/ 0, /*xStart*/ 4, /*yStart*/ 4, /*xGoal*/ 0, /*yGoal*/ 0, /*Result*/ 17)]
        [TestCase(/*MapID*/ 2, /*xStart*/ 0, /*yStart*/ 0, /*xGoal*/ 2, /*yGoal*/ 4, /*Result*/ 7)]
        [TestCase(/*MapID*/ 1, /*xStart*/ 0, /*yStart*/ 0, /*xGoal*/ 0, /*yGoal*/ 0, /*Result*/ 1)]
        [TestCase(/*MapID*/ 3, /*xStart*/ 1, /*yStart*/ 4, /*xGoal*/ 2, /*yGoal*/ 0, /*Result*/ 6)]
        [TestCase(/*MapID*/ 4, /*xStart*/ 0, /*yStart*/ 0, /*xGoal*/ 0, /*yGoal*/ 2, /*Result*/ 5)]
        [TestCase(/*MapID*/ 3, /*xStart*/ 2, /*yStart*/ 2, /*xGoal*/ 0, /*yGoal*/ 2, /*Result*/ 3)]

        public void Dijkstra_Solves_Raw_Data(int mapId, int xStart, int yStart, int xGoal, int yGoal, int expectedLength)
        {
            byte[,] map = mapId == 0 ? m_Map_0 : m_Map_1;
            if (mapId == 2)
            {
                map = m_Map_2;
            }
            if (mapId == 3)
            {
                map = m_Map_3;
            }
            if (mapId == 4)
            {
                map = m_Map_4;
            }

            //We flip the array values to match horizontal values with X coordinates and vertical values with Y coordinates
            //So (0 , 0) coordinates starts from the bottom left and not from the top left and Y coordinates from bottom to top and not
            //from top to bottom as the default indexing 2D array system.
            for (int iRun = map.GetLength(0) - 1, i = 0; iRun >= 0; iRun--, i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[iRun, j] == 0)
                    {
                        m_Accessibles.Add(new Vector2Int(j, i));                        
                    }
                }
            }
            IPathFinder pathFinder = new Dijkstra(m_Accessibles);
            IEnumerable<Vector2Int> path = pathFinder.FindPath(new Vector2Int(xStart, yStart), new Vector2Int(xGoal, yGoal));            
            Assert.AreEqual(expectedLength, path.Count());
            m_Accessibles.Clear();
        }    
    
        [Test]
        [TestCase("map_1", 17, -4, 0, -2, 92)]
        [TestCase("map_2", 24, 0, 12, -9, 116)]
        [TestCase("map_3", 0, 0, 5, -1, 7)]
        public void Dijkstra_Solves_Path(string map, int x0, int y0, int x1, int y1, int result)
        {
            MapData mapData = m_MapReader.ReadMap(map);
            IPathFinder pathFinder = new Dijkstra(mapData.WalkableTiles);
            IEnumerable<Vector2Int> path = pathFinder.FindPath(new Vector2Int(x0, y0), new Vector2Int(x1, y1));
            Assert.AreEqual(result, path.Count());
        }
    }
}
