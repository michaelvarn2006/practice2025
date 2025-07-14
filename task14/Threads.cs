using System.Threading;
namespace task14;

public class DefiniteIntegral
{

    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        double result = 0.0;
        int totalSteps = (int)((b - a) / step);
        double stepCounter = (b - a) / totalSteps;
        int stepsPerThread = totalSteps / threadsNumber;
        Thread[] threads = new Thread[threadsNumber];
        Barrier barrier = new Barrier(threadsNumber + 1);

        for (int t = 0; t < threadsNumber; t++)
        {
            int localStartStep = t * stepsPerThread;
            int localTotalSteps = stepsPerThread;
            threads[t] = new Thread(() =>
            {
                double localSum = 0.0;
                for (int i = 0; i < localTotalSteps; i++)
                {
                    int idx = localStartStep + i;
                    double x0 = a + idx * stepCounter;
                    double x1 = x0 + stepCounter;
                    double y0 = function(x0);
                    double y1 = function(x1);
                    localSum += (y0 + y1) * (x1 - x0) / 2.0;
                }
                double oldResult, newResult;
                do
                {
                    oldResult = result;
                    newResult = oldResult + localSum;
                } while (Interlocked.CompareExchange(ref result, newResult, oldResult) != oldResult);
                barrier.SignalAndWait();
            });
            threads[t].Start();
        }

        barrier.SignalAndWait();
        return result;
    }
}

