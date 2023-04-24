using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static class QueueExtensions
    {

        public static Queue<T> Remove<T>(this Queue<T> queue, T toRemove)
        {
            return new Queue<T>(queue.Where(x => !toRemove.Equals(x)));
        }
        
        public static Queue<T> Remove<T>(this Queue<T> queue, IEnumerable<T> toRemove)
        {
            return new Queue<T>(queue.Where(x => !toRemove.Contains(x)));
        }
    }
}