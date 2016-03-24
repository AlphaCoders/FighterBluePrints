
public class Begin
{
    #region Init
    
    public int gtime ;                           // Game Time
    public int fps ;                             // tmp Frame Number
    public int maxFps ;                          // Max FPS
    public int fastFactor ;                      // Ex: x 2 times the original speed
    public Dictionary<int,Fighter> allUnits      // Holds ( id , Fighter ) Relationship
    public int groundWidth ;                     // Battle Ground Width
    public int groundLength ;                    // Battle Ground Length
    public int grid[,,] ;                        // 3D Maxtrix ...(x,y,0) = id at x,y and (x,y,1) = gtime at x,y
    public int arrows[,,] ;                      // Same as grid but (x,y,0) = Flag(0/1) and (x,y,1) = gtime
    
    #endregion 
    
    #region Constructors
    
    public Begin()
    {
        allUnits = new Dictionary<int,Fighter>();
        groundWidth = 700 ;
        groundLength = 1200 ;
        grid = new int[groundLength,groundWidth,2] ;
        arrows = new int[groundLength,groundWidth,2] ;
        
        // Here we must add our capital
        // Also add enemy capitals as Static Units
        gtime = 0 ;
        fps = 0 ;
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
