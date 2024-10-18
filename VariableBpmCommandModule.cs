#if !Sony
using ScriptPortal.Vegas;
#else
using Sony.Vegas;
#endif

using System.Collections;
using System.Collections.Generic;

namespace VariableBpm
{
	public class LayerRepeaterCommandModule : ICustomCommandModule
	{
		private Vegas myVegas;

		public void InitializeModule(Vegas vegas)
		{
			myVegas = vegas;
		}

		public ICollection GetCustomCommands()
		{
			List<CustomCommand> customCommands = new List<CustomCommand>();

			new VariableBpmCommand().VariableBpmInit(myVegas, ref customCommands);

			return customCommands;
		}
	}
}