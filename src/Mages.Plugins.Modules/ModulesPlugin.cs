namespace Mages.Plugins.Modules
{
    using Mages.Core;
    using Mages.Core.Runtime.Functions;
    using System;
    using System.Collections.Generic;

    public static class ModulesPlugin
    {
        public static readonly String Name = "Modules";
        public static readonly String Version = "1.0.0";
        public static readonly String Author = "Florian Rappl";

        public static void AllowModules(this Engine engine, IEnumerable<IModuleFileReader> readers, IEngineCreator creator)
        {
            var importer = new ModuleImporter(readers, creator);
            engine.SetFunction("import", new Function(args =>
            {
                var id = engine.Globals["import"] as Function;
                var directory = engine.GetDirectory();
                return Curry.MinOne(id, args) ??
                    If.Is<String>(args, fileName => importer.From(fileName, directory));
            }));
            engine.SetFunction("export", new Function(args =>
            {
                var value = args.Length != 0 ? args[0] : null;
                Cache.Assign(engine, value);
                return null;
            }));
        }
    }
}
