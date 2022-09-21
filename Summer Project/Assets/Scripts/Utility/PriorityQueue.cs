using System;
using System.Collections.Generic;

public class PriorityQueue<T, U>
{
    public struct Pair
    {
        public T Priority;
        public U Value;

        public Pair(T priority, U value)
        {
            Priority = priority;
            Value = value;
        }
    }

    public delegate bool Compare(Pair a, Pair b);

    private List<Pair> data = new List<Pair>();
    private Compare compare;

    public PriorityQueue(Compare compare)
    {
        this.compare = compare;
    }

    public bool IsEmpty()
    {
        return data.Count == 0;
    }

    public void Push(T priority, U value)
    {
        data.Add(new Pair(priority, value));
        int count = data.Count;
        if (count == 1) return;

        int current = count - 1;
        int parent;

        while (current > 0)
        {
            parent = (current - 1) / 2;

            if (compare(data[parent], data[current])) break;
            Swap(parent, current);
            
            current = parent;
        }
    }

    public Pair Pop()
    {
        int count = data.Count;
        if (count == 0) throw new Exception("The data has no values");
        
        var result = data[0];
        var last = data[count - 1];

        data.RemoveAt(count - 1);
        if (count - 1 <= 0) return result;
        
        data[0] = last;
        int current = 0;
        int child = 1;

        while (child < count - 2)
        {
            if (child < count - 3 && compare(data[child + 1], data[child])) child ++;
            if (compare(data[current], data[child])) break;
            Swap(current, child);
                
            current = child;
            child = current * 2 + 1;
        }

        return result;
    }

    private void Swap(int a, int b)
    {
        (data[a], data[b]) = (data[b], data[a]);
    }
}
