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
            _items = new Dictionary<Item, Vector2Int>(16);
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

            int sizeX = item.Size.x;
            int sizeY = item.Size.y;

            ThrowIfInvalidSize(sizeX, sizeY);

            if (_items.ContainsKey(item))
                return false;

            return IsFreeSpace(startX, startY, startX + sizeX, startY + sizeY);
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

            if (_items.ContainsKey(item))
                return false;

            int sizeX = item.Size.x;
            int sizeY = item.Size.y;

            if (!FindFreePosition(sizeX, sizeY, out Vector2Int position))
                return false;

            PlaceItem(item, sizeX, sizeY, position);
            OnAdded?.Invoke(item, position);
            return true;
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
            return FindFreePositionInternal(sizeX, sizeY, out position);
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
            return _grid[position.x, position.y] != null;
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
            return _grid[position.x, position.y] == null;
        }

        public bool IsFree(int x, int y)
        {
            return _grid[x, y] == null;
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
            if (item == null)
                throw new NullReferenceException($"Null reference {typeof(Item)}!");
            
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
            return true;
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
            
            foreach (var key in _items.Keys)
            {
                if (string.Equals(key.Name, name))
                    count++;
            }

            return count;
        }

        public bool MoveItem(Item item, Vector2Int position)
        {
            ThrowIfArgumentNull(item);

            if (!_items.TryGetValue(item, out Vector2Int oldPosition))
                return false;

            if (position == oldPosition)
                return true;

            int sizeX = item.Size.x;
            int sizeY = item.Size.y;
            int endX = position.x + sizeX;
            int endY = position.y + sizeY;

            if (position.x < 0 || position.y < 0 || endX > _width || endY > _height)
                return false;

            for (int x = position.x; x < endX; x++)
                for (int y = position.y; y < endY; y++)
                    if (_grid[x, y] != null && _grid[x, y] != item)
                        return false;

            for (int x = oldPosition.x, oldEndX = oldPosition.x + sizeX; x < oldEndX; x++)
                for (int y = oldPosition.y, oldEndY = oldPosition.y + sizeY; y < oldEndY; y++)
                    _grid[x, y] = null;

            for (int x = position.x; x < endX; x++)
                for (int y = position.y; y < endY; y++)
                    _grid[x, y] = item;

            _items[item] = position;
            OnMoved?.Invoke(item, position);
            return true;
        }

        /// <summary>
        /// Rearranges an inventory space with max free slots 
        /// </summary>
        public void OptimizeSpace()
        {
            ClearGrid();

            int count = _items.Count;
            var itemsArray = new Item[count];
            var areas = new int[count];
            
            _items.Keys.CopyTo(itemsArray, 0);

            for (int i = 0; i < count; i++)
                areas[i] = -(itemsArray[i].Size.x * itemsArray[i].Size.y);
            
            Array.Sort(areas, itemsArray);

            _items.Clear();

            foreach (Item item in itemsArray)
            {
                int sizeX = item.Size.x;
                int sizeY = item.Size.y;

                if (!FindFreePositionInternal(sizeX, sizeY, out Vector2Int pos))
                    continue;

                for (int x = pos.x, endX = pos.x + sizeX; x < endX; x++)
                    for (int y = pos.y, endY = pos.y + sizeY; y < endY; y++)
                        _grid[x, y] = item;

                _items.Add(item, pos);
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
            return _items.Keys.GetEnumerator();
        }

        /// <summary>
        /// Copies items to a specified matrix
        /// </summary>
        public void CopyTo(Item[,] matrix)
        {
            Array.Copy(_grid, matrix, _grid.Length);
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

            int sizeX = item.Size.x;
            int sizeY = item.Size.y;

            ThrowIfInvalidSize(sizeX, sizeY);

            if (_items.ContainsKey(item))
                return false;

            if (!IsFreeSpace(position.x, position.y, position.x + sizeX, position.y + sizeY))
                return false;

            PlaceItem(item, sizeX, sizeY, position);
            return true;
        }

        private void PlaceItem(Item item, int sizeX, int sizeY, Vector2Int position)
        {
            for (int i = position.x, endX = position.x + sizeX; i < endX; i++)
                for (int j = position.y, endY = position.y + sizeY; j < endY; j++)
                    _grid[i, j] = item;

            _items.Add(item, position);
        }

        private static Vector2Int[] GetPositionsInternal(Item item, Vector2Int position)
        {
            int sizeX = item.Size.x;
            int sizeY = item.Size.y;

            var positions = new Vector2Int[sizeX * sizeY];
            int curPositionIndex = 0;

            for (int i = position.x, countX = position.x + sizeX; i < countX; i++)
                for (int j = position.y, countY = position.y + sizeY; j < countY; j++)
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

            int sizeX = item.Size.x;
            int sizeY = item.Size.y;

            for (int i = position.x, endX = position.x + sizeX; i < endX; i++)
                for (int j = position.y, endY = position.y + sizeY; j < endY; j++)
                    _grid[i, j] = null;

            _items.Remove(item);
            return true;
        }
        
        private bool FindFreePositionInternal(int sizeX, int sizeY, out Vector2Int position)
        {
            position = Vector2Int.zero;
            int maxY = _height - sizeY;
            int maxX = _width - sizeX;

            for (int y = 0; y <= maxY; y++)
            {
                int endY = y + sizeY;
                int x = 0;
                while (x <= maxX)
                {
                    int blockerX = FindBlockerColumn(x, y, x + sizeX, endY);
                    if (blockerX < 0)
                    {
                        position = new Vector2Int(x, y);
                        return true;
                    }

                    x = blockerX + 1;
                }
            }

            return false;
        }

        private int FindBlockerColumn(int startX, int startY, int endX, int endY)
        {
            for (int x = startX; x < endX; x++)
                for (int y = startY; y < endY; y++)
                    if (_grid[x, y] != null)
                        return x;

            return -1;
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
    }
}
