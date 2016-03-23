
public class Begin
{
    #region Init
    
    public int gtime ;                           // Game Time
    public int fps ;                             // tmp Frame Number
    public int maxFps ;                          // Max FPS
    public int fastFactor ;                      // Ex: x 2 times the original speed
    public Dictionary<int,Fighter> Units         // Holds ( id , Fighter ) Relationship
    
    #endregion 
    
    #region Constructors
    
    public Begin()
    {
        Units = new Dictionary<int,Fighter>();
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
