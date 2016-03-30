
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
