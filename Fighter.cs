
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
    
    public Fighter( int _id , int _gid , int _wid , int _x , int _y , int _z )
    {
		id = _id ;
		gid = _gid ;
		wid = _wid ;
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
    
    #endregion
    
}








