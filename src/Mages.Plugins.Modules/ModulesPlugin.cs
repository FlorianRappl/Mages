namespace Mages.Plugins.Modules
{
    using Mages.Core;
    using Mages.Core.Runtime.Functions;
    using System;

    public static class ModulesPlugin
    {
        public static readonly String Name = "Modules";
        public static readonly String Version = "1.0.0";
        public static readonly String Author = "Florian Rappl";

        public static void AllowModules(this Engine engine, IModuleFileReader reader, IEngineCreator creator)
        {
            var importer = new ModuleImporter(reader, creator);
            engine.SetFunction("import", new Function(args =>
            {
                var id = engine.Globals["import"] as Function;
                return Curry.MinOne(id, args) ??
                    If.Is<String>(args, fileName => importer.From(fileName));
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
