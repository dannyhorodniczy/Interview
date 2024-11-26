using FluentAssertions;
using Program;
using System.Collections.Generic;
using Xunit;

namespace Tests;

public class RoombaUnitTests
{
    [Fact]
    public void GivenASetOfInstructions_WhenExecuteInstructions_ThenRoombaIsAtExpectedLocation()
    {
        // Given
        var hashset = new HashSet<(int, int)>();
        var roomba = new Roomba(hashset);

        // When
        roomba.TurnLeft();
        roomba.CurrentDirection().Should().Be("West");
        roomba.TurnLeft();
        roomba.CurrentDirection().Should().Be("South");
        roomba.TurnLeft();
        roomba.CurrentDirection().Should().Be("East");
        roomba.TurnLeft();
        roomba.CurrentDirection().Should().Be("North");

        // Then
        roomba.Move();
        roomba.Move();
        roomba.Move();
        roomba.TurnRight();
        roomba.Move();
        roomba.Move();
        roomba.CurrentDirection().Should().Be("East");
        roomba.CurrentPosition().Should().Be((2, 3));
    }
}