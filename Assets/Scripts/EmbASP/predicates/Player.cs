using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
	[Id("player")]
	public class Player
	{
		[Param(0)] private string _name;

		public string Name
		{
			get => _name;
			set => _name = value;
		}
		
	}
}

