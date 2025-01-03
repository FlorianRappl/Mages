﻿namespace Mages.Repl
{
    using Mages.Core;
    using System;

    static class InteractivityExtensions
    {
        public static void Display(this IInteractivity interactivity, ParseError error, String source)
        {
            var start = error.Start.Index - 1;
            var end = error.End.Index - 1;
            var lines = source.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            source = String.Join(" ", lines).Replace('\t', ' ');

            if (end == start)
            {
                end++;
            }

            var range = 48;
            var middle = (end + start) / 2;
            var ss = Math.Min(source.Length, Math.Max(middle - range / 2, 0));
            var se = Math.Min(Math.Max(0, middle + range / 2), source.Length);
            var snippet = source.Substring(ss, se - ss);
            interactivity.Error(snippet);
            interactivity.Error(Environment.NewLine);
            interactivity.Error(new String(' ', Math.Max(0, start - ss)));
            interactivity.Error(new String('^', Math.Max(0, end - start)));
            interactivity.Error(Environment.NewLine);
            interactivity.Display(error);
        }

        public static String GetLine(this IInteractivity interactivity, String prompt, AutoCompleteHandler handler)
        {
            interactivity.AutoComplete += handler;
            var line = interactivity.GetLine(prompt);
            interactivity.AutoComplete -= handler;
            return line;
        }
        
        public static void Display(this IInteractivity interactivity, ParseError error)
        {
            var message = error.Code.GetMessage();
            var hint = String.Format("Line {1}, Column {2}: {0}", message, error.Start.Row, error.Start.Column);
            interactivity.Error(hint);
        }
    }
}
