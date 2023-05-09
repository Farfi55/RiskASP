using it.unical.mat.embasp.languages;
using Unity.VisualScripting;

namespace EmbASP.predicates
{
	[Id("player")]
	public class PlayerPredicate
	{
		[Param(0)] public string Name;

		public PlayerPredicate()
		{
			
		}
		
		public PlayerPredicate(string name)
		{
			Name = name;
		}
		
		public string setName(string name) => Name = name;
		
		public string getName() => Name;

	}
}

