
public class Fighter
{
	
    #region Fighter Details
    
    public int id ;          // Unique ID
    public int gid ;         // Group ID 
    public int wid ;         // Weapon ID
    public int x ;           // Fighters X Cor
    public int y ;           // Fighters Y Cor
    public int z ;           // Fighters Z Cor
    public int health ;      // Health or Hitpoints
    public int attack ;      // Weapon Attack
    public int handattack ;  // Hand Attack
    public int areaattack ;  // Area Attack
    public int speed ;       // Movement Speed
    public int mode ;        // Idle[0] , Walk[1] , Attack[2] , Hurt[3] , Die[4]
    
    #endregion
    
    #region Enemy Details
    
    public int eid ;         // enemy whom the fighter is going to attack
    public int egid ;        // enemy group which is fighter is going to attack
    public int ex ;          // enemy X Cor
    public int ey ;          // enemy Y Cor
    public int ez ;          // enemy Z Cor
    
    #endregion
    
    #region Constructors
    
    Fighter( int _id , int _gid , int _wid , int _x , int _y , int _z )
    {
		id = _id ;
		gid = _gid ;
		wid = _wid ;
		x = _x ;
		y = _y ;
		z = _z ;
		mode = 0 ;
		giveWeapon( _wid );
    }
    
    #endregion
    
    #region Managing Methods
    
    giveWeapon( int _wid )
    {
    	Constants c = new Constants();
    	health = c.health( _wid );
    	attack = c.attack( _wid );
    	handattack = c.handattack( _wid );
    	areaattack = c.areaattack( _wid );
    	speed = c.speed( _wid );
    }
    
    #endregion
    
}
func01(int n)
{
	
}

