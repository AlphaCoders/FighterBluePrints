// Contains IDEONE code

using System;
using System.Collections.Generic ; 

public class Test
{
	public static void printer( SortedDictionary<int,Fighter> _allUnits )
	{
		foreach ( KeyValuePair<int,Fighter> entry in _allUnits )
		{
		    Console.WriteLine( entry.Value.ToString() );
		}

	}
	
	public static void gridprint( int[,,] _grid , int _gtime , SortedDictionary<int,Fighter> _allUnits )
	{
		Console.WriteLine("Printing Grid at time :"+_gtime);
		String pad ; 
		int u ;
		int v ;
		for ( int i = 0 ; i < _grid.GetLength(0) ; i++ )
		{
			for ( int j = 0 ; j < _grid.GetLength(1) ; j++ )
			{
				if ( _grid[i,j,0] == 0 ) 
				{
					pad = "...0" ; 
				}
				else 
				{
					pad = ""+_grid[i,j,0] ; 
				}
				
				/*
				v = _grid[i,j,1] ;
				pad += "," ;
				pad += v ;
				u = (""+v).Length ;
				for ( int k = 0 ; k < (3-u) ; k++ )
				{
					pad += ".";
				}
				*/
				
				if ( _grid[i,j,0] == 0 )
				{
					v = 0 ; 
				}
				else 
				{
					v = _allUnits[ _grid[i,j,0] ].health ;
				}
				pad += "," ;
				pad += v ;
				u = (""+v).Length ;
				for ( int k = 0 ; k < (3-u) ; k++ )
				{
					pad += ".";
				}
				
				Console.Write(  pad + " " );
			}
			Console.WriteLine();
		}
		Console.WriteLine("----------------------------------End at gtime:"+_gtime);
		
	}
	
	public static void Main()
	{
		Begin b = new Begin();
		
		//printer( b.allUnits );
		gridprint( b.grid , b.gtime , b.allUnits );
		
		foreach ( KeyValuePair<int,Fighter> entry in b.allUnits )
		{
			if ( entry.Value.pid == 1 && entry.Value.x > 2 )
			{
		    	entry.Value.ex = 13 ;
		    	entry.Value.ey = 1 ;
		    	entry.Value.ez = 0 ; 
			}
		}
		
		int cycles = 9 ;
		int limit = 10 + ( 40 * cycles ) ;
		while ( limit-- > 0  )
		{
			b.Update();
		}
		
		
	}
}


public class Begin
{
	// {
    #region Init 
    
    public int gtime ;                               // Game Time
    public int fps ;                                 // tmp Frame Number
    public int maxFps ;                              // Max FPS
    public int fastFactor ;                          // Ex: x 2 times the original speed
    public SortedDictionary<int,Fighter> allUnits ;  // Holds ( id , Fighter ) Relationship
    public int groundWidth ;                         // Battle Ground Width
    public int groundLength ;                        // Battle Ground Length
    public int[,,] grid ;                            // 3D Maxtrix ...(x,y,0) = id at x,y and (x,y,1) = gtime at x,y
    public SortedDictionary<int,Arrow> allArrows ;   // Holds ( arrowID , arrowObject) Relationship
    
    #endregion 
    
    #region Testing Data
    
    public String board ;
    
    public int UnixTime()
	{
    	DateTime epochStart=new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    	return (int)(DateTime.UtcNow - epochStart).Ticks*100 ;
	}
    
