using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
	[Id("player")]
	public class Player
	{
		[Param(0)] private string _name;

		public Player(string name)
		{
			_name = name;
		}

		public Player(){}

		public string get_name()
		{
			return _name;
		}

		public void set_name(string name)
		{
			_name = name;
		}
	}
}

