using System.Threading;
namespace task14;

public class DefiniteIntegral
{
     public static double Solve(double a, double b, Func<double, double> function, double step, int threadsnumber)
    {
        double[] results = new double[threadsnumber];

        double lenght = (b - a) / threadsnumber;

        Parallel.For(0, threadsnumber, i =>
        {
            double start = a + i * lenght;
            double end;
            if (i == threadsnumber - 1)
            {
                end = b;
            }
            else
            {
                end = start + lenght;
            }

            results[i] = SolveForOneThread(start, end, function, step);
        });

        return results.Sum();
    }

    public static double SolveForOneThread(double a, double b, Func<double, double> function, double step)
    {
        double current = 0.0;
        double next = 0.0;
        double nextVal = 0.0;

        for (double x = a; x < b; x += step)
        {
            next = Math.Min(x + step, b);
            nextVal = function(next);

            current += (function(x) + nextVal) * (next - x) / 2.0;
        }
        return current;
    }
}

