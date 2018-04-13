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
        public Graph(IEnumerable<T> vertices)
        {
            this.repVertex = new Dictionary<T, int>();
            this.adjacencyMatrix = new int[vertices.Count(), vertices.Count()];
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
        public bool CheckEdge(T v, T v1)
        {
            return this.adjacencyMatrix[repVertex[v], repVertex[v1]] > 0 ? true : false;
        }
        public void RemoveEdge(T v, T v1)
        {
            this.adjacencyMatrix[repVertex[v], repVertex[v1]] = 0;
        }
        private bool CheckEdge(int i, int j)
        {
            return this.adjacencyMatrix[i, j] > 0 ? true : false;
        }

        private int[] SortVertex()
        {
            var numberOfEdges = new Dictionary<int, int>();
            for(int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                numberOfEdges.Add(i, 0);
                for(int j = 0; j < adjacencyMatrix.GetLength(1); j++)
                {
                    numberOfEdges[i] += CheckEdge(i,j) ? 1 : 0;
                }
            }
            var sorted = from entry in numberOfEdges orderby entry.Value descending select entry.Key;
            return sorted.ToArray();
        }
        /// <summary>
        /// A method that colours the graph
        /// </summary>
        /// <param name="UpperLimit">The upper limit of each hashset Generated. If no limit, put 0</param>
        /// <returns></returns>
        /// Th
        public List<HashSet<T>> ColorGraph(int UpperLimit)
        {
            var map = new List<HashSet<T>>();
            var sortedIndex = SortVertex();
           
            int[] isColored = new int[sortedIndex.Length];
            int colors = 0;
            int noOfVertex;
            for (int i = 0; i < sortedIndex.Length; i++)
            {
                noOfVertex = 0;
                if (isColored[i] != 0)
                    continue;
                isColored[i] = ++colors;
                T present = repVertex.First(x => x.Value == sortedIndex[i]).Key;
                map.Add(new HashSet<T> { present });
                for (int j = i + 1; j < sortedIndex.Length; j++)
                {
                    T courseToBeAdded = repVertex.First(x => x.Value == sortedIndex[j]).Key;
                    if (!CheckEdge(i,j) && isColored[j] == 0)
                    {
                        if (UpperLimit!=0&&noOfVertex>=UpperLimit)
                            break;
                        isColored[j] = colors;
                        map[map.Count-1].Add(courseToBeAdded);
                        noOfVertex++;
                    }
                        
                }
                
            }
            return map;
        }
        public List<HashSet<T>> OurColorGraph()
        {
            var map = new List<HashSet<T>>();
            for(int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                map.Add(new HashSet<T>());
                for(int j= 0; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (!CheckEdge(i, j)){
                        T courseToBeAdded = repVertex.First(x => x.Value == j).Key;
                        map[map.Count - 1].Add(courseToBeAdded);
                    }
                }
            }
            return map;
        }
        //public bool Validate(List<Course> courses, Course course)
        //{
        //    //TOrDO add Student
        //    foreach (var item in courses)
        //    {
        //        if(item.Lecturers.Intersect(course.Lecturers).Count() != 0)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
        public List<HashSet<T>> ColorGraph() {
            return ColorGraph(0);
        }
    }
}
