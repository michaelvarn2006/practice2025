namespace task04;

public interface ISpaceship
{
    void MoveForward();      
    void Rotate(int angle);  
    void Fire();            
    int Speed { get; }      
    int FirePower { get; }  
}

public class Cruiser : ISpaceship
{
    public int Speed { get; }
    public int FirePower { get; }
    public int TotalDistance { get; set; }
    public int TotalAngle { get; set; }
    public int TotalFire { get; set; }

    public Cruiser()
    {
        Speed = 50;
        FirePower = 100;
    }
    public void MoveForward()
    {
        TotalDistance = TotalDistance + Speed;
    }

    public void Rotate(int angle)
    {
        TotalAngle = (angle + TotalAngle) % 360;
    } 
    

    public void Fire()
    {
        TotalFire = TotalFire + FirePower;
    }
}

public class Fighter : ISpaceship
{
    public int Speed { get; }
    public int FirePower { get; }
    public int TotalDistance { get; set; }
    public int TotalAngle { get; set; }
    public int TotalFire { get; set; }
    public Fighter()
    {
        Speed = 100;
        FirePower = 50;
    }
    public void MoveForward()
    {
        TotalDistance = TotalDistance + Speed;
    }

    public void Rotate(int angle)
    {
        TotalAngle = (angle + TotalAngle) % 360;
    }


    public void Fire()
    {
        TotalFire = TotalFire + FirePower;
    }
}
