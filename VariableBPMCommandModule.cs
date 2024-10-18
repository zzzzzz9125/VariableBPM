#if !Sony
using ScriptPortal.Vegas;
#else
using Sony.Vegas;
#endif

using System.Collections;
using System.Collections.Generic;

namespace VariableBpm
{
    public partial class VariableBpm : ICustomCommandModule
    {
        private Vegas myVegas;
        private static readonly CommandCategory category = CommandCategory.Tools;

        public void InitializeModule(Vegas Vegas)
        {
            myVegas = Vegas;
        }

        public ICollection GetCustomCommands()
        {
            List<CustomCommand> customCommands = new List<CustomCommand>();

            new VariableBpmCommand().VariableBpm(myVegas, category, ref customCommands);

            return customCommands;
        }
    }
}