using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Modules.Inventories
{
    public class Inventory : IEnumerable<Item>
    {
        public event Action<Item, Vector2Int> OnAdded;
        public event Action<Item, Vector2Int> OnRemoved;
        public event Action<Item, Vector2Int> OnMoved;
        public event Action OnCleared;

        private readonly Item[,] _grid;
        private readonly Dictionary<Item, Vector2Int> _items;
        private readonly int _width;
        private readonly int _height;

        public int Width => _width;
        public int Height => _height;
        public int Count => _items.Count;
        
        public Inventory(int width, int height)
        {
            ThrowIfInvalidSize(width, height);

            _width = width;
            _height = height;
            _grid = new Item[width, height];
            _items = new Dictionary<Item, Vector2Int>();
        }

        public Inventory(
            int width,
            int height,
            params KeyValuePair<Item, Vector2Int>[] items
        ) : this(width, height, (IEnumerable<KeyValuePair<Item, Vector2Int>>) items)
        {
        }

        public Inventory(
            int width,
            int height,
            params Item[] items
        ) : this(width, height, (IEnumerable<Item>) items)
        {
        }

        public Inventory(
            int width,
            int height,
            IEnumerable<KeyValuePair<Item, Vector2Int>> items
        ) : this(width, height)
        {
            ThrowIfArgumentNull(items);
            
            foreach (var kvp in items)
                AddItem(kvp.Key, kvp.Value);
        }

        public Inventory(
            int width,
            int height,
            IEnumerable<Item> items
        ) : this(width, height)
        {
            ThrowIfArgumentNull(items);
            
            foreach (var item in items)
                AddItem(item);
        }

        /// <summary>
        /// Creates new inventory
        /// </summary>
        public Inventory(Inventory inventory) : this(inventory._width, inventory._height)
        {
            foreach (var kvp in inventory._items)
            {
                var clone = kvp.Key.Clone();
                AddItem(clone, kvp.Value);
            }
        }

        /// <summary>
        /// Checks for adding an item on a specified position
        /// </summary>
        public bool CanAddItem(Item item, Vector2Int position)
        {
            return CanAddItem(item, position.x, position.y);
        }

        public bool CanAddItem(Item item, int startX, int startY)
        {
            if (item == null)
                return false;

            ThrowIfInvalidSize(item);
            
            if (_items.ContainsKey(item))
                return false;
            
            return IsFreeSpace(startX, startY, startX + item.Size.x, startY + item.Size.y);
        }

        /// <summary>
        /// Adds an item on a specified position
        /// </summary>
        public bool AddItem(Item item, Vector2Int position)
        {
            if (!AddItemInternal(item, position))
                return false;

            OnAdded?.Invoke(item, position);
            return true;
        }

        public bool AddItem(Item item, int startX, int startY)
        {
            return AddItem(item, new Vector2Int(startX, startY));
        }
        
        /// <summary>
        /// Checks for adding an item on a free position
        /// </summary>
        public bool CanAddItem(Item item)
        {
            if (item == null)
                return false;
            
            if(_items.ContainsKey(item))
                return false;
            
            return FindFreePosition(item, out _);
        }

        /// <summary>
        /// Adds an item on a free position
        /// </summary>
        public bool AddItem(Item item)
        {
            if (item == null)
                return false;
            
            if (!FindFreePosition(item, out Vector2Int position))
                return false;
            
            return AddItem(item, position);
        }

        /// <summary>
        /// Returns a free position for a specified item
        /// </summary>
        public bool FindFreePosition(Item item, out Vector2Int position)
        {
            return FindFreePosition(item.Size.x, item.Size.y, out position);
        }

        public bool FindFreePosition(Vector2Int size, out Vector2Int position)
        {
            return FindFreePosition(size.x, size.y, out position);
        }

        public bool FindFreePosition(int sizeX, int sizeY, out Vector2Int position)
        {
            ThrowIfInvalidSize(sizeX, sizeY);
            
            position = Vector2Int.zero;
            
            for (int y = 0; y <= _height - sizeY; y++)
                for (int x = 0; x <= _width - sizeX; x++)
                {
                    if (!IsFreeSpace(x, y, x + sizeX, y + sizeY))
                        continue;

                    position = new Vector2Int(x, y);
                    return true;
                }

            return false;
        }

        /// <summary>
        /// Checks if the specified element exists
        /// </summary>
        public bool Contains(Item item)
        {
            if (item == null)
                return false;
            
            return _items.ContainsKey(item);
        }

        /// <summary>
        /// Checks if the specified position is occupied
        /// </summary>
        public bool IsOccupied(Vector2Int position)
        {
            return !IsFreeSpace(position.x, position.y, position.x, position.y);
        }

        public bool IsOccupied(int x, int y)
        {
            return _grid[x, y] != null;
        }

        /// <summary>
        /// Checks if the specified position is free
        /// </summary>
        public bool IsFree(Vector2Int position)
        {
            return IsFreeSpace(position.x, position.y, position.x, position.y);
        }

        public bool IsFree(int x, int y)
        {
            return IsFreeSpace(x, y, x, y);
        }

        /// <summary>
        /// Removes specified item
        /// </summary>
        public bool RemoveItem(Item item)
        {
            return RemoveItem(item, out _);
        }

        public bool RemoveItem(Item item, out Vector2Int position)
        {
            if (!RemoveItemInternal(item, out position))
                return false;
            
            OnRemoved?.Invoke(item, position);
            return true;
        }

        /// <summary>
        /// Returns an item at specified position 
        /// </summary>
        public Item GetItem(Vector2Int position)
        {
            return _grid[position.x, position.y];
        }

        public Item GetItem(int x, int y)
        {
            return _grid[x, y];
        }

        public bool TryGetItem(Vector2Int position, out Item item)
        {
            return TryGetItem(position.x, position.y, out item);
        }
        
        public bool TryGetItem(int x, int y, out Item item)
        {
            if (IsPositionOutOfRange(x, y))
            {
                item = null;
                return false;
            }
            
            item = GetItem(x, y);
            return item != null;
        }

        /// <summary>
        /// Returns positions of a specified item 
        /// </summary>
        public Vector2Int[] GetPositions(Item item)
        {
            ThrowIfNull(item);
            
            if(!_items.TryGetValue(item, out Vector2Int position))
                throw new KeyNotFoundException($"Item with name {item.Name} doesn't exist!");

            return GetPositionsInternal(item, position);
        }

        public bool TryGetPositions(Item item, out Vector2Int[] positions)
        {
            if (item == null)
            {
                positions = null;
                return false;
            }

            if (!_items.TryGetValue(item, out Vector2Int position))
            {
                positions = null;
                return false;
            }

            positions = GetPositionsInternal(item, position);
            return positions != null;
        }

        /// <summary>
        /// Clears all items 
        /// </summary>
        public void Clear()
        {
            if(_items.Count == 0)
                return;
            
            _items.Clear();
            ClearGrid();
            OnCleared?.Invoke();
        }

        /// <summary>
        /// Returns count of items with a specified name
        /// </summary>
        public int GetItemCount(string name)
        {
            int count = 0;
            
            foreach (var pair in _items)
            {
                if (string.Equals(pair.Key.Name, name))
                    count++;
            }

            return count;
        }

        public bool MoveItem(Item item, Vector2Int position)
        {
            ThrowIfArgumentNull(item);

            if (!RemoveItemInternal(item, out Vector2Int oldPosition))
                return false;

            if (!AddItemInternal(item, position))
            {
                AddItemInternal(item, oldPosition);
                return false;
            }

            OnMoved?.Invoke(item, position);
            return true;
        }

        /// <summary>
        /// Rearranges an inventory space with max free slots 
        /// </summary>
        public void OptimizeSpace()
        {
            ClearGrid();

            var itemsArray = new Item[_items.Count];
            
            _items.Keys.CopyTo(itemsArray, 0);

            Array.Sort(itemsArray, ComparerBySize);

            foreach (Item item in itemsArray)
            {
                int curX = 0;
                int curY = 0;
                int sizeX = item.Size.x;
                int sizeY = item.Size.y;
                int endX = curX + sizeX;
                int endY = curY + sizeY;

                while (!IsFreeSpace(curX, curY, endX, endY) && curX < _width && curY < _height)
                {
                    curX++;

                    if (curX >= _width)
                    {
                        curX = 0;
                        curY++;
                    }
                    
                    endX = curX + sizeX;
                    endY = curY + sizeY;
                }
                
                for (int x = curX; x < endX; x++)
                    for (int y = curY; y < endY; y++)
                        _grid[x, y] = item;
            }
            return;

            int ComparerBySize(Item a, Item b)
            {
                int sizeA = a.Size.x * a.Size.y;
                int sizeB = b.Size.x * b.Size.y;

                if (sizeA < sizeB)
                    return 1;

                if (sizeA == sizeB)
                    return 0;

                return -1;
            }
        }

        /// <summary>
        /// Iterates by all items 
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Item> GetEnumerator()
        {
            foreach (var pair in _items)
                yield return pair.Key;
        }

        /// <summary>
        /// Copies items to a specified matrix
        /// </summary>
        public void CopyTo(Item[,] matrix)
        {
            for (int i = 0; i < _width; i++)
                for (int j = 0; j < _height; j++)
                    matrix[i, j] = _grid[i, j];
        }

        /// <summary>
        /// Returns an inventory matrix in string format
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder((_width + 1) * _height);

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    var item = _grid[x, y];
                    sb.Append(item != null ? item.Name : ".");
                }

                if (y < _height - 1)
                    sb.Append('\n');
            }

            return sb.ToString();
        }

        private bool AddItemInternal(Item item, Vector2Int position)
        {
            if (item == null)
                return false;

            ThrowIfInvalidSize(item);

            if (_items.ContainsKey(item))
                return false;

            if (!IsFreeSpace(position.x, position.y, position.x + item.Size.x, position.y + item.Size.y))
                return false;

            for (int i = position.x, endX = position.x + item.Size.x; i < endX; i++)
                for (int j = position.y, endY = position.y + item.Size.y; j < endY; j++)
                    _grid[i, j] = item;

            _items.Add(item, position);
            return true;
        }

        private static Vector2Int[] GetPositionsInternal(Item item, Vector2Int position)
        {
            var positions = new Vector2Int[item.Size.x * item.Size.y];
            int curPositionIndex = 0;

            for (int i = position.x, countX = position.x + item.Size.x; i < countX; i++)
                for (int j = position.y, countY = position.y + item.Size.y; j < countY; j++)
                {
                    positions[curPositionIndex] = new Vector2Int(i, j);
                    curPositionIndex++;
                }

            return positions;
        }
        
        private bool RemoveItemInternal(Item item, out Vector2Int position)
        {
            if (item == null)
            {
                position = Vector2Int.zero;
                return false;
            }

            if (!_items.TryGetValue(item, out position))
                return false;

            for (int i = position.x; i < position.x + item.Size.x; i++)
                for (int j = position.y; j < position.y + item.Size.y; j++)
                    _grid[i, j] = null;

            _items.Remove(item);
            return true;
        }
        
        private bool IsFreeSpace(int startX, int startY, int endX, int endY)
        {
            if (startX < 0 || startY < 0 || endX > _width || endY > _height)
                return false;

            for (int x = startX; x < endX; x++)
                for (int y = startY; y < endY; y++)
                    if (_grid[x, y] != null)
                        return false;

            return true;
        }

        private void ClearGrid()
        {
            Array.Clear(_grid, 0, _grid.Length);
        }

        private bool IsPositionOutOfRange(int x, int y)
        {
            return x >= _width || y >= _height || x < 0 || y < 0;
        }

        private void ThrowIfInvalidSize(Item item)
        {
            ThrowIfInvalidSize(item.Size.x, item.Size.y);
        }

        private void ThrowIfInvalidSize(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException($"Item size width = {width}, height = {height} is invalid!");
        }

        private void ThrowIfArgumentNull<T>(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException($"Argument {typeof(T)} is null!");
        }
        
        private void ThrowIfNull<T>(T obj)
        {
            if (obj == null)
                throw new NullReferenceException($"Null reference {typeof(T)}!");
        }
    }
}
