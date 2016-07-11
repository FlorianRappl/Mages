namespace Mages.Plugins.Plots
{
    using OxyPlot;
    using System;

    public static class PlotsPlugin
    {
        public static readonly String Name = "Plots";
        public static readonly String Version = "0.1.0";
        public static readonly String Author = "Florian Rappl";

        public static Type Plot
        {
            get { return typeof(PlotModel); }
        }
    }
}
