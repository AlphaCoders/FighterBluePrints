
public class Constants
{
    private int wid ;
    private int range ;
    private int health ;
    private int attack ;
    private int handattack ;
    private int areaattack ;
    private int radius ;
    private int speed ;
    private int delay ;
    
    #region Constructors
    
    public Constants( int _wid )
    {
        wid = _wid ;
        range = 0 ;
        health = 0 ;
        attack = 0 ;
        handattack = 0 ;
        areaattack = 0 ;
        radius = 0 ;
        speed = 0 ;
        delay = 0 ;
        if ( wid <= 0 )
        {
            return ; // Err Log
        }
        else if ( wid == 1 ) // PikeMen
        {   
        	range = 2 ;
            health = 300 ;
            attack = 60 ;
            handattack = attack ;
            speed = 5 ;
            delay = 1 ;
        }
        else if ( wid == 2 ) // Archers
        {
        	range = 12 ;
            health = 130 ;
            attack = 20 ;
            handattack = 10 ;
            speed = 7 ;
            delay = 5 ;
        }
        else if ( wid == 3 ) // Cavalry
        {
        	range = 3 ;
            health = 700 ;
            attack = 90 ;
            handattack = attack ;
            speed = 11 ;
            delay = 1 ;
        }
        else 
        {
            return ; // Err Log
        }
 
    }
    
    #endregion
    
    #region Getter Methods
    
    public int getRange( )
    {
        return range ;
    }
    
    public int getHealth( )
    {
        return health ;
    }
    
    public int getAttack ()
    {
        return attack ;
    }
    
    public int getHandattack ()
    {
        return handattack ;
    }
    
    public int getAreaattack ()
    {
        return areaattack ;
    }
    
    public int getRadius ()
    {
        return radius ;
    }
    
    public int getSpeed ()
    {
        return speed ;
    }
    
    public int getDelay()
    {
        return delay ;
    }

    #endregion

}
