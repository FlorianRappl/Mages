namespace Mages.Modules.Plots
{
    using OxyPlot;
    using System;

    public static class PlotsPlugin
    {
        public static readonly String Name = "Plots";
        public static readonly String Version = "1.0.0";
        public static readonly String Author = "Florian Rappl";

        public static Type Plot
        {
            get { return typeof(PlotModel); }
        }
    }
}
