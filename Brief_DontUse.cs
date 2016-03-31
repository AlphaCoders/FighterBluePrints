// Contains IDEONE code

using System;
using System.Collections.Generic ; 

public class Test
{
	public static void printer( Dictionary<int,Fighter> _allUnits )
	{
		List<int> list = new List<int>( _allUnits.Keys );
		list.Sort();
		Fighter ft ; 
		foreach ( int key in list )
		{
			_allUnits.TryGetValue(key,out ft) ;
		    Console.WriteLine( ft.ToString() );
		}

	}
	public static void Main()
	{
		Begin b = new Begin();
		
		
		printer( b.allUnits );
		
		
		/*
		int limit = 10 ;
		while ( limit-- > 0  )
		{
			b.Update();
		}
		*/
		
	}
}


public class Begin
{
    #region Init
    
    public int gtime ;                           // Game Time
    public int fps ;                             // tmp Frame Number
    public int maxFps ;                          // Max FPS
    public int fastFactor ;                      // Ex: x 2 times the original speed
    public Dictionary<int,Fighter> allUnits ;    // Holds ( id , Fighter ) Relationship
    public int groundWidth ;                     // Battle Ground Width
    public int groundLength ;                    // Battle Ground Length
    public int[,,] grid ;                        // 3D Maxtrix ...(x,y,0) = id at x,y and (x,y,1) = gtime at x,y
    public int[,,] arrows ;                      // Same as grid but (x,y,0) = Flag(0/1) and (x,y,1) = gtime
    
    #endregion 
    
    #region Testing Data
    
    public String board ;
    
    public int UnixTime()
	{
    	DateTime epochStart=new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    	return (int)(DateTime.UtcNow - epochStart).Ticks*100 ;
	}
    
    public int Test_generateid()
    {
    	Random r = new Random( UnixTime() );
    	int tmp_id ;
    	tmp_id = r.Next( 1111 , 9999 ); 
    	while ( allUnits.ContainsKey(tmp_id) )
    	{
    		tmp_id = r.Next( 1111 , 9999 ); 
    	}
    	return tmp_id ;
    }
    
    public void Test_addFighter( int _id , int _wid , int _pid , int _x , int _y )
    {
    	Fighter obj ; 
    
    	obj = new Fighter( _id , 0 , _wid , _pid , _x , _y , 0 );
    	
    	allUnits.Add( _id , obj );
    }
    
    public void Test_populategrid( String _board )
    {
    	for ( int i = 0 ; i < groundLength ; i++ )
    	{
    		for ( int j = 0 ; j < groundWidth ; j++ )
    		{
    			char c =  _board[ ( i * groundWidth ) + j ] ;
    			switch ( c )
    			{
					case 'p' :
    					grid[i,j,0] = Test_generateid() ;
    					Test_addFighter( grid[i,j,0] , 1 , 1 , i , j );// _id , _wid = 1 pike , 2 arch , 3 cav , _pid 
    					grid[i,j,1] = gtime ;
    					break ; 
					case 'e' :
						grid[i,j,0] = Test_generateid() ;
						Test_addFighter( grid[i,j,0] , 3 , 2 , i , j );
    					grid[i,j,1] = gtime ;
    					break ; 
					default :
						grid[i,j,0] = 0 ;
						grid[1,j,1] = gtime ;
						break ;
    			}
    		}
    	}
    }
    
    #endregion
    
    #region Constructors
    
    public Begin()
    {
        gtime = 0 ;
        fps = 0 ;
        maxFps = 30 ;
        fastFactor = 1 ;
        
        allUnits = new Dictionary<int,Fighter>();
        groundWidth = 12 ;
        groundLength = 15 ;
        grid = new int[groundLength,groundWidth,2] ;
        arrows = new int[groundLength,groundWidth,2] ;
        
        // Here we must add our capital
        // Also add enemy capitals as Static Units
        
        board = 
        "............" + // Player 1 
        "........p..." +
        "............" +
        "............" +
        "............" +
        "............" +
        "............" +
        "............" +
        "............" +
        "............" +
        "............" +
        "............" +
        "............" +
        "..e........." +
        "............"   // Player 2 
        ;
        
      	Test_populategrid( board );
    }
    
    #endregion
    
    #region Graphics Engine Screen Update Method
    
