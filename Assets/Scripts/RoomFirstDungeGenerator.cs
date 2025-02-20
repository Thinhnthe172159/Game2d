using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFirstDungeGenerator : SimpleRamdomWalkDungenonGenerator
{
    [SerializeField]
    private int minRoomWidth = 3, minRoomHeight = 3;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomList = ProceduralRenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition,
            new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomList);
        }
        else
        {
            floor = CreateSimpleRooms(roomList);
        }

        List<Vector2Int> roomCenter = new List<Vector2Int>();
        foreach (var room in roomList)
        {
            roomCenter.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenter);
        floor.UnionWith(corridors);
        titlemapVisualizer.PainFloorTiles(floor);
        WallGenerator.CreateWalls(floor, titlemapVisualizer);
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomList.Count; i++)
        {
            var roomBounds = roomList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            foreach (var position in roomFloor)
            {
                if(position.x >= (roomBounds.xMin + offset) && position.x < (roomBounds.xMax - offset) &&
                    position.y >= (roomBounds.yMin + offset) && position.y < (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenter)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenter[Random.Range(0, roomCenter.Count)];
        roomCenter.Remove(currentRoomCenter);
        while (roomCenter.Count > 0)
        {
            Vector2Int closest = FindClosestPoinTo(currentRoomCenter, roomCenter);
            roomCenter.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int closest)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);

        Vector2Int lastDirection = Vector2Int.zero;
        Vector2Int perpendicularDirection = Vector2Int.zero;

        while (position.y != closest.y)
        {
            if (closest.y > position.y)
            {
                position += Vector2Int.up;
                lastDirection = Vector2Int.up;
            }
            else if (closest.y < position.y)
            {
                position += Vector2Int.down;
                lastDirection = Vector2Int.down;
            }

            perpendicularDirection = GetPerpendicularDirection(lastDirection);
            corridor.Add(position);
            corridor.Add(position + perpendicularDirection);
        }

        while (position.x != closest.x)
        {
            if (closest.x > position.x)
            {
                position += Vector2Int.right;
                lastDirection = Vector2Int.right;
            }
            else if (closest.x < position.x)
            {
                position += Vector2Int.left;
                lastDirection = Vector2Int.left;
            }

            perpendicularDirection = GetPerpendicularDirection(lastDirection);
            corridor.Add(position);

            if (lastDirection == Vector2Int.    right || lastDirection == Vector2Int.left)
            {
                corridor.Add(position + Vector2Int.up);
            }
            else
            {
                corridor.Add(position + Vector2Int.right);
            }
        }

        return corridor;
    }

    private Vector2Int GetPerpendicularDirection(Vector2Int direction)
    {
        if (direction == Vector2Int.up || direction == Vector2Int.down)
            return Vector2Int.right;
        else
            return Vector2Int.up;
    }


    private Vector2Int FindClosestPoinTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenter)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenter)
        {
            float currentDistance = Vector2Int.Distance(currentRoomCenter, position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
}
