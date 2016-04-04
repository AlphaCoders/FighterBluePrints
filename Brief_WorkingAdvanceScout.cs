// Need to check over kill condition by incrementing arch attack to 690 on enemy cav
// make movement a bit strategic
// make ......
// Contains IDEONE code

using System;
using System.Collections.Generic;

public class Test
{
    public static void printer(SortedDictionary<int, Fighter> _allUnits)
    {
        foreach (KeyValuePair<int, Fighter> entry in _allUnits)
        {
            Console.WriteLine(entry.Value.ToString());
        }

    }

    public static void gridprint(int[, ,] _grid, int _gtime, SortedDictionary<int, Fighter> _allUnits)
    {
        Console.WriteLine("Printing Grid at time :" + _gtime);
        String pad;
        int u;
        int v;
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                if (_grid[i, j, 0] == 0)
                {
                    pad = "...0";
                }
                else
                {
                    pad = "" + _grid[i, j, 0];
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

                if (_grid[i, j, 0] == 0)
                {
                    v = 0;
                }
                else
                {
                    v = _allUnits[_grid[i, j, 0]].health;
                }
                pad += ",";
                pad += v;
                u = ("" + v).Length;
                for (int k = 0; k < (3 - u); k++)
                {
                    pad += ".";
                }

                Console.Write(pad + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("----------------------------------End at gtime:" + _gtime);

    }

    public static void Main()
    {
        Begin b = new Begin();

        //printer( b.allUnits );
        gridprint(b.grid, b.gtime, b.allUnits);

        foreach (KeyValuePair<int, Fighter> entry in b.allUnits)
        {
            if (entry.Value.x > 2)
            {
                if (entry.Value.pid == 1)
                {
                    if (entry.Value.wid == 1)
                    {
                        entry.Value.gid = 101;
                        entry.Value.egid = 203; // Enemy Cavalry on Board
                    }
                    else if (entry.Value.wid == 2)
                    {
                        entry.Value.gid = 102;
                        entry.Value.egid = 203;
                    }
                    else
                    {
                        entry.Value.gid = 103;
                    }

                }
                else
                {
                    entry.Value.gid = 203;
                }
            }
        }

        int cycles = 9;
        int limit = 10 + (40 * cycles);
        while (limit-- > 0)
        {
            b.Update();
        }
        Console.ReadLine();

    }
}


public class Begin
{
    // {
    #region Init

    public int gtime;                               // Game Time
    public int fps;                                 // tmp Frame Number
    public int maxFps;                              // Max FPS
    public int fastFactor;                          // Ex: x 2 times the original speed
    public SortedDictionary<int, Fighter> allUnits;  // Holds ( id , Fighter ) Relationship
    public int groundWidth;                         // Battle Ground Width
    public int groundLength;                        // Battle Ground Length
    public int[, ,] grid;                            // 3D Maxtrix ...(x,y,0) = id at x,y and (x,y,1) = gtime at x,y
    public SortedDictionary<int, Arrow> allArrows;   // Holds ( arrowID , arrowObject) Relationship

    #endregion

    #region Testing Data

    public int first_time_print = 0;
    public String board;

    public int UnixTime()
    {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (int)(DateTime.UtcNow - epochStart).Ticks * 100;
    }

    public int Test_generateid(int _pid)
    {
        Random r = new Random(UnixTime());
        int tmp_id;
        tmp_id = (_pid * 1000) + r.Next(111, 999);
        while (allUnits.ContainsKey(tmp_id))
        {
            tmp_id = (_pid * 1000) + r.Next(111, 999);
        }
        return tmp_id;
    }

    public void Test_addFighter(int _id, int _wid, int _pid, int _x, int _y)
    {
        Fighter obj;

        obj = new Fighter(_id, 0, _wid, _pid, _x, _y, 0);

        allUnits.Add(_id, obj);
    }

    public void Test_populategrid(String _board)
    {
        for (int i = 0; i < groundLength; i++)
        {
            for (int j = 0; j < groundWidth; j++)
            {
                char c = _board[(i * groundWidth) + j];
                switch (c)
                {
                    case 'p':
                        grid[i, j, 0] = Test_generateid(1);//_pid
                        Test_addFighter(grid[i, j, 0], 1, 1, i, j);// _id , _wid = 1 pike , 2 arch , 3 cav , _pid 
                        grid[i, j, 1] = gtime;
                        break;
                    case 'a':
                        grid[i, j, 0] = Test_generateid(1);//_pid
                        Test_addFighter(grid[i, j, 0], 2, 1, i, j);// _id , _wid = 1 pike , 2 arch , 3 cav , _pid 
                        grid[i, j, 1] = gtime;
                        break;
                    case 'c':
                        grid[i, j, 0] = Test_generateid(1);//_pid
                        Test_addFighter(grid[i, j, 0], 3, 1, i, j);// _id , _wid = 1 pike , 2 arch , 3 cav , _pid 
                        grid[i, j, 1] = gtime;
                        break;
                    case 'e':
                        grid[i, j, 0] = Test_generateid(2);
                        Test_addFighter(grid[i, j, 0], 3, 2, i, j);
                        grid[i, j, 1] = gtime;
                        break;
                    default:
                        grid[i, j, 0] = 0;
                        grid[1, j, 1] = 0;
                        break;
                }
            }
        }
    }

    #endregion

    #region Constructors

    public Begin()
    {
        gtime = 0;
        fps = 0;
        maxFps = 40;
        fastFactor = 1;

        allUnits = new SortedDictionary<int, Fighter>();
        groundWidth = 9;
        groundLength = 15;
        grid = new int[groundLength, groundWidth, 2];
        allArrows = new SortedDictionary<int, Arrow>();
        // Here we must add our capital
        // Also add enemy capitals as Static Units


        board =
        "........." + // Player 1 
        "........." +
        "..a...aa." +
        "........." +
        "..ppppp.." +
        "..ppppp.." +
        "..ppppp.." +
        "..ppppp.." +
        "........." + // OLD BOARD
        "........." +
        "........." +
        "........." +
        "..eeeee.." +
        "..eeeee.." +
        "..eeeee.."   // Player 2 
        ;


        gtime = 1;

        Test_populategrid(board);
    }

    #endregion
    //}
    #region Graphics Engine Screen Update Method

    public void Update()
    {
        if (fps == maxFps / fastFactor) // Equivalent to 1 second in gametime
        {
            Console.WriteLine("Cycle Begins");
            List<int> deadUnits = new List<int>();
            foreach (KeyValuePair<int, Fighter> entry in allUnits)
            {
                if (entry.Value.health < 1)
                {
                    deadUnits.Add(entry.Value.id); // Registering Casualities = Dead Units
                }
                else
                {
                    entry.Value.is_processed = false;
                    entry.Value.thealth = entry.Value.health ;
                    grid[entry.Value.x, entry.Value.y, 1] = gtime + 1;
                }
            }
            foreach (int deadUnitID in deadUnits)
            {
                grid[allUnits[deadUnitID].x, allUnits[deadUnitID].y, 0] = 0;
                grid[allUnits[deadUnitID].x, allUnits[deadUnitID].y, 1] = 0;// Freeing grid space
                allUnits.Remove(deadUnitID);// Cleaning up the Dead
                // Change animation to death animation
            }

            // Make movement 
            Dictionary<int, int> counts = new Dictionary<int, int>();  // _gid , number of units belonging to gid
            Dictionary<int, int> centroids_X = new Dictionary<int, int>(); // _gid , _centeroid_XCOR
            Dictionary<int, int> centroids_Y = new Dictionary<int, int>(); // _gid , _centeroid_YCOR 
            Dictionary<int, int> centroids_Z = new Dictionary<int, int>(); // _gid , _centeroid_ZCOR 
            int tgid;
            foreach (KeyValuePair<int, Fighter> entry in allUnits)
            {

                tgid = entry.Value.gid;
                if (counts.ContainsKey(tgid))
                {
                    centroids_X[tgid] += entry.Value.x;
                    centroids_Y[tgid] += entry.Value.y;
                    //centroids_Z[ tgid ] = ((centroids_Z[ tgid ]*counts[tgid]) + entry.Value.z )/ (counts[tgid]+1);
                    counts[tgid] += 1;
                }
                else
                {
                    centroids_X.Add(tgid, entry.Value.x);
                    centroids_Y.Add(tgid, entry.Value.y);
                    //centroids_Z.Add( tgid , entry.Value.z ) ;
                    counts.Add(tgid, 1);
                }
            }
            foreach (KeyValuePair<int, int> ent in counts)
            {
                int _gid = ent.Key;
                centroids_X[_gid] = centroids_X[_gid] / counts[_gid]; // average
                centroids_Y[_gid] = centroids_Y[_gid] / counts[_gid];
            }

            /*
            foreach( KeyValuePair<int,int> ent in counts )
            {
                int _gid = ent.Key ;
                if ( _gid > 0 )
                {
                    Console.WriteLine( "{0}:{3} has centroid at ({1},{2})" , _gid , centroids_X[_gid] , centroids_Y[_gid] , counts[_gid] );
                }
            }
            // Centroid Display Code
            */


            int up = 1;// Units Processed , Sentinel to Start = 1 , Later on up = 0 , up++ on processing a unit
            while (up > 0)// No change after an iteration , then fate says no further processing happens
            {
                List<int>[] Genie = new List<int>[25];
                for (int j = Genie.Length - 1; j >= 0; j--)
                {
                    Genie[j] = new List<int>(); // Initializing the Genie For Strategic Movement
                }
                foreach (KeyValuePair<int, Fighter> entry in allUnits) // Unit Processing
                {
                    if (entry.Value.is_processed)
                    {
                        continue; // Skip the processed units 
                    }
                    //Console.WriteLine( entry.Value.ToString() );
                    if (entry.Value.eid > -1)
                    {
                        entry.Value.mode = 2; //attack animation triggered
                        if (allUnits.ContainsKey(entry.Value.eid))
                        {
                            entry.Value.ex = allUnits[entry.Value.eid].x;
                            entry.Value.ey = allUnits[entry.Value.eid].y;
                            entry.Value.ez = allUnits[entry.Value.eid].z;
                        }
                        else
                        {
                            entry.Value.mode = 0;
                        }
                    }
                    else if (entry.Value.egid > -1)
                    {
                        entry.Value.ex = centroids_X[entry.Value.egid] - (centroids_X[entry.Value.gid] - entry.Value.x);
                        entry.Value.ey = centroids_Y[entry.Value.egid] - (centroids_Y[entry.Value.gid] - entry.Value.y);
                        entry.Value.mode = 1; // walk to ex , ey , ez 
                    }
                    else if (entry.Value.ex > -1 || entry.Value.ey > -1 || entry.Value.ez > -1)
                    {
                        // unit is assigned to move to ex , ey , ez
                        entry.Value.mode = 1; // walk to ex , ey , ez 
                    }
                    else
                    {
                        entry.Value.mode = 0;// scount or idle mode
                    }



                    if (entry.Value.mode == 0)
                    {

                        int ax, ay; // Archer X , Y
                        int wx, wy; // Watch X , Y
                        ax = entry.Value.x;
                        ay = entry.Value.y;
                        int prange = entry.Value.range; // Generally 12 
                        int nrange = -prange; // -12 
                        bool should_search = true;
                        
                    //{ Advanced Scout
                    
				        int dir = 0;
				        int i = 0 ;
				        int j = 0 ;
				        bool phase_stop = false ;
				        int nearest_enemy = -1 ;
				        int nearest_distance = int.MaxValue ;
				        int ci = i, cj = j , di = i , dj = j ;
				        while ( cj <= prange )// if one limiter is out of bounds so are all limiters
				        {
				            if (dir == 0)// right
				            {
				                j++;
				                if ( j > cj )
				                {
				                    dir = 1;
				                    cj = j;
				                    if ( phase_stop ){
				                    	break ;
				                    }
				                }
				            }
				            else if (dir == 1)// down
				            {
				                i++;
				                if (i > ci)
				                {
				                    dir = 2;
				                    ci = i;
				                    if ( phase_stop ){
				                    	break ;
				                    }
				                }
				            }
				            else if (dir == 2)// left
				            {
				                j--;
				                if (j < dj)
				                {
				                    dir = 3;
				                    dj = j;
				                    if ( phase_stop ){
				                    	break ;
				                    }
				                }
				            }
				            else // up
				            {
				                i--;
				                if (i < di)
				                {
				                    dir = 0;
				                    di = i;
				                    if ( phase_stop ){
				                    	break ;
				                    }
				                }
				            }
				            
				            wx = ax + i ;
				            wy = ay + j ; 
			                if (wx >= 0 && wx < groundLength && wy >= 0 && wy < groundWidth)
                            {
                                if (grid[wx, wy, 1] >= gtime && allUnits.ContainsKey(grid[wx, wy, 0]) && allUnits[grid[wx, wy, 0]].tid != entry.Value.tid)
                                {
                                	int edist = calculateDistanceSquare( ax , ay, wx , wy );
                                    if ( edist < nearest_distance && allUnits[ grid[wx,wy,0] ].thealth > 0 )// Health condition to avoid over kill
                                    {
                                        nearest_distance = edist ;
                                        nearest_enemy = grid[wx,wy,0]; // enemy id 
                                        phase_stop = true ; // because one enemy found atleast with some health
                                    }
                                
                                }
                            }
                            
				        }
				            
		            	if ( phase_stop )
		            	{
                            if (entry.Value.wid % 2 == 0) // Ranged Units
                            {
                                if (!allArrows.ContainsKey(entry.Value.id))// Check if i already had an arrow registered on my id
                                {
                                    // new Arrow ( _id , _eid , _dmg )
                                    Arrow arw = new Arrow(entry.Value.id, nearest_enemy , entry.Value.attack);
                                    allArrows.Add(entry.Value.id, arw);
                                    allUnits[ nearest_enemy ].thealth -= entry.Value.attack ;
                                    Console.WriteLine("Arrow Released from id:{2} onto:({0},{1})" , allUnits[nearest_enemy].x , allUnits[nearest_enemy].y , entry.Value.id );
                                }

                            }
                            else
                            {
                                entry.Value.ex = allUnits[nearest_enemy].x; // Aggressive
                                entry.Value.ey = allUnits[nearest_enemy].y;
                                entry.Value.ez = allUnits[nearest_enemy].z;
                                Console.WriteLine("Aggressive Unit {2} onto:({0},{1})" , allUnits[nearest_enemy].x , allUnits[nearest_enemy].y , entry.Value.id );
                            }
		            	}
                        //}
                     

                    }

                    int free_squares = 0;
                    bool can_move = false;
                    int shortest_distance = int.MaxValue;
                    int current_distance = calculateDistanceSquare(entry.Value.x, entry.Value.y, entry.Value.ex, entry.Value.ey);// if ex|ey =-1 then implication is idle
                    int shortest_x = -1;
                    int shortest_y = -1;

                    // check for meele attack
                    int ux, uy;
                    int dx, dy;
                    ux = entry.Value.x;
                    uy = entry.Value.y;
                    dx = -1; dy = -1;
                    int[] DX = new int[] { -1, 0, 1, -1, 1, -1, 0, 1, -1, 0, 1, -2, 2, -2, 2, -2, 2, -1, 0, 1 };
                    int[] DY = new int[] { 1, 1, 1, 0, 0, -1, -1, -1, 2, 2, 2, 1, 1, 0, 0, -1, -1, -2, -2, -2 };


                    for (int i = 0; i < DX.Length; i++)
                    {
                        if (i >= 8 && entry.Value.wid != 3)
                        {
                            break; // >= 8 are cavalry co-ordinates
                        }

                        dx = ux + DX[i];
                        dy = uy + DY[i];
                        if (dx >= 0 && dx < groundLength && dy >= 0 && dy < groundWidth)
                        {
                            if (i < 8 && grid[dx, dy, 1] >= gtime)// Meele Attack
                            {
                                if (allUnits.ContainsKey(grid[dx, dy, 0]) && allUnits[grid[dx, dy, 0]].tid != entry.Value.tid)// Enemy
                                {
                                    // Marking enemy as enemy
                                    entry.Value.mode = 2;
                                    entry.Value.eid = grid[dx, dy, 0];
                                    can_move = false;
                                    break;
                                }
                                else // Friendly
                                {
                                    // Friendly Block ...Ask To Make Way...Version2.0..Not Now	
                                }
                            }
                            else if (grid[dx, dy, 1] < gtime) // Movement
                            {
                                if (entry.Value.mode == 1)
                                {
                                    int distance = calculateDistanceSquare(dx, dy, entry.Value.ex, entry.Value.ey);
                                    if (distance < current_distance)
                                    {
                                        free_squares++;
                                        if (distance < shortest_distance)
                                        {
                                            can_move = true;
                                            shortest_distance = distance;
                                            shortest_x = dx;
                                            shortest_y = dy;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Means i >= 8 and grid [ i , j , 1 ] >= gtime ...occupied far square
                            }
                        }
                    }

                    entry.Value.should_move = can_move;
                    entry.Value.nx = shortest_x;
                    entry.Value.ny = shortest_y;
                    Genie[free_squares].Add(entry.Value.id);

                }

                up = 0;
                for (int j = Genie.Length - 1; j >= 0; j--)
                {
                    foreach (int _id in Genie[j])
                    {
                        Fighter entryValue = allUnits[_id];

                        if (entryValue.mode == 0)
                        {
                            entryValue.is_processed = true;
                            up++;
                        }

                        // Movement 
                        if (entryValue.mode == 1)
                        {
                            if (entryValue.should_move)
                            {
                                if (grid[entryValue.nx, entryValue.ny, 1] >= gtime)
                                {
                                    // Room occupied by quicker unit ... Fail Safe to avoid over writing of grid positions
                                }
                                else
                                {
                                	bool should_print = false ;
                                	//if ( entryValue.wid == 1 && entryValue.pid == 1 ){ 
                                		//should_print = true ;
                                		//}
                                	if ( should_print )
                                	{
                                    	Console.Write( " {2} moved from ({0},{1})" , entryValue.x , entryValue.y , entryValue.id );
                                	}
                                	
                                    grid[entryValue.x, entryValue.y, 0] = 0;
                                    grid[entryValue.x, entryValue.y, 1] = 0; // Making Room For Other Units

                                    entryValue.x = entryValue.nx;
                                    entryValue.y = entryValue.ny;	// New Square

                                    grid[entryValue.x, entryValue.y, 0] = entryValue.id;
                                    grid[entryValue.x, entryValue.y, 1] = gtime + 1; // Registering New Place in Grid

                                    entryValue.is_processed = true;
                                    up++;
                                    
                                    if ( should_print )
                                    {
                                    	Console.Write( " to ({0},{1})" , entryValue.x , entryValue.y );
	                                    Console.WriteLine( " Destiny to ({0},{1}) FreeSquares:{2}" , entryValue.ex , entryValue.ey , j );
                                    }
                                }
                            }
                            else
                            {
                                // Should check again for Room
                            }
                        }

                        // Attack
                        if (entryValue.mode == 2)
                        {
                            if (allUnits.ContainsKey(entryValue.eid)) // Checking if enemy is alive
                            {
                                allUnits[entryValue.eid].health -= entryValue.attack;
                                //Console.WriteLine(entryValue.id + " attacked " + entryValue.eid);
                            }
                            else
                            {
                                entryValue.eid = -1; // Enemy Got Killed
                            }

                            if (!counts.ContainsKey(entryValue.egid))
                            {
                                entryValue.egid = -1; // Enemy Group is dead
                            }

                            entryValue.is_processed = true;
                            up++;
                        }

                    }
                }
                Console.WriteLine( up+" units processed in this loop");
            }
            // End of While

            gtime++;
            fps = 0;

            Test.gridprint(grid, gtime, allUnits);// helper to be removed later
        }


        fps++;

    }

    #endregion

    #region Helper Methods

    public int calculateDistanceSquare(int x1, int y1, int x2, int y2)
    {
        return ((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1));
    }

    #endregion
}


public class Fighter
{

    #region Fighter Details

    public int id;          // Unique ID
    public int gid;         // Group ID 
    public int wid;         // Weapon ID
    public int pid;         // Player ID
    public int tid;         // Team ID
    public int x;           // Fighters X Cor
    public int y;           // Fighters Y Cor
    public int z;           // Fighters Z Cor
    public int range;      // Fighters Sight Range
    public int health;      // Health or Hitpoints
    public int attack;      // Weapon Attack
    public int handattack;  // Hand Attack
    public int areaattack;  // Area Attack
    public int radius;      // Area Attack Radius
    public int speed;       // Movement Speed
    public int delay;       // Delay betwen attacks
    public int mode;        // Idle[0] , Walk[1] , Attack[2] , Hurt[3] , Die[4]
    public int angle;       // Fighters angle along SkyToEarth Axis

    #endregion

    #region Enemy Details

    public int eid;         // enemy whom the fighter is going to attack
    public int egid;        // enemy group which is fighter is going to attack
    public int ex;          // enemy X Cor
    public int ey;          // enemy Y Cor
    public int ez;          // enemy Z Cor

    #endregion

    #region Helper Variables

    public bool is_processed; // Whether processed
    public bool should_move;  // Can Unit Move
    public int nx;            // Next X Cor
    public int ny;            // Next Y Cor
    public int thealth;       // Sum up arrow dmg

    #endregion

    #region Constructors

    public Fighter(int _id, int _gid, int _wid, int _pid, int _x, int _y, int _z)
    {
        id = _id;
        gid = _gid;
        wid = _wid;
        pid = _pid;
        x = _x;
        y = _y;
        z = _z;
        mode = 0;
        angle = 0;
        giveWeapon(_wid);
        eid = -1;
        egid = -1;
        ex = -1;
        ey = -1;
        ez = -1;
        tid = 1; // By default Team1
        if (_pid % 2 == 0)
        {
            tid = 2; // All even pids to Team2
        }
        is_processed = false;
        should_move = false;
        nx = -1;
        ny = -1;
        thealth = health ;
    }

    #endregion

    #region Managing Methods

    private void giveWeapon(int _wid)
    {
        Constants c = new Constants(_wid);
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
            " ID : {0}" +
            " GID : {1}" +
            " WID : {2}" +
            " PID : {3}" +
            " + : {4}" +
            " ({5},{6},{7})" +
            " ({8},{9},{10})" +
            " EID : {11}" +
            " EGID : {12}" +

        "", id, gid, wid, pid, health, x, y, z, ex, ey, ez, eid, egid);
    }

    #endregion

}


public class Constants
{
    private int wid;
    private int range;
    private int health;
    private int attack;
    private int handattack;
    private int areaattack;
    private int radius;
    private int speed;
    private int delay;

    #region Constructors

    public Constants(int _wid)
    {
        wid = _wid;
        range = 0;
        health = 0;
        attack = 0;
        handattack = 0;
        areaattack = 0;
        radius = 0;
        speed = 0;
        delay = 0;
        if (wid <= 0)
        {
            return; // Err Log
        }
        else if (wid == 1) // PikeMen
        {
            range = 2;
            health = 300;
            attack = 60;
            handattack = attack;
            speed = 5;
            delay = 1;
        }
        else if (wid == 2) // Archers
        {
            range = 12;
            health = 130;
            attack = 20;
            handattack = 10;
            speed = 7;
            delay = 5;
        }
        else if (wid == 3) // Cavalry
        {
            range = 3;
            health = 700;
            attack = 90;
            handattack = attack;
            speed = 11;
            delay = 1;
        }
        else
        {
            return; // Err Log
        }

    }

    #endregion

    #region Getter Methods

    public int getRange()
    {
        return range;
    }

    public int getHealth()
    {
        return health;
    }

    public int getAttack()
    {
        return attack;
    }

    public int getHandattack()
    {
        return handattack;
    }

    public int getAreaattack()
    {
        return areaattack;
    }

    public int getRadius()
    {
        return radius;
    }

    public int getSpeed()
    {
        return speed;
    }

    public int getDelay()
    {
        return delay;
    }

    #endregion

}


public class Arrow
{
    private int id;              // Fighter id who fired Arrow
    private int eid;             // Target Enemy id 
    private int dmg;             // Damage value when Arrow hits target
    private int ax;              // Arrows X Cor
    private int ay;              // Arrows Y Cor
    private int az;              // Arrows Z Cor
    private int angle;           // Arrows rotation along SkyToEarth Axis ( theta )
    private int upangle;         // Arrows rotation wrt XY Plane and Z ( Phi )

    #region Constructors

    public Arrow(int _id, int _eid, int _dmg)
    {
        id = _id;
        eid = _eid;
        dmg = _dmg;
        // ax , ay , az should be derived from fighter class using dictionary
        // initialize angle , upangle and process the path in Managing Methods section
        // need parabolic trajectory managing methods here
    }

    #endregion

    #region Managing Methods

    #endregion

}