    public void Update() 
    {
        if ( fps == maxFps / fastFactor ) // Equivalent to 1 second in gametime
        {
            gtime++ ;
            fps = 0 ;
        }
        
        // Here we make the movement and attacks and health deductions
        
        
        fps++ ;
        
    }
    
    #endregion
}


public class Fighter
{
	
    #region Fighter Details
    
    public int id ;          // Unique ID
    public int gid ;         // Group ID 
    public int wid ;         // Weapon ID
    public int pid ;         // Player ID
    public int x ;           // Fighters X Cor
    public int y ;           // Fighters Y Cor
    public int z ;           // Fighters Z Cor
    public int health ;      // Health or Hitpoints
    public int attack ;      // Weapon Attack
    public int handattack ;  // Hand Attack
    public int areaattack ;  // Area Attack
    public int radius ;      // Area Attack Radius
    public int speed ;       // Movement Speed
    public int delay ;       // Delay betwen attacks
    public int mode ;        // Idle[0] , Walk[1] , Attack[2] , Hurt[3] , Die[4]
    public int angle ;       // Fighters angle along SkyToEarth Axis
    
    #endregion
    
    #region Enemy Details
    
    public int eid ;         // enemy whom the fighter is going to attack
    public int egid ;        // enemy group which is fighter is going to attack
    public int ex ;          // enemy X Cor
    public int ey ;          // enemy Y Cor
    public int ez ;          // enemy Z Cor
    public int teid ;        // temporary enemy in betwen fighter and enemy
    public int tex ;         // temporary enemy X Cor
    public int tey ;         // temporary enemy Y Cor
    public int tez ;         // temporary enemy Z Cor
    
    #endregion
    
    #region Constructors
    
    public Fighter( int _id , int _gid , int _wid , int _pid , int _x , int _y , int _z )
    {
		id = _id ;
		gid = _gid ;
		wid = _wid ;
		pid = _pid ;
		x = _x ;
		y = _y ;
		z = _z ;
		mode = 0 ;
		angle = 0 ;
		giveWeapon( _wid );
    }
    
    #endregion
    
    #region Managing Methods
    
    private void giveWeapon( int _wid )
    {
    	Constants c = new Constants( _wid );
    	health = c.getHealth();
    	attack = c.getAttack();
    	handattack = c.getHandattack();
    	areaattack = c.getAreaattack();
    	radius = c.getRadius();
    	speed = c.getSpeed();
    	delay = c.getDelay();
    }
    
   
    
    public override String ToString()
    {
    	return String.Format(
    		"     ID : {0} \n " +
        //  "    GID : {1} \n " +
    		"    WID : {2} \n " +
    		"    PID : {3} \n " +
    		"      X : {4} \n " +
    		"      Y : {5} \n " +
    	//  "      Z : {6} \n " +
    		" HEALTH : {7} \n " +
    		" ATTACK : {8} \n " +
    	"",id,gid,wid,pid,x,y,z,health,attack);
    }
    
    #endregion
    
}


public class Constants
{
    private int wid ;
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
            health = 300 ;
            attack = 60 ;
            handattack = attack ;
            speed = 5 ;
            delay = 1 ;
        }
        else if ( wid == 2 ) // Archers
        {
            health = 130 ;
            attack = 20 ;
            handattack = 10 ;
            speed = 7 ;
            delay = 5 ;
        }
        else if ( wid == 3 ) // Cavalry
        {
            health = 700 ;
            attack = 150 ;
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


public class Arrow
{
    private int id ;              // Fighter id who fired Arrow
    private int eid ;             // Target Enemy id 
    private int dmg ;             // Damage value when Arrow hits target
    private int ax ;              // Arrows X Cor
    private int ay ;              // Arrows Y Cor
    private int az ;              // Arrows Z Cor
    private int angle ;           // Arrows rotation along SkyToEarth Axis ( theta )
    private int upangle ;         // Arrows rotation wrt XY Plane and Z ( Phi )
    
    #region Constructors
    
    public Arrow( int _id , int _eid , int _dmg )
    {
        id = _id ;
        eid = _eid ;
        dmg = _dmg ;
        // ax , ay , az should be derived from fighter class using dictionary
        // initialize angle , upangle and process the path in Managing Methods section
        // need parabolic trajectory managing methods here
    }
    
    #endregion

    #region Managing Methods

    #endregion

}







