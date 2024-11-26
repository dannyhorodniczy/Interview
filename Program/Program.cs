using FluentAssertions;
using System;
using System.Collections.Generic;

namespace Program;

/*
 * Shopify Pair programming interview 1
 * Starts off super basic:
 * --> build a robot that navigates around a room (move, turnLeft, turnRight)
 * --> once you are near completion of the implementation, something new is added
 * --> sometimes it's functionality, sometimes it's writing tests
 * --> I was asked to add an obstacle placer and ensure that the roomba could not move through obstacles
 * --> there is a mistake in my solution, when the roomba changes position, it does not update the hashset
 * --> the hashset is supposed to be the shared state between the obstaclePlacer and the roomba
 * --> so now the obstaclePlacer could place an object on the roomba (whoops)
 * --> follow up questions: what would you change about this implementation?
 * --> I said: add tests, encapsulate the hashmap (weird that it's public)
 * --> we discussed having map objects that would accept a # of roombas (to encapsulate the map)
 * --> and their starting positions, as well as obstacle locations
 * --> he asked about a 3D implementation, how would it change the implementation?
 * --> he asked about what if we
 */

public class Program
{
    public static void Main(string[] args)
    {
        var hashset = new HashSet<(int, int)>();
        var roomba = new Roomba(hashset);


        var obstaclePlacer = new ObstaclePlacer(hashset);
        obstaclePlacer.PlaceObstacle((2, 3));

        roomba.Move();
        roomba.Move();
        roomba.Move();
        roomba.TurnRight();
        roomba.Move();
        roomba.Move().Should().BeFalse();

        // place an object at (2,3)
        // object cannot be placed at rooba location
        // roomba cannot occupy the space of the object


    }
}

public class ObstaclePlacer
{
    private readonly HashSet<(int, int)> _occupiedSpaces;

    public ObstaclePlacer(HashSet<(int, int)> occupiedSpaces)
    {
        _occupiedSpaces = occupiedSpaces;
    }

    public bool PlaceObstacle((int x, int y) obstaclePosition)
    {
        if (_occupiedSpaces.Contains(obstaclePosition))
        {
            return false;
        }

        return _occupiedSpaces.Add(obstaclePosition);
    }
}

public interface IRoomba
{
    string CurrentDirection();

    (int, int) CurrentPosition();

    // move in your current direction 1 space
    bool Move();

    void TurnLeft();

    void TurnRight();
}

public class Roomba : IRoomba
{
    private (int, int) _currentPosition;
    private int _currentDirection;

    // cartesian coordinates
    private int[][] _cwDirections = [
        [0,1], // up 0
        [1,0], // right 1
        [0,-1], // down 2
        [-1,0]  // left 3
        ];
    private readonly HashSet<(int, int)> _occupiedSpaces;

    private enum Direction
    {
        North,
        East,
        South,
        West
    }

    public Roomba(HashSet<(int, int)> occupiedSpaces)
    {
        _currentPosition = (0, 0);
        _currentDirection = 0;
        Console.WriteLine($"Direction: {(Direction) _currentDirection}, Location: {_currentPosition.Item1}, {_currentPosition.Item2}");
        this._occupiedSpaces = occupiedSpaces;
    }

    public string CurrentDirection()
    {
        return $"{(Direction) _currentDirection}";
    }

    public (int, int) CurrentPosition()
    {
        return _currentPosition;
    }

    public bool Move()
    {
        switch (_currentDirection)
        {
            case 0: // north
                if (_occupiedSpaces.Contains((_currentPosition.Item1, _currentPosition.Item2 + 1)))
                {
                    Console.WriteLine("This space is occupied!");
                    return false;
                }
                _currentPosition.Item2++;
                break;
            case 1: // east
                if (_occupiedSpaces.Contains((_currentPosition.Item1 + 1, _currentPosition.Item2)))
                {
                    Console.WriteLine("This space is occupied!");
                    return false;
                }
                _currentPosition.Item1++;
                break;
            case 2: // south
                if (_occupiedSpaces.Contains((_currentPosition.Item1, _currentPosition.Item2 - 1)))
                {
                    Console.WriteLine("This space is occupied!");
                    return false;
                }
                _currentPosition.Item2--;
                break;
            case 3: // west
                if (_occupiedSpaces.Contains((_currentPosition.Item1 - 1, _currentPosition.Item2)))
                {
                    Console.WriteLine("This space is occupied!");
                    return false;
                }
                _currentPosition.Item1--;
                break;
            default:
                break;
        }

        Console.WriteLine($"Direction: {(Direction) _currentDirection}, Location: {_currentPosition.Item1}, {_currentPosition.Item2}");

        return true;
    }

    public void TurnLeft()
    {
        if (_currentDirection == 0)
        {
            _currentDirection = 3;
        }
        else
        {
            _currentDirection--;
        }

        Console.WriteLine($"Direction: {(Direction) _currentDirection}, Location: {_currentPosition.Item1}, {_currentPosition.Item2}");
    }

    public void TurnRight()
    {
        if (_currentDirection == 3)
        {
            _currentDirection = 0;
        }
        else
        {
            _currentDirection++;
        }

        Console.WriteLine($"Direction: {(Direction) _currentDirection}, Location: {_currentPosition.Item1}, {_currentPosition.Item2}");
    }
}

/*
 * 1. Make a Roomba API
 * --> The API can: turn left and right, go forward
 */