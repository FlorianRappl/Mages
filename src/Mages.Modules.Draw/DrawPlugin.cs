namespace Mages.Modules.Draw
{
    using System;

    public static class DrawPlugin
    {
        public static readonly String Name = "Draw";
        public static readonly String Version = "1.0.0";
        public static readonly String Author = "Florian Rappl";

        public static Type Canvas
        {
            get { return typeof(Canvas); }
        }
    }
}
