using System;
using System.Collections.Generic;

namespace BW
{
public class PriorityQueue<T>
{
    private List<(T Item, int Priority)> heap = new List<(T, int)>();

    private int Compare(int left, int right)
    {
        return heap[left].Priority.CompareTo(heap[right].Priority); // 우선순위 낮은 값이 더 높은 우선순위
    }

    public void Enqueue(T item, int priority)
    {
        heap.Add((item, priority));
        HeapifyUp(heap.Count - 1); // 새 요소를 힙에 추가한 뒤 재정렬
    }

    public T Dequeue()
    {
        if (heap.Count == 0)
        {
            throw new InvalidOperationException("Queue is empty");
        }

        var root = heap[0].Item;

        // 힙의 마지막 요소를 루트로 이동 후 재정렬
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);
        HeapifyDown(0);

        return root;
    }

    public void Clear()
    {
        heap.Clear();
    }

    public bool IsEmpty()
    {
        return heap.Count == 0;
    }

    public int Count
    {
        get { return heap.Count; }
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;

            if (Compare(index, parentIndex) >= 0)
            {
                break;
            }

            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void HeapifyDown(int index)
    {
        while (true)
        {
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;
            int smallest = index;

            if (leftChild < heap.Count && Compare(leftChild, smallest) < 0)
            {
                smallest = leftChild;
            }

            if (rightChild < heap.Count && Compare(rightChild, smallest) < 0)
            {
                smallest = rightChild;
            }

            if (smallest == index)
            {
                break;
            }

            Swap(index, smallest);
            index = smallest;
        }
    }

    private void Swap(int indexA, int indexB)
    {
        var temp = heap[indexA];
        heap[indexA] = heap[indexB];
        heap[indexB] = temp;
    }
}

}


