using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSystem
{
    //Timi
    public class Graph<T>
    {
        private Dictionary<T, int> repVertex;
        private int[,] adjacencyMatrix;
        public Graph(List<T> vertices)
        {
            this.repVertex = new Dictionary<T, int>();
            this.adjacencyMatrix = new int[vertices.Count, vertices.Count];
            int i = 0;
            foreach (T item in vertices)
            {
                this.repVertex.Add(item,i++);
            }
        }

        public void SetEdge(T v, T v1)
        {
            this.adjacencyMatrix[repVertex[v], repVertex[v1]] = 1;
        }
        public void RemoveEdge(T v, T v1)
        {
            this.adjacencyMatrix[repVertex[v], repVertex[v1]] = 0;
        }
        public void SetEdge(T v, T v1, int cost)
        {
            this.adjacencyMatrix[repVertex[v], repVertex[v1]] = cost;
        }

        public int GetEdgeCount(T v, T v1)
        {
            return this.adjacencyMatrix[repVertex[v], repVertex[v1]];
        }

        private int[] SortVertex()
        {
            var numberOfEdges = new Dictionary<int, int>();
            for(int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                numberOfEdges.Add(i, 0);
                for(int j = 0; j < adjacencyMatrix.GetLength(1); j++)
                {
                    numberOfEdges[i] += (adjacencyMatrix[i, j] > 0) ? 1 : 0;
                }
            }
            var sorted = new SortedDictionary<int,int>(numberOfEdges);
            return sorted.Keys.ToArray();
        }
        //TODO: Check this delegate
        /// <summary>
        /// A method that colors the graph
        /// </summary>
        /// <param name="test">a condition the vertices having the same color is tested with</param>
        public Dictionary<T,List<T>> ColorGraph(Predicate<int> test)
        {
            var map = new Dictionary<T, List<T>>();
            var sortedIndex = SortVertex();
            byte[] isColored = new byte[sortedIndex.Length];
            byte colors = 0;
            int noOfVertex;
            for (int i = 0; i < sortedIndex.Length; i++)
            {
                noOfVertex = 0;
                if (isColored[i] != 0)
                    continue;
                isColored[i] = ++colors;
                T present = repVertex.First(x => x.Value == sortedIndex[i]).Key;
                map.Add(present , new List<T>());
                for (int j = i + 1; j < sortedIndex.Length; j++)
                {
                    if(adjacencyMatrix[i,j] == 0 && isColored[j] == 0)
                    {
                        if (!test(noOfVertex))
                            break;
                        isColored[j] = colors;
                        map[present].Add(repVertex.First(x => x.Value == sortedIndex[j]).Key);
                    }
                        
                }
                
            }
            return map;
        }
    }
}
