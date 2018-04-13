using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSystem
{
    //Timi
    public class Graph
    {
        private Dictionary<Course, int> repVertex;
        private bool[,] adjacencyMatrix;
        public Graph(List<Course> vertices)
        {
            this.repVertex = new Dictionary<Course, int>();
            this.adjacencyMatrix = new bool[vertices.Count, vertices.Count];
            int i = 0;
            foreach (Course item in vertices)
            {
                this.repVertex.Add(item,i++);
            }
        }

        public void SetEdge(Course v, Course v1)
        {
            this.adjacencyMatrix[repVertex[v], repVertex[v1]] = true;
        }
        public void RemoveEdge(Course v, Course v1)
        {
            this.adjacencyMatrix[repVertex[v], repVertex[v1]] = false;
        }

        private int[] SortVertex()
        {
            var numberOfEdges = new Dictionary<int, int>();
            for(int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                numberOfEdges.Add(i, 0);
                for(int j = 0; j < adjacencyMatrix.GetLength(1); j++)
                {
                    numberOfEdges[i] += (adjacencyMatrix[i, j]) ? 1 : 0;
                }
            }
            var sorted = from entry in numberOfEdges orderby entry.Value descending select entry.Key;
            return sorted.ToArray();
        }
        /// <summary>
        /// A method that colors the graph
        /// </summary>
        public Dictionary<Course,List<Course>> ColorGraph()
        {
            var map = new Dictionary<Course, List<Course>>();
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
                Course present = repVertex.First(x => x.Value == sortedIndex[i]).Key as Course;
                map.Add(present , new List<Course>());
                for (int j = i + 1; j < sortedIndex.Length; j++)
                {
                    Course courseToBeAdded = repVertex.First(x => x.Value == sortedIndex[j]).Key as Course;
                    if (!adjacencyMatrix[i, j] && isColored[j] == 0 && Validate(map[present], courseToBeAdded));
                    {
                        if (noOfVertex == Venue.NumberOfVenues)
                            break;
                        isColored[j] = colors;
                        map[present].Add(courseToBeAdded);
                        noOfVertex++;
                    }
                        
                }
                
            }
            return map;
        }
        public bool Validate(List<Course> courses, Course course)
        {
            //TODO add Student
            foreach (var item in courses)
            {
                if(item.Lecturers.Intersect(course.Lecturers).Count() != 0)
                {
                    return false;
                }
            }
            return true;
        }
        
    }
}
