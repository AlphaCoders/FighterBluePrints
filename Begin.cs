
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
