using Xunit;
using task04;
namespace task04tests;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        ISpaceship fighter = new Fighter();
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(50, fighter.FirePower);
    }

    [Fact]
    public void Fighter_ShouldnBeStrongerThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.FirePower < cruiser.FirePower);
    }
    [Fact]
    public void Cruiser_TotalStatsCheck()
    {
        var cruiser = new Cruiser();
        cruiser.MoveForward();
        cruiser.MoveForward();
        cruiser.Rotate(360);
        cruiser.Rotate(5);
        cruiser.Fire();
        Assert.Equal(100, cruiser.TotalDistance);
        Assert.Equal(5, cruiser.TotalAngle);
        Assert.Equal(100, cruiser.TotalFire);
    }
    [Fact]
    public void Fighter_TotalStatsCheck()
    {
        var fighter = new Fighter();
        fighter.MoveForward();
        fighter.MoveForward();
        fighter.Rotate(410);
        fighter.Fire();
        Assert.Equal(200, fighter.TotalDistance);
        Assert.Equal(50, fighter.TotalAngle);
        Assert.Equal(50, fighter.TotalFire);
    }
}
