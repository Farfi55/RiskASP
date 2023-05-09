using it.unical.mat.embasp.languages;
using Unity.VisualScripting;

namespace EmbASP.predicates
{
	[Id("player")]
	public class Player
	{
		[Param(0)] public string Name;

		public Player(string name)
		{
			Name = name;
		}
		
	}
}

