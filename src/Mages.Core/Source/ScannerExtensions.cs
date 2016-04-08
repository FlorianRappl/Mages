namespace Mages.Core.Source
{
    using System;

    static class ScannerExtensions
    {
        public static Boolean PeekMoveNext(this IScanner scanner, Int32 character)
        {
            if (scanner.MoveNext())
            {
                if (scanner.Current == character)
                {
                    return true;
                }

                scanner.MoveBack();
            }

            return false;
        }
    }
}
