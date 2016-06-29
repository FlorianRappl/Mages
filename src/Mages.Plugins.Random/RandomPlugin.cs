namespace Mages.Plugins.Random
{
    using System;

    public static class RandomPlugin
    {
        public static readonly String Name = "Random";
        public static readonly String Version = "1.0.0";
        public static readonly String Author = "Florian Rappl";

        public static Type Rng
        {
            get { return typeof(Generators); }
        }
    }
}
