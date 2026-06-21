using System;
using System.Collections;
using System.Collections.Generic;
class Program
{
    static void Main(string[] args)
    {
        // Проверка добавления и расширения массива
        var stack = new SmartStack<int>();
        stack.Push(10);
        stack.Push(20);
        stack.Push(30);
        stack.Push(40);

        Console.WriteLine($"После 4 элементов: Count = {stack.Count}, Capacity = {stack.Capacity}");

        stack.Push(50);
        Console.WriteLine($"После 5-го элемента: Count = {stack.Count}, Capacity = {stack.Capacity}");

        // Проверка извлечения данных
        Console.WriteLine($"Вершина (Peek): {stack.Peek()}");
        Console.WriteLine($"Удалено (Pop): {stack.Pop()}");
        Console.WriteLine($"Новая вершина: {stack.Peek()}");

        // Проверка поиска элементов
        Console.WriteLine($"Содержит 20: {stack.Contains(20)}");
        Console.WriteLine($"Содержит 99: {stack.Contains(99)}");

        // Тест перебора элементов
        Console.Write("Содержимое стека: ");
        foreach (var item in stack)
        {
            Console.Write($"{item} ");
        }
        Console.WriteLine();

        // Тест массового добавления
        var batch = new int[] { 100, 200, 300 };
        stack.PushRange(batch);
        Console.WriteLine($"После PushRange: Count = {stack.Count}, Вершина = {stack.Peek()}");

        // Тест индексатора
        Console.WriteLine($"Элемент на глубине 0: {stack[0]}");
        Console.WriteLine($"Элемент на глубине 2: {stack[2]}");
    }
}

public class SmartStack<T> : IEnumerable<T>
{
    private T[] _storage;
    public int Count { get; private set; }
    public int Capacity => _storage.Length;

    // 1. Конструктор по умолчанию
    public SmartStack()
    {
        _storage = new T[4];
        Count = 0;
    }
    // 2. Конструктор с размером
    public SmartStack(int initialSize)
    {
        if (initialSize < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(initialSize), "Размер не может быть меньше нуля.");
        }

        _storage = new T[initialSize];
        Count = 0;
    }
    // 3. Конструктор из коллекции 
    public SmartStack(IEnumerable<T> sourceData)
    {
        if (sourceData == null)
            throw new ArgumentNullException(nameof(sourceData));

        int requiredLength = 0;
        using (var enumerator = sourceData.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                requiredLength++;
            }
        }

        _storage = new T[requiredLength == 0 ? 4 : requiredLength];
        Count = 0;

        using (var enumerator = sourceData.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                _storage[Count] = enumerator.Current;
                Count++;
            }
        }
    }
    // 4. Метод Push
    public void Push(T value)
    {
        if (Count >= _storage.Length)
        {
            ExpandStorage(_storage.Length == 0 ? 4 : _storage.Length * 2);
        }

        _storage[Count] = value;
        Count++;
    }
    // 5. Метод PushRange
    public void PushRange(IEnumerable<T> sourceData)
    {

        if (sourceData == null)
            throw new ArgumentNullException(nameof(sourceData));

        int elementsToAdd = 0;
        foreach (var element in sourceData)
        {
            elementsToAdd++;
        }

        if (elementsToAdd == 0) return;

        int projectedSize = Count + elementsToAdd;

        if (projectedSize > _storage.Length)
        {
            int newLimit = _storage.Length > 0 ? _storage.Length : 4;
            while (newLimit < projectedSize)
            {
                newLimit *= 2;
            }
            ExpandStorage(newLimit);
        }
        foreach (var element in sourceData)
        {
            _storage[Count] = element;
            Count++;
        }
    }
    // 6. Метод Pop
    public T Pop()
    {
        if (Count <= 0)
            throw new InvalidOperationException("Попытка извлечения из пустого стека.");

        Count--;
        T topValue = _storage[Count];
        _storage[Count] = default;

        return topValue;
    }
    // 7. Метод Peek
    public T Peek()
    {
        if (Count <= 0)
            throw new InvalidOperationException("Попытка чтения из пустого стека.");

        return _storage[Count - 1];
    }
    // 8. Метод Contains
    public bool Contains(T value)
    {
        for (int k = 0; k < Count; k++)
        {
            if (EqualityComparer<T>.Default.Equals(_storage[k], value))
            {
                return true;
            }
        }
        return false;
    }
    // Индексатор
    public T this[int depthIndex]
    {
        get
        {
            if (depthIndex < 0 || depthIndex >= Count)
                throw new ArgumentOutOfRangeException(nameof(depthIndex));

            return _storage[Count - 1 - depthIndex];
        }
        set
        {
            if (depthIndex < 0 || depthIndex >= Count)
                throw new ArgumentOutOfRangeException(nameof(depthIndex));

            _storage[Count - 1 - depthIndex] = value;
        }
    }
    // Реализация IEnumerable<T>
    public IEnumerator<T> GetEnumerator()
    {
        int index = Count - 1;
        while (index >= 0)
        {
            yield return _storage[index];
            index--;
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    // Вспомогательный метод 
    private void ExpandStorage(int newSize)
    {
        T[] expandedArray = new T[newSize];
        Array.Copy(_storage, expandedArray, Count);
        _storage = expandedArray;
    }
}
