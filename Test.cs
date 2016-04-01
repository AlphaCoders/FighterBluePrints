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