    public int Test_generateid( int _pid )
    {
    	Random r = new Random( UnixTime() );
    	int tmp_id ;
    	tmp_id = (_pid*1000) + r.Next( 111 , 999 ); 
    	while ( allUnits.ContainsKey(tmp_id) )
    	{
    		tmp_id = (_pid*1000)+ r.Next( 111 , 999 ); 
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
    					grid[i,j,0] = Test_generateid(1) ;
    					Test_addFighter( grid[i,j,0] , 1 , 1 , i , j );// _id , _wid = 1 pike , 2 arch , 3 cav , _pid 
    					grid[i,j,1] = gtime ;
    					break ; 
					case 'a' :
    					grid[i,j,0] = Test_generateid(1) ;
    					Test_addFighter( grid[i,j,0] , 2 , 1 , i , j );// _id , _wid = 1 pike , 2 arch , 3 cav , _pid 
    					grid[i,j,1] = gtime ;
    					break ;
					case 'e' :
						grid[i,j,0] = Test_generateid(2) ;
						Test_addFighter( grid[i,j,0] , 3 , 2 , i , j );
    					grid[i,j,1] = gtime ;
    					break ; 
					default :
						grid[i,j,0] = 0 ;
						grid[1,j,1] = 0 ;
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
        maxFps = 40 ;
        fastFactor = 1 ;
        
        allUnits = new SortedDictionary<int,Fighter>();
        groundWidth = 9 ;
        groundLength = 15 ;
        grid = new int[groundLength,groundWidth,2] ;
        allArrows = new SortedDictionary<int,Arrow>();
        // Here we must add our capital
        // Also add enemy capitals as Static Units
        
        board = 
        "........." + // Player 1 
        "........." +
        "..a......" +
        "........." +
        "....aaaa." +
        "....pppp." +
        "....pppp." +
        "........." +
        "........." +
        "........." +
        "........." +
        "........." +
        ".eeee...." +
        "..ee....." +
        "........."   // Player 2 
        ;
        
        gtime = 1 ; 
        
      	Test_populategrid( board );
    }
    
    #endregion
    //}
    #region Graphics Engine Screen Update Method
    
    public void Update() 
    {
        if ( fps == maxFps / fastFactor ) // Equivalent to 1 second in gametime
        {
        	Console.WriteLine( "Cycle Begins" );
        	List<int> deadUnits = new List<int>() ;
        	foreach ( KeyValuePair<int,Fighter> entry in allUnits )
        	{
        		if ( entry.Value.health < 1 )
				{
					deadUnits.Add( entry.Value.id ); // Registering Casualities = Dead Units
				}
        	}
        	foreach( int deadUnitID in deadUnits )
        	{
        		grid[ allUnits[deadUnitID].x , allUnits[deadUnitID].y , 0 ] = 0 ;
				grid[ allUnits[deadUnitID].x , allUnits[deadUnitID].y , 1 ] = 0 ;// Freeing grid space
        		allUnits.Remove( deadUnitID );// Cleaning up the Dead
        		// Change animation to death animation
        	}
        	
        	// Make movement 
        	Dictionary<int,int> counts = new Dictionary<int,int>() ;  // _gid , number of units belonging to gid
        	Dictionary<int,int> centroids_X  = new Dictionary<int,int>() ; // _gid , _centeroid_XCOR
        	Dictionary<int,int> centroids_Y  = new Dictionary<int,int>() ; // _gid , _centeroid_YCOR 
        	Dictionary<int,int> centroids_Z  = new Dictionary<int,int>() ; // _gid , _centeroid_ZCOR 
        	int tgid ;
        	foreach ( KeyValuePair<int,Fighter> entry in allUnits )
        	{
        		tgid = entry.Value.gid ;
        		if ( counts.ContainsKey ( tgid ) )
        		{
        			centroids_X[ tgid ] = ((centroids_X[ tgid ]*counts[tgid]) + entry.Value.x )/ (counts[tgid]+1);
        			centroids_Y[ tgid ] = ((centroids_Y[ tgid ]*counts[tgid]) + entry.Value.y )/ (counts[tgid]+1);
        			centroids_Z[ tgid ] = ((centroids_Z[ tgid ]*counts[tgid]) + entry.Value.z )/ (counts[tgid]+1);
        			counts[ tgid ] += 1 ; 
        		}
        		else 
        		{
        			centroids_X.Add( tgid , entry.Value.x ) ;
        			centroids_Y.Add( tgid , entry.Value.y ) ;
        			centroids_Z.Add( tgid , entry.Value.z ) ;
        			counts.Add( tgid , 1 ) ; 
        		}
        	}
        	
        	
        	foreach ( KeyValuePair<int,Fighter> entry in allUnits ) // Unit Processing
			{
				if ( entry.Value.eid > -1 )
				{
			    	entry.Value.mode = 2 ; //attack animation triggered
			    	if (allUnits.ContainsKey( entry.Value.eid ))
			    	{
			    		entry.Value.ex = allUnits[ entry.Value.eid ].x ;
			    		entry.Value.ey = allUnits[ entry.Value.eid ].y ;
			    		entry.Value.ez = allUnits[ entry.Value.eid ].z ;
			    	}
			    	else
			    	{
			    		entry.Value.mode = 0 ;
			    	}
				}
				else if ( entry.Value.egid > -1 )
				{
					entry.Value.ex = centroids_X[ entry.Value.egid ] ;
					entry.Value.ey = centroids_Y[ entry.Value.egid ] ;
					entry.Value.ez = centroids_Z[ entry.Value.egid ] ;
					entry.Value.mode = 1 ; // walk to ex , ey , ez 
				}
				else if ( entry.Value.ex > -1 || entry.Value.ey > -1 || entry.Value.ez > -1 )
				{
					// unit is assigned to move to ex , ey , ez
					entry.Value.mode = 1 ; // walk to ex , ey , ez 
				}
				else {
					entry.Value.mode = 0 ;// scount or idle mode
				}
				
				if ( entry.Value.mode == 0 )
				{
					
					int ax , ay ; // Archer X , Y
					int wx , wy ; // Watch X , Y
					ax = entry.Value.x ;
					ay = entry.Value.y ;
					int prange = entry.Value.range ; // Generally 12 
					int nrange = -prange ; // -12 
					bool should_search = true ; 
					for ( int i = nrange ; i < prange && should_search ; i++ ){
						for ( int j = nrange ; j < prange && should_search ; j++ ){
							wx = ax + i ;
							wy = ay + j ;
							if ( wx >= 0 && wx < groundLength && wy >= 0 && wy < groundWidth )
							{
								if ( grid[ wx , wy , 1 ] >= gtime && allUnits.ContainsKey(grid[ wx , wy , 0 ]) && allUnits[grid[ wx , wy , 0 ]].tid != entry.Value.tid  )
								{
									if ( allUnits[grid[ wx , wy , 0 ]].health < 1 )
									{
										// No over kill
									}
									else
									{
										if ( entry.Value.wid % 2 == 0 ) // Ranged Units
										{
											if ( ! allArrows.ContainsKey( entry.Value.id ) )// Check if i already had an arrow registered on my id
											{
												// new Arrow ( _id , _eid , _dmga )
												Arrow arw = new Arrow( entry.Value.id , grid[ wx , wy , 0 ].id , entry.Value.attack );  
												allArrows.Add( arw );
											}
											Console.WriteLine( "Arrow Released from id:"+entry.Value.id+" onto:("+wx+","+wy+")" );
											
										}
										else 
										{
											entry.Value.ex = allUnits[grid[ wx , wy , 0 ]].x ; // Aggressive
											entry.Value.ey = allUnits[grid[ wx , wy , 0 ]].y ;
											entry.Value.ez = allUnits[grid[ wx , wy , 0 ]].z ;
										}
										should_search = false ; // Scout Successfull .. so exit loops
									}
								}
							}
						}
					}
					
				}
				
					
				bool can_move = false ; 
				int shortest_distance = int.MaxValue ;
				int shortest_x = -1 ;
				int shortest_y = -1 ;
				
				// check for meele attack
				int ux , uy ;
				int dx , dy ;
				ux = entry.Value.x ;
				uy = entry.Value.y ;
				dx = -1 ; dy = -1 ; 
				int[] DX = new int[] { -1 , 0 , 1 , -1 , 1 , -1 ,  0 ,  1 } ;
				int[] DY = new int[] {  1 , 1 , 1 ,  0 , 0 , -1 , -1 , -1 } ;
				
				
				for ( int i = 0 ; i < DX.Length ; i++ )
				{
					dx = ux + DX[i] ;
					dy = uy + DY[i] ;	
					if ( dx >= 0 && dx < groundLength && dy >= 0 && dy < groundWidth )
					{
						if ( grid[ dx , dy , 1 ] >= gtime )
						{
							if ( allUnits.ContainsKey( grid[ dx , dy , 0 ] ) && allUnits[grid[ dx , dy , 0 ]].tid != entry.Value.tid )// Enemy
							{
								// Marking enemy as enemy
								entry.Value.mode = 2 ;
								entry.Value.eid = grid[ dx , dy , 0 ] ;
								// Marking myself as enemy to my enemy
								if ( allUnits[ entry.Value.eid ].mode == 0 )
								{
									allUnits[ entry.Value.eid ].mode = 1 ; // Taunt the enemy to attack ; works for archers ;-)
									allUnits[ entry.Value.eid ].ex = entry.Value.x ;
									allUnits[ entry.Value.eid ].ey = entry.Value.y ;
									allUnits[ entry.Value.eid ].ez = entry.Value.z ;
								}
								break ; 
							}
							else // Friendly
							{
								// Friendly Block ...Ask To Make Way...Version2.0..Not Now	
							}
						}
						else {
							can_move = true ;
							int distance = calculateDistanceSquare( dx , dy , entry.Value.ex , entry.Value.ey );
							if ( distance < shortest_distance )
							{
								shortest_distance = distance ;
								shortest_x = dx ;
								shortest_y = dy ;
							}
						}
					}
				}
				
				
				if ( entry.Value.mode == 1 && can_move )
				{
					grid[ entry.Value.x , entry.Value.y , 0 ] = 0 ;
					grid[ entry.Value.x , entry.Value.y , 1 ] = 0 ; // Making Room For Other Units
					entry.Value.x = shortest_x ;
					entry.Value.y = shortest_y ;
				}
				
				
				//Attack
				if ( entry.Value.mode == 2 )
				{
					if ( allUnits.ContainsKey(entry.Value.eid) ) // Checking if enemy is alive
					{
						allUnits[ entry.Value.eid ].health -= entry.Value.attack ; 
					}
					else {
						entry.Value.eid = -1 ; // Enemy Got Killed
					}
					
					if ( ! counts.ContainsKey( entry.Value.egid ) )
					{
						entry.Value.egid = -1 ; // Enemy Group is dead
					}
					
					
				}
				
				grid[ entry.Value.x , entry.Value.y , 0 ] = entry.Value.id ;
				grid[ entry.Value.x , entry.Value.y , 1 ] = gtime+1 ; // Finished Processing Unit
			}
        	
        	
        	
            gtime++ ;
            fps = 0 ;
            
            Test.gridprint( grid , gtime , allUnits );// helper to be removed later
        }
        
        
        fps++ ;
        
    }
    
    #endregion
    
    #region Helper Methods
    
    public int calculateDistanceSquare( int x1 , int y1 , int x2 , int y2 )
    {
    	return ((x2-x1)*(x2-x1)) + ((y2-y1)*(y2-y1)) ;
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
    public int tid ;         // Team ID
    public int x ;           // Fighters X Cor
    public int y ;           // Fighters Y Cor
    public int z ;           // Fighters Z Cor
    public int range  ;      // Fighters Sight Range
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
		eid = -1 ; 
		egid = -1 ;
		ex = -1 ;
		ey = -1 ;
		ez = -1 ;
		tid = 1 ; // By default Team1
		if ( _pid % 2 == 0 )
		{
			tid = 2 ; // All even pids to Team2
		}
    }
    
    #endregion
    
    #region Managing Methods
    
    private void giveWeapon( int _wid )
    {
    	Constants c = new Constants( _wid );
    	range = c.getRange();
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
    		"     ID : {0} \t " +
        //  "    GID : {1} \t " +
    		"    WID : {2} \t " +
    		"    PID : {3} \t " +
    		"      X : {4} \t " +
    		"      Y : {5} \t " +
    	//  "      Z : {6} \t " +
    		" HEALTH : {7} \t " +
    		" ATTACK : {8} \t " +
    	"",id,gid,wid,pid,x,y,z,health,attack);
    }
    
    #endregion
    
}


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







