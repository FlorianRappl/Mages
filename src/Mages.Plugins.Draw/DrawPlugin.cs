namespace Mages.Plugins.Draw
{
    using System;

    public static class DrawPlugin
    {
        public static readonly String Name = "Draw";
        public static readonly String Version = "0.2.0";
        public static readonly String Author = "Florian Rappl";

        public static Type Canvas
        {
            get { return typeof(Canvas); }
        }
    }
}
