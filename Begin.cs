
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
    
    public int Test_generateid( int _pid )
    {
    	Random r = new Random( UnixTime() );
    	return r.Next( 1111 , 9999 );
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
    					// create Fighter with id and add to all units 
    					grid[i,j,1] = gtime ;
    					break ; 
					case 'e' :
						grid[i,j,0] = Test_generateid(2) ;
						
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
        groundWidth = 15 ;
        groundLength = 7 ;
        grid = new int[groundLength,groundWidth,2] ;
        arrows = new int[groundLength,groundWidth,2] ;
        
        // Here we must add our capital
        // Also add enemy capitals as Static Units
        
        board = 
        "..............." +
        "..........e...." +
        "..............." +
        "..............." +
        "..............." +
        "..p............" +
        "..............." 
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
