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
        public void SetEdge(T v, T v1, int cost)
        {
            this.adjacencyMatrix[repVertex[v], repVertex[v1]] = cost;
        }

        public int GetEdgeCount(T v, T v1)
        {
            return this.adjacencyMatrix[repVertex[v], repVertex[v1]];
        }
    }
}
