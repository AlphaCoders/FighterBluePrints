public class World
{
    #region Init
    
    public int[,] grid ;    // World's Grid 
    public int gtime ;      // Game Time
    
    #endregion 
    
    #region Constructors
    
    public World()
    {
        grid = new int[500,500] ;
        gtime = 0 ;
    }
    
    #endregion
    
    #region Update
    
    public void Update()
    {
        // Here we make the movement and attacks and health deductions
        gtime++ ;
    }
    
    #endregion
}
